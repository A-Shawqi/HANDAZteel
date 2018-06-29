using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public class Results
    {
        public double MxStart { get; set; }
        public double MxEnd { get; set; }
        public double MyStart { get; set; }
        public double MyEnd { get; set; }
        public double AxialForceStart { get; set; }
        public double AxialForceEnd { get; set; }
        public double MaxMomentPositve { get; set; }
        public double MaxMomentNegative { get; set; }
        public double MaxAxialForceTenstion { get; set; }
        public double MaxAxialForceCompression { get; set; }
     
       public Results(double _mxStart,double _mxEnd, double _myStart,double _myEnd,double _axialForceStart ,double _axialForceEnd ,
            double _maxMomentPositve, double _maxMomentNegative ,double _maxAxialForceTenstion ,double _maxAxialForceCompression)
        {
            MxStart = _mxStart;
            MxEnd = _mxEnd;
            MyStart = _myStart;
            MyEnd = _myEnd;
            MaxMomentPositve = _maxMomentPositve;
            MaxMomentNegative =  _maxMomentNegative;
            MaxAxialForceCompression =  _maxAxialForceCompression;
            MaxAxialForceTenstion = _maxAxialForceTenstion;
        }
    }
}
