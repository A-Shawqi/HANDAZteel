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
    [KnownType(typeof(HndzFrameSingleBay2D))]
    [KnownType(typeof(HndzFrameMonoSlope2D))]
    [KnownType(typeof(HndzFrameMultiSpan12D))]
    [KnownType(typeof(HndzFrameMultiSpan22D))]
    [KnownType(typeof(HndzFrameMultiSpan32D))]
    [KnownType(typeof(HndzFrameMultiGable2D))]
    public abstract class HndzFrame2D: HndzProduct
    {
        #region Properties

        #endregion
        #region Constructors
        protected HndzFrame2D(string name, string description,  HndzStorey storey = null):
                   base(name,description, storey )
        {
        }
        protected HndzFrame2D() : this(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
        }
        #endregion

    }
}
