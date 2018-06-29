using HANDAZ.Entities;
using HANDAZ.PEB.BIM;
using HANDAZteel.BIMtest;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Section.SectionTypes;
using Xbim.Common.Geometry;
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
using XbimGeometry.Interfaces;

namespace HANDAZ.IFCGenerator
{
    public class IFCbuilder
    {
        static void Main(string[] args)
        {
            HndzProject project = new HndzProject();
            HndzBuilding building = new HndzBuilding(project);
            HndzStorey storey = new HndzStorey(building);

            HndzFrameSingleBay3D frame3D = new HndzFrameSingleBay3D(/*length*/24000,  /*baySpacing*/ 6000,   /*Width*/20000, 
                 5000, 2000, HndzLocationEnum.Cairo,HndzRoofSlopeEnum.From1To5, HndzRoofAccessibilityEnum.Inaccessible, 
                 HndzBuildingEnclosingEnum.PartiallyEnclosed, HndzImportanceFactorEnum.II,null,null,null,null,null,storey);
         bool isGenerated=   ConvertToIFC.GenerateIFCProject(project, "E:\\test13June");

            if (isGenerated)
            {
                Console.WriteLine("      Succeed       ");
            }
            else
            {
                Console.WriteLine("      Failed        ");

            }
            Console.ReadKey();
        }
    }
}





#region Assemble Pyramid

//IfcCartesianPoint ptop = model.Instances.New<IfcCartesianPoint>();
//IfcCartesianPoint p1 = model.Instances.New<IfcCartesianPoint>();
//IfcCartesianPoint p2 = model.Instances.New<IfcCartesianPoint>();
//IfcCartesianPoint p3 = model.Instances.New<IfcCartesianPoint>();
//ptop.SetXYZ(0, 0, 1000);
//p1.SetXYZ(-1000, 1000, 0);
//p2.SetXYZ(1000, 1000, 0);
//p3.SetXYZ(0, -1000, 0);
//IfcClosedShell shell1 = model.Instances.New<IfcClosedShell>();


////   Face No.1
//IfcPolyLoop loop1 = model.Instances.New<IfcPolyLoop>();
//loop1.Polygon.Add(p1);
//loop1.Polygon.Add(p2);
//loop1.Polygon.Add(p3);

//IfcFaceOuterBound fac1 = model.Instances.New<IfcFaceOuterBound>();
//fac1.Bound = loop1;
//fac1.Orientation = new IfcBoolean(true);


//IfcFace face1 = model.Instances.New<IfcFace>();
//face1.Bounds.Add(fac1);
//shell.CfsFaces.Add(face1);


///// End Face No.1
///// 


////   Face No.2
//IfcPolyLoop loop2 = model.Instances.New<IfcPolyLoop>();
//loop2.Polygon.Add(ptop);
//loop2.Polygon.Add(p1);
//loop2.Polygon.Add(p2);

//IfcFaceOuterBound fac2 = model.Instances.New<IfcFaceOuterBound>();
//fac2.Bound = loop2;
//fac2.Orientation = new IfcBoolean(true);

//IfcFace face2 = model.Instances.New<IfcFace>();
//face2.Bounds.Add(fac2);
//shell.CfsFaces.Add(face2);

///// End Face No.2
///// 

////   Face No.3
//IfcPolyLoop loop3 = model.Instances.New<IfcPolyLoop>();
//loop3.Polygon.Add(ptop);
//loop3.Polygon.Add(p2);
//loop3.Polygon.Add(p3);

//IfcFaceOuterBound fac3 = model.Instances.New<IfcFaceOuterBound>();
//fac3.Bound = loop3;
//fac3.Orientation = new IfcBoolean(true);

//IfcFace face3 = model.Instances.New<IfcFace>();
//face3.Bounds.Add(fac3);
//shell.CfsFaces.Add(face3);

///// End Face No.3
///// 

////   Face No.4
//IfcPolyLoop loop4 = model.Instances.New<IfcPolyLoop>();
//loop4.Polygon.Add(ptop);
//loop4.Polygon.Add(p1);
//loop4.Polygon.Add(p3);

//IfcFaceOuterBound fac4 = model.Instances.New<IfcFaceOuterBound>();
//fac4.Bound = loop4;
//fac4.Orientation = new IfcBoolean(true);

//IfcFace face4 = model.Instances.New<IfcFace>();
//face4.Bounds.Add(fac4);
//shell.CfsFaces.Add(face4);

///// End Face No.3

#endregion