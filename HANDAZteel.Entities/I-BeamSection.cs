using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;


namespace HANDAZ.PEB.Entities
{
    public class I_BeamSection
    {
        //XbimModel Model;
        //IfcBeamType elementType;
        //IfcLabel elementLabel;
        //IfcMaterial ifcMaterial;
        //IfcIShapeProfileDef profile;
        string sectionName;
        string sectionTag;
        double profileHeight;
        double profileWidth;
        double flangThickness;
        double webThicnkness;
        double filletRadius;
        Node midpoint;
        Material material;
        public string SectionName
        {
            get
            {
                return sectionName;
            }

            set
            {
                sectionName = value;
            }
        }
        public string SectionTag
        {
            get
            {
                return sectionTag;
            }

            set
            {
                sectionTag = value;
            }
        }
        public double ProfileHeight
        {
            get
            {
                return profileHeight;
            }

            set
            {
                profileHeight = value;
            }
        }
        public double ProfileWidth
        {
            get
            {
                return profileWidth;
            }

            set
            {
                profileWidth = value;
            }
        }
        public double FlangThickness
        {
            get
            {
                return flangThickness;
            }

            set
            {
                flangThickness = value;
            }
        }
        public double WebThicnkness
        {
            get
            {
                return webThicnkness;
            }

            set
            {
                webThicnkness = value;
            }
        }
        public double FilletRadius
        {
            get
            {
                return filletRadius;
            }

            set
            {
                filletRadius = value;
            }
        }
        //public IfcMaterial IfcMaterial
        //{
        //    get
        //    {
        //        return ifcMaterial;
        //    }

        //    set
        //    {
        //        ifcMaterial = value;
        //    }
        //}
        //public IfcIShapeProfileDef Profile
        //{
        //    get
        //    {
        //        return profile;
        //    }

        //    set
        //    {
        //        profile = value;
        //    }
        //}
        //public IfcLabel ElementLabel
        //{
        //    get
        //    {
        //        return elementLabel;
        //    }

        //    set
        //    {
        //        elementLabel = value;
        //    }
        //}
        //public IfcBeamType ElementType
        //{
        //    get
        //    {
        //        return elementType;
        //    }

        //    set
        //    {
        //        elementType = value;
        //    }
        //}

        public Node Midpoint
        {
            get
            {
                return midpoint;
            }

            set
            {
                midpoint = value;
            }
        }

        public Material Material
        {
            get
            {
                return material;
            }

            set
            {
                material = value;
            }
        }

        public I_BeamSection(string _SectionName, string _SectionTag, double _ProfileHeight, double _ProfileWidth
            , double _FlangThickness, double _WebThicnkness, double _FilletRadius, Material _material)
        {
            Material = _material;
            SectionName = _SectionName;
            SectionTag = _SectionTag;
            ProfileHeight = _ProfileHeight;
            ProfileWidth = _ProfileWidth;
            FlangThickness = _FlangThickness;
            WebThicnkness = _WebThicnkness;
            FilletRadius = _FilletRadius;
            Midpoint = new Node(ProfileWidth / 2, ProfileHeight / 2,0);

        }
        //    #region
        //    public bool CreateIbeamSection(XbimModel _Model, string _SectionName, string _SectionTag, double _ProfileHeight, double _ProfileWidth
        //        , double _FlangThickness, double _WebThicnkness, double _FilletRadius)
        //    {
        //        using (XbimReadWriteTransaction txn = Model.BeginTransaction("Set Beam Specifications"))
        //        {

        //            IfcMaterial _material = Model.Instances.New<IfcMaterial>();
        //            IfcBeamType _elementType = Model.Instances.New<IfcBeamType>();
        //            IfcLabel _elementLabel = new IfcLabel(_SectionName);
        //            IfcIShapeProfileDef _profile = Model.Instances.New<IfcIShapeProfileDef>();

        //            _profile.ProfileName = SectionName;
        //            _profile.FilletRadius = FilletRadius;
        //            _profile.FlangeThickness = FlangThickness;
        //            _profile.OverallDepth = ProfileHeight;
        //            _profile.OverallWidth = ProfileWidth;
        //            _profile.WebThickness = WebThicnkness;
        //            _profile.ProfileType = IfcProfileTypeEnum.AREA;

        //            _material.Name = "Steel";

        //            _elementType.PredefinedType = IfcBeamTypeEnum.BEAM;
        //            _elementType.Tag = _SectionTag;
        //            _elementType.Name = _SectionName;
        //            _elementType.ElementType = elementLabel;
        //            _elementType.ApplicableOccurrence = elementLabel;

        //            Profile = _profile;
        //            IfcMaterial = _material;
        //            ElementLabel = _elementLabel;
        //            ElementType = _elementType;

        //            if (Model.Validate(txn.Modified(), Console.Out) == 0)
        //            {
        //                txn.Commit();
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        }

        //    }
        //    }
        //#endregion
    }
}
