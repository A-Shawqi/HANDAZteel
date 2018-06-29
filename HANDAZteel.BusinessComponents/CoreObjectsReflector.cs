using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.Entities;

namespace HANDAZ.PEB.BusinessComponents
{
    public static class CoreObjectsReflector
    {
        public static Structure CreateStructure(string _projectName, Node _Origin, double _Length, double _Width, int _NumberOfFrames, double _BaySpacing, List<Frame> _Frames, List<Purlin> _Purlins)
       {
            Structure S = new Structure(_projectName, _Origin, _Length, _Width, _NumberOfFrames, _BaySpacing, _Frames, _Purlins);
            return S;
       }
       public static Node CreateNode(double _x, double _y , double _z)
       {
            Node N = new Node(_x,_y,_z);
            return N;
       }
       public static Beam CreateBeam(int _BeamId, Node _beamStart, Node _beamEnd, I_BeamSection _BeamSection)
        {
          
            Beam B = new Beam( _beamStart, _beamEnd, _BeamSection);
            return B;
        }
        public static I_BeamSection CreateSection(string _SectionName, string _SectionTag, double _ProfileHeight, double _ProfileWidth
            , double _FlangThickness, double _WebThicnkness, double _FilletRadius, Material _material)
        {
            I_BeamSection Sec = new I_BeamSection(_SectionName,  _SectionTag, _ProfileHeight, _ProfileWidth, _FlangThickness, _WebThicnkness, _FilletRadius, _material);
            return Sec;
        }
        public static Material CreateMaterial(string _materialName, double _unitWeight, double _youngsModulas, double _poissionRatio, double _shearModulas
       , double _thermalExpansion, double _dampingRatio, double _charchteristicResistance, double _designResistance, double _shearReductionFactor, double _tensionLimitstress)
        {
            Material M = new Material(_materialName, _unitWeight, _youngsModulas, _poissionRatio, _shearModulas
              , _thermalExpansion, _dampingRatio, _charchteristicResistance, _designResistance, _shearReductionFactor, _tensionLimitstress);
            return M;
        }
        public static Column CreateColumn(int _columnId, Node _columnStart, Node _columnEnd, I_BeamSection _columnSection)
        {
            Column C = new Column(_columnStart, _columnEnd, _columnSection);
            return C;

        }
        public static Frame CreateFrame(double _ridgeHeight, double _eveHeight, List<Beam> _beams, List<Column> _columns,
            List<Support> _supports, Node _origin,double _width)
        {
            Frame F = new Frame( _ridgeHeight,  _eveHeight, _beams,  _columns, _supports,  _origin, _width);
            return F;
        }
        public static Purlin CreatePurlin(int _purlinId, Node _purlinStart, Node _purlinEnd, I_BeamSection _purlinSection)
        {
            Purlin P = new Purlin( _purlinStart,  _purlinEnd,  _purlinSection);
            return P;
        }
     
    }
}
