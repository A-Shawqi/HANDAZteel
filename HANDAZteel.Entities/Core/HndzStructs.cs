
using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace HANDAZ.Entities {
    [DataContract]
    [Serializable]
    

    public struct HndzFlange
    {
        [DataMember, XmlAttribute]

        public float Bf { get; set; }
        [DataMember, XmlAttribute]

        public float Tf { get; set; }

        public HndzFlange(float bf, float tf)
        {
            Bf = bf;
            Tf = tf;
        }
    }
    [DataContract] [Serializable]
    public struct HndzWeb
    {
        [DataMember, XmlAttribute]

        public float Hw { get; set; }
        [DataMember, XmlAttribute]

        public float Tw { get; set; }
        public HndzWeb(float hw, float tw)
        {
            Hw = hw;
            Tw = tw;
        }
    }
    [DataContract]
    [Serializable]

    public struct HndzWeld
    {
        [DataMember, XmlAttribute]

        public float Tweld { get; set; }
        [DataMember, XmlAttribute]

        public float Lweld { get; set; }

        public HndzWeld(float t, float l)
        {
            Tweld = t;
            Lweld = l;
        }
    }
    [DataContract]    [Serializable]
    public class Person
    {
        [DataMember, XmlAttribute]

        public string Name { get; set; }
        [DataMember, XmlAttribute]

        public string LastName { get; set; }
        [DataMember, XmlAttribute]

        public string MidName { get; set; }
        [DataMember, XmlAttribute]

        public string Address { get; set; }
        [DataMember, XmlAttribute]

        public string Email { get; set; }
        [DataMember, XmlAttribute]

        public string Organization { get; set; }
        [DataMember, XmlAttribute]

        public string Role { get; set; }

        public Person(string name, string lastName, string midName, string address, string email, string organization, string role)
        {
            Name = name;
            LastName = lastName;
            MidName = midName;
            Address = address;
            Email = email;
            Organization = organization;
            Role = role;
        }
        public Person():this("Handz Name", "Handz LastName", "Handz MidName", "Handz Address",
                "Handz Email", "Handz Organization", "Handz Role")
        {

        }
        public void SetName(string name)
        {
            Name = name;
            //this = new Person(name, LastName, MidName, Address, Email, Organization, Role);

        }
        public void SetLastName(string lastName)
        {
            LastName = lastName;
            //this = new Person(Name, lastName, MidName, Address, Email, Organization, Role);
        }
        public void SetOrganization(string organization)
        {
            Organization = organization;
            //this = new Person(Name, LastName, MidName, Address, Email, organization, Role);
        }
        public static Person DefaultPerson()
        {
            return new Person("Handz Name", "Handz LastName", "Handz MidName", "Handz Address",
                "Handz Email", "Handz Organization", "Handz Role");
        }
        
    }



}