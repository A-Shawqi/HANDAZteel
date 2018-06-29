using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;
using UnitsNet.Units;

namespace HANDAZ.Entities
{
    [DataContract]  [Serializable]  [XmlSerializerFormat]
    public class HndzProject : HndzRoot
    {

        #region Properties
       [DataMember, XmlAttribute]
        public Person Owner { get; set; }
       [DataMember, XmlAttribute]
        public Person Designer { get; set; }
       [DataMember, XmlAttribute]
        public Person Consultant { get; set; }
       [DataMember, XmlAttribute]
        public Person Contractor { get; set; }
       [DataMember, XmlAttribute]
        public HndzCity Location { get; set; }
       //[DataMember, XmlAttribute]
       // public HndzLengthUnitSystem Units { get; set; } 
      // [DataMember, XmlAttribute]
        //[XmlArray("Buildings")]
        [DataMember]
        public ICollection<HndzBuilding> Buildings { get; set; }
       [DataMember, XmlAttribute]
        public HndzWCS GlobalCoordinateSystem { get; set; }
       [DataMember, XmlAttribute]
        public HndzSite Site { get; set; }
        [DataMember, XmlAttribute]
        public LengthUnit LengthUnit { get; set; }
        [DataMember, XmlAttribute]
        public AreaUnit AreaUnit { get; set; }
        [DataMember, XmlAttribute]
        public TemperatureUnit TemperatureUnit { get; set; }
        [DataMember, XmlAttribute]
        public MassUnit MassUnit { get; set; }
        [DataMember, XmlAttribute]
        public ForceUnit ForceUnit { get; set; }
        #endregion

        #region Constructors

        public HndzProject( String name, String description, Person owner,
                       Person designer, Person consultant, Person contractor, HndzCity location, HndzLengthUnitSystem units,
                       ICollection<HndzBuilding> buildings, HndzWCS globalCoordinateSystem,HndzSite site) : base(name, description)
        {
            Owner = owner;
            Designer = designer;
            Consultant = consultant;
            Contractor = contractor;
            Location = location;
            Buildings = buildings;
            GlobalCoordinateSystem = globalCoordinateSystem;
            Site = site;
            if (Owner == null) //temp to avoid Ifc Error
            {
                Owner = new Person();
            }
        }
        public HndzProject(ICollection<HndzBuilding> buildings, HndzSite site) : this(HndzResources.DefaultName, HndzResources.DefaultDescription,default(Person), default(Person), default(Person), default(Person),
                                                               null,HndzLengthUnitSystem.mm,buildings,
                                                                HndzWCS.WGS84,site)
        {

        }
        public HndzProject() : this(HndzResources.DefaultName, HndzResources.DefaultDescription, default(Person), default(Person), default(Person),
                                   default(Person), null, HndzLengthUnitSystem.mm, null, HndzWCS.WGS84, null)
        {

        }


        public HndzProject(Person owner, Person designer, Person consultant,
            Person contractor, HndzCity location, /*HndzLengthUnitSystem units,*/ ICollection<HndzBuilding> buildings,
            HndzWCS globalCoordinateSystem, HndzSite site, LengthUnit lengthUnit, AreaUnit areaUnit,
            TemperatureUnit temperatureUnit, MassUnit massUnit, ForceUnit forceUnit): base(HndzResources.DefaultName, HndzResources.DefaultDescription)
        {
            Owner = owner;
            Designer = designer;
            Consultant = consultant;
            Contractor = contractor;
            Location = location;
            //Units = units;
            Buildings = buildings;
            GlobalCoordinateSystem = globalCoordinateSystem;
            Site = site;
            LengthUnit = lengthUnit;
            AreaUnit = areaUnit;
            TemperatureUnit = temperatureUnit;
            MassUnit = massUnit;
            ForceUnit = forceUnit;
        }
        #endregion
        //ToDo: IfcProject implementation
    }
}
