﻿using Rhino.Geometry;
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
    [KnownType(typeof(HndzPurlinStandrdCase))]
    public abstract class HndzPurlin : HndzStructuralElement
    {
        #region properties

        #endregion
        #region Constructors
        protected HndzPurlin(String name, String description, HndzLine extrusionLine,HndzProfile profile, HndzStorey storey = null,
                            double baseOffset = 0, Vector3d direction = default(Vector3d)) :
                        base(name, description, extrusionLine,profile, storey, baseOffset)
        {
        }
        protected HndzPurlin() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null,null)
        {
        }
        #endregion
    }
}
