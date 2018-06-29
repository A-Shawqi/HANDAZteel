using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace HANDAZ.Entities
{

    [DataContract]  [Serializable]  [XmlSerializerFormat]
    [KnownType(typeof(HndzStructuralElement))]
    [KnownType(typeof(HndzArchitecturalMaterial))]
    public abstract class HndzMaterial: HndzRoot
    {
        public HndzMaterial()
        {
        }
        public HndzMaterial(string matName):base(matName)
        {
        }
        public HndzMaterialType MatType { get; set; }
        public HndzProductDiscipline DisciplineType { get; set; }
    }
}