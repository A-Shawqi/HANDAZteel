using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.Entities
{
    [DataContract]
    public class HndzStraightStairFlight : HndzStairFlight
    {

        #region Properties

        #endregion

        #region Constructors
        public HndzStraightStairFlight(string name, string description, HndzStorey baseStorey, double width, double length,
            Point3d origin, int numberOfRisers, int numberOfTreads, double riserHeight, double treadLength)
             : base(name, description, baseStorey, numberOfRisers, numberOfTreads, riserHeight, treadLength,
                   new HndzRectangularProfile(new Rectangle3d(new Plane(origin, Vector3d.ZAxis), width, length)))
        {
        }
        public HndzStraightStairFlight()
            : this(HndzResources.DefaultName, HndzResources.DefaultDescription, null, 0, 0, new Point3d(0, 0, 0), 0, 0, 0, 0)
        {

        }
        #endregion

        #region Methods
        #endregion

        #region Overridden Methods
        public override string ToString() => "Hndz-StraightStairFlight";
        #endregion

    }
}
