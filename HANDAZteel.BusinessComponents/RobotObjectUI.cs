using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.AnalysisTools;
using HANDAZ.PEB.Entities;
using HANDAZ.Entities;

namespace HANDAZ.PEB.BusinessComponents
{
   public static class RobotObjectUI
    {
        public static bool Robotstart()
        {
            RobotInit.RobotKickStart();
            RobotInit.StartRobot();
          return  RobotInit.New2dFrameProject();

        }
        public static bool DrawBeam(int beamId, Node _startPoint, Node _endPoint)
        {
            return RobotGeometry.DrawBeam(beamId, _startPoint, _endPoint);
        }
        public static bool DrawColumn(int beamId, Node _startPoint, Node _endPoint)
        {
            return RobotGeometry.DrawColumn(beamId, _startPoint, _endPoint);
        }
        public static bool DrawBeam(Beam _beam)
        {
            return RobotGeometry.DrawBeam(_beam);
        }
        public static bool DrawColumn(Column _column)
        {
            return RobotGeometry.DrawColumn(_column);
        }
        public static void Refresh()
        {
           //  RobotInit.refresh();
        }
        #region Purlin 
        // public static bool DrawPurlin(int beamId, Node _startPoint, Node _endPoint)
        //{
        //    return RobotStart.DrawPurlin(beamId,_startPoint, _endPoint);
        //}
        #endregion
        public static bool DrawFrame(Frame Frame)
        {
            return RobotGeometry.DrawFrame(Frame);
        }
        public static bool DrawFrame(List<Node> FrameNodes)
        {
            return RobotGeometry.DrawFrame(FrameNodes);
        }
        public static bool DrawGrid(List<Grid> Grids)
        {
            return RobotGeometry.DrawGrids(Grids);
        }
        //public static bool SetCases()
        //{
        //    return RobotStart.SetCases();
        //}
        public static bool SetSupports(List<Node> FrameNodes)
        {
            return RobotGeometry.SetSupports(FrameNodes);
        }
        public static bool SetSupports(Frame Frame)
        {
            return RobotGeometry.SetSupports(Frame);
        }
        public static bool SetColumnSections(Frame frame)
        {
            return RobotDefinitions.SetColumnSections(frame);
        }
        public static bool SetBeamSections(Frame frame)
        {
            return RobotDefinitions.SetBeamSections(frame);
        }
        public static bool SetMaterial(Material material)
        {
            return RobotDefinitions.SetMaterial(material);
        }
        public static bool SetOwnWeight()
        {
            return RobotLoadAssigns.SetOwnWeight();
        }
        public static bool SetCoverLoad(Frame frame)
        {
            return RobotLoadAssigns.SetCoverLoad( frame);
        }
        public static bool SetLiveLoad(Frame frame)
        {
            return RobotLoadAssigns.SetLiveLoad(frame); 
        }
        public static bool SetWindLoad(Frame frame)
        {
            return RobotLoadAssigns.SetWindLoad(frame);
        }
        public static bool SetTempretureload()
        {
            return RobotLoadAssigns.SetTempretureload();
        }
        public static bool SetEarthQuakeLoad()
        {
            return RobotLoadAssigns.SetEarthQuakeLoad();
        }
        public static bool SetloadCases(Frame frame)
        {
            return RobotLoadAssigns.SetloadCases(frame);
        }
        public static void Analyze()
        {
            RobotResults.Analyze();
        }
        public static List<ResultsR> GetResults()
        {
           return  RobotResults.GetResults();
        }
        public static bool SetLoadCombinations()
        {
            return RobotLoadAssigns.SetLoadCombinations();
        }
        public static void Design(Frame frame)
        {
            RobotResults.Design(frame);
        } 
        public static bool ROBOTDESIGN(HndzFrameSingleBay3D frame)
        {
            RobotDesigner.Design(frame);
            return true;
        }
        public static bool SaveFile(string FileName)
        {
            RobotInit.WriteFile(FileName);
            return true;
        }
       // public static 
    }
}
