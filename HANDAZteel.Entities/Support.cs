using HANDAZ.PEB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.PEB.Entities
{
    public enum SupportTypeEnum
    {
        Roller,
        Pinned,
        Fixed
    }
    public class Support
    {
        public SupportTypeEnum SupportType { get; set; }
        public static int ID { get; set; }
        public Node Position { get; set; }
        public Support(Node _position, SupportTypeEnum supportType)
        {
            SupportType = supportType;
            ID = ID + 1;
            Position = _position;
        }
    }
}
