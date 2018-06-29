using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDAZ.PEB.BusinessLogic
{
   public class ProjectInformation
    {
        public string ProjectName { get; set; }
        public Person Owner { get; set; }
        public Person Designer { get; set; }
        public Person Contractor { get; set; }
        public Person Consultant { get; set; }
    }
   public class Person
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public Organization Organization { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
    public class Organization
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
    }
}
