using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzColumnStandardCase))]
    public abstract class HndzColumn : HndzStructuralElement
    {
        //static uint Id;
        #region properties
       [DataMember, XmlAttribute]
        public double UnconnectedHeight { get; set; }
       [DataMember, XmlAttribute]
        public HndzStorey TopStorey { get; set; } // ana msh m7tagha
       [DataMember, XmlAttribute]
        public double TopOffset { get; set; } // ana msh m7tagha
        #endregion
        #region Constructors
        protected HndzColumn(String name, String description, HndzLine extrusionLine,HndzProfile profile, HndzStorey storey = null, Double baseOffset = 0,
                              HndzProductDiscipline discipline = HndzProductDiscipline.Structural) :
                             base(name, description,extrusionLine,profile, storey, baseOffset)
        {
            UnconnectedHeight = extrusionLine.RhinoLine.Length;
        }

       //ToDo: another constructor take column base point and extrusion direction and extrusion length to assemble the extrusion line

        /// <summary>
        /// ToDo:we agreed that profile won't define elements position , how we'll locate them without its start and end point
        /// </summary>
        //protected HndzColumn(String name, String description, HndzStorey baseStorey, HndzStorey topStorey,
        //                    Double baseOffset = 0, Double topOffset = 0,
        //                    HndzProductDiscipline discipline = HndzProductDiscipline.Structural) :
        //                        base(name, description, baseStorey, baseOffset, 0, dir, discipline)
        //{
        //    //LocalId = ++Id;
        //    TopStorey = topStorey;
        //    TopOffset = topOffset;
        //    if (BuildingStorey != null && TopStorey != null)
        //    {
        //        UnconnectedHeight = (TopStorey.Elevation + TopOffset) - (BuildingStorey.Elevation + BaseOffset);
        //        ExtrusionLength = UnconnectedHeight;
        //    }
        //}
        protected HndzColumn() : this(HndzResources.DefaultName, HndzResources.DefaultDescription,null,null)
        {
        }
        #endregion
    }
}
