using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.Entities
{
    [KnownType(typeof(HndzStraightStairFlight))]
    [DataContract]
    public class HndzStair : HndzBuildingElement
    {
        #region Properties
        [DataMember]
        public ICollection<HndzStairFlight> Flights;

        [DataMember]
        public ICollection<HndzStairLanding> Landings;
        #endregion

        #region Constr
        public HndzStair(string name, string description, ICollection<HndzStairFlight> flights, ICollection<HndzStairLanding> landings, HndzStorey storey = null, Double baseOffset = 0,
                                   HndzProductDiscipline discipline = HndzProductDiscipline.None) :
                                   base(name, description, storey, baseOffset, discipline)
        {
            Flights = flights;
            Landings = landings;
        }
        public HndzStair( HndzStorey storey = null, Double baseOffset = 0,
                                     HndzProductDiscipline discipline = HndzProductDiscipline.None) :
                                     this(HndzResources.DefaultName, HndzResources.DefaultDescription, new List<HndzStairFlight>(), new List<HndzStairLanding>(),
                                         storey, baseOffset, discipline)
        {
        }
      
        #endregion

        #region Methods
        public double GetStairHeight()
        {
            double height = 0;
            foreach (HndzStairFlight flight in Flights)
            {
                height += flight.GetFlightHeight();
            }
            foreach (var landing in Landings)
            {
                height += landing.SlabThickness;
            }
            return height;
        }
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-Stair";
        #endregion

    }
}
