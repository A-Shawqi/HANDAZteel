using HANDAZ.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.AnalysisTools.STAADPro
{
    public class STAADProFileWriter
    {
        static private string ModelDirectory = STAADProResources.ModelDirectory;
        private StreamWriter writer;
        static private STAADProLoadPattern deadLoad = null;
        public STAADProFileWriter(string fileName, HndzUnitsEnum units = HndzUnitsEnum.Ton_m_C)
        {
            string modelPath = ModelDirectory + System.IO.Path.DirectorySeparatorChar + fileName + ".std";
            if (!System.IO.Directory.Exists(ModelDirectory))
            {
                System.IO.Directory.CreateDirectory(ModelDirectory);
            }
            string fullPath = Path.GetFullPath(modelPath);
            writer = new StreamWriter(fullPath);
            writer.WriteLine("STAAD PLANE");

            writer.WriteLine("START JOB INFORMATION");
            writer.WriteLine("ENGINEER DATE "+DateTime.Now.Date);//TODO: Check the validity of date
            writer.WriteLine("END JOB INFORMATION");

            writer.WriteLine("INPUT WIDTH 79");
            writer.WriteLine("UNIT METER MTON");//TODO: Handle Units
        }
        public void DefineMaterial(STAADProMaterial material)
        {
            writer.WriteLine("DEFINE MATERIAL START");
            writer.WriteLine("ISOTROPIC "+material.Name);
            writer.WriteLine("E "+material.E);
            writer.WriteLine("Poisson "+material.Poisson);
            writer.WriteLine("DENSITY "+material.Density);
            writer.WriteLine("ALPHA "+material.Alpha);
            writer.WriteLine("DAMP "+material.DAMP);
            writer.WriteLine("END DEFINE MATERIAL");
        }
        public void AddJoints(IEnumerable<STAADProPoint> points)
        {
            writer.WriteLine("JOINT COORDINATES");//TODO: Find this line first and append it if not pre written
            foreach (STAADProPoint point in points)
            {
                if (point.IsSTAADDefined == false)
                {
                    writer.WriteLine(string.Format("{0} {1} {2} {3};", point.Number, point.X, point.Y, point.Z));
                    point.IsSTAADDefined = true;
                }
            }
        }

        public void AddMembers(params IEnumerable<STAADProMember>[] members)
        {
            writer.WriteLine("MEMBER INCIDENCES");//TODO: Find this line first and append it if not pre written
            foreach (IEnumerable<STAADProMember> param in members)
            {
                foreach (STAADProMember member in param)
                {
                    if (member.IsSTAADDefined == false)
                    {
                        writer.WriteLine(string.Format("{0} {1} {2};", member.Number, member.StartPoint.Number, member.EndPoint.Number));
                        member.IsSTAADDefined = true;
                    }
                }
            }

            //Define Sections
            writer.WriteLine("MEMBER PROPERTY AMERICAN");
            foreach (IEnumerable<STAADProMember> param in members)
            {
                foreach (STAADProMember member in param)
                {
                    DefineMemberSection(member);
                }
            }
        }

        private void DefineMemberSection(STAADProMember m)
        {
            STAADProTaperedSection s = (STAADProTaperedSection)m.Section;//TODO change this
            writer.WriteLine(string.Format("{0} TAPERED {1} {2} {3} {4} {5} {6} {7}",m.Number,s.StartDepth, s.WebThickness, 
                s.EndDepth,s.TopFlangeWidth,s.TopFlangeThickness,s.BotFlangeWidth,s.BotFlangeThickness));
        }
        public void DefineConstants()
        {
            writer.WriteLine("CONSTANTS");
            writer.WriteLine("MATERIAL STEEL ALL");
        }
        public void DefineSuppots(IEnumerable<STAADProPoint> points, IEnumerable<STAADProSupportTypeEnum> supports)
        {
            writer.WriteLine("SUPPORTS");
            STAADProPoint[] pointsArr = points.ToArray();
            STAADProSupportTypeEnum[] supportsArr = supports.ToArray();
            for (int i = 0; i < pointsArr.Length; i++)
            {
                writer.WriteLine(string.Format("{0} {1}",pointsArr[i].Number,supportsArr[i].ToString()));
            }
        }
        public void DefineLoadPattern(STAADProLoadPattern pattern)
        {
            if(deadLoad == null)
            {
                deadLoad = new STAADProLoadPattern(STAADProLoadTypeEnum.DEAD);
                writer.WriteLine(string.Format("LOAD {0} LOADTYPE DEAD TITLE DL",deadLoad.Number));
                writer.WriteLine("SELFWEIGHT Y -1");
            }
            writer.WriteLine(string.Format("LOAD {0} LOADTYPE {1} TITLE {2}",pattern.Number,pattern.LoadType.ToString(),pattern.Name));
        }
        public void AddMemberLoad(STAADProMember m, STAADProUniformLoad l)
        {
            writer.WriteLine("MEMBER Load");
            writer.WriteLine(string.Format("{0} UNI {1} {2}",m.Number,l.Direction.ToString(),l.Value));
        }
        public void AddMemberLoad(IEnumerable<STAADProMember> ms, STAADProUniformLoad l)
        {
            foreach (STAADProMember m in ms)
            {
                AddMemberLoad(m, l);
            }
        }
        public void AddLoadCombination(STAADProLoadCombination c)
        {
            writer.WriteLine(string.Format("LOAD COMB {0} {1}",c.Number,c.Name));
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<float,STAADProLoadPattern> l in c.LoadPatterns)
            {
                str.AppendFormat("{0} {1} ", l.Value.Number, l.Key);
            }
            writer.WriteLine(str.ToString());
        }
        public void PerformAnalysis()
        {
            writer.WriteLine("PERFORM ANALYSIS PRINT ALL");
        }
        public void PerformDesign(IEnumerable<STAADProLoadCombination> cs=null)
        {
            if (cs!=null)
            {
                StringBuilder str = new StringBuilder();
                str.Append("LOAD LIST ");
                foreach (STAADProLoadCombination c in cs)
                {
                    str.Append(c.Number);
                    str.Append(" ");
                }
                writer.WriteLine(str.ToString());
            }
            else
            {
                writer.WriteLine("LOAD LIST ALL");
            }
            //TODO: Temp parameters and hardcoded
            writer.WriteLine("Parameter 1");
            writer.WriteLine("CODE AISC UNIFIED 2005");
            writer.WriteLine("Method ASD");
            writer.WriteLine("FYLD 345000 ALL");
            writer.WriteLine("BEAM 1 ALL");
            writer.WriteLine("*TRACK 2 ALL");
            writer.WriteLine("CB 0 ALL");
            //writer.WriteLine("****LZ,KZ");
            //writer.WriteLine("KZ 1.5 MEMB 1 4");
            //writer.WriteLine("LZ " + B_W + " MEMB 2 3");
            //writer.WriteLine("KY 1.0 MEMB 1 4");
            //writer.WriteLine("LY 1.5 MEMB 2 3");
            //writer.WriteLine("UNB " + E_HT + " MEMB 1 4");
            //writer.WriteLine("UNB 1.5 MEMB 2 3");
            //writer.WriteLine("UNT " + E_HT + " MEMB 1 4");
            //writer.WriteLine("UNT 1.5 MEMB 2 3");
            writer.WriteLine("****************************** CHECK CODE AND WEIGHT");
            writer.WriteLine("CHECK CODE ALL");
            writer.WriteLine("UNIT METER KG");
            writer.WriteLine("STEEL TAKE OFF ALL");
            writer.WriteLine("FINISH");
        }
        public void CloseFile()
        {
            writer.Close();
        }

        public static string GetPath(string modelName)
        {
            return Path.GetFullPath(ModelDirectory + System.IO.Path.DirectorySeparatorChar + modelName + ".std");
        }
    }
}
