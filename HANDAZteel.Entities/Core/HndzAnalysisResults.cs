using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
   [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzAnalysisResults:HndzRoot
    {
       [DataMember, XmlAttribute]
        private double station;
       [DataMember, XmlAttribute]
        private string loadCase;
       [DataMember, XmlAttribute]
        private double axial;
       [DataMember, XmlAttribute]
        private double shear2;
       [DataMember, XmlAttribute]
        private double shear3;
       [DataMember, XmlAttribute]
        private double tortionalMoment;
       [DataMember, XmlAttribute]
        private double moment2;
       [DataMember, XmlAttribute]
        private double moment3;



        public HndzAnalysisResults(double station, string loadCase, double axial, double shear2, double shear3, double tortionalMoment, double moment2, double moment3)
        {
            this.station = station;
            this.loadCase = loadCase;
            this.axial = axial;
            this.shear2 = shear2;
            this.shear3 = shear3;
            this.tortionalMoment = tortionalMoment;
            this.moment2 = moment2;
            this.moment3 = moment3;
        }

        public double Station
        {
            get
            {
                return station;
            }

            set
            {
                station = value;
            }
        }

        public string LoadCase
        {
            get
            {
                return loadCase;
            }

            set
            {
                loadCase = value;
            }
        }

        public double Axial
        {
            get
            {
                return axial;
            }

            set
            {
                axial = value;
            }
        }

        public double Shear2
        {
            get
            {
                return shear2;
            }

            set
            {
                shear2 = value;
            }
        }

        public double Shear3
        {
            get
            {
                return shear3;
            }

            set
            {
                shear3 = value;
            }
        }

        public double TortionalMoment
        {
            get
            {
                return tortionalMoment;
            }

            set
            {
                tortionalMoment = value;
            }
        }

        public double Moment2
        {
            get
            {
                return moment2;
            }

            set
            {
                moment2 = value;
            }
        }

        public double Moment3
        {
            get
            {
                return moment3;
            }

            set
            {
                moment3 = value;
            }
        }
    }
}