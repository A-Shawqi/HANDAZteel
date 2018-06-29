using HANDAZ.PEB.Entities;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools
{
    public class RobotGeometry : RobotInit
    {
        public static bool DrawBeam(int beamId, Node _startPoint, Node _endPoint)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(_startPoint.Id, _startPoint.X, _startPoint.Y, _startPoint.Z);
            nodeServer.Create(_endPoint.Id, _endPoint.X, _endPoint.Y, _endPoint.Z);
            barServer.Create(beamId, _startPoint.Id, _endPoint.Id);
            robotApp.Interactive = 1;
            return true;
        }
        //=========================
        //Beam overload to Take a Beam Object From Core 
        public static bool DrawBeam(Beam beam)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(beam.BeamStart.Id, beam.BeamStart.X, beam.BeamStart.Y, beam.BeamStart.Z);
            nodeServer.Create(beam.BeamEnd.Id, beam.BeamEnd.X, beam.BeamEnd.Y, beam.BeamEnd.Z);
            barServer.Create(beam.Id, beam.BeamStart.Id, beam.BeamEnd.Id);
            robotApp.Interactive = 1;
            return true;
        }
        //=====================
        //Draw Column 
        public static bool DrawColumn(int _Id, Node _startPoint, Node _endPoint)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(_startPoint.Id, _startPoint.X, _startPoint.Y, _startPoint.Z);
            nodeServer.Create(_endPoint.Id, _endPoint.X, _endPoint.Y, _endPoint.Z);
            barServer.Create(_Id, _startPoint.Id, _endPoint.Id);
            robotApp.Interactive = 1;
            return true;
        }
        //overload to take a genric type of column from core 
        public static bool DrawColumn(Column Column)
        {
            robotApp.Interactive = 0;
            nodeServer.Create(Column.ColumnStart.Id, Column.ColumnStart.X, Column.ColumnStart.Y, Column.ColumnStart.Z);
            nodeServer.Create(Column.ColumnEnd.Id, Column.ColumnEnd.X, Column.ColumnEnd.Y, Column.ColumnEnd.Z);
            barServer.Create(Column.Id, Column.ColumnStart.Id, Column.ColumnEnd.Id);
            robotApp.Interactive = 1;
            return true;
        }
        #region Purlin 

        #endregion
        public static bool DrawFrame(Frame Frame)
        {
            robotApp.Interactive = 0;
            for (int i = 0; i < Frame.Beams.Count; i++)
            {
                DrawBeam(Frame.Beams[i]);
            }
            for (int i = 0; i < Frame.Columns.Count; i++)
            {
                DrawColumn(Frame.Columns[i]);
            }
            robotApp.Interactive = 1;
            return true;
        }
        public static bool DrawFrame(List<Node> frameNodes)
        {
            robotApp.Interactive = 0;
            for (int i = 0; i < frameNodes.Count; i++)
            {
                nodeServer.Create(frameNodes[i].Id, frameNodes[i].X, frameNodes[i].Y, frameNodes[i].Z);
            }
            for (int i = 0; i < frameNodes.Count - 1; i++)
            {
                barServer.Create((i + 1), frameNodes[i].Id, frameNodes[i + 1].Id);
            }
            robotApp.Interactive = 1;
            return true;
        }
        public static bool DrawGrids(List<Grid> Grids)
        {
            return true;
        }
        public static bool SetSupports(List<Node> frameNodes)
        {
            robotApp.Interactive = 0;
            IRobotNode leftSupportnode = (IRobotNode)nodeServer.Get(frameNodes[0].Id);
            leftSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, "Pinned");
            IRobotNode rightSupportnode = (IRobotNode)nodeServer.Get(frameNodes[frameNodes.Count - 1].Id);
            rightSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, "Pinned");
            robotApp.Interactive = 1;
            return true;
        }
        public static bool SetSupports(Frame frame)
        {
            robotApp.Interactive = 0;
            IRobotNode leftSupportnode = (IRobotNode)nodeServer.Get(frame.Supports[0].Position.Id);
            leftSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, frame.Supports[0].SupportType.ToString());
            IRobotNode rightSupportnode = (IRobotNode)nodeServer.Get(frame.Supports[1].Position.Id);
            rightSupportnode.SetLabel(IRobotLabelType.I_LT_SUPPORT, frame.Supports[1].SupportType.ToString());
            robotApp.Interactive = 1;
            return true;
        }
    }
}
