#region Copyright
/*Copyright (C) 2015 Wosad Inc

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wosad.Common.Mathematics;
using Wosad.Common.Section.Interfaces;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ServiceModel;

namespace Wosad.Common.Section.SectionTypes
{
    /// <summary>
    /// Generic I-shape with geometric parameters provided in a constructor.
    /// This shape has sharp corners, as is typical for built-up shapes.
    /// </summary>
    /// 

    [DataContract]
    [Serializable]
    [XmlSerializerFormat]
    [KnownType(typeof(SectionI))]
    public class SectionI : CompoundShape, ISectionI
    {
        public SectionI(string Name, double d, double b_f, double t_f,
          double t_w)
          : base(Name)
        {
            this._d = d;
            this._b_f = b_f;
            this._t_f = t_f;
            this._b_fTop = b_f;
            this._t_fTop = t_f;
            this._b_fBot = b_f;
            this._t_fBot = t_f;
            this._t_w = t_w;
        }

        public SectionI(string Name, double d, double b_fTop, double b_fBot,
            double t_fTop, double t_fBot, double t_w)
            : base(Name)
        {
            this._d = d;
            this._b_fTop = b_fTop;
            this._t_fTop = t_fTop;
            this._b_fBot = b_fBot;
            this._t_fBot = t_fBot;
            this._t_w = t_w;
        }

        #region Properties specific to I-Beam

        [DataMember, XmlAttribute]

        private double _b_f;
        [DataMember, XmlAttribute]

        public double b_f
        {
            get { return _b_f; }
            set { _b_f = value; }
        }

        [DataMember, XmlAttribute]

        private double _d;
        [DataMember, XmlAttribute]

        public double d
        {
            get { return _d; }
            set { _d = value; }

        }

        [DataMember, XmlAttribute]

        private double _h_o;
        [DataMember, XmlAttribute]

        public double h_o
        {
            get
            {
                double df = _d - (this.tf / 2 + this.t_fBot / 2);
                return _h_o;
            }
            set
            {
                _h_o = value;
            }
        }
        [DataMember, XmlAttribute]

        private double _b_fTop;
        [DataMember, XmlAttribute]

        public double b_fTop
        {
            get { return _b_fTop; }
            set {  _b_fTop=value; }
        }
        [DataMember, XmlAttribute]

        private double _t_f;
        [DataMember, XmlAttribute]

        public double tf
        {
            set { _t_f = value; }
            get { return _t_f; }
        }
        [DataMember, XmlAttribute]

        private double _b_fBot;
        [DataMember, XmlAttribute]

        public double b_fBot
        {
            set { _b_fBot = value; }
            get { return _b_fBot; }
        }
        [DataMember, XmlAttribute]

        private double _t_fBot;
        [DataMember, XmlAttribute]

        public double t_fBot
        {
            set { _t_fBot = value; }
            get { return _t_fBot; }
        }

        [DataMember, XmlAttribute]

        private double _t_fTop;
        [DataMember, XmlAttribute]

        public double t_fTop
        {
            set { _t_fTop = value; }
            get { return _t_fTop; }
        }
        [DataMember, XmlAttribute]

        private double _t_w;
        [DataMember, XmlAttribute]

        public double t_w
        {
            set { _t_w = value; }
            get { return _t_w; }
        }

        //private double filletDistance;

        //public double FilletDistance
        //{
        //    get { return filletDistance; }
        //    set { filletDistance = value; }
        //}


        //[DataMember, XmlAttribute]
        double _T;
        //[DataMember, XmlAttribute]

        public double T
        {
            set
            {
                T = value;
            }
            get
            {
                _T = _d - t_fBot - tf;
                return _T;
            }
        }
        #endregion


        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// x-axis, each occupying full width of section.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            double t_f = this.tf;
            double b_f = this.b_fTop;

            CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d - t_f / 2));
            CompoundShapePart BottomFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, t_f / 2));
            CompoundShapePart Web = new CompoundShapePart(t_w, d - 2 * t_f, new Point2D(0, d / 2));

            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                 TopFlange,
                 Web,
                 BottomFlange
            };
            return rectX;
        }

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// y-axis, each occupying full height of section. The rectangles are rotated 90 deg., 
        /// because internally the properties are calculated  with respect to x-axis.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            double FlangeThickness = this.tf;
            double FlangeWidth = this.b_fTop;


            //review this ....

            // I-shape converted to X-shape 
            double FlangeOverhang = (b_f - t_w) / 2.0;
            CompoundShapePart LeftFlange = new CompoundShapePart(2 * tf, FlangeOverhang, new Point2D(0, b_f - FlangeOverhang / 2));
            CompoundShapePart RightFlange = new CompoundShapePart(2 * tf, FlangeOverhang, new Point2D(0, FlangeOverhang / 2));
            CompoundShapePart Web = new CompoundShapePart(d, t_w, new Point2D(0, b_f / 2));

            List<CompoundShapePart> rectY = new List<CompoundShapePart>()
            {
                LeftFlange,
                Web,
                RightFlange
            };
            return rectY;
        }

        /// <summary>
        /// From:
        /// TORSIONAL SECTION PROPERTIES OF STEEL SHAPES
        ///Canadian Institute of Steel Construction, 2002
        /// </summary>
        protected override void CalculateWarpingConstant()
        {
            double d = this.d;
            double t_1 = tf;
            double t_2 = t_fBot;
            double b_1 = b_fTop;
            double b_2 = b_fBot;

            double d_p = d - ((t_1 + t_2) / 2);
            double a = 1 / (Math.Pow(1 + (b_1 / b_2), 3) * (t_1 / t_2));
            this._C_w = (Math.Pow(d_p, 2) * Math.Pow(b_1, 3) * t_1 * a) / 12;

        }

        //[DataMember, XmlAttribute]
        public double h_web
        {
            get
            {
                return d - (tf + t_fBot);
            }
            set
            {
                h_web = value;
            }

        }
        // public SectionI(string Name, double d, double b_f, double t_f, 
        //     double t_w)
        //     : base(Name)
        // {

        //     this._d = d;
        //     this._b_f = b_f;
        //     this._t_f = t_f;
        //     this._b_fTop = b_f;
        //     this._t_fTop = t_f;
        //     this._b_fBot = b_f;
        //     this._t_fBot = t_f;
        //     this._t_w = t_w;
        // }

        // public SectionI(string Name, double d, double b_fTop, double b_fBot,
        //     double t_fTop, double t_fBot, double t_w)
        //     : base(Name)
        // {
        //     this._d = d;
        //     this._b_fTop = b_fTop;
        //     this._t_fTop = t_fTop;
        //     this._b_fBot = b_fBot;
        //     this._t_fBot = t_fBot;
        //     this._t_w = t_w;
        // }

        // #region Properties specific to I-Beam


        // private double _b_f;
        //[DataMember, XmlAttribute]

        // public double b_f
        // {
        //     get { return _b_f; }
        //     set { _b_f = value; }
        // }

        // private double _d;
        //[DataMember, XmlAttribute]
        // public double d
        // {
        //     get { return _d; }
        //     set { _d = value; }
        // }

        // private double _h_o;
        ////[DataMember, XmlAttribute]
        // public double h_o
        // {
        //     get {
        //         double df = _d - (this.tf / 2 + this.t_fBot / 2);
        //         return _h_o; }
        //     set { h_o = value; }

        // }
        // private double _b_fTop;
        //[DataMember, XmlAttribute]
        // public double b_fTop
        // {
        //     get { return _b_fTop; }

        //     set { _b_fTop = value; }


        // }
        // private double _t_f;
        //[DataMember, XmlAttribute]
        // public double tf
        // {
        //     get { return _t_f; }
        //     set { _t_f = value; }

        // }
        // private double _b_fBot;
        //[DataMember, XmlAttribute]
        // public double b_fBot
        // {
        //     get { return _b_fBot; }
        //     set { _b_fBot = value; }

        // }
        // private double _t_fBot;
        //[DataMember, XmlAttribute]
        // public double t_fBot
        // {
        //     get { return _t_fBot; }
        //     set { _t_fBot = value; }

        // }
        // private double _t_fTop;
        //[DataMember, XmlAttribute]
        // public double t_fTop
        // {
        //     get { return _t_fTop; }
        //     set { _t_fTop = value; }
        // }
        // private double _t_w;
        //[DataMember, XmlAttribute]
        // public double t_w
        // {
        //     get { return _t_w; }
        //     set { _t_w = value; }
        // }


        // //private double filletDistance;

        // //public double FilletDistance
        // //{
        // //    get { return filletDistance; }
        // //    set { filletDistance = value; }
        // //}


        // double _T;
        //[DataMember, XmlAttribute]
        // public double T
        // {
        //     get
        //     {
        //         _T = _d - t_fBot - tf;
        //         return _T;
        //     }
        //     set { _T = value; }

        // }
        // #endregion


        // /// <summary>
        // /// Defines a set of rectangles for analysis with respect to 
        // /// x-axis, each occupying full width of section.
        // /// </summary>
        // /// <returns>List of analysis rectangles</returns>
        // public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        // {
        //     double t_f = this.tf;
        //     double b_f = this.b_fTop;

        //     CompoundShapePart TopFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, d - t_f / 2));
        //     CompoundShapePart BottomFlange = new CompoundShapePart(b_f, t_f, new Point2D(0, t_f / 2));
        //     CompoundShapePart Web = new CompoundShapePart(t_w, d - 2 * t_f, new Point2D(0, d / 2));

        //     List<CompoundShapePart> rectX = new List<CompoundShapePart>()
        //     {
        //          TopFlange,  
        //          Web,
        //          BottomFlange
        //     };
        //     return rectX;
        // }

        // /// <summary>
        // /// Defines a set of rectangles for analysis with respect to 
        // /// y-axis, each occupying full height of section. The rectangles are rotated 90 deg., 
        // /// because internally the properties are calculated  with respect to x-axis.
        // /// </summary>
        // /// <returns>List of analysis rectangles</returns>
        // public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        // {
        //     double FlangeThickness = this.tf;
        //     double FlangeWidth = this.b_fTop;


        //     //review this ....

        //     // I-shape converted to X-shape 
        //     double FlangeOverhang = (b_f - t_w) / 2.0;
        //     CompoundShapePart LeftFlange = new CompoundShapePart(2* tf, FlangeOverhang, new Point2D(0, b_f - FlangeOverhang/2));
        //     CompoundShapePart RightFlange = new CompoundShapePart(2*tf, FlangeOverhang, new Point2D(0, FlangeOverhang/2));
        //     CompoundShapePart Web = new CompoundShapePart(d, t_w, new Point2D(0, b_f / 2));

        //     List<CompoundShapePart> rectY = new List<CompoundShapePart>()
        //     {
        //         LeftFlange,
        //         Web,
        //         RightFlange
        //     };
        //     return rectY;
        // }

        // /// <summary>
        // /// From:
        // /// TORSIONAL SECTION PROPERTIES OF STEEL SHAPES
        // ///Canadian Institute of Steel Construction, 2002
        // /// </summary>
        // protected override void CalculateWarpingConstant()
        // {
        //     double d = this.d;
        //     double t_1 = tf;
        //     double t_2 = t_fBot;
        //     double b_1 = b_fTop;
        //     double b_2 = b_fBot;

        //     double d_p=d-((t_1+t_2) / 2);
        //     double a =1/(Math.Pow(1+(b_1/b_2 ),3)*(t_1/t_2 ) );
        //     this._C_w =(Math.Pow(d_p,2)*Math.Pow(b_1,3)* t_1*a)/12;

        // }
        ////[DataMember, XmlAttribute]

        // public double h_web
        // {
        //     get 
        //     {
        //         return d - (tf + t_fBot);
        //     }
        //     set { h_web = value; }

        // }
    }
}
