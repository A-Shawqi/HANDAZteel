using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HANDAZ.Entities
{
    interface IHndzRoot
    {
        Guid GlobalId { get; set; }
        String Name { get; set; }
        String Description { get; set; }
  
    }
}