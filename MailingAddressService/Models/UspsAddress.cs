using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MailingAddressService.Models
{
    [XmlRoot(ElementName = "Address")]
    public class UspsAddress
    {
        public UspsAddress(Address address)
        {
            this.Address1 = address.Line1;
            this.Address2 = address.Line2;
            this.City = address.City;
            this.State = address.State;
            this.Zip5 = address.Zip;
        }
        public UspsAddress()
        { 
        }
        [XmlElement(ElementName = "Address1")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "Address2")]
        public string Address2 { get; set; }
        [XmlElement(ElementName = "City")]
        public string City { get; set; }
        [XmlElement(ElementName = "State")]
        public string State { get; set; }
        [XmlElement(ElementName = "Zip5")]
        public string Zip5 { get; set; }
        [XmlElement(ElementName = "Zip4")]
        public string Zip4 { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "AddressValidateRequest")]
    public class AddressValidateRequest
    {
        [XmlElement(ElementName = "Revision")]
        public string Revision { get; set; }
        [XmlElement(ElementName = "Address")]
        public UspsAddress Address { get; set; }
        [XmlAttribute(AttributeName = "USERID")]
        public string USERID { get; set; }
    }
}