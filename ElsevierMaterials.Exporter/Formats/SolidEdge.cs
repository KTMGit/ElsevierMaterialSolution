using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public static class SolidEdge
    {      
        private const string NAME = "Name";
        private const string VALUE = "Value";
        private const string INDEX = "Index";
        private const string PROPERTY = "Property";
        private const string DISPLAYNAME = "displayname";

        /// <summary>
        /// Fills the XML solid edge.
        /// </summary>
        /// <returns></returns>
        public static XDocument FillXMLSolidEdge(string standard, string name, bool isSteel, IList<ExportPropertyGeneral> properties)
        {
     
            XDocument xml = new XDocument();
            XElement MaterialLibraryFile = new XElement("MaterialLibraryFile");
            XElement Version = new XElement("Version");
            Version.SetAttributeValue("Value", "1");
            XElement MaterialData = new XElement("MaterialData");
            XElement Materials = new XElement("Materials");
            XElement Material = new XElement("Material");
            Material.SetAttributeValue(NAME, name + " " + standard);
            XElement Property = new XElement(PROPERTY);
          
            if (isSteel)
            {
                Property.SetAttributeValue(VALUE, "Steel");
            }
            else
            {
                Property.SetAttributeValue(VALUE, "Nonferrous");
            }
            Property.SetAttributeValue(NAME, "Face Style");
            Property.SetAttributeValue(VALUE, "20");
            Material.Add(Property);
            Property = new XElement(PROPERTY);
            Property.SetAttributeValue(VALUE, "ANSI32(Steel)");
            Property.SetAttributeValue(NAME, "Fill Style");
            Property.SetAttributeValue(INDEX, "21");
            Material.Add(Property);
            Property = new XElement(PROPERTY);
            Property.SetAttributeValue(VALUE, "Stainless steel");
            Property.SetAttributeValue(NAME, "VSPlus Style");
            Property.SetAttributeValue(INDEX, "22");
            Material.Add(Property);
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalDensity, "23");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "24");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalThermalConductivity, "25");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalSpecificThermalCapacity, "26");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalModulusOfElasticity, "27");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.PhysicalPoissonCoefficient, "28");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.MechanicalYield, "29");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.MechanicalTensile, "30");
            CreateXMLPropertyElement(properties, Material, PropertyTypeEnum.MechanicalElongation, "31");       
            Materials.Add(Material);
            MaterialData.Add(Materials);
            MaterialLibraryFile.Add(Version);
            MaterialLibraryFile.Add(MaterialData);
            xml.Add(MaterialLibraryFile);
            return xml;
        }
        /// <summary>
        /// Creates the XML property element.
        /// </summary>
        /// <param name="Material">The material.</param>
        /// <param name="type">The type.</param>
        /// <param name="number">The number.</param>
        private static void CreateXMLPropertyElement(IList<ExportPropertyGeneral> properties, XElement Material, PropertyTypeEnum type, string number)
        {
            XElement Property = new XElement(PROPERTY);
            Property.SetAttributeValue(VALUE, FormatWithZeroes(properties, type));
            Property.SetAttributeValue(NAME, ReturnNameOfElement(type));
            Property.SetAttributeValue(INDEX, number);
            Material.Add(Property);        
        }
        /// <summary>
        /// Formats the with zeroes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string FormatWithZeroes(IList<ExportPropertyGeneral> properties, PropertyTypeEnum type)
        {

            var property = (from u in properties where u.Type == type select u).FirstOrDefault();
            if (property != null)
            {
                string value = property.Value;
                switch (type)
                {
                    case PropertyTypeEnum.None:
                        break;
                    case PropertyTypeEnum.PhysicalModulusOfElasticity:
                        return AdddecimalPlaces((double.Parse(value) * 1000000000).ToString());
                    case PropertyTypeEnum.PhysicalPoissonCoefficient:
                        return AdddecimalPlaces((double.Parse(value)).ToString());
                    case PropertyTypeEnum.PhysicalDensity:
                        return AdddecimalPlaces((double.Parse(value) * 1000).ToString());
                    case PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                        return AdddecimalPlaces(string.Format("{0:f20}", (double.Parse(value) / 1000000)));
                    case PropertyTypeEnum.PhysicalThermalConductivity:
                        return AdddecimalPlaces(double.Parse(value).ToString());
                    case PropertyTypeEnum.PhysicalSpecificThermalCapacity:
                        return AdddecimalPlaces((double.Parse(value)).ToString());
                    case PropertyTypeEnum.MechanicalYield:
                        return AdddecimalPlaces((double.Parse(value) * 1000000).ToString());
                    case PropertyTypeEnum.MechanicalTensile:
                        return AdddecimalPlaces((double.Parse(value) * 1000000).ToString());
                    case PropertyTypeEnum.MechanicalElongation:
                        return AdddecimalPlaces(value);
                    default:
                        break;
                }
            }
            return "0.00000000000000000000";
        }
        /// <summary>
        /// Adddecimals the places.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string AdddecimalPlaces(string value)
        {
            if (!value.Contains("."))
            {
                value = value + ".00000000000000000000";
                return value;
            }
            for (int i = value.Split('.')[1].Length; i < 20; i++)
            {
                value = value + "0";
            }
            return value;
        }
        /// <summary>
        /// Returns the name of element.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string ReturnNameOfElement(PropertyTypeEnum type)
        {
            switch (type)
            {
                case PropertyTypeEnum.PhysicalModulusOfElasticity:
                    return "Modulus of Elasticity";
                case PropertyTypeEnum.PhysicalPoissonCoefficient:
                    return "Poisson's Ratio";
                case PropertyTypeEnum.PhysicalDensity:
                    return "Density";
                case PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                    return "Coef. of Thermal Exp";
                case PropertyTypeEnum.PhysicalThermalConductivity:
                    return "Thermal Conductivity";
                case PropertyTypeEnum.MechanicalYield:
                    return "Yield Stress";
                case PropertyTypeEnum.MechanicalTensile:
                    return "Ultimate Stress";
                case PropertyTypeEnum.MechanicalElongation:
                    return "Elongation";
                case PropertyTypeEnum.PhysicalSpecificThermalCapacity:
                    return "Specific Heat";
                default:
                    return "";
            }
        }

    }
}

