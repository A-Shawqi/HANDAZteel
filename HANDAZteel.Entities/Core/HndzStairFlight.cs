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
    public class HndzStairFlight : HndzBuildingElement
    {
        #region Properties
        /// <summary>
        /// Profile for the 2d boundary of the stair flight(FootPrint)
        /// </summary>
        [DataMember]
        public HndzProfile Profile { get; set; }
        [DataMember]
        public int NumberOfRisers { get; set; }
        [DataMember]
        public int NumberOfTreads { get; set; }
        [DataMember]
        public double RiserHeight { get; set; }
        [DataMember]
        public double TreadLength { get; set; }
        /// <summary>
        /// Risers Lines--> should be updated support polylines or arcs!
        /// these list of lines should be sorted from bottom to top
        /// </summary>
        [DataMember]
        public ICollection<Line> RisersLines { get; set; } = new List<Line>();
        /// <summary>
        /// Stair Path with correct direction
        /// </summary>
        [DataMember]
        public HndzPolyline StairPath { get; set; }
        #endregion

        #region Constr
        public HndzStairFlight(string name, string description, HndzStorey baseStorey, int numberOfRisers,
            int numberOfTreads, double riserHeight, double treadLength, HndzProfile profile)
            : base(name, description, baseStorey)
        {
            Profile = profile;
            NumberOfRisers = numberOfRisers;
            NumberOfTreads = numberOfTreads;
            RiserHeight = riserHeight;
            TreadLength = treadLength;
        }
        #endregion

        #region Methods
        public double GetFlightHeight()
        {
            return NumberOfRisers * RiserHeight;
        }
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-StairFlight";
        #endregion

    }
}
