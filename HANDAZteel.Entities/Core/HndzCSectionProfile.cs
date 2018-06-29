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
    [KnownType(typeof(HndzCSectionProfile))]
    public class HndzCSectionProfile : HndzProfile
    {
        #region Properties
     [DataMember, XmlAttribute]
        public SectionChannel C_Section { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor takes profile name, description, Section and X-Direction
        /// </summary>
        /// <param name="name">name for the profile section i.e IPE 280 HEA 300 ...</param>
        /// <param name="orientation">description for the profile</param>
        /// <param name="CSection">section represents flange and web dimensions of steel channel</param>
        /// <param name="centroid">the centroid of the section</param>
        /// <param name="direction">Reference X-Direction of the profile</param>
        public HndzCSectionProfile(string name, string orientation, SectionChannel CSection,
                                        Vector2d direction) : base(name, orientation, direction)
        {
            C_Section = CSection;
        }

        /// <summary>
        /// Constructor takes profile Section and X-Direction
        /// </summary>
        /// <param name="name">name for the profile section i.e IPE 280 HEA 300 ...</param>
        /// <param name="description">description for the profile</param>
        /// <param name="CSection">section represents flange and web dimensions of steel channel</param>
        /// <param name="centroid">the centroid of the section</param>
        /// <param name="orientation">Reference X-Direction of the profile</param>
        public HndzCSectionProfile(SectionChannel CSection,
                                        Vector2d orientation) : this(HndzResources.DefaultName, HndzResources.DefaultDescription, CSection, orientation)
        {
        }
        /// <summary>
        /// Constructor on the fly
        /// </summary>
        public HndzCSectionProfile() : base(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }
        #endregion
        #region Methods
        public Curve ConvertItoCurve()
        {
            double h = C_Section.d;
            double tw = C_Section.t_w;
            double w = C_Section.b_f;
            double tf = C_Section.t_f;


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
            double h = C_Section.d * Math.Cos(xDeflection) + C_Section.d * Math.Sin(xDeflection);
            double tw = C_Section.t_w * Math.Cos(xDeflection) + C_Section.t_w * Math.Sin(xDeflection);
            double w = C_Section.b_f * Math.Cos(xDeflection) + C_Section.b_f * Math.Sin(xDeflection);
            double tf = C_Section.t_f * Math.Cos(xDeflection) + C_Section.t_f * Math.Sin(xDeflection);


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
