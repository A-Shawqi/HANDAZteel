using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;


namespace HANDAZ.PEB.Entities
{
    public class Beam
    {

        
        bool isTapered;
       
        private static int idCounter;
        public Node BeamStart { get; set; }
        public Node BeamEnd { get; set; }
        public Line Beamline { get; set; }
        public BeamTaperedProfile BeamSectionAtStartNode { get; set; }
        public BeamTaperedProfile BeamSectionATEndNode { get; set; }
        public I_BeamSection BeamSection { get; set; }
        public int Id { get; set; }
        enum BeamUse
        {
            MainBeam,
            SecondaryBeam
        }
        public Beam() { }
        public Beam(Node _BeamStart, Node _BeamEnd, I_BeamSection _BeamSection)
        {
            Id = ++idCounter;
            BeamStart = _BeamStart;
            BeamEnd = _BeamEnd;
            Point3d beamStart = new Point3d(_BeamStart.X, _BeamStart.Y, _BeamStart.Z);
            Point3d beamEnd = new Point3d(_BeamEnd.X, _BeamEnd.Y, _BeamEnd.Z);
            Beamline = new Line(beamStart, beamEnd);
            BeamSection = _BeamSection;
            isTapered = false;

        }
        public Beam(Node _BeamStart, Node _BeamEnd, BeamTaperedProfile _taperedAtStartNode,
        BeamTaperedProfile _taperedATEndNode)
        {
            Id = ++idCounter;
            BeamStart = _BeamStart;
            BeamEnd = _BeamEnd;
            Point3d beamStart = new Point3d(_BeamStart.X, _BeamStart.Y, _BeamStart.Z);
            Point3d beamEnd = new Point3d(_BeamEnd.X, _BeamEnd.Y, _BeamEnd.Z);
            Beamline = new Line(beamStart, beamEnd);
            BeamSectionAtStartNode = _taperedAtStartNode;
            BeamSectionATEndNode = _taperedATEndNode;
            isTapered = true;

            //public Beam(Node _BeamStart, Node _BeamEnd, BeamTaperedProfile _taperedAtStartNode, BeamTaperedProfile _taperedATEndNode, Results _beamLoads) :
            //    this(_BeamStart, _BeamEnd, _taperedAtStartNode, _taperedATEndNode)
            //{
            //    BeamLoad = _beamLoads;
            //}


            #region XbimBeam
            //public bool CreateIfcBeam(XbimModel _Model, Point3d _BeamStart, Point3d _BeamEnd, I_BeamSection _BeamSection)
            //{
            //    using (XbimReadWriteTransaction txn = Model.BeginTransaction("Create Column"))
            //    {
            //        IfcBeam Beam = Model.Instances.New<IfcBeam>();
            //        Beam.Name = "Beam" + BeamSection.Profile.ProfileName;
            //        //Beam.SetDefiningType(BeamSection.ElementType, Model);
            //        //Beam.SetMaterial(BeamSection.Material);
            //        ///=======================
            //        /// //=====================
            //        /// Huge Mistake , To work it has formulate a new profile which is weird as fuck 
            //        ////represent wall as a rectangular profile
            //        //IfcMaterial _material = Model.Instances.New<IfcMaterial>();
            //        //_material.Name = "Steel";

            //        Profile = Model.Instances.New<IfcIShapeProfileDef>();
            //        Profile.ProfileName = _BeamSection.Profile.ProfileName;
            //        Profile.FilletRadius = _BeamSection.Profile.FilletRadius;
            //        Profile.FlangeThickness = _BeamSection.Profile.FlangeThickness;
            //        Profile.OverallDepth = _BeamSection.Profile.OverallDepth;
            //        Profile.OverallWidth = _BeamSection.Profile.OverallWidth;
            //        Profile.WebThickness = _BeamSection.Profile.WebThickness;
            //        Profile.ProfileType = IfcProfileTypeEnum.AREA;
            //        // BeamSection.Profile

            //        IfcCartesianPoint insertPoint = Model.Instances.New<IfcCartesianPoint>();
            //        insertPoint.SetXY(0, 0); //insert at arbitrary position
            //        Profile.Position = Model.Instances.New<IfcAxis2Placement2D>();
            //        Profile.Position.Location = insertPoint;

            //        //model as a swept area solid
            //        IfcExtrudedAreaSolid body = Model.Instances.New<IfcExtrudedAreaSolid>();
            //        body.Depth = Beamline.Length;
            //        body.SweptArea = Profile;
            //        body.ExtrudedDirection = Model.Instances.New<IfcDirection>();
            //        body.ExtrudedDirection.SetXYZ(0, 0, 1);

            //        //parameters to insert the geometry in the model
            //        IfcCartesianPoint origin = Model.Instances.New<IfcCartesianPoint>();
            //        origin.SetXYZ(0, 0, 0);
            //        body.Position = Model.Instances.New<IfcAxis2Placement3D>();
            //        body.Position.Location = origin;

            //        //Create a Definition shape to hold the geometry
            //        IfcShapeRepresentation shape = Model.Instances.New<IfcShapeRepresentation>();
            //        //k shape.ContextOfItems = Model.Instances.IfcProject.ModelContext();
            //        shape.RepresentationType = "SweptSolid";
            //        shape.RepresentationIdentifier = "Body";
            //        shape.Items.Add(body);

            //        //Create a Product Definition and add the model geometry to the wall
            //        IfcProductDefinitionShape rep = Model.Instances.New<IfcProductDefinitionShape>();
            //        rep.Representations.Add(shape);

            //        Beam.Representation = rep;

            //        //now place the wall into the model
            //        IfcLocalPlacement lp = Model.Instances.New<IfcLocalPlacement>();
            //        IfcAxis2Placement3D ax3d = Model.Instances.New<IfcAxis2Placement3D>();
            //        ax3d.Location = origin;
            //        ax3d.RefDirection = Model.Instances.New<IfcDirection>();
            //        ax3d.RefDirection.SetXYZ(0, 1, 0);
            //        ax3d.Axis = Model.Instances.New<IfcDirection>();
            //        ax3d.Axis.SetXYZ(0, 0, 1);


            //        lp.RelativePlacement = ax3d;
            //        Beam.ObjectPlacement = lp;

            //        //validate write any errors to the console and commit if ok, otherwise abort
            //        if (model.Validate(Console.Out) == 0)
            //        {
            //            txn.Commit();
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //  }
            #endregion
        }
    }
}