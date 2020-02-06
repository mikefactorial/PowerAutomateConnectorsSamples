using MailingAddressService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace MailingAddressService.Controllers
{
    public class UspsController : ApiController
    {
        // POST api/values
        [HttpPost]
        public Address GetAddress(Address inAddress)
        {
            XDocument requestDoc = new XDocument(
                new XElement("AddressValidateRequest",
                    new XAttribute("USERID", "004EXALE4989"),
                    new XElement("Revision", "1"),
                    new XElement("Address",
                        new XAttribute("ID", "0"),
                        new XElement("Address1", inAddress.Line1),
                        new XElement("Address2", inAddress.Line2),
                        new XElement("City", inAddress.City),
                        new XElement("State", inAddress.State),
                        new XElement("Zip5", inAddress.Zip),
                        new XElement("Zip4", "")
                    )
                )
            );

            try
            {
                var url = "http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
                Console.WriteLine(url);
                var client = new WebClient();
                var response = client.DownloadString(url);

                var xdoc = XDocument.Parse(response.ToString());
                Address verifiedAddress = new Address();
                Console.WriteLine(xdoc.ToString());
                foreach (XElement element in xdoc.Descendants("Address"))
                {
                    verifiedAddress.Line1 = GetXMLElement(element, "Address1");
                    verifiedAddress.Line2 = GetXMLElement(element, "Address2");
                    if (string.IsNullOrEmpty(verifiedAddress.Line1) && !string.IsNullOrEmpty(verifiedAddress.Line2))
                    {
                        verifiedAddress.Line1 = verifiedAddress.Line2;
                        verifiedAddress.Line2 = string.Empty;
                    }
                    verifiedAddress.City = GetXMLElement(element, "City");
                    verifiedAddress.State = GetXMLElement(element, "State");
                    verifiedAddress.Zip = $"{GetXMLElement(element, "Zip5")}-{GetXMLElement(element, "Zip4")}";
                }

                return verifiedAddress;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }


        private static string GetXMLElement(XElement element, string name)
        {
            var el = element.Element(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        private static string GetXMLAttribute(XElement element, string name)
        {
            var el = element.Attribute(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }
    }
}
