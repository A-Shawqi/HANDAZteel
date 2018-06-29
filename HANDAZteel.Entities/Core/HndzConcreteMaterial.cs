using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HANDAZ.Entities
{
    public class HndzConcreteMaterial:HndzStructuralMaterial
    {
        public HndzConcreteMaterial(string name):base(name,HndzMaterialType.Concrete)
        {
            throw new NotImplementedException("Concrete Material is not implemented yet");
        }
    }
}