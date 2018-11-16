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
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalDensity, "23");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "24");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalThermalConductivity, "25");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalSpecificThermalCapacity, "26");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalModulusOfElasticity, "27");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.PhysicalPoissonCoefficient, "28");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.MechanicalYield, "29");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.MechanicalTensile, "30");
            CreateXMLPropertyElement(properties, Material, TMPropertyTypeEnum.MechanicalElongation, "31");       
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
        private static void CreateXMLPropertyElement(IList<ExportPropertyGeneral> properties, XElement Material, TMPropertyTypeEnum type, string number)
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
        private static string FormatWithZeroes(IList<ExportPropertyGeneral> properties, TMPropertyTypeEnum type)
        {

            var property = (from u in properties where u.Type == type select u).FirstOrDefault();
            if (property != null)
            {
                string value = property.Value;
                switch (type)
                {
                    case TMPropertyTypeEnum.None:
                        break;
                    case TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                        return AdddecimalPlaces((double.Parse(value) * 1000000000).ToString());
                    case TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                        return AdddecimalPlaces((double.Parse(value)).ToString());
                    case TMPropertyTypeEnum.PhysicalDensity:
                        return AdddecimalPlaces((double.Parse(value) * 1000).ToString());
                    case TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                        return AdddecimalPlaces(string.Format("{0:f20}", (double.Parse(value) / 1000000)));
                    case TMPropertyTypeEnum.PhysicalThermalConductivity:
                        return AdddecimalPlaces(double.Parse(value).ToString());
                    case TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:
                        return AdddecimalPlaces((double.Parse(value)).ToString());
                    case TMPropertyTypeEnum.MechanicalYield:
                        return AdddecimalPlaces((double.Parse(value) * 1000000).ToString());
                    case TMPropertyTypeEnum.MechanicalTensile:
                        return AdddecimalPlaces((double.Parse(value) * 1000000).ToString());
                    case TMPropertyTypeEnum.MechanicalElongation:
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
        public static string ReturnNameOfElement(TMPropertyTypeEnum type)
        {
            switch (type)
            {
                case TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                    return "Modulus of Elasticity";
                case TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                    return "Poisson's Ratio";
                case TMPropertyTypeEnum.PhysicalDensity:
                    return "Density";
                case TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                    return "Coef. of Thermal Exp";
                case TMPropertyTypeEnum.PhysicalThermalConductivity:
                    return "Thermal Conductivity";
                case TMPropertyTypeEnum.MechanicalYield:
                    return "Yield Stress";
                case TMPropertyTypeEnum.MechanicalTensile:
                    return "Ultimate Stress";
                case TMPropertyTypeEnum.MechanicalElongation:
                    return "Elongation";
                case TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:
                    return "Specific Heat";
                default:
                    return "";
            }
        }

    }
}

