using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Wosad.Common.Section.SectionTypes;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzISectionProfile))]
    public class HndzISectionProfile : HndzProfile
    {
        #region Properties
      [DataMember, XmlAttribute]
        public SectionI I_Section { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor takes profile name, description, Section and X-Direction
        /// </summary>
        /// <param name="name">name for the profile section i.e IPE 280 HEA 300 ...</param>
        /// <param name="description">description for the profile</param>
        /// <param name="IBuiltUpSection">section represents flange and web dimensions</param>
        /// <param name="centroid">the centroid of the section</param>
        /// <param name="direction">Reference X-Direction of the profile</param>
        public HndzISectionProfile(string name, string description, SectionI IBuiltUpSection,
                                        Vector2d direction=default(Vector2d)) : base(name, description, direction)
        {
            I_Section = IBuiltUpSection;
        }

        /// <summary>
        /// Constructor takes profile Section and X-Direction
        /// </summary>
        /// <param name="name">name for the profile section i.e IPE 280 HEA 300 ...</param>
        /// <param name="description">description for the profile</param>
        /// <param name="IBuiltUpSection">section represents flange and web dimensions</param>
        /// <param name="centroid">the centroid of the section</param>
        /// <param name="orientation">Reference X-Direction of the profile</param>
        public HndzISectionProfile(SectionI IBuiltUpSection,
                                        Vector2d orientation = default(Vector2d)) : this(HndzResources.DefaultName,
                                            HndzResources.DefaultDescription, IBuiltUpSection, orientation)
        {
        }

        public HndzISectionProfile() : base(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }
        #endregion
        #region Methods
        public Curve ConvertItoCurve()
        {
            double h = I_Section.d;
            double tw = I_Section.t_w;
            double w = I_Section.b_f;
            double tf = I_Section.tf;


            ICollection<Point3d> points = new Point3d[] {
                new Point3d(-w / 2,-h / 2,0),
                new Point3d(-w / 2,-h / 2 + tf,0),
                new Point3d(-tw / 2,-h / 2 + tf,0),
                new Point3d(-tw / 2, h / 2 - tf,0),
                new Point3d(-w / 2, h / 2 - tf,0),
                new Point3d(-w / 2, h / 2,0),
                new Point3d(w / 2, h / 2,0),
                new Point3d(w / 2, h / 2 - tf,0),
                new Point3d(-w / 2,-h / 2,0),
            };
            return new PolylineCurve(points);
        }
        public List<Point3d> ConvertItoPoints(double xDeflection)
        {
            double h = I_Section.d * Math.Cos(xDeflection) + I_Section.d * Math.Sin(xDeflection);
            double tw = I_Section.t_w * Math.Cos(xDeflection) + I_Section.t_w * Math.Sin(xDeflection);
            double w = I_Section.b_f * Math.Cos(xDeflection) + I_Section.b_f * Math.Sin(xDeflection);
            double tf = I_Section.tf * Math.Cos(xDeflection) + I_Section.tf * Math.Sin(xDeflection);


            ICollection<Point3d> points = new Point3d[] {

                new Point3d(-w/2 /* * Math.Sin(xDeflection) + -w/2  * Math.Cos(xDeflection)*/ , -h/2       /** Math.Cos(xDeflection)  +-h/2       * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-w/2 /* * Math.Sin(xDeflection) + -w/2  * Math.Cos(xDeflection)*/ , -h/2 + tf  /** Math.Cos(xDeflection)  +-h/2 + tf  * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-tw/2/* * Math.Sin(xDeflection) + -tw/2 * Math.Cos(xDeflection)*/ , -h/2 + tf  /** Math.Cos(xDeflection)  +-h/2 + tf  * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-tw/2/* * Math.Sin(xDeflection) + -tw/2 * Math.Cos(xDeflection)*/ , h/2 - tf   /** Math.Cos(xDeflection)  +h/2 - tf   * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-w/2 /* * Math.Sin(xDeflection) + -w/2  * Math.Cos(xDeflection)*/ , h/2 - tf   /** Math.Cos(xDeflection)  +h/2 - tf   * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-w/2 /* * Math.Sin(xDeflection) + -w/2  * Math.Cos(xDeflection)*/ , h/2        /** Math.Cos(xDeflection)  +h/2        * Math.Sin(xDeflection)*/ ,0),
                new Point3d(w/2  /* * Math.Sin(xDeflection) + w/2   * Math.Cos(xDeflection)*/ , h/2        /** Math.Cos(xDeflection)  +h/2        * Math.Sin(xDeflection)*/ ,0),
                new Point3d(w/2  /* * Math.Sin(xDeflection) + w/2   * Math.Cos(xDeflection)*/ , h/2 - tf   /** Math.Cos(xDeflection)  +h/2 - tf   * Math.Sin(xDeflection)*/ ,0),
                new Point3d(tw/2 /* * Math.Sin(xDeflection) + tw/2  * Math.Cos(xDeflection)*/ , h/2 - tf   /** Math.Cos(xDeflection)  +h/2 - tf   * Math.Sin(xDeflection)*/ ,0),
                new Point3d(tw/2 /* * Math.Sin(xDeflection) + tw/2  * Math.Cos(xDeflection)*/ , -h/2 + tf  /** Math.Cos(xDeflection)  +-h/2 + tf  * Math.Sin(xDeflection)*/ ,0),
                new Point3d(w/2  /* * Math.Sin(xDeflection) + w/2   * Math.Cos(xDeflection)*/ , -h/2 + tf  /** Math.Cos(xDeflection)  +-h/2 + tf  * Math.Sin(xDeflection)*/ ,0),
                new Point3d(w/2  /* * Math.Sin(xDeflection) + w/2   * Math.Cos(xDeflection)*/ , -h/2       /** Math.Cos(xDeflection)  +-h/2       * Math.Sin(xDeflection)*/ ,0),
                new Point3d(-w/2 /* * Math.Sin(xDeflection) + -w/2  * Math.Cos(xDeflection)*/ , -h/2       /** Math.Cos(xDeflection)  +-h/2       * Math.Sin(xDeflection)*/ ,0)
            };
            return new List<Point3d>(points);
        }
        #endregion
    }
}
