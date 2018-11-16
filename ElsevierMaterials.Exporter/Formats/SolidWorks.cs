using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class SolidWorks
    {

        private const string NAME = "name";
        private const string VALUE = "value";
        private const string DISPLAYNAME = "displayname";
        /// <summary>
        /// Fills the XML solid work.
        /// </summary>
        /// <returns></returns>
        public static XDocument FillXMLSolidWork(string standard, string name, bool isSteel, IList<ExportPropertyGeneral> properties)
        {
         
            XDocument xml = new XDocument(new XDeclaration("1.0", "Unicode", "yes"), null);
            XNamespace ns = "http://www.solidworks.com/sldmaterials";
            XNamespace c1 = "http://www.solidworks.com/sldmaterials";
            XNamespace c2 = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace c3 = "http://www.solidworks.com/sldcolorwatch";
            XElement mstns1 = new XElement(ns + "materials");
            mstns1.SetAttributeValue(XNamespace.Xmlns + "mstns", c1);
            mstns1.SetAttributeValue(XNamespace.Xmlns + "msdata", "urn:schemas-microsoft-com:xml-msdata");
            mstns1.SetAttributeValue(XNamespace.Xmlns + "xsi", c2);
            mstns1.SetAttributeValue(XNamespace.Xmlns + "sldcolorswatch", c3);
            XElement curves = new XElement("curves");
            curves.SetAttributeValue("id", "curve0");
            XElement point = new XElement("point");
            point.SetAttributeValue("x", "1.0");
            point.SetAttributeValue("y", "1.0");
            curves.Add(point);
            point = new XElement("point");
            point.SetAttributeValue("x", "2.0");
            point.SetAttributeValue("y", "1.0");
            curves.Add(point);
            point = new XElement("point");
            point.SetAttributeValue("x", "3.0");
            point.SetAttributeValue("y", "1.0");
            curves.Add(point);
            XElement classification = new XElement("classification");

            if (isSteel)
            {
                classification.SetAttributeValue(NAME, "steel");
            }
            else
            {
                classification.SetAttributeValue(NAME, "nonferrous");
            }       
            XElement material = new XElement("material");
            material.SetAttributeValue(NAME, name + " " + standard);
            XElement shaders = new XElement("shaders");
            XElement cgShaders = new XElement("cgshader");
            cgShaders.SetAttributeValue(NAME, "Steel AISI 304");
            shaders.Add(cgShaders);
            XElement pwShaders = new XElement("pwshader");
            pwShaders.SetAttributeValue(NAME, "Stainless Steel");
            shaders.Add(pwShaders);
            XElement swtexture = new XElement("swtexture");
            swtexture.SetAttributeValue("path", "images\\textures\\metal\\cast\\cast_fine.jpg");
            shaders.Add(swtexture);
            material.Add(shaders);
            XElement swatchColor = new XElement("swatchcolor");
            swatchColor.SetAttributeValue("RGB", "c0c0c0");
            XNamespace c4 = "http://www.solidworks.com/sldcolorwatch";
            XElement sldcolorswatch = new XElement(c4 + "Optical");
            sldcolorswatch.SetAttributeValue("Ambient", "1.000000");
            sldcolorswatch.SetAttributeValue("Transparency", "0.000000");
            sldcolorswatch.SetAttributeValue("Diffuse", "1.000000");
            sldcolorswatch.SetAttributeValue("Specularity", "1.000000");
            sldcolorswatch.SetAttributeValue("Shininess", "0.310000");
            sldcolorswatch.SetAttributeValue("Emission", "0.000000");
            swatchColor.Add(sldcolorswatch);
            material.Add(swatchColor);
            XElement xhatch = new XElement("xhatch");
            xhatch.SetAttributeValue(NAME, "ANSI32 (Steel)");
            xhatch.SetAttributeValue("angle", "0.0");
            xhatch.SetAttributeValue("scale", "1.0");
            material.Add(xhatch);
            XElement physicalproperties = new XElement("physicalproperties");
            CreateXMLPropertyElement(properties, physicalproperties, "EX", PropertyTypeEnum.PhysicalModulusOfElasticity);
            CreateXMLPropertyElement(properties, physicalproperties, "EY", PropertyTypeEnum.PhysicalModulusOfElasticity);
            CreateXMLPropertyElement(properties, physicalproperties, "EZ", PropertyTypeEnum.PhysicalModulusOfElasticity);
            CreateXMLPropertyElement(properties, physicalproperties, "NUXY", PropertyTypeEnum.PhysicalPoissonCoefficient);
            CreateXMLPropertyElement(properties, physicalproperties, "ALPX", PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
            CreateXMLPropertyElement(properties, physicalproperties, "DENS", PropertyTypeEnum.PhysicalDensity);
            CreateXMLPropertyElement(properties, physicalproperties, "KX", PropertyTypeEnum.PhysicalThermalConductivity);
            CreateXMLPropertyElement(properties, physicalproperties, "KY", PropertyTypeEnum.PhysicalThermalConductivity);
            CreateXMLPropertyElement(properties, physicalproperties, "C", PropertyTypeEnum.PhysicalSpecificThermalCapacity);
            CreateXMLPropertyElement(properties, physicalproperties, "SIGXT", PropertyTypeEnum.MechanicalTensile);
            CreateXMLPropertyElement(properties, physicalproperties, "SIGYLD", PropertyTypeEnum.MechanicalYield);
            material.Add(physicalproperties);
            mstns1.Add(curves);
            classification.Add(material);
            mstns1.Add(classification);
            xml.Add(mstns1);
            return xml;
        }
        /// <summary>
        /// Creates the XML property element.
        /// </summary>
        /// <param name="physicalproperties">The physicalproperties.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="type">The type.</param>
        private static void CreateXMLPropertyElement(IList<ExportPropertyGeneral> properties, XElement physicalproperties, string elementName, PropertyTypeEnum type)
        {
            string value = "";
            XElement EX = new XElement(elementName);
            value = formatValue(properties, type);
            if (value != null)
            {
                EX.SetAttributeValue(VALUE, value);
                EX.SetAttributeValue(DISPLAYNAME, ReturnNameOfElement(type));
                physicalproperties.Add(EX);
            }
        }
        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string formatValue(IList<ExportPropertyGeneral> properties, PropertyTypeEnum type)
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
                        value = string.Format("{0:f3}", double.Parse(value) / 100);
                        value = value + "e+11";
                        return value;
                    case PropertyTypeEnum.PhysicalPoissonCoefficient:
                        value = string.Format("{0:f4}", double.Parse(value));
                        return value;
                    case PropertyTypeEnum.PhysicalDensity:
                        value = (double.Parse(value) * 1000).ToString();
                        return value;
                    case PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                        value = string.Format("{0:f9}", double.Parse(value) / 1000000);
                        return value;
                    case PropertyTypeEnum.PhysicalThermalConductivity:
                        value = string.Format("{0:f4}", double.Parse(value));
                        return value;
                    case PropertyTypeEnum.PhysicalSpecificThermalCapacity:
                        value = string.Format("{0:f1}", double.Parse(value));
                        return value;
                    case PropertyTypeEnum.MechanicalYield:
                        value = string.Format("{0:f3}", double.Parse(value) / 100);
                        value = value + "e+8";
                        return value;
                    case PropertyTypeEnum.MechanicalTensile:
                        value = string.Format("{0:f3}", double.Parse(value) / 100);
                        value = value + "e+8";
                        return value;
                    default:
                        return "0.0";
                }
            }

            return null;
        }
        /// <summary>
        /// Formats the with e.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isGPa">if set to <c>true</c> [is g pa].</param>
        /// <returns></returns>
        public static string FormatWithE(string value, bool isGPa)
        {
            value = string.Format("{0:f3}", double.Parse(value) / 100);
            value = value + "e+11";
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
                    return "Elastic Modulus";
                case PropertyTypeEnum.PhysicalPoissonCoefficient:
                    return "Poissons Ratio";
                case PropertyTypeEnum.PhysicalDensity:
                    return "Density";
                case PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                    return "Thermal Expansion Coefficient";
                case PropertyTypeEnum.PhysicalThermalConductivity:
                    return "Thermal Conductivity";
                case PropertyTypeEnum.MechanicalYield:
                    return "Yield Strength";
                case PropertyTypeEnum.MechanicalTensile:
                    return "Tensile Strength";
                case PropertyTypeEnum.PhysicalSpecificThermalCapacity:
                    return "Specific Heat";
                default:
                    return "";
            }      
        }

    }
}





