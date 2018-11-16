using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ElsevierMaterials.Exporter.Formats
{
    public class KTMXml
    {


        public static XDocument FillKTMXml(string standard, string name, IList<Models.ExportPropertyGeneral> properties)
        {
         
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", null), null);   
            XElement Material = new XElement("Material");

            Material.SetAttributeValue("designation", name);
            Material.SetAttributeValue("standard", standard);


            FillPropertiesData(Material, properties);
         
       
            xml.Add(Material);
            return xml;
        }

        private static void FillPropertiesData(XElement Material, IList<Models.ExportPropertyGeneral> properties)
        {

            XElement Properties = new XElement("Propertes");            

            foreach (var item in properties)
            {          
                    XElement Property = new XElement("Property");
                    XElement Name = new XElement("Name");
                    Name.Value = item.Name;
                    Property.Add(Name);
                    XElement Temperature = new XElement("Temperature");
                    Temperature.Value = item.Temperature == 999 ? "-" : item.Temperature.ToString();
                    Property.Add(Temperature);
                    XElement Value = new XElement("Value");
                    if (item.Values != null && item.Values.Count > 0)
                    {
                        string val = "";
                        foreach (var item1 in item.Values)
                        {
                            val = val + item1.X.Trim() + ", " + item1.Y.ToString().Trim() + "; ";
                        }
                        Value.Value = val;

                    }
                    else
                    {
                        Value.Value = item.Value;
                    }

                    Property.Add(Value);
                    XElement Unit = new XElement("Unit");

                    Unit.Value = item.Unit == null ? "" : item.Unit;
                    Property.Add(Unit);
                    XElement Comment = new XElement("Note");
                    Comment.Value = item.Note == null ? "" : HtmlRemoval.StripTagsRegex(item.Note);
                    Property.Add(Comment);
                    Properties.Add(Property);
            }
            Material.Add(Properties);
        }

    }
}