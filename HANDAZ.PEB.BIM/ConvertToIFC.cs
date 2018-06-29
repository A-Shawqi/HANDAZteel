using HANDAZ.Entities;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using Xbim.COBieLiteUK;
using Xbim.Common.Geometry;
using Xbim.FilterHelper;
using Xbim.Ifc2x3.Extensions;
using Xbim.Ifc2x3.GeometricConstraintResource;
using Xbim.Ifc2x3.GeometricModelResource;
using Xbim.Ifc2x3.GeometryResource;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.MaterialResource;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.PresentationOrganizationResource;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.RepresentationResource;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.Ifc2x3.TopologyResource;
using Xbim.IO;
using Xbim.ModelGeometry.Scene;
using Xbim.XbimExtensions.Interfaces;
using XbimExchanger.IfcToCOBieLiteUK;
using XbimGeometry.Interfaces;

namespace HANDAZ.PEB.BIM
{
    public class ConvertToIFC
    {

        public static bool GenerateIFCProject(HndzProject project, string savingPath)
        {

            XbimModel model = CreateandInitModel(project);
            if (project.Buildings == null)
            {
                return false;
            }

            foreach (HndzBuilding buildingHndz in project.Buildings)
            {
                IfcBuilding buildingIFC = CreateBuilding(model, buildingHndz);

                if (buildingHndz.Stories == null)
                {
                    return false;
                }

                List<List<IfcProduct>> buildingProducts = new List<List<IfcProduct>>(buildingHndz.Stories.Count);
                foreach (HndzStorey storey in buildingHndz.Stories)
                {
                    if (storey.HndzProducts != null)
                    {


                        List<IfcProduct> StoreyProducts = new List<IfcProduct>(storey.HndzProducts.Count);

                        foreach (HndzProduct product in storey.HndzProducts)
                        {
                            if (product is HndzWallStandardCase)
                            {
                                HndzWallStandardCase genericProductHndz = product as HndzWallStandardCase;
                                StoreyProducts.Add(CreateWall(model, genericProductHndz));
                            }
                            if (product is HndzWallArc)
                            {
                                HndzWallArc genericProductHndz = product as HndzWallArc;
                                //StoreyProducts.Add(CreateArcedWall(model, genericProductHndz));
                            }
                            if (product is HndzFrame3D)
                            {

                                #region Frame Single Bay Assembly
                                if (product is HndzFrameSingleBay3D)
                                {
                                    HndzFrameSingleBay3D frame3D = product as HndzFrameSingleBay3D;
                                    foreach (HndzFrameSingleBay2D frame2D in frame3D.Frames2D)
                                    {
                                        StoreyProducts.Add(CreateTaperedColumn(model, frame2D.RightColumn));
                                        StoreyProducts.Add(CreateTaperedColumn(model, frame2D.LeftColumn));
                                        StoreyProducts.Add(CreateTaperedBeam(model, frame2D.RightBeam));
                                        StoreyProducts.Add(CreateTaperedBeam(model, frame2D.LeftBeam));
                                    }
                                    foreach (HndzPurlin purlin in frame3D.Purlins)
                                    {
                                        StoreyProducts.Add(CreateBeam(model, purlin));
                                    }
                                }
                                #endregion

                                #region Frame Mono Slope Assembly
                                if (product is HndzFrameMonoSlope3D)
                                {
                                    HndzFrameMonoSlope3D frame3D = product as HndzFrameMonoSlope3D;
                                    foreach (HndzFrameMonoSlope2D frame2D in frame3D.Frames2D)
                                    {
                                        StoreyProducts.Add(CreateTaperedColumn(model, frame2D.RightColumn));
                                        StoreyProducts.Add(CreateTaperedColumn(model, frame2D.LeftColumn));
                                        StoreyProducts.Add(CreateTaperedBeam(model, frame2D.Beam));
                                    }
                                    foreach (HndzPurlin purlin in frame3D.Purlins)
                                    {
                                        StoreyProducts.Add(CreateBeam(model, purlin));
                                    }
                                }
                                #endregion
                            }
                        }
                        buildingProducts.Add(StoreyProducts);
                    }
                }
                AssembleBuilding(model, buildingIFC, 3000, buildingProducts);
            }
            try
            {

                model.SaveAs(savingPath, XbimStorageType.IFC);
                model.SaveAs(savingPath, XbimStorageType.XBIM);
                //model.SaveAs(filePath, XbimStorageType.XBIM);
                CreateWexBimFromIfc(model, savingPath);
                IfcToCoBieLiteUkTest(savingPath
                    + ".ifc");
                return true;
            }
            catch (Exception ex)
            {
                //ToDo:return Saving Error Notification and handle it in ASP
                return false;
            }

        }


        public static void GenerateIFCFrame(HndzFrame3D frame3D, string savingPath)
        {
            HndzFrameSingleBay3D frame3Dsingle = null;
            if (frame3D is HndzFrameSingleBay3D)
            {
                 frame3Dsingle = frame3D as HndzFrameSingleBay3D;
            }
            // create project
            HndzProject project = new HndzProject();
            project.Name = "Handaz Building";
            project.Description = "Handaz Building Description";
            project.GlobalCoordinateSystem = HndzWCS.WGS84;
            project.Owner = new Person("HANDAZ", "HANDAZ", "HANDAZ", "HANDAZ", "HANDAZ", "HANDAZ", "HANDAZ");

            //test stories
            HndzBuilding bui = new HndzBuilding(project);
            HndzStorey storey = new HndzStorey(bui, 0);
            // create project
            if (frame3Dsingle == null)
            {
                frame3Dsingle = new HndzFrameSingleBay3D(24000, 6000, 20000, 6000, 2000, HndzLocationEnum.Cairo,
                  HndzRoofSlopeEnum.From1To10, HndzRoofAccessibilityEnum.Inaccessible, HndzBuildingEnclosingEnum.PartiallyEnclosed, HndzImportanceFactorEnum.II);
            }
            if (frame3Dsingle.BuildingStorey == null)
            {
                frame3Dsingle.BuildingStorey = storey;
            }
            XbimModel model = CreateandInitModel(frame3Dsingle.BuildingStorey.Building.Project);
            IfcBuilding building = CreateBuilding(model, frame3Dsingle.BuildingStorey.Building);

            List<IfcProduct> tt = new List<IfcProduct>();
           
            foreach (HndzFrameSingleBay2D frame2D in frame3Dsingle.Frames2D)
            {
                tt.Add(CreateTaperedColumn(model, frame2D.RightColumn));
                tt.Add(CreateTaperedColumn(model, frame2D.LeftColumn));
                tt.Add(CreateTaperedBeam(model, frame2D.RightBeam));
                tt.Add(CreateTaperedBeam(model, frame2D.LeftBeam));
            }
            foreach (HndzPurlin purlin in frame3Dsingle.Purlins)
            {
                tt.Add(CreateBeam(model, purlin));
            }

            List<List<IfcProduct>> temp = new List<List<IfcProduct>>();
            temp.Add(tt);

            AssembleBuilding(model, building, 4000, temp);


            try
            {

                model.SaveAs(savingPath,XbimStorageType.IFC);
                model.SaveAs(savingPath,XbimStorageType.XBIM);
                //model.SaveAs(filePath, XbimStorageType.XBIM);
                CreateWexBimFromIfc(model, savingPath);
                IfcToCoBieLiteUkTest(savingPath
                    +".ifc");

            }
            catch (Exception ex)
            {

            }
        }

        static private XbimModel CreateandInitModel(HndzProject hndzProject)
        {
            //IfcRelAggregates
            //Crashes here.....Fixed in this update :D

            // XbimModel model = XbimModel.CreateModel(hndzProject.Name + "No." + hndzProject.GlobalId + ".xBIM", Xbim.XbimExtensions.XbimDBAccess.ReadWrite); //create an empty model
            XbimModel model = XbimModel.CreateTemporaryModel(); //to fix ASP.Net fatal error

            if (model != null)
            {

                //Begin a transaction as all changes to a model are transacted
                using (XbimReadWriteTransaction txn = model.BeginTransaction("Initialize Model"))
                {
                    //do once only initialization of model application and editor values
                    model.DefaultOwningUser.ThePerson.GivenName = hndzProject.Owner.Name;
                    model.DefaultOwningUser.ThePerson.FamilyName = hndzProject.Owner.LastName;
                    model.DefaultOwningUser.TheOrganization.Name = hndzProject.Owner.Organization;
                    model.DefaultOwningApplication.ApplicationIdentifier = "A_Shawky";/*IFCFileResources.ApplicationIdentifier;*/
                    model.DefaultOwningApplication.ApplicationDeveloper.Name = "A_Shawky";/*IFCFileResources.ApplicationDevelopers;*/
                    model.DefaultOwningApplication.ApplicationFullName = "A_Shawky";/*IFCFileResources.ApplicationFullName;*/
                    model.DefaultOwningApplication.Version = "A_Shawky";/*IFCFileResources.CurrentApplicationVersion;*/

                    //set up a project and initialize the defaults

                    var project = model.Instances.New<IfcProject>();
                    project.Initialize(ProjectUnits.SIUnitsUK);
                    project.Name = hndzProject.Name;
                    project.OwnerHistory.OwningUser = model.DefaultOwningUser;
                    project.OwnerHistory.OwningApplication = model.DefaultOwningApplication;

                    //validate and commit changes
                    if (model.Validate(txn.Modified(), Console.Out) == 0)
                    {
                        txn.Commit();
                        return model;
                    }
                }
            }
            return null; //failed so return nothing

        }

        static private IfcBuilding CreateBuilding(XbimModel model, HndzBuilding hndzBuilding)
        {
            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create Building"))
            {
                var building = model.Instances.New<IfcBuilding>();
                building.Name = hndzBuilding.Name;
                building.OwnerHistory.OwningUser = model.DefaultOwningUser;
                building.OwnerHistory.OwningApplication = model.DefaultOwningApplication;
                building.ElevationOfRefHeight = hndzBuilding.RefHeight;
                building.CompositionType = IfcElementCompositionEnum.ELEMENT;

                building.ObjectPlacement = model.Instances.New<IfcLocalPlacement>();
                var localPlacement = building.ObjectPlacement as IfcLocalPlacement;

                if (localPlacement != null && localPlacement.RelativePlacement == null)
                {

                    localPlacement.RelativePlacement = model.Instances.New<IfcAxis2Placement3D>();
                    var placement = localPlacement.RelativePlacement as IfcAxis2Placement3D;
                    placement.SetNewLocation(0.0, 0.0, 0.0);
                }

                model.IfcProject.AddBuilding(building);
                //validate and commit changes
                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return building;
                }

            }
            return null;
        }


        private static void AssembleBuilding(XbimModel model, IfcBuilding building, float levelHeight, List<List<IfcProduct>> productsLists)
        {
            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create Storey"))
            {
                int floorNumber = 0;
                if (productsLists != null)
                {


                    foreach (List<IfcProduct> ProductsStorey in productsLists)
                    {
                        var storey = model.Instances.New<IfcBuildingStorey>();
                        storey.Name = "Level " + ++floorNumber;
                        storey.OwnerHistory.OwningUser = model.DefaultOwningUser;
                        storey.OwnerHistory.OwningApplication = model.DefaultOwningApplication;
                        storey.Elevation = levelHeight;
                        storey.CompositionType = IfcElementCompositionEnum.ELEMENT;
                        //storey.GlobalId = new Xbim.Ifc2x3.UtilityResource.IfcGloballyUniqueId();

                        storey.ObjectPlacement = model.Instances.New<IfcLocalPlacement>();
                        var localPlacement = storey.ObjectPlacement as IfcLocalPlacement;

                        if (localPlacement != null && localPlacement.RelativePlacement == null)
                        {

                            localPlacement.RelativePlacement = model.Instances.New<IfcAxis2Placement3D>();
                            var placement = localPlacement.RelativePlacement as IfcAxis2Placement3D;
                            placement.SetNewLocation(0.0, 0.0, floorNumber * levelHeight);
                        }


                        IfcRelContainedInSpatialStructure RelationSpatial = model.Instances.New<IfcRelContainedInSpatialStructure>();


                        foreach (IfcProduct item in ProductsStorey)
                        {
                            //storey.AddElement(item);
                            RelationSpatial.RelatedElements.Add(item);
                            ///=============================
                            //IfcRelContainedInSpatialStructure RelationSpatial = model.Instances.New<IfcRelContainedInSpatialStructure>();
                            //RelationSpatial.RelatedElements.Add(item);
                            //RelationSpatial.RelatingStructure = storey;


                        }

                        RelationSpatial.RelatingStructure = storey;

                        building.AddToSpatialDecomposition(storey);
                    }
                }

                //validate and commit changes
                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                }
                else
                {
                    using (StreamWriter str = new StreamWriter("E:\\storey " + floorNumber + " Errors"))
                    {
                        model.Validate(txn.Modified(), str);
                    }
                }

            }
        }


        private static void AdjustExtrusion(XbimModel model, IfcExtrudedAreaSolid body, HndzExtrudedElement genericProducthndz,
                                            IfcProduct genericProductIfc)
        {
            IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
            axisOrigin.SetXYZ(0, 0, 0);

            IfcCartesianPoint elementStartPoint = model.Instances.New<IfcCartesianPoint>();
            elementStartPoint.SetXYZ(genericProducthndz.ExtrusionLine.baseNode.Point.X,
                genericProducthndz.ExtrusionLine.baseNode.Point.Y, genericProducthndz.ExtrusionLine.baseNode.Point.Z); //insert at arbitrary position//****************Need Revision

            body.Depth = genericProducthndz.ExtrusionLine.RhinoLine.Length;
            body.ExtrudedDirection = model.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);

            //parameters to insert the geometry in the model

            body.Position = model.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = axisOrigin;

            //body.Position.RefDirection = model.Instances.New<IfcDirection>();
            //body.Position.RefDirection.SetXYZ(1, 0, 0);

            //Create a Definition shape to hold the geometry
            IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
            shape.ContextOfItems = model.IfcProject.ModelContext();
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);

            //Create a Product Definition and add the model geometry to the wall
            IfcProductDefinitionShape rep = model.Instances.New<IfcProductDefinitionShape>();
            rep.Representations.Add(shape);
            genericProductIfc.Representation = rep;

            //now place the wall into the model
            #region ProfileVectorDir.

            //Vector3d perpendicularVector = new Vector3d(genericProducthndz.Profile.OrientationInPlane.X, genericProducthndz.Profile.OrientationInPlane.Y, 0);
            //Plane extrusionPlane;

            //bool aa = genericProducthndz.ExtrusionLine.RhinoLine.TryGetPlane(out extrusionPlane);
            //if (aa)
            //{ perpendicularVector = extrusionPlane.ZAxis; }

            // Vector3d elementDirection = genericProducthndz.ExtrusionLine.RhinoLine.Direction;
            //Plane profilePlane = new Plane(genericProducthndz.ExtrusionLine.baseNode.Point, elementDirection);
            // Vector3d profileXdirection = profilePlane.XAxis;
            Vector3d xDir = genericProducthndz.ExtrusionLine.RhinoLine.UnitTangent;
            Vector3d yDir = xDir;
            Vector3d zDir = xDir;
            Vector3d orthoToExtrusion = Vector3d.YAxis;
            if (xDir.X == 0)
            {
                orthoToExtrusion = Vector3d.XAxis;
            }
            if (xDir.Z == 0)
            {
                orthoToExtrusion = Vector3d.ZAxis;
            }
            bool isRorated = yDir.Rotate(Math.PI / 2, Vector3d.XAxis);
            if (!isRorated)
            {

            }
            isRorated = zDir.Rotate(Math.PI / 2, Vector3d.ZAxis);
            if (!isRorated)
            {

            }
            #endregion

            IfcLocalPlacement lp = model.Instances.New<IfcLocalPlacement>();
            IfcAxis2Placement3D ax3D = model.Instances.New<IfcAxis2Placement3D>();
            ax3D.Location = elementStartPoint;
            ax3D.RefDirection = model.Instances.New<IfcDirection>();
            //ax3D.RefDirection.SetXYZ(perpendicularVector.X, perpendicularVector.Y, perpendicularVector.Z);  //Y-Axis
            ax3D.RefDirection.SetXYZ(orthoToExtrusion.X, orthoToExtrusion.Y, orthoToExtrusion.Z);  //Y-Axis
            //ax3D.RefDirection.SetXYZ(0, 1, 0);  //Y-Axis
            ax3D.Axis = model.Instances.New<IfcDirection>();
            ax3D.Axis.SetXYZ(genericProducthndz.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Z);          //Z-Axis
            ax3D.Axis.SetXYZ(xDir.X, xDir.Y, xDir.Z);          //Z-Axis


            //XbimVector3D X_Dir = new XbimVector3D(extrusionPlane.XAxis.X, extrusionPlane.XAxis.Y, extrusionPlane.XAxis.Z);
            //XbimVector3D Y_Dir = new XbimVector3D(extrusionPlane.YAxis.X, extrusionPlane.YAxis.Y, extrusionPlane.YAxis.Z);
            //XbimVector3D Z_Dir = new XbimVector3D(extrusionPlane.ZAxis.X, extrusionPlane.ZAxis.Y, extrusionPlane.ZAxis.Z);
            //ax3D.P.Insert(0,X_Dir);
            //ax3D.P.Insert(1,Y_Dir);
            //ax3D.P.Insert(2,Z_Dir);

            lp.RelativePlacement = ax3D;
            genericProductIfc.ObjectPlacement = lp;

            //Where Clause: The IfcWallStandard relies on the provision of an IfcMaterialLayerSetUsage
            IfcMaterialLayerSetUsage ifcMaterialLayerSetUsage = model.Instances.New<IfcMaterialLayerSetUsage>();
            IfcMaterialLayerSet ifcMaterialLayerSet = model.Instances.New<IfcMaterialLayerSet>();
            IfcMaterialLayer ifcMaterialLayer = model.Instances.New<IfcMaterialLayer>();
            ifcMaterialLayer.LayerThickness = 10;
            ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
            ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
            ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
            ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.NEGATIVE;
            ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 150;

            // Add material to wall
            IfcMaterial material = model.Instances.New<IfcMaterial>();
            material.Name = "STEEL";
            IfcRelAssociatesMaterial ifcRelAssociatesMaterial = model.Instances.New<IfcRelAssociatesMaterial>();
            ifcRelAssociatesMaterial.RelatingMaterial = material;
            ifcRelAssociatesMaterial.RelatedObjects.Add(genericProductIfc);

            ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

            // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
            IfcPresentationLayerAssignment ifcPresentationLayerAssignment = model.Instances.New<IfcPresentationLayerAssignment>();
            ifcPresentationLayerAssignment.Name = "HANDZteel Assignment";
            ifcPresentationLayerAssignment.AssignedItems.Add(shape);


            // linear segment as IfcPolyline with two points is required for IfcWall
            IfcPolyline ifcPolyline = model.Instances.New<IfcPolyline>();
            IfcCartesianPoint startPoint = model.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(genericProducthndz.ExtrusionLine.baseNode.Point.X, genericProducthndz.ExtrusionLine.baseNode.Point.Y);
            IfcCartesianPoint endPoint = model.Instances.New<IfcCartesianPoint>();
            endPoint.SetXY(genericProducthndz.ExtrusionLine.EndNode.Point.X, genericProducthndz.ExtrusionLine.EndNode.Point.Y);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);

            IfcShapeRepresentation shape2D = model.Instances.New<IfcShapeRepresentation>();
            shape2D.ContextOfItems = model.IfcProject.ModelContext();
            shape2D.RepresentationIdentifier = "Axis";
            shape2D.RepresentationType = "Curve2D";
            shape2D.Items.Add(ifcPolyline);
            rep.Representations.Add(shape2D);
        }

        private static IfcProduct CreateTaperedColumn(XbimModel model, HndzStructuralElement genericProducthndz)
        {
            //HndzISectionProfile startProfile = new HndzISectionProfile(new SectionI("start", 2000, 1000, 200, 100), new Vector2d(0, 1));
            //HndzISectionProfile endProfile = new HndzISectionProfile(new SectionI("end", 2000 * 2, 1000 * 2, 200 * 2, 100 * 2), new Vector2d(0, 1));
            //genericProducthndz.Profile = new HndzITaperedProfile(startProfile, endProfile);

            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {

                IfcColumn genericProductIfc = model.Instances.New<IfcColumn>();
                IfcColumnType elementType = model.Instances.New<IfcColumnType>();
                elementType.Name = "Ay7aga";
                elementType.PredefinedType = IfcColumnTypeEnum.COLUMN;


                IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
                axisOrigin.SetXYZ(0, 0, 0);


                /////////////////////////////////////new/////////////////////

                if (genericProducthndz.Profile is HndzITaperedProfile)
                {
                    HndzITaperedProfile genericProfilehndz = genericProducthndz.Profile as HndzITaperedProfile;
                    AdjustShapeFaces(model, genericProducthndz, genericProductIfc, genericProfilehndz);
                }

                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                return null;
            }
        }

        private static void AdjustShapeFaces(XbimModel model, HndzStructuralElement genericProducthndz, IfcProduct genericProductIfc, HndzITaperedProfile genericProfilehndz)
        {
            IfcCartesianPoint elementStartPoint = model.Instances.New<IfcCartesianPoint>();
            elementStartPoint.SetXYZ(genericProducthndz.ExtrusionLine.baseNode.Point.X,
                genericProducthndz.ExtrusionLine.baseNode.Point.Y, genericProducthndz.ExtrusionLine.baseNode.Point.Z); //insert at arbitrary position//****************Need Revision

            IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
            axisOrigin.SetXYZ(0, 0, 0);


            Point3d[][] facesEdgePoints = GetEgdePoints(genericProfilehndz, genericProducthndz.ExtrusionLine);

            IfcClosedShell shell = model.Instances.New<IfcClosedShell>();
            foreach (Point3d[] facePoints in facesEdgePoints)
            {
                IfcFace face = model.Instances.New<IfcFace>();
                IfcFaceOuterBound faceBoundary = model.Instances.New<IfcFaceOuterBound>();
                IfcPolyLoop loop = model.Instances.New<IfcPolyLoop>();

                foreach (Point3d point in facePoints)
                {
                    IfcCartesianPoint edgePoint = model.Instances.New<IfcCartesianPoint>();
                    edgePoint.SetXYZ(point.X, point.Y, point.Z);
                    loop.Polygon.Add(edgePoint);
                }
                faceBoundary.Bound = loop;
                faceBoundary.Orientation = new IfcBoolean(true);
                face.Bounds.Add(faceBoundary);
                shell.CfsFaces.Add(face);
            }

            IfcFacetedBrep facesBrep = model.Instances.New<IfcFacetedBrep>();
            facesBrep.Outer = shell;

            //Create a Definition shape to hold the geometry
            IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
            shape.ContextOfItems = model.IfcProject.ModelContext();
            shape.RepresentationType = "Brep";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(facesBrep);

            //now place the wall into the model
            #region ProfileVectorDir.


            Vector3d xDir = genericProducthndz.ExtrusionLine.RhinoLine.UnitTangent;
            Vector3d yDir = xDir;
            Vector3d zDir = xDir;
            Vector3d orthoToExtrusion = Vector3d.YAxis;
            if (xDir.X == 0)
            {
                orthoToExtrusion = Vector3d.XAxis;
            }
            if (xDir.Z == 0)
            {
                orthoToExtrusion = Vector3d.ZAxis;
            }

            #endregion
            IfcAxis2Placement3D naturalPlacement = model.Instances.New<IfcAxis2Placement3D>();
            naturalPlacement.Location = axisOrigin;
            naturalPlacement.RefDirection = model.Instances.New<IfcDirection>();
            //ax3D.RefDirection.SetXYZ(perpendicularVector.X, perpendicularVector.Y, perpendicularVector.Z);  //Y-Axis
            naturalPlacement.RefDirection.SetXYZ(0, 1, 0);  //Y-Axis
                                                            //ax3D.RefDirection.SetXYZ(0, 1, 0);  //Y-Axis
            naturalPlacement.Axis = model.Instances.New<IfcDirection>();
            naturalPlacement.Axis.SetXYZ(genericProducthndz.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Z);          //Z-Axis
            naturalPlacement.Axis.SetXYZ(0, 0, 1);




            IfcLocalPlacement lp = model.Instances.New<IfcLocalPlacement>();
            IfcAxis2Placement3D ax3D = model.Instances.New<IfcAxis2Placement3D>();
            ax3D.Location = elementStartPoint;
            ax3D.RefDirection = model.Instances.New<IfcDirection>();
            //ax3D.RefDirection.SetXYZ(perpendicularVector.X, perpendicularVector.Y, perpendicularVector.Z);  //Y-Axis
            ax3D.RefDirection.SetXYZ(orthoToExtrusion.X, orthoToExtrusion.Y, orthoToExtrusion.Z);  //Y-Axis
                                                                                                   //ax3D.RefDirection.SetXYZ(0, 1, 0);  //Y-Axis
            ax3D.Axis = model.Instances.New<IfcDirection>();
            ax3D.Axis.SetXYZ(genericProducthndz.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Z);          //Z-Axis
            ax3D.Axis.SetXYZ(xDir.X, xDir.Y, xDir.Z);          //Z-Axis

            //repMap.MappingOrigin = ax3D;

            lp.RelativePlacement = ax3D;
            genericProductIfc.ObjectPlacement = lp;



            IfcCartesianTransformationOperator3D trans = model.Instances.New<IfcCartesianTransformationOperator3D>();
            trans.LocalOrigin = axisOrigin;

            IfcRepresentationMap repMap = model.Instances.New<IfcRepresentationMap>();
            repMap.MappedRepresentation = shape;
            repMap.MappingOrigin = naturalPlacement;

            IfcMappedItem mappedItem = model.Instances.New<IfcMappedItem>();
            mappedItem.MappingSource = repMap;
            mappedItem.MappingTarget = trans;


            IfcShapeRepresentation shapeRep = model.Instances.New<IfcShapeRepresentation>();
            shapeRep.ContextOfItems = model.IfcProject.ModelContext();
            shapeRep.RepresentationType = "MappedRepresentation";
            shapeRep.RepresentationIdentifier = "Body";
            shapeRep.Items.Add(mappedItem);

            IfcPresentationLayerAssignment repAssignment = model.Instances.New<IfcPresentationLayerAssignment>();
            repAssignment.AssignedItems.Add(shape);
            repAssignment.AssignedItems.Add(shapeRep);
            repAssignment.Name = "yaMosahl";

            IfcProductDefinitionShape prodShape = model.Instances.New<IfcProductDefinitionShape>();
            prodShape.Representations.Add(shapeRep);
            genericProductIfc.Representation = prodShape;


            //Where Clause: The Ifc relies on the provision of an IfcMaterialLayerSetUsage
            IfcMaterialLayerSetUsage ifcMaterialLayerSetUsage = model.Instances.New<IfcMaterialLayerSetUsage>();
            IfcMaterialLayerSet ifcMaterialLayerSet = model.Instances.New<IfcMaterialLayerSet>();
            IfcMaterialLayer ifcMaterialLayer = model.Instances.New<IfcMaterialLayer>();
            ifcMaterialLayer.LayerThickness = 10;
            ifcMaterialLayerSet.MaterialLayers.Add(ifcMaterialLayer);
            ifcMaterialLayerSetUsage.ForLayerSet = ifcMaterialLayerSet;
            ifcMaterialLayerSetUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
            ifcMaterialLayerSetUsage.DirectionSense = IfcDirectionSenseEnum.NEGATIVE;
            ifcMaterialLayerSetUsage.OffsetFromReferenceLine = 150;

            // Add material to wall
            IfcMaterial material = model.Instances.New<IfcMaterial>();
            material.Name = "STEEL";
            IfcRelAssociatesMaterial ifcRelAssociatesMaterial = model.Instances.New<IfcRelAssociatesMaterial>();
            ifcRelAssociatesMaterial.RelatingMaterial = material;
            ifcRelAssociatesMaterial.RelatedObjects.Add(genericProductIfc);

            ifcRelAssociatesMaterial.RelatingMaterial = ifcMaterialLayerSetUsage;

            // IfcPresentationLayerAssignment is required for CAD presentation in IfcWall or IfcWallStandardCase
            IfcPresentationLayerAssignment ifcPresentationLayerAssignment = model.Instances.New<IfcPresentationLayerAssignment>();
            ifcPresentationLayerAssignment.Name = "HANDZteel Assignment";
            ifcPresentationLayerAssignment.AssignedItems.Add(shape);


            // linear segment as IfcPolyline with two points is required for IfcWall
            IfcPolyline ifcPolyline = model.Instances.New<IfcPolyline>();
            IfcCartesianPoint startPoint = model.Instances.New<IfcCartesianPoint>();
            startPoint.SetXY(genericProducthndz.ExtrusionLine.baseNode.Point.X, genericProducthndz.ExtrusionLine.baseNode.Point.Y);
            IfcCartesianPoint endPoint = model.Instances.New<IfcCartesianPoint>();
            endPoint.SetXY(genericProducthndz.ExtrusionLine.EndNode.Point.X, genericProducthndz.ExtrusionLine.EndNode.Point.Y);
            ifcPolyline.Points.Add(startPoint);
            ifcPolyline.Points.Add(endPoint);

            IfcShapeRepresentation shape2D = model.Instances.New<IfcShapeRepresentation>();
            shape2D.ContextOfItems = model.IfcProject.ModelContext();
            shape2D.RepresentationIdentifier = "Axis";
            shape2D.RepresentationType = "Curve2D";
            shape2D.Items.Add(ifcPolyline);
            prodShape.Representations.Add(shape2D);
        }

        private static IfcProduct CreateTaperedBeam(XbimModel model, HndzStructuralElement genericProducthndz)
        {
            //HndzISectionProfile startProfile = new HndzISectionProfile(new SectionI("start", 2000 * 2, 1000 * 2, 200 * 2, 100 * 2), new Vector2d(0, 1));
            //HndzISectionProfile endProfile = new HndzISectionProfile(new SectionI("end", 2000, 1000, 200, 100), new Vector2d(0, 1));
            //genericProducthndz.Profile = new HndzITaperedProfile(startProfile, endProfile);

            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {

                IfcBeam genericProductIfc = model.Instances.New<IfcBeam>();
                IfcBeamType elementType = model.Instances.New<IfcBeamType>();
                elementType.Name = "Ay7aga";
                elementType.PredefinedType = IfcBeamTypeEnum.BEAM;


                IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
                axisOrigin.SetXYZ(0, 0, 0);

                IfcCartesianPoint elementStartPoint = model.Instances.New<IfcCartesianPoint>();
                elementStartPoint.SetXYZ(genericProducthndz.ExtrusionLine.baseNode.Point.X,
                    genericProducthndz.ExtrusionLine.baseNode.Point.Y, genericProducthndz.ExtrusionLine.baseNode.Point.Z); //insert at arbitrary position//****************Need Revision


                /////////////////////////////////////new/////////////////////

                if (genericProducthndz.Profile is HndzITaperedProfile)
                {
                    HndzITaperedProfile genericProfilehndz = genericProducthndz.Profile as HndzITaperedProfile;

                    AdjustShapeFaces(model, genericProducthndz, genericProductIfc, genericProfilehndz);
                }

                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                return null;
            }
        }

        private static Point3d[][] GetEgdePoints(HndzITaperedProfile genericProfilehndz, HndzLine extrusionLine)
        {
            SectionI startSection = genericProfilehndz.StartProfile.I_Section;
            Vector2d startSectionOrientation = genericProfilehndz.StartProfile.OrientationInPlane;

            Vector2d endSectionOrientation = genericProfilehndz.EndProfile.OrientationInPlane;

            if (startSectionOrientation == default(Vector2d) || endSectionOrientation == default(Vector2d))
            {
                startSectionOrientation = endSectionOrientation = genericProfilehndz.OrientationInPlane;
            }
            else if (startSectionOrientation == null)
            {
                startSectionOrientation = genericProfilehndz.OrientationInPlane;
            }

            else if (endSectionOrientation == null)
            {
                endSectionOrientation = genericProfilehndz.OrientationInPlane;
            }

            SectionI endSection = genericProfilehndz.EndProfile.I_Section;
            Point3d[] startSectionPoints = new Point3d[12];
            Point3d[] endSectionPoints = new Point3d[12];

            double h = startSection.d;
            double tw = startSection.t_w;
            double w = startSection.b_f;
            double tf = startSection.tf;

            if (genericProfilehndz.OrientationInPlane == new Vector2d(1, 0))
            {

                startSectionPoints = new Point3d[12]
                   {
                new Point3d(-w/2, -h/2     ,0),
                new Point3d(-w/2, -h/2 + tf,0),
                new Point3d(-tw/2, -h/2 + tf,0),
                new Point3d(-tw/2, h/2 - tf ,0),
                new Point3d(-w/2, h/2 - tf ,0),
                new Point3d(-w/2, h/2      ,0),
                new Point3d(w/2 , h/2      ,0),
                new Point3d(w/2 , h/2 - tf ,0),
                new Point3d(tw/2, h/2 - tf ,0),
                new Point3d(tw/2, -h/2 + tf,0),
                new Point3d(w/2 , -h/2 + tf,0),
                new Point3d(w/2 , -h/2     ,0)
                   };
            }
            else
            {
                startSectionPoints = new Point3d[12]
                   {
                new Point3d(h/2, -w/2        ,0),    //0
                new Point3d(h/2-tf, -w/2     ,0),    //1
                new Point3d(h/2-tf,-tw/2     ,0),     //2
                new Point3d(-h/2+tf,- tw/2   ,0),     //3
                new Point3d(-h/2+tf, -w/2    ,0),    //4
                new Point3d(-h/2, -w/2       ,0),    //5
                new Point3d(-h/2 , w/2       ,0),     //6
                new Point3d(-h/2+tf , w/2    ,0),       //7
                new Point3d(-h/2+tf, tw/2    ,0),       //8
                new Point3d(h/2-tf, tw/2     ,0),       //9
                new Point3d(h/2-tf , w/2     ,0),      //10
                new Point3d(h/2 , w/2        ,0)       //11
                   };
            }
            h = endSection.d;
            tw = endSection.t_w;
            w = endSection.b_f;
            tf = endSection.tf;

            double X = extrusionLine.RhinoLine.ToX - extrusionLine.RhinoLine.ToX;
            double Y = extrusionLine.RhinoLine.ToY - extrusionLine.RhinoLine.ToY;
            double Z = extrusionLine.RhinoLine.ToZ - extrusionLine.RhinoLine.ToZ;
            if (genericProfilehndz.OrientationInPlane == new Vector2d(1, 0))
            {

                ///////Assuming that there isn't any inclination in the line 
                //    endSectionPoints = new Point3d[12]  {
                //    new Point3d(-w/2 +X  , -h/2      +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-w/2 +X  , -h/2 + tf +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-tw/2+X  , -h/2 + tf +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-tw/2+X  , h/2 - tf  +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-w/2 +X  , h/2 - tf  +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-w/2 +X  , h/2       +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(w/2  +X  , h/2       +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(w/2  +X  , h/2 - tf  +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(tw/2 +X  , h/2 - tf  +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(tw/2 +X  , -h/2 + tf +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(w/2  +X  , -h/2 + tf +Y,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(w/2  +X  , -h/2      +Y,extrusionLine.RhinoLine.Length+Z)
                //};
                endSectionPoints = new Point3d[12]  {
                new Point3d(-w/2   , -h/2      ,extrusionLine.RhinoLine.Length),
                new Point3d(-w/2   , -h/2 + tf ,extrusionLine.RhinoLine.Length),
                new Point3d(-tw/2  , -h/2 + tf ,extrusionLine.RhinoLine.Length),
                new Point3d(-tw/2  , h/2 - tf  ,extrusionLine.RhinoLine.Length),
                new Point3d(-w/2   , h/2 - tf  ,extrusionLine.RhinoLine.Length),
                new Point3d(-w/2   , h/2       ,extrusionLine.RhinoLine.Length),
                new Point3d(w/2    , h/2       ,extrusionLine.RhinoLine.Length),
                new Point3d(w/2    , h/2 - tf  ,extrusionLine.RhinoLine.Length),
                new Point3d(tw/2   , h/2 - tf  ,extrusionLine.RhinoLine.Length),
                new Point3d(tw/2   , -h/2 + tf ,extrusionLine.RhinoLine.Length),
                new Point3d(w/2    , -h/2 + tf ,extrusionLine.RhinoLine.Length),
                new Point3d(w/2    , -h/2      ,extrusionLine.RhinoLine.Length)
            };
            }
            else
            {
                //    endSectionPoints = new Point3d[12]  {
                //    new Point3d(h/2    +X , -w/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(h/2-tf +X , -w/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(h/2-tf +X ,-tw/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2+tf+X ,- tw/2+Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2+tf+X , -w/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2   +X , -w/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2   +X , w/2  +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2+tf+X , w/2  +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(-h/2+tf+X , tw/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(h/2-tf +X , tw/2 +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(h/2-tf +X , w/2  +Y    ,extrusionLine.RhinoLine.Length+Z),
                //    new Point3d(h/2    +X , w/2  +Y    ,extrusionLine.RhinoLine.Length+Z)
                //};

                endSectionPoints = new Point3d[12]  {
                new Point3d(h/2     , -w/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(h/2-tf  , -w/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(h/2-tf  ,-tw/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2+tf ,- tw/2    ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2+tf , -w/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2    , -w/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2    , w/2      ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2+tf , w/2      ,extrusionLine.RhinoLine.Length),
                new Point3d(-h/2+tf , tw/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(h/2-tf  , tw/2     ,extrusionLine.RhinoLine.Length),
                new Point3d(h/2-tf  , w/2      ,extrusionLine.RhinoLine.Length),
                new Point3d(h/2     , w/2      ,extrusionLine.RhinoLine.Length)
            };
            }
            Point3d[][] facesEdgePoints = new Point3d[14][];

            facesEdgePoints[0] = new Point3d[startSectionPoints.Length]; //start I face
            for (int i = 0; i < startSectionPoints.Length; i++)
            {
                facesEdgePoints[0][i] = startSectionPoints[i];
            }

            facesEdgePoints[1] = new Point3d[endSectionPoints.Length]; // end I face
            for (int i = 0; i < endSectionPoints.Length; i++)
            {
                facesEdgePoints[1][i] = endSectionPoints[i];
            }

            facesEdgePoints[2] = new Point3d[4] { startSectionPoints[0],endSectionPoints[0],
                                                endSectionPoints[11],startSectionPoints[11]}; // 1st flange rec side face

            for (uint i = 3; i < 14; i++)
            {
                uint edgeID = i - 3;
                facesEdgePoints[i] = new Point3d[4] { startSectionPoints[edgeID],endSectionPoints[edgeID],
                                                endSectionPoints[edgeID+1],startSectionPoints[edgeID+1]}; // 2nd flange rec side face
            }

            return facesEdgePoints;
        }

        private static IfcColumn CreateColumn(XbimModel model, HndzStructuralElement genericProducthndz)
        {

            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {

                IfcColumn genericProductIfc = model.Instances.New<IfcColumn>();
                IfcColumnType elementType = model.Instances.New<IfcColumnType>();
                elementType.PredefinedType = IfcColumnTypeEnum.COLUMN;
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile genericProfilehndz = genericProducthndz.Profile as HndzRectangularProfile;

                    IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzISectionProfile)
                {
                    HndzISectionProfile genericProfilehndz = genericProducthndz.Profile as HndzISectionProfile;

                    IfcIShapeProfileDef ifcGenericProfile = AssignIProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                AdjustExtrusion(model, body, genericProducthndz, genericProductIfc);


                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                return null;
            }
        }
        private static IfcIShapeProfileDef AssignIProfile(XbimModel model, HndzStructuralElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzISectionProfile gnericProfileHndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + "I beam (flange " + gnericProfileHndz.I_Section.b_f + " x " + gnericProfileHndz.I_Section.t_fTop + " and web "
                + gnericProfileHndz.I_Section.d + " x " + gnericProfileHndz.I_Section.t_w + " mm";


            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcLabel columnLabel = new IfcLabel(typeText);
            elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.SetDefiningType(elementType, model);

            #endregion

            IfcIShapeProfileDef ifcGenericProfile = model.Instances.New<IfcIShapeProfileDef>();
            ifcGenericProfile.FlangeThickness = gnericProfileHndz.I_Section.tf;
            ifcGenericProfile.WebThickness = gnericProfileHndz.I_Section.t_w;
            ifcGenericProfile.OverallWidth = gnericProfileHndz.I_Section.b_f;
            ifcGenericProfile.OverallDepth = gnericProfileHndz.I_Section.d;
            ifcGenericProfile.FilletRadius = 10;//ToDo:make it zero after explore ISection

            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.RefDirection.SetXY(gnericProfileHndz.OrientationInPlane.X, gnericProfileHndz.OrientationInPlane.Y);
            ifcGenericProfile.Position.RefDirection.SetXY(1, 0);
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            //ifcGenericProfile.Position.Location.SetXY(genericProducthndz.ExtrusionLine.baseNode.Point.X,
            //     genericProducthndz.ExtrusionLine.baseNode.Point.Y);
            return ifcGenericProfile;
        }

        private static IfcRectangleProfileDef AssignRectangularProfile(XbimModel model, HndzExtrudedElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzRectangularProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + (int)genericProfilehndz.Rectangle.Width + " x "
                              + (int)genericProfilehndz.Rectangle.Height + " mm";

            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcLabel columnLabel = new IfcLabel(typeText);
            elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.SetDefiningType(elementType, model);

            #endregion

            //represent column as a rectangular profile
            IfcRectangleProfileDef ifcGenericProfile = model.Instances.New<IfcRectangleProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.XDim = genericProfilehndz.Rectangle.Height;
            ifcGenericProfile.YDim = genericProfilehndz.Rectangle.Width;

            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.RefDirection.SetXY(genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            return ifcGenericProfile;
        }

        private static IfcCShapeProfileDef AssignCsectionProfile(XbimModel model, HndzStructuralElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzCSectionProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + " C-Chanel (flange " + genericProfilehndz.C_Section.b_f + " x " + genericProfilehndz.C_Section.t_f + " and web "
                + genericProfilehndz.C_Section.d + " x " + genericProfilehndz.C_Section.t_w + " mm";


            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcLabel columnLabel = new IfcLabel(typeText);
            elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;


            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.SetDefiningType(elementType, model);

            #endregion

            IfcCShapeProfileDef ifcGenericProfile = model.Instances.New<IfcCShapeProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.WallThickness = genericProfilehndz.C_Section.t_f;
            //MyColumnPofile.WebThickness = Iprofile.C_Section.t_w; //ToDo:purlin web and flange thickness are the same!!!!
            ifcGenericProfile.Width = genericProfilehndz.C_Section.b_f;
            ifcGenericProfile.Depth = genericProfilehndz.C_Section.d;
            ifcGenericProfile.Girth = 5;//ToDo:What's that
            ifcGenericProfile.InternalFilletRadius = 10;

            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.RefDirection.SetXY(genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            //ifcGenericProfile.Position.RefDirection.SetXY(1,0);
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            return ifcGenericProfile;
        }

        private static IfcBeam CreateBeam(XbimModel model, HndzStructuralElement genericProducthndz)
        {
            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcBeam genericProductIfc = model.Instances.New<IfcBeam>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                IfcBeamType elementType = model.Instances.New<IfcBeamType>();
                elementType.PredefinedType = IfcBeamTypeEnum.BEAM;


                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile genericProfilehndz = genericProducthndz.Profile as HndzRectangularProfile;

                    IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }

                if (genericProducthndz.Profile is HndzISectionProfile)
                {
                    HndzISectionProfile genericProfilehndz = genericProducthndz.Profile as HndzISectionProfile;

                    IfcIShapeProfileDef ifcGenericProfile = AssignIProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzCSectionProfile)
                {
                    HndzCSectionProfile genericProfilehndz = genericProducthndz.Profile as HndzCSectionProfile;
                    IfcCShapeProfileDef ifcGenericProfile = AssignCsectionProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }

                AdjustExtrusion(model, body, genericProducthndz, genericProductIfc);

                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return genericProductIfc;
                }

                return null;
            }
        }


        private static IfcBeam CreatePurlin(XbimModel model, HndzPurlin genericProducthndz)
        {
            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create Purlin"))
            {
                IfcBeam beam = model.Instances.New<IfcBeam>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = genericProducthndz.ExtrusionLine.RhinoLine.Length;

                IfcBeamType elementType = model.Instances.New<IfcBeamType>();
                elementType.PredefinedType = IfcBeamTypeEnum.USERDEFINED;

                IfcMaterial material = model.Instances.New<IfcMaterial>();
                material.Name = "STEEL";
                beam.SetMaterial(material);
                IfcCartesianPoint insertPoint = model.Instances.New<IfcCartesianPoint>();

                insertPoint.SetXY(genericProducthndz.ExtrusionLine.baseNode.Point.X, genericProducthndz.ExtrusionLine.baseNode.Point.Y); //insert at arbitrary position//****************Need Revision

                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile recProfile = genericProducthndz.Profile as HndzRectangularProfile;

                    #region Type & Material &Tags

                    string typeText = "HANDAZ-Column-Rectangular " + (int)recProfile.Rectangle.Width + " x "
                                      + (int)recProfile.Rectangle.Height + " mm";

                    elementType.Tag = typeText;
                    elementType.Name = typeText;
                    IfcLabel columnLabel = new IfcLabel(typeText);
                    elementType.ElementType = columnLabel;
                    elementType.ApplicableOccurrence = columnLabel;



                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.SetDefiningType(elementType, model);

                    #endregion

                    //represent column as a rectangular profile
                    IfcRectangleProfileDef MyPurlinPofile = model.Instances.New<IfcRectangleProfileDef>();
                    MyPurlinPofile.ProfileType = IfcProfileTypeEnum.AREA;
                    MyPurlinPofile.XDim = recProfile.Rectangle.Height;
                    MyPurlinPofile.YDim = recProfile.Rectangle.Width;
                    MyPurlinPofile.Position = model.Instances.New<IfcAxis2Placement2D>();
                    MyPurlinPofile.Position.RefDirection = model.Instances.New<IfcDirection>();
                    MyPurlinPofile.Position.RefDirection.SetXY(recProfile.OrientationInPlane.X, recProfile.OrientationInPlane.Y);
                    //MyColumnPofile.Position.Location = insertPoint;

                    //model as a swept area solid
                    body.SweptArea = MyPurlinPofile;
                }
                if (genericProducthndz.Profile is HndzISectionProfile)
                {
                    HndzISectionProfile Iprofile = genericProducthndz.Profile as HndzISectionProfile;

                    #region Type & Material &Tags

                    string typeText = "HANDAZ-Purlin-I beam (flange " + Iprofile.I_Section.b_f + " x " + Iprofile.I_Section.t_fTop + " and web "
                        + Iprofile.I_Section.d + " x " + Iprofile.I_Section.t_w + " mm";


                    elementType.Tag = typeText;
                    elementType.Name = typeText;
                    IfcLabel columnLabel = new IfcLabel(typeText);
                    elementType.ElementType = columnLabel;
                    elementType.ApplicableOccurrence = columnLabel;



                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.SetDefiningType(elementType, model);

                    #endregion
                    // IfcPolyline pl = model.Instances.New<IfcPolyline>();

                    //List<Point3d> tempPoints= Iprofile.ConvertItoPoints();
                    // foreach (Point3d point in tempPoints)
                    // {
                    //     IfcCartesianPoint tempPoint = model.Instances.New<IfcCartesianPoint>();
                    //     tempPoint.SetXYZ(point.X, point.Y, point.Z);
                    //     pl.Points.Add(tempPoint);
                    // }

                    IfcIShapeProfileDef MyPurlinPofile = model.Instances.New<IfcIShapeProfileDef>();
                    MyPurlinPofile.FlangeThickness = Iprofile.I_Section.tf;
                    MyPurlinPofile.WebThickness = Iprofile.I_Section.t_w;
                    MyPurlinPofile.OverallWidth = Iprofile.I_Section.b_f;
                    MyPurlinPofile.OverallDepth = Iprofile.I_Section.d;
                    MyPurlinPofile.FilletRadius = 10;

                    MyPurlinPofile.Position = model.Instances.New<IfcAxis2Placement2D>();
                    MyPurlinPofile.Position.RefDirection = model.Instances.New<IfcDirection>();
                    MyPurlinPofile.Position.RefDirection.SetXY(Iprofile.OrientationInPlane.X, Iprofile.OrientationInPlane.Y);
                    //MyColumnPofile.Position.Location = insertPoint;

                    //model as a swept area solid
                    body.SweptArea = MyPurlinPofile;
                }
                if (genericProducthndz.Profile is HndzCSectionProfile)
                {
                    HndzCSectionProfile Cprofile = genericProducthndz.Profile as HndzCSectionProfile;

                    #region Type & Material &Tags

                    string typeText = "HANDAZ-Purlin-C-Chanel (flange " + Cprofile.C_Section.b_f + " x " + Cprofile.C_Section.t_f + " and web "
                        + Cprofile.C_Section.d + " x " + Cprofile.C_Section.t_w + " mm";


                    elementType.Tag = typeText;
                    elementType.Name = typeText;
                    IfcLabel columnLabel = new IfcLabel(typeText);
                    elementType.ElementType = columnLabel;
                    elementType.ApplicableOccurrence = columnLabel;


                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.SetDefiningType(elementType, model);

                    #endregion

                    IfcCShapeProfileDef MyPurlinPofile = model.Instances.New<IfcCShapeProfileDef>();
                    MyPurlinPofile.WallThickness = Cprofile.C_Section.t_f;
                    //MyColumnPofile.WebThickness = Iprofile.C_Section.t_w; //ToDo:purlin web and flange thickness are the same!!!!
                    MyPurlinPofile.Width = Cprofile.C_Section.b_f;
                    MyPurlinPofile.Depth = Cprofile.C_Section.d;
                    MyPurlinPofile.InternalFilletRadius = 10;

                    MyPurlinPofile.Position = model.Instances.New<IfcAxis2Placement2D>();
                    MyPurlinPofile.Position.RefDirection = model.Instances.New<IfcDirection>();
                    MyPurlinPofile.Position.RefDirection.SetXY(Cprofile.OrientationInPlane.X, Cprofile.OrientationInPlane.Y);


                    //model as a swept area solid
                    body.SweptArea = MyPurlinPofile;
                }
                body.ExtrudedDirection = model.Instances.New<IfcDirection>();
                body.ExtrudedDirection.SetXYZ(0, 0, 1);
                //body.ExtrudedDirection.SetXYZ(Hndzcol.ExtrusionLine.RhinoLine.Direction.X, Hndzcol.ExtrusionLine.RhinoLine.Direction.Y, Hndzcol.ExtrusionLine.RhinoLine.Direction.Z);


                //parameters to insert the geometry in the model
                IfcCartesianPoint origin = model.Instances.New<IfcCartesianPoint>();
                origin.SetXYZ(genericProducthndz.ExtrusionLine.baseNode.Point.X, genericProducthndz.ExtrusionLine.baseNode.Point.Y, genericProducthndz.ExtrusionLine.baseNode.Point.Z);

                body.Position = model.Instances.New<IfcAxis2Placement3D>();
                //body.Position.Location = origin;

                body.Position.RefDirection = model.Instances.New<IfcDirection>();
                body.Position.RefDirection.SetXYZ(1, 0, 0);

                //Create a Definition shape to hold the geometry
                IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
                shape.ContextOfItems = model.IfcProject.ModelContext();
                shape.RepresentationType = "SweptSolid";
                shape.RepresentationIdentifier = "Body";
                shape.Items.Add(body);

                //Create a Product Definition and add the model geometry to the column
                IfcProductDefinitionShape rep = model.Instances.New<IfcProductDefinitionShape>();
                rep.Representations.Add(shape);

                beam.Representation = rep;

                //now place the column into the model
                IfcLocalPlacement lp = model.Instances.New<IfcLocalPlacement>();
                IfcAxis2Placement3D ax3d = model.Instances.New<IfcAxis2Placement3D>();
                ax3d.Location = origin;

                Vector3d perpendicularVector = new Vector3d(genericProducthndz.Profile.OrientationInPlane.X, genericProducthndz.Profile.OrientationInPlane.Y, 0);

                Plane extrusionPlane;
                bool aa = genericProducthndz.ExtrusionLine.RhinoLine.TryGetPlane(out extrusionPlane);
                if (aa)
                { perpendicularVector = extrusionPlane.ZAxis; }



                ax3d.RefDirection = model.Instances.New<IfcDirection>();
                ax3d.RefDirection.SetXYZ(perpendicularVector.X, perpendicularVector.Y, perpendicularVector.Z);
                ax3d.Axis = model.Instances.New<IfcDirection>();
                ax3d.Axis.SetXYZ(genericProducthndz.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Z);

                lp.RelativePlacement = ax3d;
                beam.ObjectPlacement = lp;

                #region Owner Data
                beam.OwnerHistory.OwningUser = model.DefaultOwningUser;
                beam.OwnerHistory.OwningApplication = model.DefaultOwningApplication;
                #endregion

                //validate write any errors to the console and commit if OK, otherwise abort
                string temp = Path.GetTempPath();
                //if (model.Validate(txn.Modified(), File.CreateText("E:\\Column" + column.GlobalId + "Errors.txt")) == 0)
                // if (model.Validate(txn.Modified(), File.CreateText(temp + "Column" + column.GlobalId + "Errors.txt")) == 0)
                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return beam;
                }

                return null;
            }
        }

        private static IfcWallStandardCase CreateWall(XbimModel model, HndzWallStandardCase genericProducthndz)
        {
            using (XbimReadWriteTransaction txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcWallStandardCase genericProductIfc = model.Instances.New<IfcWallStandardCase>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                IfcWallType elementType = model.Instances.New<IfcWallType>();
                elementType.PredefinedType = IfcWallTypeEnum.NOTDEFINED;

                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, genericProducthndz.WallThickness, genericProducthndz.BaseLine.Length);
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(genericProducthndz.BaseLine.Direction.X, genericProducthndz.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(model, genericProducthndz, genericProductIfc, elementType, genericProfilehndz);
                body.SweptArea = ifcGenericProfile;

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                //IfcDoorLiningProperties doorProp = model.Instances.New<IfcDoorLiningProperties>();
                //doorProp.CasingDepth = 20;
                //doorProp.CasingThickness = 20;
                //doorProp.LiningDepth = 30;
                //doorProp.LiningThickness = 10;

                //IfcDoorPanelProperties panelProp = model.Instances.New<IfcDoorPanelProperties>();
                //panelProp.PanelPosition = IfcDoorPanelPositionEnum.LEFT;
                //panelProp.

                AdjustExtrusion(model, body, genericProducthndz, genericProductIfc);

                if (model.Validate(txn.Modified(), Console.Out) == 0)
                {
                    txn.Commit();
                    return genericProductIfc;
                }

                return null;
            }
        }

        private static void CreateWexBimFromIfc(XbimModel model, string ifcFileFullPath)
        {

            var wexBimFileName = Path.ChangeExtension(ifcFileFullPath, "wexbim");
            var xbimFile = Path.ChangeExtension(ifcFileFullPath, "xbim");




            if (model != null)
            {

                var context = new Xbim3DModelContext(model);
                context.CreateContext(geomStorageType: XbimGeometryType.PolyhedronBinary);
                var wexBimFilename = Path.ChangeExtension(ifcFileFullPath, "wexBIM");
                using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
                {
                    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                    {
                        Console.WriteLine("Creating " + wexBimFilename);
                        context.Write(wexBimBinaryWriter);
                        wexBimBinaryWriter.Close();
                    }
                    wexBiMfile.Close();
                }







                //    using (var wexBimFile = new FileStream(wexBimFileName, FileMode.Create))
                //{

                //    using (var binaryWriter = new BinaryWriter(wexBimFile))
                //    {



                //        using (var model = new XbimModel())
                //        {
                //            model.CreateFrom(ifcFileFullPath, xbimFile, null, true, false);

                //            Xbim3DModelContext geomContext = new Xbim3DModelContext(model);

                //            geomContext.CreateContext(XbimGeometryType.PolyhedronBinary);

                //            geomContext.Write(binaryWriter);

                //        }
                //    }
            }
        }


        private static void CreateWexBimFromIfc(string ifcFileFullPath)
        {
            var wexBimFileName = Path.ChangeExtension(ifcFileFullPath, "wexbim");
            var xbimFile = Path.ChangeExtension(ifcFileFullPath, "xbim");

            using (var wexBimFile = new FileStream(wexBimFileName, FileMode.Create))
            {

                using (var binaryWriter = new BinaryWriter(wexBimFile))
                {
                    using (var model = new XbimModel())
                    {

                        model.CreateFrom(ifcFileFullPath, xbimFile, null, true, false);

                        var geomContext = new Xbim3DModelContext(model);
                        // ISGenetrated is false here

                        geomContext.CreateContext(XbimGeometryType.PolyhedronBinary); // Stack OverFlow Exception

                        geomContext.Write(binaryWriter);


                    }

                }

            }


        }


        public static void IfcToCoBieLiteUkTest(string ifcFilePath)
        {
            using (var m = new XbimModel())
            {
                var ifcTestFile = ifcFilePath;
                var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
                var jsonFile = Path.ChangeExtension(ifcTestFile, "json");
                m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
                var facilities = new List<Facility>();

                OutPutFilters rolefilters = new OutPutFilters();
                RoleFilter reqRoles = RoleFilter.Unknown; //RoleFilter.Architectural |  RoleFilter.Mechanical | RoleFilter.Electrical | RoleFilter.FireProtection | RoleFilter.Plumbing;
                rolefilters.ApplyRoleFilters(reqRoles);

                var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities, null, rolefilters);
                facilities = ifcToCoBieLiteUkExchanger.Convert();

                foreach (var facilityType in facilities)
                {
                    facilityType.WriteJson(jsonFile, true);

                }

            }
        }
    }
}
