
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class AutodeskNastran
    {

        private const string NAME = "name";
        private const string matid = "matid";       
  

        private const string DISPLAYNAME = "displayname";
        private const string VALUE = "value";



        public static System.Xml.Linq.XDocument FillXML(int vkKey, string name, string standard, bool isSteel, IList<ExportPropertyGeneral> properties)
        {
           

            XDocument xml = new XDocument();
            var decl = new XDeclaration("1.0", null, null);
            xml.Declaration = decl;
            XElement materials = new XElement("materials");
            XElement classification = new XElement("classification");


            if (isSteel)
            {
                classification.SetAttributeValue(VALUE, "Steel");
            }
            else
            {
                classification.SetAttributeValue(VALUE, "Plastics");
            }

            //if (vkKey > 3000000)
            //{
            //    classification.SetAttributeValue(VALUE, "Nonferrous");
            
            //}
            //else if (vkKey > 1000000 && vkKey < 3000000)
            //{
           
            //}                 
            //else
            //{
                //switch (export.MaterialGroup)
                //{
                //    case Ktm.Core.Plus.PlusGroup.Cements:
                //        classification.SetAttributeValue(VALUE, "Cements");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Ceramics:
                //        classification.SetAttributeValue(VALUE, "Ceramics");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Composites:
                //        classification.SetAttributeValue(VALUE, "Composites");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Fibers:
                //        classification.SetAttributeValue(VALUE, "Fibers");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Foams:
                //        classification.SetAttributeValue(VALUE, "Foams");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Honeycombs:
                //        classification.SetAttributeValue(VALUE, "Honeycombs");
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.None:
                //        break;
                //    case Ktm.Core.Plus.PlusGroup.Polymers:
                //        classification.SetAttributeValue(VALUE, "Polymers");
                //        break;
                //    default:
                //        break;
                //}
              
            //}


            XElement material = new XElement("material");

            if (standard == "-" || standard == "")
            {
                material.SetAttributeValue(NAME, name);
            }
            else
            {
                material.SetAttributeValue(NAME, name + " " + standard);
            }
        
            material.SetAttributeValue(matid, "1");
            XElement physicalproperties = new XElement("physicalproperties");


            
            if (properties.Where(m=>m.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalModulusOfElasticity);
            }

            if (properties.Where(m => m.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalPoissonCoefficient);
            }

            if (properties.Where(m => m.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion);
            }
            if (properties.Where(m => m.Type == TMPropertyTypeEnum.PhysicalDensity).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalDensity);
            }

            if (properties.Where(m => m.Type == TMPropertyTypeEnum.PhysicalThermalConductivity).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalThermalConductivity);
            }

            if (properties.Where(m => m.Type == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.PhysicalSpecificThermalCapacity);
            }

            if (properties.Where(m => m.Type == TMPropertyTypeEnum.MechanicalTensile).Any())
            {
                CreateXMLPropertyElement(properties, physicalproperties, Models.TMPropertyTypeEnum.MechanicalTensile);
            }
 


            material.Add(physicalproperties);
            classification.Add(material);
            materials.Add(classification);
      
            xml.Add(materials);
            
            return xml;
        }


        private static void CreateXMLPropertyElement(IList<Models.ExportPropertyGeneral> properties, XElement Material, Models.TMPropertyTypeEnum type)
        {
            string propNodeName = "";
            switch (type)
            {
                case Models.TMPropertyTypeEnum.None:
                    break;
                case Models.TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                    propNodeName = "EX";
                    break;
                case Models.TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                    propNodeName = "NUXY";
                    break;
                case Models.TMPropertyTypeEnum.PhysicalDensity:
                    propNodeName = "DENS";
                    break;
                case Models.TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                    propNodeName = "ALPX";
                    break;
                case Models.TMPropertyTypeEnum.PhysicalThermalConductivity:
                    propNodeName = "KX";
                    break;
                case Models.TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:
                    propNodeName = "C";
                    break;
                case Models.TMPropertyTypeEnum.MechanicalTensile:
                    propNodeName = "SIGXT";
                    break;
                default:
                    break;
            }

            XElement Property = new XElement(propNodeName);
            Property.SetAttributeValue(DISPLAYNAME, ReturnNameOfElement(type));
            Property.SetAttributeValue(VALUE, FormatWithZeroes(properties, type));   
        
            Material.Add(Property);
        }


        private static string FormatWithZeroes(IList<Models.ExportPropertyGeneral> properties, Models.TMPropertyTypeEnum type)
        {

            var property = (from u in properties where u.Type == type select u).FirstOrDefault();
            if (property != null)
            {
                string value = property.Value;
                switch (type)
                {
                    case Models.TMPropertyTypeEnum.None:
                        break;
                    case Models.TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                        return AdddecimalPlaces((double.Parse(value) * 1000000000).ToString());
                    case Models.TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                        return AdddecimalPlaces((double.Parse(value)).ToString());
                    case Models.TMPropertyTypeEnum.PhysicalDensity:
                        return AdddecimalPlaces((double.Parse(value) * 1000).ToString());
                    case Models.TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                        return AdddecimalPlaces(string.Format("{0:f7}", (double.Parse(value) / 1000000)));
                    case Models.TMPropertyTypeEnum.PhysicalThermalConductivity:
                        return AdddecimalPlaces(double.Parse(value).ToString());
                    case Models.TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:
                        return AdddecimalPlaces((double.Parse(value)).ToString());                 
                    case Models.TMPropertyTypeEnum.MechanicalTensile:
                        return AdddecimalPlaces((double.Parse(value) * 1000000).ToString());
                
                    default:
                        break;
                }
            }
            return "";
        }





        private static string AdddecimalPlaces(string value)
        {
            if (!value.Contains("."))
            {
                value = value + ".";
                return value;
            }
          
            return value;
        }

        public static string ReturnNameOfElement(Models.TMPropertyTypeEnum type)
        {
            switch (type)
            {
                case TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                    return "Elastic modulus";
                case TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                    return "Poisson's ratio";
                case TMPropertyTypeEnum.PhysicalDensity:
                    return "Mass density";
                case TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                    return "Thermal expansion coefficient";
                case TMPropertyTypeEnum.PhysicalThermalConductivity:
                    return "Thermal conductivity";
               case TMPropertyTypeEnum.MechanicalTensile:
                    return "Tensile Strength in X";        
                case TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:
                    return "Specific heat";
                default:
                    return "";
            }
        }
    }
}