using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HANDAZ.PEB.AnalysisTools;
using HANDAZ.PEB.Entities;

namespace HANDAZ.PEB.BusinessComponents
{
    public static class API_Object_UI
    {
        public static bool Robotstart()
        {
            RobotStart.InitRobot();
            return RobotStart.StartRobot();
        }
        public static bool New2dFrameProject()
        {
            return RobotStart.New2dFrameProject();
        }
        public static bool DrawBeam(int beamId, Node _startPoint, Node _endPoint)
        {
            return RobotStart.DrawBeam(beamId, _startPoint, _endPoint);
        }
        public static bool DrawColumn(int beamId, Node _startPoint, Node _endPoint)
        {
            return RobotStart.DrawColumn(beamId, _startPoint, _endPoint);
        }
        public static bool DrawBeam(Beam _beam)
        {
            return RobotStart.DrawBeam(_beam);
        }
        public static bool DrawColumn(Column _column)
        {
            return RobotStart.DrawColumn(_column);
        }
        #region Purlin 
        // public static bool DrawPurlin(int beamId, Node _startPoint, Node _endPoint)
        //{
        //    return RobotStart.DrawPurlin(beamId,_startPoint, _endPoint);
        //}
        #endregion
        public static bool DrawFrame(Frame Frame)
        {
            return RobotStart.DrawFrame(Frame);
        }
        public static bool DrawFrame(List<Node> FrameNodes)
        {
            return RobotStart.DrawFrame(FrameNodes);
        }
        public static bool DrawGrid(List<Grid> Grids)
        {
            return RobotStart.DrawGrids(Grids);
        }
        //public static bool SetCases()
        //{
        //    return RobotStart.SetCases();
        //}
        public static bool SetSupports(List<Node> FrameNodes)
        {
            return RobotStart.SetSupports(FrameNodes);
        }
        public static bool SetSupports(Frame Frame)
        {
            return RobotStart.SetSupports(Frame);
        }
        public static bool SetColumnSections(Frame frame)
        {
            return RobotStart.SetColumnSections(frame);
        }
        public static bool SetBeamSections(Frame frame)
        {
            return RobotStart.SetBeamSections(frame);
        }
        public static bool SetMaterial(Material material)
        {
            return RobotStart.SetMaterial(material);
        }
        public static bool SetOwnWeight()
        {
            return RobotStart.SetOwnWeight();
        }
        public static bool SetCoverLoad(Structure Structure)
        {
            return RobotStart.SetCoverLoad(Structure);
        }
        public static bool SetLiveLoad(Structure Structure)
        {
            return RobotStart.SetLiveLoad(Structure); ;
        }
        public static bool SetWindLoad(Structure Structure)
        {
            return RobotStart.SetWindLoad(Structure);
        }
        public static bool SetTempretureload()
        {
            return RobotStart.SetTempretureload();
        }
        public static bool SetEarthQuakeLoad()
        {
            return RobotStart.SetEarthQuakeLoad();
        }
        public static bool SetloadCases(Structure Structure)
        {
            return RobotStart.SetloadCases(Structure);
        }
        public static void Design()
        {
             RobotStart.Design();
        }
        public static void GetResults()
        {
            RobotStart.GetResults();
        }
    }
}