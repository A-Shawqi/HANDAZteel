using System;
using System.IO;
using Xbim.Ifc4.ProfileResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Xbim.Common;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc;
using Xbim.Common.Step21;
using Xbim.Ifc4.Kernel;
using Xbim.ModelGeometry.Scene;
using HANDAZ.Entities;
using System.Collections.Generic;
using Xbim.Ifc4.MeasureResource;
using Rhino.Geometry;
using Wosad.Common.Section.SectionTypes;
using Xbim.Ifc4.MaterialResource;
using Xbim.Ifc4.TopologyResource;
using Xbim.Ifc4.PresentationOrganizationResource;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.UtilityResource;
using Xbim.Ifc4.ActorResource;
using Xbim.Ifc4.ArchitectureDomain;
using Xbim.IO;

namespace HANDAZ.PEB.BIM
{
    public static class ConvertToIFC
    {
        private static IfcStore model;
        private static IfcOrganization GetDefaultOrganization()
        {
            IfcOrganization org = model.Instances.New<IfcOrganization>();
            org.Description = IFCFileResources.Description;
            org.Identification = IFCFileResources.ApplicationIdentifier;
            org.Name = IFCFileResources.ApplicationFullName;
            return org;
        }
        private static IfcApplication GetDefaultApp()
        {
            IfcApplication app = model.Instances.New<IfcApplication>();
            app.ApplicationDeveloper = GetDefaultOrganization();
            app.ApplicationFullName = IFCFileResources.ApplicationFullName;
            app.ApplicationIdentifier = IFCFileResources.ApplicationIdentifier;
            app.Version = IFCFileResources.CurrentApplicationVersion;
            return app;
        }
        private static IfcOwnerHistory GetDefaultOwnerHistory()
        {
            IfcOwnerHistory history = model.Instances.New<IfcOwnerHistory>();
            history.ChangeAction = IfcChangeActionEnum.ADDED;
            history.CreationDate = new IfcTimeStamp(19011992);
            history.LastModifiedDate = new IfcTimeStamp(DateTime.Now.ToFileTime());
            history.LastModifyingApplication = GetDefaultApp();
            history.LastModifyingUser = GetDefaultPersonInOrg();
            history.OwningApplication = GetDefaultApp();
            history.OwningUser = GetDefaultPersonInOrg();
            history.State = IfcStateEnum.READWRITE;
            return history;
        }
        private static IfcPerson GetDefaultPerson()
        {
            IfcPerson person = model.Instances.New<IfcPerson>();
            person.FamilyName = "Shawky";
            person.GivenName = "Ahmed";
            person.Identification = IFCFileResources.ApplicationIdentifier;
            return person;
        }
        private static IfcPersonAndOrganization GetDefaultPersonInOrg()
        {
            IfcPersonAndOrganization person = model.Instances.New<IfcPersonAndOrganization>();
            person.TheOrganization = GetDefaultOrganization();
            person.ThePerson = GetDefaultPerson();
            return person;
        }

        private const int MeshTolerance = 10;
        //public static void test()
        //{
        //    IfcStore model = CreateandInitModel("Hello Column");
        //    if ( != null)
        //    {
        //        IfcBuilding building = CreateBuilding( "Default Building", 2000);

        //        var col = CreateColumn( 4000, 300, 2400);
        //        if (col != null)
        //        {
        //            model.SaveAs("A.Shawky.ifc");

        //            using (IfcStore ifcStore = IfcStore.Open("ColumnWithArbitraryClosedProfileDef.ifc"))
        //            {
        //                var context = new Xbim3DModelContext(ifcStore);
        //                context.CreateContext();
        //                using (var ms = new FileStream("ColumnWithArbitraryClosedProfileDef.wexBIM", FileMode.Create))
        //                {
        //                    using (var binaryWriter = new BinaryWriter(ms))
        //                    {
        //                        ifcStore.SaveAsWexBim(binaryWriter);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}
        private static void CreateWexBimFromIfc(string ifcFileFullPath)
        {
            var wexBimFileName = Path.ChangeExtension(ifcFileFullPath, "wexbim");
            var xbimFile = Path.ChangeExtension(ifcFileFullPath, "xbim");

            var context = new Xbim3DModelContext(model);
            context.CreateContext();
            var wexBimFilename = Path.ChangeExtension(xbimFile, "wexBIM");
            using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
            {
                using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                {
                    Console.WriteLine("Creating " + wexBimFilename);
                    model.SaveAsWexBim(wexBimBinaryWriter);
                    wexBimBinaryWriter.Close();
                }
                wexBiMfile.Close();
            }

            //if (model != null)
            //{

            //    var context = new Xbim3DModelContext(model);
            //    context.CreateContext();
            //    //context.CreateContext(XbimGeometryType.PolyhedronBinary);
            //    var wexBimFilename = Path.ChangeExtension(ifcFileFullPath, "wexBIM");
            //    using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
            //    {
            //        using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
            //        {
            //            Console.WriteLine("Creating " + wexBimFilename);

            //            //context.Write(wexBimBinaryWriter);
            //            wexBimBinaryWriter.Close();
            //        }
            //        wexBiMfile.Close();
            //    }
            //}
        }
        private static void CreateJsonFromIfc(string ifcFileFullPath)
        {
            //using (var model = new Xbim.IO.IfcStore())
            //{
            //    // model.CreateFrom(ifcFileFullPath);
            //    ifcFileFullPath = Path.ChangeExtension(ifcFileFullPath, "ifc");

            //    model.CreateFrom(ifcFileFullPath, null, null, true, true);
            //    var facilities = new List<Facility>();
            //    var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger( facilities);
            //    facilities = ifcToCoBieLiteUkExchanger.Convert();

            //    foreach (var facilityType in facilities)
            //    {
            //        var jsonFile = Path.ChangeExtension(ifcFileFullPath, ".json");
            //        facilityType.WriteJson(jsonFile, true);
            //        break;
            //    }
            //}
        }
        private static IfcBuilding CreateBuilding(HndzBuilding hndzBuilding)
        {
            using (var txn = model.BeginTransaction("Create Building"))
            {
                var building = model.Instances.New<IfcBuilding>();
                building.Name = hndzBuilding.Name;

                building.OwnerHistory = GetDefaultOwnerHistory();
                //building.OwnerHistory.OwningApplication = model.DefaultOwningApplication;

                building.ElevationOfRefHeight = hndzBuilding.RefHeight;
                building.CompositionType = IfcElementCompositionEnum.ELEMENT;

                building.ObjectPlacement = model.Instances.New<IfcLocalPlacement>();
                var localPlacement = building.ObjectPlacement as IfcLocalPlacement;

                if (localPlacement != null && localPlacement.RelativePlacement == null)
                {

                    localPlacement.RelativePlacement = model.Instances.New<IfcAxis2Placement3D>();
                    var placement = localPlacement.RelativePlacement as IfcAxis2Placement3D;
                    var placementOrigin = model.Instances.New<IfcCartesianPoint>();
                    placementOrigin.SetXYZ(0, 0, 0);
                    placement.Location = placementOrigin;//15/10
                }
                IfcProject thisProj = null;
                foreach (var item in model.Instances)
                {
                    if (item is IfcProject)
                    {
                        thisProj = item as IfcProject;
                    }
                }
                building.CompositionType = IfcElementCompositionEnum.ELEMENT;

                thisProj.AddBuilding(building);
                //model.IfcProject.AddBuilding(building);
                //validate and commit changes
                //if (.Validate(Console.Out) == 0)
                //{
                try
                {
                    txn.Commit();
                    return building;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
                //    }
                //    else txn.Rollback();
                //}
                //return null;
            }
        }


        /// <summary>
        /// Sets up the basic parameters any model must provide, units, ownership etc
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <returns></returns>
        static IfcStore CreateandInitModel(HndzProject hndzProject)
        {
            //XbimExtensions;
            //IModel model = new Xbim.XbimExtensions.XbimMemoryModel(); //create an empty model
            XbimEditorCredentials xmi = new XbimEditorCredentials();
            xmi.ApplicationDevelopersName = IFCFileResources.ApplicationDevelopers;
            xmi.ApplicationFullName = IFCFileResources.ApplicationName;
            xmi.ApplicationIdentifier = IFCFileResources.ApplicationIdentifier;
            xmi.ApplicationVersion = IFCFileResources.CurrentApplicationVersion;
            xmi.EditorsFamilyName = "Shawky";
            xmi.EditorsGivenName = "Ahmed";
            xmi.EditorsOrganisationName = IFCFileResources.ApplicationFullName;

            model = IfcStore.Create(xmi, XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel);

            //model = IfcStore.Create(xmi, IfcSchemaVersion.Ifc2X3, XbimStoreType.InMemoryModel);


            //Begin a transaction as all changes to a model are transacted
            using (var txn = model.BeginTransaction("Initialize Model"))
            {
                //set up a project and initialize the defaults

                IfcProject project = model.Instances.New<IfcProject>();
                project.Initialize(ProjectUnits.SIUnitsUK);
                project.Name = hndzProject.Name;
                project.OwnerHistory = GetDefaultOwnerHistory();
                project.Phase = "phase 1";
                project.UnitsInContext = model.Instances.New<IfcUnitAssignment>();
                //project = model.Instances.FirstOrDefault<IfcProject>(c => c.Name == "Hello Column");

                //validate and commit changes
                try
                {
                    txn.Commit();
                    return model;
                }
                catch (Exception)
                {

                    txn.RollBack();
                }
                return null;

            }
            //return null; //failed so return nothing

        }

        /// <summary>
        /// This creates a wall and it's geometry, many geometric representations are possible and extruded rectangular footprint is chosen as this is commonly used for standard case walls
        /// </summary>
        /// <param name="model"></param>
        /// <param name="length">Length of the rectangular footprint</param>
        /// <param name="width">Width of the rectangular footprint (width of the wall)</param>
        /// <param name="height">Height to extrude the wall, extrusion is vertical</param>
        /// <returns></returns>
        private static IfcColumn CreateColumn(HndzStructuralElement genericProducthndz)
        {

            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {

                IfcColumn genericProductIfc = model.Instances.New<IfcColumn>();



                IfcColumnType elementType = model.Instances.New<IfcColumnType>();
                elementType.PredefinedType = IfcColumnTypeEnum.COLUMN;
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile genericProfilehndz = genericProducthndz.Profile as HndzRectangularProfile;

                    IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);
                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzPolylineProfile)
                {
                    HndzPolylineProfile genericProfilehndz = genericProducthndz.Profile as HndzPolylineProfile;

                    IfcArbitraryClosedProfileDef ifcGenericProfile = AssignPolylineProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzCircularProfile)
                {
                    HndzCircularProfile genericProfilehndz = genericProducthndz.Profile as HndzCircularProfile;

                    IfcCircleProfileDef ifcGenericProfile = AssignCircularProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);
                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzISectionProfile)
                {
                    HndzISectionProfile genericProfilehndz = genericProducthndz.Profile as HndzISectionProfile;

                    IfcIShapeProfileDef ifcGenericProfile = AssignIProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                AdjustExtrusion(body, genericProducthndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Reinforced Concrete", 60);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
                //if (.Validate(txn.Modified(), Console.Out) == 0)
                //{
                //    txn.Commit();
                //    return genericProductIfc;
                //}
                //return null;
            }
        }

        private static IfcCircleProfileDef AssignCircularProfile(HndzStructuralElement genericProducthndz, IfcColumn genericProductIfc, IfcColumnType elementType, HndzCircularProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + "R = " + (int)genericProfilehndz.MyCircle.Radius + " mm";

            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //IfcLabel columnLabel = new IfcLabel(typeText);
            //elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.AddDefiningType(elementType);

            #endregion

            //represent column as a rectangular profile
            IfcCircleProfileDef ifcGenericProfile = model.Instances.New<IfcCircleProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.Radius = genericProfilehndz.MyCircle.Radius;
            //ifcGenericProfile.Position.Location = origin;

            //IfcCartesianPoint origin = model.Instances.New<IfcCartesianPoint>();
            //origin.SetXYZ(genericProfilehndz.MyCircle.Center.X, genericProfilehndz.MyCircle.Center.Y, genericProfilehndz.MyCircle.Center.Z);

            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.RefDirection.SetXY(genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            //ifcGenericProfile.Position.Location.SetXY(genericProfilehndz.MyCircle.Center.X, genericProfilehndz.MyCircle.Center.Y);
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            return ifcGenericProfile;
        }

        //static IfcColumn CreateColumn(IModel model, double length, double width, double height)
        //{
        //    //
        //    //begin a transaction
        //    using (Transaction txn = model.BeginTransaction("Create Column") as Transaction)
        //    {
        //        IfcColumn col = model.Instances.New<IfcColumn>();
        //        col.Name = "Column With ArbitraryClosedProfileDef";

        //        //represent wall as a rectangular profile
        //        //IfcRectangleProfileDef rectProf = model.Instances.New<IfcRectangleProfileDef>();
        //        //IfcCircleProfileDef rectProf = model.Instances.New<IfcCircleProfileDef>();

        //        //Creating arbit
        //        IfcArbitraryClosedProfileDef rectProf = model.Instances.New<IfcArbitraryClosedProfileDef>();
        //        IfcCompositeCurve mycompCurve = model.Instances.New<IfcCompositeCurve>();
        //        IfcPolyline myPolyline = model.Instances.New<IfcPolyline>();

        //        IfcCartesianPoint p0 = model.Instances.New<IfcCartesianPoint>();
        //        p0.SetXY(200, 100);

        //        IfcCartesianPoint p1 = model.Instances.New<IfcCartesianPoint>();
        //        p1.SetXY(0, 100);

        //        IfcCartesianPoint p2 = model.Instances.New<IfcCartesianPoint>();
        //        p2.SetXY(0, 0);

        //        IfcCartesianPoint p3 = model.Instances.New<IfcCartesianPoint>();
        //        p3.SetXY(400, 0);

        //        IfcCartesianPoint p4 = model.Instances.New<IfcCartesianPoint>();
        //        p4.SetXY(400, 600);

        //        IfcCartesianPoint p5 = model.Instances.New<IfcCartesianPoint>();
        //        p5.SetXY(0, 600);

        //        IfcCartesianPoint p6 = model.Instances.New<IfcCartesianPoint>();
        //        p6.SetXY(0, 500);

        //        IfcCartesianPoint p7 = model.Instances.New<IfcCartesianPoint>();
        //        p7.SetXY(200, 500);

        //        myPolyline.Points.Add(p0);
        //        myPolyline.Points.Add(p1);
        //        myPolyline.Points.Add(p2);
        //        myPolyline.Points.Add(p3);
        //        myPolyline.Points.Add(p4);
        //        myPolyline.Points.Add(p5);
        //        myPolyline.Points.Add(p6);
        //        myPolyline.Points.Add(p7);

        //        IfcCompositeCurveSegment seg = model.Instances.New<IfcCompositeCurveSegment>();
        //        seg.ParentCurve = myPolyline;
        //        mycompCurve.Segments.Add(seg);

        //        //create ifc segment curve (ARC)
        //        IfcCompositeCurveSegment seg2 = model.Instances.New<IfcCompositeCurveSegment>();
        //        IfcTrimmedCurve myArc = model.Instances.New<IfcTrimmedCurve>();
        //        IfcCircle myCirc = model.Instances.New<IfcCircle>();
        //        myCirc.Radius = 213.554;
        //        IfcCartesianPoint cP = model.Instances.New<IfcCartesianPoint>();
        //        cP.SetXY(125.1312, 300);

        //        IfcAxis2Placement3D plcment = model.Instances.New<IfcAxis2Placement3D>();
        //        plcment.Location = cP;
        //        plcment.RefDirection = model.Instances.New<IfcDirection>();
        //        plcment.RefDirection.SetXY(0, 1);
        //        myCirc.Position = plcment;

        //        myArc.BasisCurve = myCirc;
        //        //IfcParameterValue param1 = new IfcParameterValue(.5);
        //        //IfcParameterValue param2 = new IfcParameterValue(.9);

        //        //IList<IfcCartesianPoint> mypos = new List<IfcCartesianPoint>() { p0, p7 };

        //        //XbimListSet<IfcCartesianPoint> mylist = mypos ;
        //        IfcTrimmingSelect v1 = p7;//new IfcCartesianPoint(0,0,0);
        //        IfcTrimmingSelect v2 = p0;// new IfcCartesianPoint(0,0,0);
        //        //v1 = p0;
        //        //v2 = p7;
        //        //TrimmingSelectList v2 = 

        //        //v1.Add(p0);
        //        //v2.Add(p7);

        //        //Type lst = typeof(XbimListSet<double>);
        //        //XbimListSet<double>.
        //        //var mmmm = (XbimListSet<double>)Activator.CreateInstance(lst);

        //        //TrimmingSelectList myTList = model.Instances.New<TrimmingSelectList>();

        //        myArc.Trim1.Add(v1);
        //        myArc.Trim2.Add(v2);

        //        //myArc.SetTrim1(p0, true);
        //        //myArc.SetTrim2(p7, true);

        //        seg2.ParentCurve = myArc;

        //        mycompCurve.Segments.Add(seg2);

        //        //myArc.BasisCurve = 

        //        rectProf.OuterCurve = mycompCurve;

        //        rectProf.ProfileType = IfcProfileTypeEnum.AREA;
        //        //rectProf.Radius = 500;
        //        //rectProf.YDim = length;

        //        IfcCartesianPoint insertPoint = model.Instances.New<IfcCartesianPoint>();
        //        insertPoint.SetXY(0, 400); //insert at arbitrary position
        //        //rectProf.Position = model.Instances.New<IfcAxis2Placement2D>();
        //        //rectProf.Position.Location = insertPoint;

        //        //model as a swept area solid
        //        IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
        //        body.Depth = height;
        //        body.SweptArea = rectProf;
        //        body.ExtrudedDirection = model.Instances.New<IfcDirection>();
        //        body.ExtrudedDirection.SetXYZ(0, 0, 1);

        //        //parameters to insert the geometry in the model
        //        IfcCartesianPoint origin = model.Instances.New<IfcCartesianPoint>();
        //        origin.SetXYZ(0, 0, 0);
        //        body.Position = model.Instances.New<IfcAxis2Placement3D>();
        //        body.Position.Location = origin;

        //        //Create a Definition shape to hold the geometry
        //        IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
        //        IfcProject thisProj = null;
        //        foreach (var item in model.Instances)
        //        {
        //            if (item is IfcProject)
        //            {
        //                thisProj = item as IfcProject;
        //            }
        //        }
        //        //model.Instances.OfType<IfcProject>.
        //        shape.ContextOfItems = thisProj.ModelContext;
        //        shape.RepresentationType = "SweptSolid";
        //        shape.RepresentationIdentifier = "Body";
        //        shape.Items.Add(body);
        //        //shape.Items.Add_Reversible(body);

        //        //Create a Product Definition and add the model geometry to the wall
        //        IfcProductDefinitionShape rep = model.Instances.New<IfcProductDefinitionShape>();
        //        rep.Representations.Add(shape);
        //        //rep.Representations.Add_Reversible(shape);

        //        col.Representation = rep;

        //        //now place the wall into the model
        //        IfcLocalPlacement lp = model.Instances.New<IfcLocalPlacement>();
        //        IfcAxis2Placement3D ax3d = model.Instances.New<IfcAxis2Placement3D>();
        //        ax3d.Location = origin;
        //        ax3d.RefDirection = model.Instances.New<IfcDirection>();
        //        ax3d.RefDirection.SetXYZ(0, 1, 0);
        //        ax3d.Axis = model.Instances.New<IfcDirection>();
        //        ax3d.Axis.SetXYZ(0, 0, 1);


        //        lp.RelativePlacement = ax3d;
        //        col.ObjectPlacement = lp;

        //        foreach (var item in model.Instances)
        //        {
        //            if (item is IfcBuilding)
        //            {
        //                (item as IfcBuilding).AddElement(col);
        //            }
        //        }


        //        txn.Commit();
        //        return col;

        //        //validate write any errors to the console and commit if ok, otherwise abort
        //        //if (.Validate(Console.Out) == 0)
        //        //{
        //        //    txn.Commit();
        //        //    return wall;
        //        //}
        //        //else
        //        //    txn.Rollback();
        //    }
        //    return null;
        //}


        /// <summary>
        /// Generates IFC 3D BIM model contains all valid elements entire a given project
        /// </summary>
        /// <param name="project">project contains all required elements to be generated</param>
        /// <param name="savingPath">path of the generated IFC file</param>
        /// <returns>if IFC file successfully generated or not</returns>
        public static bool GenerateIFCProject(HndzProject project, string savingPath)
        {
            /*model =*/
            CreateandInitModel(project);
            if (project.Buildings == null) return false;

            foreach (HndzBuilding buildingHndz in project.Buildings)
            {
                IfcBuilding buildingIFC = CreateBuilding(buildingHndz);

                if (buildingHndz.Stories == null) return false;

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
                                var wall = CreateWall(genericProductHndz);
                                StoreyProducts.Add(wall);
                                if (genericProductHndz.WallOpenings != null)
                                {
                                    foreach (HndzWallOpening opening in genericProductHndz.WallOpenings)
                                    {
                                        var openingIfc = CreateOpenining(opening, wall);
                                        StoreyProducts.Add(openingIfc);
                                        if (opening is HndzDoor)
                                        {
                                            StoreyProducts.Add(CreateDoor(opening, openingIfc));
                                        }
                                        else if (opening is HndzWindow)
                                        {
                                            StoreyProducts.Add(CreateWindow(opening, openingIfc));
                                        }
                                    }
                                }
                            }
                            if (product is HndzWallArc)
                            {
                                HndzWallArc genericProductHndz = product as HndzWallArc;
                                StoreyProducts.Add(CreateArcedWall(genericProductHndz));
                            }
                            if (product is HndzCurtainWallStandardCase)
                            {
                                HndzCurtainWallStandardCase genericProductHndz = product as HndzCurtainWallStandardCase;
                                StoreyProducts.Add(CreateCurtainWall(genericProductHndz));
                            }
                            if (product is HndzCurtainWallArc)
                            {
                                HndzCurtainWallArc genericProductHndz = product as HndzCurtainWallArc;
                                StoreyProducts.Add(CreateArcedCurtainWall(genericProductHndz));
                            }
                            if (product is HndzBeamStandardCase)
                            {
                                HndzBeamStandardCase genericProductHndz = product as HndzBeamStandardCase;
                                StoreyProducts.Add(CreateBeam(genericProductHndz));
                            }
                            if (product is HndzColumnStandardCase)
                            {
                                HndzColumnStandardCase genericProductHndz = product as HndzColumnStandardCase;
                                StoreyProducts.Add(CreateColumn(genericProductHndz));
                            }
                            if (product is HndzSlab)
                            {
                                HndzSlab genericProductHndz = product as HndzSlab;
                                StoreyProducts.Add(CreateSlab(genericProductHndz));
                            }
                            //if (product is HndzFloor)
                            //{
                            //    HndzSlab genericProductHndz = product as HndzSlab;
                            //    StoreyProducts.Add(CreateSlab( genericProductHndz));
                            //}
                            #region Steel Frames


                            if (product is HndzFrame3D)
                            {

                                #region Frame Single Bay Assembly
                                if (product is HndzFrameSingleBay3D)
                                {
                                    HndzFrameSingleBay3D frame3D = product as HndzFrameSingleBay3D;
                                    foreach (HndzFrameSingleBay2D frame2D in frame3D.Frames2D)
                                    {
                                        StoreyProducts.Add(CreateTaperedColumn(frame2D.RightColumn));
                                        StoreyProducts.Add(CreateTaperedColumn(frame2D.LeftColumn));
                                        StoreyProducts.Add(CreateTaperedBeam(frame2D.RightBeam));
                                        StoreyProducts.Add(CreateTaperedBeam(frame2D.LeftBeam));
                                    }
                                    foreach (HndzPurlin purlin in frame3D.Purlins)
                                    {
                                        StoreyProducts.Add(CreateBeam(purlin));
                                    }
                                }
                                #endregion

                                #region Frame Mono Slope Assembly
                                //if (product is HndzFrameMonoSlope3D)
                                //{
                                //    HndzFrameMonoSlope3D frame3D = product as HndzFrameMonoSlope3D;
                                //    foreach (HndzFrameMonoSlope2D frame2D in frame3D.Frames2D)
                                //    {
                                //        StoreyProducts.Add(CreateTaperedColumn( frame2D.RightColumn));
                                //        StoreyProducts.Add(CreateTaperedColumn( frame2D.LeftColumn));
                                //        StoreyProducts.Add(CreateTaperedBeam( frame2D.Beam));
                                //    }
                                //    foreach (HndzPurlin purlin in frame3D.Purlins)
                                //    {
                                //        StoreyProducts.Add(CreateBeam( purlin));
                                //    }
                                //}
                                #endregion
                            }
                            #endregion
                        }
                        buildingProducts.Add(StoreyProducts);
                    }
                }
                AssembleBuilding(buildingIFC, 3000, buildingProducts);
            }
            try
            {

                model.SaveAs(savingPath, StorageType.Ifc);
                model.SaveAs(savingPath, StorageType.Xbim);
                //model.SaveAs(filePath, XbimStorageType.XBIM);
                //using (var wexBiMfile = new FileStream(savingPath, FileMode.Create, FileAccess.Write))
                //{
                //    using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
                //    {
                //        Console.WriteLine("Creating " + savingPath);
                //        model.SaveAsWexBim(wexBimBinaryWriter);
                //        wexBimBinaryWriter.Close();
                //    }
                //    wexBiMfile.Close();
                //}

                //using (IfcStore ifcStore = IfcStore.Open(savingPath + ".ifc"))
                //{
                //    var context = new Xbim3DModelContext(ifcStore);
                //    //context.CreateContext();
                //    using (var ms = new FileStream(savingPath + ".wexBIM", FileMode.Create, FileAccess.Write))
                //    {
                //        using (var binaryWriter = new BinaryWriter(ms))
                //        {
                //            ifcStore.SaveAsWexBim(binaryWriter);
                //        }
                //    }
                //}

                CreateWexBimFromIfc( savingPath);
                IfcToCoBieLiteUkTest(savingPath + ".ifc");
                //IfcStore model2 = IfcStore.Open(savingPath);

                return true;
            }
            catch (Exception ex)
            {
                //ToDo:return Saving Error Notification and handle it in ASP
                return false;
            }

        }


        private static IfcProduct CreateSlab(HndzSlab genericProductHndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProductHndz.ToString()))
            {
                IfcSlab genericProductIfc = model.Instances.New<IfcSlab>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = genericProductHndz.ExtrusionLine.RhinoLine.Length;

                IfcSlabType elementType = model.Instances.New<IfcSlabType>();
                elementType.PredefinedType = IfcSlabTypeEnum.ROOF;
                elementType.Name = "Ay 7aga";

                #region Assemble Arced PolyLine Profile


                if (genericProductHndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile genericProfilehndz = genericProductHndz.Profile as HndzRectangularProfile;

                    IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(genericProductHndz, genericProductIfc, elementType, genericProfilehndz);

                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProductHndz.Profile is HndzPolylineProfile)
                {
                    HndzPolylineProfile genericProfilehndz = genericProductHndz.Profile as HndzPolylineProfile;

                    //IfcPolyline ifcGenericProfile = model.Instances.New<IfcPolyline>();
                    IfcArbitraryClosedProfileDef ifcGenericProfile = AssignPolylineProfile(genericProductHndz, genericProductIfc, elementType, genericProfilehndz);

                    #endregion

                    body.SweptArea = ifcGenericProfile;
                }
                AdjustExtrusion(body, genericProductHndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Reinforced Concrete", (float)genericProductHndz.SlabThickness);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                }
                return null;
            }
        }

        private static IfcArbitraryClosedProfileDef NewMethod(HndzPolylineProfile genericProfilehndz, IfcPolyline PolyLineOuter)
        {
            foreach (Point3d point in genericProfilehndz.MyHndzPolyline.ApproxRhPolyline.ToArray())
            {
                IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                p_temp.SetXYZ(point.X, point.Y, point.Z);
                PolyLineOuter.Points.Add(p_temp);
            }
            IfcArbitraryClosedProfileDef ifcGenericProfile = model.Instances.New<IfcArbitraryClosedProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.ProfileName = IFCFileResources.ApplicationIdentifier + "Slab Profile";
            ifcGenericProfile.OuterCurve = PolyLineOuter;
            return ifcGenericProfile;
        }

        private static IfcBuilding AssembleBuilding(IfcBuilding building, float levelHeight, List<List<IfcProduct>> productsLists)
        {
            using (var txn = model.BeginTransaction("Create Storey"))
            {
                int floorNumber = 0;
                if (productsLists != null)
                {
                    foreach (List<IfcProduct> ProductsStorey in productsLists)
                    {
                        IfcBuildingStorey storey = model.Instances.New<IfcBuildingStorey>();
                        storey.Name = "Level " + ++floorNumber;
                        storey.OwnerHistory = GetDefaultOwnerHistory();
                        //storey.OwnerHistory.OwningApplication = model.DefaultOwningApplication;
                        storey.Elevation = levelHeight;
                        storey.CompositionType = IfcElementCompositionEnum.ELEMENT;
                        //storey.GlobalId = new Xbim.Ifc2x3.UtilityResource.IfcGloballyUniqueId();

                        storey.ObjectPlacement = model.Instances.New<IfcLocalPlacement>();
                        IfcLocalPlacement localPlacement = storey.ObjectPlacement as IfcLocalPlacement;

                        if (localPlacement != null && localPlacement.RelativePlacement == null)
                        {

                            localPlacement.RelativePlacement = model.Instances.New<IfcAxis2Placement3D>();
                            IfcAxis2Placement3D placement = localPlacement.RelativePlacement as IfcAxis2Placement3D;
                            IfcCartesianPoint placementPoint = model.Instances.New<IfcCartesianPoint>();
                            placementPoint.SetXYZ(0.0, 0.0,/*0*/ floorNumber * levelHeight);
                            placement.Location = placementPoint;
                            //placement.SetNewLocation(0.0, 0.0,/*0*/ floorNumber * levelHeight);//ToDo:meaningless

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
                try
                {
                    txn.Commit();
                    return building;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }

            }
        }
        //Vector3d zDirection = genericProducthndz.ExtrusionLine.RhinoLine.UnitTangent;
        //Vector3d yDirection = new Vector3d(0, 1, 0);
        //Vector3d xDirection = new Vector3d(1, 0, 0);
        //    if (genericProducthndz is HndzStructuralElement)
        //    {
        //        HndzStructuralElement strucElement = genericProducthndz as HndzStructuralElement;
        //        if (strucElement.Profile is HndzITaperedProfile)
        //        {

        //        }
        //    }
        //    else if(genericProducthndz is HndzArchitecturalElement)
        //    {

        //    }
        //    else
        //    {
        //      throw  new NotImplementedException("till now u can use only HndzStructuralElement or HndzArchitecturalElement");
        //    }

        private static void AdjustExtrusion(IfcExtrudedAreaSolid body, HndzExtrudedElement genericProducthndz,
                                            IfcProduct genericProductIfc)
        {
            IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
            axisOrigin.SetXYZ(0, 0, 0);
            IfcCartesianPoint elementStartPoint = model.Instances.New<IfcCartesianPoint>();
            elementStartPoint.SetXYZ(genericProducthndz.ExtrusionLine.baseNode.Point.X,
                genericProducthndz.ExtrusionLine.baseNode.Point.Y, /*genericProducthndz.ExtrusionLine.baseNode.Point.Z +*/ genericProducthndz.BuildingStorey.Elevation); //insert at arbitrary position//****************Need Revision
            body.Depth = genericProducthndz.ExtrusionLine.RhinoLine.Length;
            body.ExtrudedDirection = model.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);//Assumption
            body.Position = model.Instances.New<IfcAxis2Placement3D>();
            //body.Position.Location = axisOrigin;
            if (body.SweptArea is IfcArbitraryClosedProfileDef)
            {
                body.Position.Location = axisOrigin;
                body.Position.Location.Z = elementStartPoint.Z / 2;
            }
            else
            {
                body.Position.Location = elementStartPoint;

            }

            //Create a Definition shape to hold the geometry
            IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
            shape.ContextOfItems = model.Instances.FirstOrDefault<IfcProject>(e => e.Name == genericProducthndz.BuildingStorey.Building.Project.Name).ModelContext;
            shape.RepresentationType = "SweptSolid";
            shape.RepresentationIdentifier = "Body";
            shape.Items.Add(body);

            //Create a Product Definition and add the model geometry to the wall
            IfcProductDefinitionShape rep = model.Instances.New<IfcProductDefinitionShape>();
            rep.Representations.Add(shape);
            genericProductIfc.Representation = rep;

            //now place the element into the model
            #region ProfileVectorDir.
            Vector3d LocalXAxis = genericProducthndz.ExtrusionLine.RhinoLine.UnitTangent;
            Vector3d LocalZAxis = Vector3d.CrossProduct(LocalXAxis, Vector3d.YAxis);
            if (LocalXAxis.Z != 1)
            {
                LocalZAxis = Vector3d.CrossProduct(LocalXAxis, Vector3d.ZAxis);
            }
            #endregion

            IfcLocalPlacement lp = model.Instances.New<IfcLocalPlacement>();
            IfcAxis2Placement3D ax3D = model.Instances.New<IfcAxis2Placement3D>();
            ax3D.Location = axisOrigin;
            //if (body.SweptArea is IfcArbitraryClosedProfileDef)
            //{
            //    ax3D.Location = axisOrigin;
            //    ax3D.Location.Z = elementStartPoint.Z / 2;
            //}
            //else
            //{
            //    ax3D.Location = elementStartPoint;

            //}
            if (genericProducthndz is HndzBeam)
            {
                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    ax3D.Location.Z += genericProducthndz.BuildingStorey.Elevation + genericProducthndz.BuildingStorey.StoreyHeight - ((HndzRectangularProfile)genericProducthndz.Profile).Rectangle.Height / 2;
                }
            }
            ax3D.RefDirection = model.Instances.New<IfcDirection>();
            ax3D.RefDirection.SetXYZ(LocalZAxis.X, LocalZAxis.Y, LocalZAxis.Z);  //Y-Axis
            ax3D.Axis = model.Instances.New<IfcDirection>();
            ax3D.Axis.SetXYZ(genericProducthndz.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.ExtrusionLine.RhinoLine.Direction.Z);          //Z-Axis
            ax3D.Axis.SetXYZ(LocalXAxis.X, LocalXAxis.Y, LocalXAxis.Z);

            lp.RelativePlacement = ax3D;
            genericProductIfc.ObjectPlacement = lp;


        }

        private static void AdjustOpeningExtrusion(IfcExtrudedAreaSolid body, HndzWallOpening genericProducthndz,
                                           IfcProduct genericProductIfc)
        {
            IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
            axisOrigin.SetXYZ(0, 0, 0);

            IfcCartesianPoint elementStartPoint = model.Instances.New<IfcCartesianPoint>();
            elementStartPoint.SetXYZ(genericProducthndz.Position.X,
                genericProducthndz.Position.Y,/* genericProducthndz.Position.Z +*/ genericProducthndz.BuildingStorey.Elevation + genericProducthndz.BaseOffset); //insert at arbitrary position//****************Need Revision

            //body.Depth = genericProducthndz.Wall.ExtrusionLine.RhinoLine.Length-100;//temp magic number
            body.Depth = genericProducthndz.Height;
            body.ExtrudedDirection = model.Instances.New<IfcDirection>();
            body.ExtrudedDirection.SetXYZ(0, 0, 1);

            //parameters to insert the geometry in the model

            body.Position = model.Instances.New<IfcAxis2Placement3D>();
            body.Position.Location = elementStartPoint;
            if (body.SweptArea is IfcArbitraryClosedProfileDef)
            {
                body.Position.Location = axisOrigin;
            }

            //Create a Definition shape to hold the geometry
            IfcShapeRepresentation shape = model.Instances.New<IfcShapeRepresentation>();
            //shape.ContextOfItems = model.IfcProject.ModelContext();
            shape.ContextOfItems = model.Instances.FirstOrDefault<IfcProject>(e => e.Name == genericProducthndz.BuildingStorey.Building.Project.Name).ModelContext;
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
            Vector3d xDir = genericProducthndz.Wall.ExtrusionLine.RhinoLine.UnitTangent;
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
            ax3D.Location = axisOrigin;
            ax3D.RefDirection = model.Instances.New<IfcDirection>();
            //ax3D.RefDirection.SetXYZ(perpendicularVector.X, perpendicularVector.Y, perpendicularVector.Z);  //Y-Axis
            ax3D.RefDirection.SetXYZ(orthoToExtrusion.X, orthoToExtrusion.Y, orthoToExtrusion.Z);  //Y-Axis
            //ax3D.RefDirection.SetXYZ(0, 1, 0);  //Y-Axis
            ax3D.Axis = model.Instances.New<IfcDirection>();
            //ax3D.Axis.SetXYZ(genericProducthndz.Wall.ExtrusionLine.RhinoLine.Direction.X, genericProducthndz.Wall.ExtrusionLine.RhinoLine.Direction.Y, genericProducthndz.Wall.ExtrusionLine.RhinoLine.Direction.Z);          //Z-Axis
            ax3D.Axis.SetXYZ(xDir.X, xDir.Y, xDir.Z);          //Z-Axis


            //XbimVector3D X_Dir = new XbimVector3D(extrusionPlane.XAxis.X, extrusionPlane.XAxis.Y, extrusionPlane.XAxis.Z);
            //XbimVector3D Y_Dir = new XbimVector3D(extrusionPlane.YAxis.X, extrusionPlane.YAxis.Y, extrusionPlane.YAxis.Z);
            //XbimVector3D Z_Dir = new XbimVector3D(extrusionPlane.ZAxis.X, extrusionPlane.ZAxis.Y, extrusionPlane.ZAxis.Z);
            //ax3D.P.Insert(0,X_Dir);
            //ax3D.P.Insert(1,Y_Dir);
            //ax3D.P.Insert(2,Z_Dir);

            lp.RelativePlacement = ax3D;
            genericProductIfc.ObjectPlacement = lp;


        }
        private static IfcProduct CreateTaperedColumn(HndzStructuralElement genericProducthndz)
        {
            //HndzISectionProfile startProfile = new HndzISectionProfile(new SectionI("start", 2000, 1000, 200, 100), new Vector2d(0, 1));
            //HndzISectionProfile endProfile = new HndzISectionProfile(new SectionI("end", 2000 * 2, 1000 * 2, 200 * 2, 100 * 2), new Vector2d(0, 1));
            //genericProducthndz.Profile = new HndzITaperedProfile(startProfile, endProfile);

            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
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
                    AdjustShapeFaces(genericProducthndz, genericProductIfc, genericProfilehndz);
                }

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
            }
        }

        private static void AdjustShapeFaces(HndzStructuralElement genericProducthndz, IfcProduct genericProductIfc, HndzITaperedProfile genericProfilehndz)
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
            shape.ContextOfItems = model.Instances.FirstOrDefault<IfcProject>(e => e.Name == genericProducthndz.BuildingStorey.Building.Project.Name).ModelContext;

            //shape.ContextOfItems = model.IfcProject.ModelContext();
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
            shape.ContextOfItems = model.Instances.FirstOrDefault<IfcProject>(e => e.Name == genericProducthndz.BuildingStorey.Building.Project.Name).ModelContext;

            // shapeRep.ContextOfItems = model.IfcProject.ModelContext();
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



        }

        private static IfcProduct CreateTaperedBeam(HndzStructuralElement genericProducthndz)
        {
            //HndzISectionProfile startProfile = new HndzISectionProfile(new SectionI("start", 2000 * 2, 1000 * 2, 200 * 2, 100 * 2), new Vector2d(0, 1));
            //HndzISectionProfile endProfile = new HndzISectionProfile(new SectionI("end", 2000, 1000, 200, 100), new Vector2d(0, 1));
            //genericProducthndz.Profile = new HndzITaperedProfile(startProfile, endProfile);

            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
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

                    AdjustShapeFaces(genericProducthndz, genericProductIfc, genericProfilehndz);
                }

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
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

        //private static IfcColumn CreateColumn( HndzStructuralElement genericProducthndz)
        //{

        //    using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
        //    {

        //        IfcColumn genericProductIfc = model.Instances.New<IfcColumn>();
        //        IfcColumnType elementType = model.Instances.New<IfcColumnType>();
        //        elementType.PredefinedType = IfcColumnTypeEnum.COLUMN;
        //        IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

        //        if (genericProducthndz.Profile is HndzRectangularProfile)
        //        {
        //            HndzRectangularProfile genericProfilehndz = genericProducthndz.Profile as HndzRectangularProfile;

        //            IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile( genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

        //            //model as a swept area solid
        //            body.SweptArea = ifcGenericProfile;
        //        }
        //        if (genericProducthndz.Profile is HndzISectionProfile)
        //        {
        //            HndzISectionProfile genericProfilehndz = genericProducthndz.Profile as HndzISectionProfile;

        //            IfcIShapeProfileDef ifcGenericProfile = AssignIProfile( genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


        //            //model as a swept area solid
        //            body.SweptArea = ifcGenericProfile;
        //        }
        //        AdjustExtrusion( body, genericProducthndz, genericProductIfc);


        //        try
        //        {
        //            txn.Commit();
        //            return genericProductIfc;
        //        }
        //        catch (Exception)
        //        {

        //            txn.RollBack();
        //            return null;
        //        }
        //    }
        //}
        private static IfcIShapeProfileDef AssignIProfile(HndzStructuralElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzISectionProfile gnericProfileHndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + "I beam (flange " + gnericProfileHndz.I_Section.b_f + " x " + gnericProfileHndz.I_Section.t_fTop + " and web "
                + gnericProfileHndz.I_Section.d + " x " + gnericProfileHndz.I_Section.t_w + " mm";


            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //IfcLabel columnLabel = new IfcLabel(typeText);
            //elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.AddDefiningType(elementType);

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

        private static IfcRectangleProfileDef AssignRectangularProfile(HndzExtrudedElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzRectangularProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + (int)genericProfilehndz.Rectangle.Width + " x "
                              + (int)genericProfilehndz.Rectangle.Height + " mm";

            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //IfcLabel columnLabel = new IfcLabel(typeText);
            //elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.AddDefiningType(elementType);

            #endregion

            //represent column as a rectangular profile
            IfcRectangleProfileDef ifcGenericProfile = model.Instances.New<IfcRectangleProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.XDim = genericProfilehndz.Rectangle.Height;
            ifcGenericProfile.YDim = genericProfilehndz.Rectangle.Width;

            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            ifcGenericProfile.Position.RefDirection.SetXY(genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            if (ifcGenericProfile.Position.RefDirection.X == 0 && ifcGenericProfile.Position.RefDirection.Y == 0)
            {
                ifcGenericProfile.Position.RefDirection.SetXY(0, 1);//for beams because its original profile not located in XY
            }

            return ifcGenericProfile;
        }

        private static IfcRectangleProfileDef AssignOpeningRectangularProfile(HndzWallOpening genericProducthndz,
           IfcProduct genericProductIfc, /*IfcBuildingElementType elementType,*/ HndzRectangularProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + (int)genericProfilehndz.Rectangle.Width + " x "
                              + (int)genericProfilehndz.Rectangle.Height + " mm";

            //elementType.Tag = typeText;
            //elementType.Name = typeText;
            //IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            //genericProductIfc.AddDefiningType(elementType);

            #endregion

            //represent column as a rectangular profile
            IfcRectangleProfileDef ifcGenericProfile = model.Instances.New<IfcRectangleProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.XDim = genericProfilehndz.Rectangle.Height;
            ifcGenericProfile.YDim = genericProfilehndz.Rectangle.Width;

            ifcGenericProfile.Position = model.Instances.New<IfcAxis2Placement2D>();
            ifcGenericProfile.Position.RefDirection = model.Instances.New<IfcDirection>();
            ifcGenericProfile.Position.RefDirection.SetXY(genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            //ifcGenericProfile.Position.RefDirection.SetXYZ(0,genericProfilehndz.OrientationInPlane.X, genericProfilehndz.OrientationInPlane.Y);
            ifcGenericProfile.Position.Location = model.Instances.New<IfcCartesianPoint>();
            ifcGenericProfile.Position.Location.SetXY(0, 0);
            return ifcGenericProfile;
        }

        private static IfcArbitraryClosedProfileDef AssignPolylineProfile(HndzExtrudedElement genericProductHndz, IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzPolylineProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProductHndz.ToString() + (int)genericProfilehndz.MyHndzPolyline.ApproxRhPolyline.Length + " mm";

            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //IfcLabel columnLabel = new IfcLabel(typeText);
            //elementType.ElementType = columnLabel;
            elementType.ApplicableOccurrence = columnLabel;



            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.AddDefiningType(elementType);

            #endregion

            IfcPolyline PolyLineOuter = model.Instances.New<IfcPolyline>();

            foreach (Point3d point in genericProfilehndz.MyHndzPolyline.ApproxRhPolyline.ToArray())
            {
                IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                p_temp.SetXYZ(point.X, point.Y, point.Z);
                PolyLineOuter.Points.Add(p_temp);
            }
            IfcArbitraryClosedProfileDef ifcGenericProfile = model.Instances.New<IfcArbitraryClosedProfileDef>();
            ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ifcGenericProfile.ProfileName = IFCFileResources.ApplicationIdentifier + "Slab Profile";
            ifcGenericProfile.OuterCurve = PolyLineOuter;
            return ifcGenericProfile;
        }
        private static IfcCShapeProfileDef AssignCsectionProfile(HndzStructuralElement genericProducthndz,
            IfcProduct genericProductIfc, IfcBuildingElementType elementType, HndzCSectionProfile genericProfilehndz)
        {
            #region Type & Material &Tags

            string typeText = genericProducthndz.ToString() + " C-Chanel (flange " + genericProfilehndz.C_Section.b_f + " x " + genericProfilehndz.C_Section.t_f + " and web "
                + genericProfilehndz.C_Section.d + " x " + genericProfilehndz.C_Section.t_w + " mm";


            elementType.Tag = typeText;
            elementType.Name = typeText;
            IfcIdentifier columnLabel = new IfcIdentifier(typeText);
            //IfcLabel columnLabel = new IfcLabel(typeText);
            //elementType.ElementType = columnLabel;
            //elementType.ApplicableOccurrence = columnLabel;


            //genericProductIfc.Tag = typeText;
            genericProductIfc.Name = typeText;
            genericProductIfc.Description = typeText;
            genericProductIfc.AddDefiningType(elementType);

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

        private static IfcBeam CreateBeam(HndzStructuralElement genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcBeam genericProductIfc = model.Instances.New<IfcBeam>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                IfcBeamType elementType = model.Instances.New<IfcBeamType>();
                elementType.PredefinedType = IfcBeamTypeEnum.BEAM;


                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile genericProfilehndz = genericProducthndz.Profile as HndzRectangularProfile;

                    IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);

                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }

                if (genericProducthndz.Profile is HndzISectionProfile)
                {
                    HndzISectionProfile genericProfilehndz = genericProducthndz.Profile as HndzISectionProfile;

                    IfcIShapeProfileDef ifcGenericProfile = AssignIProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }
                if (genericProducthndz.Profile is HndzCSectionProfile)
                {
                    HndzCSectionProfile genericProfilehndz = genericProducthndz.Profile as HndzCSectionProfile;
                    IfcCShapeProfileDef ifcGenericProfile = AssignCsectionProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);


                    //model as a swept area solid
                    body.SweptArea = ifcGenericProfile;
                }

                AdjustExtrusion(body, genericProducthndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Reinforced Concrete", 50);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
            }
        }


        private static IfcCurtainWall CreateCurtainWall(HndzCurtainWallStandardCase genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcCurtainWall genericProductIfc = model.Instances.New<IfcCurtainWall>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                IfcCurtainWallType elementType = model.Instances.New<IfcCurtainWallType>();
                elementType.PredefinedType = IfcCurtainWallTypeEnum.USERDEFINED;

                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, genericProducthndz.WallThickness, genericProducthndz.BaseLine.Length);
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(genericProducthndz.BaseLine.Direction.X, genericProducthndz.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);
                body.SweptArea = ifcGenericProfile;

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustExtrusion(body, genericProducthndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Glass", 50);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }

                //if (.Validate(txn.Modified(), Console.Out) == 0)
                //{
                //    txn.Commit();
                //    return genericProductIfc;
                //}

                //return null;
            }
        }
        private static IfcCurtainWall CreateArcedCurtainWall(HndzCurtainWallArc genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcCurtainWall genericProductIfc = model.Instances.New<IfcCurtainWall>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = genericProducthndz.UnconnectedHeight;

                IfcCurtainWallType elementType = model.Instances.New<IfcCurtainWallType>();
                elementType.PredefinedType = IfcCurtainWallTypeEnum.NOTDEFINED;
                elementType.Name = "Ay 7aga";

                #region Assemble Arced PolyLine Profile

                IfcPolyline PolyLineOuter = model.Instances.New<IfcPolyline>();

                Arc arcCenter = genericProducthndz.BaseArc;
                int nMesh = (int)(genericProducthndz.BaseArc.Diameter * MeshTolerance / arcCenter.Radius); /// TODO: need to be more generic meshing

                Arc arcExternal = arcCenter;
                Arc arcInternal = arcCenter;

                arcExternal.Radius += genericProducthndz.WallThickness / 2;
                arcInternal.Radius -= genericProducthndz.WallThickness / 2;

                //foreach (Arc arc  in new Arc[] { arcExternal, arcInternal })

                for (int j = 0; j <= nMesh; j++)
                {
                    double relativeMesh = (double)j / nMesh;
                    IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                    p_temp.SetXY(GetPointOnArc(arcExternal, relativeMesh).X, GetPointOnArc(arcExternal, relativeMesh).Y);
                    PolyLineOuter.Points.Add(p_temp);
                }
                for (int j = nMesh; j >= 0; j--)   //to reverse direction to avoid adding intersecting wall
                {
                    double relativeMesh = j / (double)nMesh;

                    IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                    p_temp.SetXY(GetPointOnArc(arcInternal, relativeMesh).X, GetPointOnArc(arcInternal, relativeMesh).Y);
                    PolyLineOuter.Points.Add(p_temp);
                }


                IfcArbitraryClosedProfileDef ifcGenericProfile = model.Instances.New<IfcArbitraryClosedProfileDef>();
                ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
                ifcGenericProfile.ProfileName = IFCFileResources.ApplicationIdentifier + "Arc Profile R=" + arcCenter.Radius;
                ifcGenericProfile.OuterCurve = PolyLineOuter;


                #endregion

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustExtrusion(body, genericProducthndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Glass", 50);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
            }
        }

        private static IfcWallStandardCase CreateWall(HndzWallStandardCase genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcWallStandardCase genericProductIfc = model.Instances.New<IfcWallStandardCase>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                IfcWallType elementType = model.Instances.New<IfcWallType>();
                elementType.PredefinedType = IfcWallTypeEnum.NOTDEFINED;

                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, genericProducthndz.WallThickness, genericProducthndz.BaseLine.Length);
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(genericProducthndz.BaseLine.Direction.X, genericProducthndz.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignRectangularProfile(genericProducthndz, genericProductIfc, elementType, genericProfilehndz);
                body.SweptArea = ifcGenericProfile;

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                SetMaterial(genericProductIfc, "Bricks", 12);

                AdjustExtrusion(body, genericProducthndz, genericProductIfc);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }


            }
        }
        /// <summary>
        /// Set IFC element material and layer properties
        /// </summary>
        /// <param name="genericProductIfc">IFC element</param>
        /// <param name="name">material label</param>
        /// <param name="thickness">layer thickness</param>
        private static void SetMaterial(IfcDefinitionSelect genericProductIfc, string name, float thickness)
        {
            IfcMaterial material = model.Instances.New<IfcMaterial>();
            material.Category = $@"{IFCFileResources.ApplicationIdentifier} {name} category";
            material.Description = $@"{IFCFileResources.ApplicationIdentifier} {name} Description";
            material.Name = name;


            IfcMaterialLayer materialLayer = model.Instances.New<IfcMaterialLayer>();
            materialLayer.Category = $@"{IFCFileResources.ApplicationIdentifier} {name} Layer Category";
            materialLayer.LayerThickness = thickness;
            materialLayer.Material = material;

            IfcMaterialLayerSet materialLayerSet = model.Instances.New<IfcMaterialLayerSet>();
            materialLayerSet.LayerSetName = $@"{IFCFileResources.ApplicationIdentifier} {name} layer set";
            materialLayerSet.MaterialLayers.Add(materialLayer);


            IfcMaterialLayerSetUsage materialUsage = model.Instances.New<IfcMaterialLayerSetUsage>();
            materialUsage.DirectionSense = IfcDirectionSenseEnum.NEGATIVE;
            materialUsage.LayerSetDirection = IfcLayerSetDirectionEnum.AXIS2;
            materialUsage.OffsetFromReferenceLine = 100;
            materialUsage.ForLayerSet = materialLayerSet;


            IfcRelAssociatesMaterial relMaterial = model.Instances.New<IfcRelAssociatesMaterial>();
            relMaterial.OwnerHistory = GetDefaultOwnerHistory();
            relMaterial.RelatedObjects.Add(genericProductIfc);
            relMaterial.RelatingMaterial = materialUsage;
        }

        private static IfcOpeningElement CreateOpenining(HndzWallOpening openingHndz, IfcWall containigWall)
        {
            using (var txn = model.BeginTransaction("Create" + openingHndz.ToString()))
            {
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

                IfcOpeningElement opening = model.Instances.New<IfcOpeningElement>();
                //IfcOpeningElement elementType = model.Instances.New<IfcDoorType>();
                opening.PredefinedType = IfcOpeningElementTypeEnum.OPENING;
                IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
                axisOrigin.SetXYZ(openingHndz.Position.X, openingHndz.Position.Y, openingHndz.Position.Y);

                opening.Name = "Opening";
                opening.OwnerHistory = GetDefaultOwnerHistory();


                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, openingHndz.Wall.WallThickness, openingHndz.Width);
                HndzWallStandardCase containingWall = openingHndz.Wall as HndzWallStandardCase;
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(containingWall.BaseLine.Direction.X, containingWall.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignOpeningRectangularProfile(openingHndz, opening, genericProfilehndz);

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustOpeningExtrusion(body, openingHndz, opening);
                SetMaterial(opening, "void", 0);

                IfcRelVoidsElement relVoid = model.Instances.New<IfcRelVoidsElement>();
                relVoid.OwnerHistory = GetDefaultOwnerHistory();
                relVoid.RelatedOpeningElement = opening;
                relVoid.RelatingBuildingElement = containigWall;

                try
                {
                    txn.Commit();
                    return opening;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }

                //if (.Validate(txn.Modified(), Console.Out) == 0)
                //{
                //    txn.Commit();
                //    return genericProductIfc;
                //}

                //return null;
            }
        }

        private static IfcDoor CreateDoor(HndzWallOpening openingHndz, IfcOpeningElement openingIfc)
        {
            using (var txn = model.BeginTransaction("Create" + openingHndz.ToString()))
            {
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

                IfcDoor door = model.Instances.New<IfcDoor>();
                IfcDoorType elementType = model.Instances.New<IfcDoorType>();
                elementType.PredefinedType = IfcDoorTypeEnum.DOOR;
                door.AddDefiningType(elementType);
                IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
                axisOrigin.SetXYZ(openingHndz.Position.X, openingHndz.Position.Y, openingHndz.Position.Y);

                door.Name = "Door";
                door.OverallHeight = openingHndz.Height;
                door.OverallWidth = openingHndz.Width;
                door.PredefinedType = IfcDoorTypeEnum.DOOR;
                door.OwnerHistory = GetDefaultOwnerHistory();



                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, openingHndz.Wall.WallThickness, openingHndz.Width);
                HndzWallStandardCase containingWall = openingHndz.Wall as HndzWallStandardCase;
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(containingWall.BaseLine.Direction.X, containingWall.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignOpeningRectangularProfile(openingHndz, door, genericProfilehndz);

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustOpeningExtrusion(body, openingHndz, door);
                SetMaterial(door, "Wood", 50);

                IfcRelFillsElement fillRel = model.Instances.New<IfcRelFillsElement>();
                fillRel.OwnerHistory = GetDefaultOwnerHistory();
                fillRel.RelatedBuildingElement = door;
                fillRel.RelatingOpeningElement = openingIfc;

                IfcDoorLiningProperties doorProp = model.Instances.New<IfcDoorLiningProperties>();
                doorProp.CasingDepth = 20;
                doorProp.CasingThickness = 20;
                doorProp.LiningDepth = 30;
                doorProp.LiningThickness = 10;

                IfcDoorPanelProperties panelProp = model.Instances.New<IfcDoorPanelProperties>();
                panelProp.PanelPosition = IfcDoorPanelPositionEnum.LEFT;
                panelProp.PanelDepth = 100;
                panelProp.PanelOperation = IfcDoorPanelOperationEnum.SWINGING;
                panelProp.PanelPosition = IfcDoorPanelPositionEnum.LEFT;
                panelProp.PanelWidth = 500;

                IfcDoorStyle doorStyle = model.Instances.New<IfcDoorStyle>();
                doorStyle.ConstructionType = IfcDoorStyleConstructionEnum.ALUMINIUM_WOOD;
                doorStyle.Description = "Handaz Door Description";
                doorStyle.OperationType = IfcDoorStyleOperationEnum.SINGLE_SWING_RIGHT;
                doorStyle.OwnerHistory = GetDefaultOwnerHistory();
                doorStyle.ParameterTakesPrecedence = true;
                doorStyle.Sizeable = true;
                doorStyle.Tag = "A.S Door";

                IfcMaterial materialDoor = model.Instances.New<IfcMaterial>();
                materialDoor.Category = "material category";
                materialDoor.Description = "material description";
                materialDoor.Name = "material name";

                IfcRelAssociatesMaterial materialRel = model.Instances.New<IfcRelAssociatesMaterial>();
                materialRel.RelatedObjects.Add(doorStyle);
                materialRel.RelatingMaterial = materialDoor;

                IfcRelDefinesByType typeRel = model.Instances.New<IfcRelDefinesByType>();
                typeRel.Name = "door type name";
                typeRel.OwnerHistory = GetDefaultOwnerHistory();
                typeRel.RelatingType = doorStyle;
                typeRel.RelatedObjects.Add(door);

                try
                {
                    txn.Commit();
                    return door;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
                //if (.Validate(txn.Modified(), Console.Out) == 0)
                //{
                //    txn.Commit();
                //    return genericProductIfc;
                //}

                //return null;
            }
        }
        private static IfcWindow CreateWindow(HndzWallOpening openingHndz, IfcOpeningElement openingIfc)
        {
            using (var txn = model.BeginTransaction("Create" + openingHndz.ToString()))
            {
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();

                IfcWindow window = model.Instances.New<IfcWindow>();
                IfcWindowType elementType = model.Instances.New<IfcWindowType>();
                elementType.PredefinedType = IfcWindowTypeEnum.WINDOW;
                IfcCartesianPoint axisOrigin = model.Instances.New<IfcCartesianPoint>();
                axisOrigin.SetXYZ(openingHndz.Position.X, openingHndz.Position.Y, openingHndz.Position.Y);

                window.Name = "Window";
                window.OverallHeight = openingHndz.Height;
                window.OverallWidth = openingHndz.Width;
                window.PredefinedType = IfcWindowTypeEnum.WINDOW;
                window.OwnerHistory = GetDefaultOwnerHistory();



                Rectangle3d tempRec = new Rectangle3d(Plane.WorldXY, openingHndz.Wall.WallThickness, openingHndz.Width);
                HndzWallStandardCase containingWall = openingHndz.Wall as HndzWallStandardCase;
                HndzRectangularProfile genericProfilehndz = new HndzRectangularProfile(tempRec, new Vector2d(containingWall.BaseLine.Direction.X, containingWall.BaseLine.Direction.Y));

                IfcRectangleProfileDef ifcGenericProfile = AssignOpeningRectangularProfile(openingHndz, window, genericProfilehndz);

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustOpeningExtrusion(body, openingHndz, window);
                SetMaterial(window, "Wood", 50);

                IfcRelFillsElement fillRel = model.Instances.New<IfcRelFillsElement>();
                fillRel.OwnerHistory = GetDefaultOwnerHistory();
                fillRel.RelatedBuildingElement = window;
                fillRel.RelatingOpeningElement = openingIfc;


                try
                {
                    txn.Commit();
                    return window;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }

                //if (.Validate(txn.Modified(), Console.Out) == 0)
                //{
                //    txn.Commit();
                //    return genericProductIfc;
                //}

                //return null;
            }
        }

        private static IfcWallStandardCase CreateArcedWall(HndzWallArc genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create" + genericProducthndz.ToString()))
            {
                IfcWallStandardCase genericProductIfc = model.Instances.New<IfcWallStandardCase>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = genericProducthndz.UnconnectedHeight;

                IfcWallType elementType = model.Instances.New<IfcWallType>();
                elementType.PredefinedType = IfcWallTypeEnum.NOTDEFINED;
                elementType.Name = "Ay 7aga";

                #region Assemble Arced PolyLine Profile

                IfcPolyline PolyLineOuter = model.Instances.New<IfcPolyline>();

                Arc arcCenter = genericProducthndz.BaseArc;
                int nMesh = (int)(genericProducthndz.BaseArc.Diameter * MeshTolerance / arcCenter.Radius); /// TODO: need to be more generic meshing

                Arc arcExternal = arcCenter;
                Arc arcInternal = arcCenter;

                arcExternal.Radius += genericProducthndz.WallThickness / 2;
                arcInternal.Radius -= genericProducthndz.WallThickness / 2;

                //foreach (Arc arc  in new Arc[] { arcExternal, arcInternal })

                for (int j = 0; j <= nMesh; j++)
                {
                    double relativeMesh = (double)j / nMesh;
                    IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                    p_temp.SetXYZ(GetPointOnArc(arcExternal, relativeMesh).X, GetPointOnArc(arcExternal, relativeMesh).Y, GetPointOnArc(arcExternal, relativeMesh).Z);
                    PolyLineOuter.Points.Add(p_temp);
                }
                for (int j = nMesh; j >= 0; j--)   //to reverse direction to avoid adding intersecting wall
                {
                    double relativeMesh = j / (double)nMesh;

                    IfcCartesianPoint p_temp = model.Instances.New<IfcCartesianPoint>();
                    p_temp.SetXYZ(GetPointOnArc(arcInternal, relativeMesh).X, GetPointOnArc(arcInternal, relativeMesh).Y, GetPointOnArc(arcInternal, relativeMesh).Z);
                    PolyLineOuter.Points.Add(p_temp);
                }

                IfcArbitraryClosedProfileDef ifcGenericProfile = model.Instances.New<IfcArbitraryClosedProfileDef>();
                ifcGenericProfile.ProfileType = IfcProfileTypeEnum.AREA;
                ifcGenericProfile.ProfileName = IFCFileResources.ApplicationIdentifier + "Arc Profile R=" + arcCenter.Radius;
                ifcGenericProfile.OuterCurve = PolyLineOuter;
                #endregion

                //model as a swept area solid
                body.SweptArea = ifcGenericProfile;

                AdjustExtrusion(body, genericProducthndz, genericProductIfc);
                SetMaterial(genericProductIfc, "Bricks", 50);

                try
                {
                    txn.Commit();
                    return genericProductIfc;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
            }
        }
        private static IfcBeam CreatePurlin(HndzPurlin genericProducthndz)
        {
            using (var txn = model.BeginTransaction("Create Purlin"))
            {
                IfcBeam beam = model.Instances.New<IfcBeam>();
                IfcExtrudedAreaSolid body = model.Instances.New<IfcExtrudedAreaSolid>();
                body.Depth = genericProducthndz.ExtrusionLine.RhinoLine.Length;

                IfcBeamType elementType = model.Instances.New<IfcBeamType>();
                elementType.PredefinedType = IfcBeamTypeEnum.USERDEFINED;

                IfcMaterial material = model.Instances.New<IfcMaterial>();
                material.Name = "STEEL";
                //beam.SetMatemarial(material);
                IfcCartesianPoint insertPoint = model.Instances.New<IfcCartesianPoint>();

                insertPoint.SetXY(genericProducthndz.ExtrusionLine.baseNode.Point.X, genericProducthndz.ExtrusionLine.baseNode.Point.Y); //insert at arbitrary position/***************Need Revision

                if (genericProducthndz.Profile is HndzRectangularProfile)
                {
                    HndzRectangularProfile recProfile = genericProducthndz.Profile as HndzRectangularProfile;

                    #region Type & Material &Tags

                    string typeText = "HANDAZ-Column-Rectangular " + (int)recProfile.Rectangle.Width + " x "
                                      + (int)recProfile.Rectangle.Height + " mm";

                    elementType.Tag = typeText;
                    elementType.Name = typeText;
                    IfcIdentifier columnIdentifier = new IfcIdentifier(typeText);
                    //IfcLabel columnLabel = new IfcLabel(typeText);
                    //elementType.ElementType = columnLabel;
                    elementType.ApplicableOccurrence = columnIdentifier;



                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.AddDefiningType(elementType);

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
                    //IfcLabel columnLabel = new IfcLabel(typeText);
                    //elementType.ElementType = columnLabel;
                    IfcIdentifier columnIdentifier = new IfcIdentifier(typeText);
                    elementType.ApplicableOccurrence = columnIdentifier;



                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.AddDefiningType(elementType);

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
                    IfcIdentifier columnLabel = new IfcIdentifier(typeText);
                    //IfcLabel columnLabel = new IfcLabel(typeText);
                    //elementType.ElementType = columnLabel;
                    elementType.ApplicableOccurrence = columnLabel;


                    beam.Tag = typeText;
                    beam.Name = typeText;
                    beam.Description = typeText;
                    beam.AddDefiningType(elementType);

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
                shape.ContextOfItems = model.Instances.FirstOrDefault<IfcProject>(e => e.Name == genericProducthndz.BuildingStorey.Building.Project.Name).ModelContext;

                //shape.ContextOfItems = model.IfcProject.ModelContext();
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
                beam.OwnerHistory = GetDefaultOwnerHistory();
                //beam.OwnerHistory.OwningApplication = model.DefaultOwningApplication;
                #endregion

                //validate write any errors to the console and commit if OK, otherwise abort
                //if (.Validate(txn.Modified(), File.CreateText("E:\\Column" + column.GlobalId + "Errors.txt")) == 0)
                // if (.Validate(txn.Modified(), File.CreateText(temp + "Column" + column.GlobalId + "Errors.txt")) == 0)
                try
                {
                    txn.Commit();
                    return beam;
                }
                catch (Exception)
                {

                    txn.RollBack();
                    return null;
                }
            }
        }

        //private static void CreateWexBimFromIfc( string ifcFileFullPath)
        //{

        //    var wexBimFileName = Path.ChangeExtension(ifcFileFullPath, "wexbim");
        //    var xbimFile = Path.ChangeExtension(ifcFileFullPath, "xbim");




        //    if ( != null)
        //    {

        //        var context = new Xbim3DModelContext();
        //        context.CreateContext(/*geomStorageType: XbimGeometryType.PolyhedronBinary);
        //        var wexBimFilename = Path.ChangeExtension(ifcFileFullPath, "wexBIM");
        //        using (var wexBiMfile = new FileStream(wexBimFilename, FileMode.Create, FileAccess.Write))
        //        {
        //            using (var wexBimBinaryWriter = new BinaryWriter(wexBiMfile))
        //            {
        //                Console.WriteLine("Creating " + wexBimFilename);
        //                //context.Write(wexBimBinaryWriter);
        //                wexBimBinaryWriter.Close();
        //            }
        //            wexBiMfile.Close();
        //        }







        //        //    using (var wexBimFile = new FileStream(wexBimFileName, FileMode.Create))
        //        //{

        //        //    using (var binaryWriter = new BinaryWriter(wexBimFile))
        //        //    {



        //        //        using (var model = new IfcStore())
        //        //        {
        //        //            model.CreateFrom(ifcFileFullPath, xbimFile, null, true, false);

        //        //            Xbim3DModelContext geomContext = new Xbim3DModelContext();

        //        //            geomContext.CreateContext(XbimGeometryType.PolyhedronBinary);

        //        //            geomContext.Write(binaryWriter);

        //        //        }
        //        //    }
        //    }
        //}

        private static void IfcToCoBieLiteUkTest(string ifcFilePath)
        {
            //using (var m = new IfcStore())
            //{
            //    var ifcTestFile = ifcFilePath;
            //    var xbimTestFile = Path.ChangeExtension(ifcTestFile, "xbim");
            //    var jsonFile = Path.ChangeExtension(ifcTestFile, "json");
            //    m.CreateFrom(ifcTestFile, xbimTestFile, null, true, true);
            //    var facilities = new List<Facility>();

            //    OutPutFilters rolefilters = new OutPutFilters();
            //    RoleFilter reqRoles = RoleFilter.Unknown; //RoleFilter.Architectural |  RoleFilter.Mechanical | RoleFilter.Electrical | RoleFilter.FireProtection | RoleFilter.Plumbing;
            //    rolefilters.ApplyRoleFilters(reqRoles);

            //    var ifcToCoBieLiteUkExchanger = new IfcToCOBieLiteUkExchanger(m, facilities, null, rolefilters);
            //    facilities = ifcToCoBieLiteUkExchanger.Convert();

            //    foreach (var facilityType in facilities)
            //    {
            //        facilityType.WriteJson(jsonFile, true);

            //    }

            //}
        }

        private static Point3d GetPointOnArc(Arc rhArc, double param)
        {
            double r = rhArc.Radius;
            double angleAtParam = rhArc.StartAngle + (Math.Abs((2.0 * Math.PI) - rhArc.StartAngle + rhArc.EndAngle) % (2.0 * Math.PI)) * param;
            double xAtParam = r * Math.Cos(angleAtParam) + rhArc.Center.X;
            double yAtParam = r * Math.Sin(angleAtParam) + rhArc.Center.Y;
            Point3d pointAtParam = new Point3d(xAtParam, yAtParam, 0);
            return pointAtParam;
        }
    }
}
