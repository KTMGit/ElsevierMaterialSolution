using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{

    public class ANSYS
    {
   
        private const string REVISION = "** Revision - 0";
        private const string UNITSYSTEM = "***** UNIT SYSTEM******";

        /// <summary>
        /// Fills the ansys.
        /// </summary>
        /// <returns></returns>
        public static XDocument FillANSYS(int vkKey, string name, IList<ExportPropertyGeneral> properties)
        {
          
            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", null), null);
            XElement EngineeringData = new XElement("EngineeringData");
            EngineeringData.SetAttributeValue("version", "14.0.0.367");
            EngineeringData.SetAttributeValue("versiondate", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            xml.Add(EngineeringData);
            XElement Notes = new XElement("Notes", "");
            EngineeringData.Add(Notes);
            XElement Materials = new XElement("Materials");
            EngineeringData.Add(Materials);
            XElement MatML_Doc = new XElement("MatML_Doc");
            Materials.Add(MatML_Doc);
            XElement Material = new XElement("Material");
            MatML_Doc.Add(Material);
            XElement BulkDetails = new XElement("BulkDetails");
            Material.Add(BulkDetails);
            XElement Name = new XElement("Name");
            Name.Value = name;
            BulkDetails.Add(Name);
            FillPropertyData(BulkDetails, properties, vkKey);
            XElement Metadata = new XElement("Metadata");
            MatML_Doc.Add(Metadata);
            FillMetaData(Metadata, properties);
            return xml;
        }
        /// <summary>
        /// Fills the property data.
        /// </summary>
        /// <param name="BulkDetails">The bulk details.</param>
        private static void FillPropertyData(XElement BulkDetails, IList<ExportPropertyGeneral> properties, int vkKey)
        {
     
            //Physical Density
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalDensity select u).Any())
            {
                ExportPropertyGeneral propertyDensity = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalDensity select u).FirstOrDefault();
                CreatePropertyData(vkKey, properties, BulkDetails, (Double.Parse(propertyDensity.Value) * 1000).ToString(), "float", "pr0", new ANSYSQualifier() { Value = "Dependent", Name = "Variable Type" });
            }

            //Physical
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalDensity || u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient select u).Any())
            {
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr1", new ANSYSQualifier() { Value = "Isotropic", Name = "Behavior" }, new ANSYSQualifier() { Value = "Young's Modulus and Poisson's Ratio", Name = "Derive from" });
            }



            //Physical Thermal Conductivity
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalThermalConductivity select u).Any())
            {
                string tmpValue = "";
                string tmpQualifier = "";

                foreach (var prop in properties.Where(p => p.Type == TMPropertyTypeEnum.PhysicalThermalConductivity))
                {
                    tmpQualifier += ",Dependent";
                    tmpValue += "," + Double.Parse(prop.Value).ToString();
                }
                tmpValue = tmpValue.Trim().TrimStart(',');
                tmpQualifier = tmpQualifier.Trim().TrimStart(',');

                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr20", new ANSYSQualifier() { Value = "Isotropic", Name = "Behavior" });
            }


            //Physical PropertiesOfficial Mean Coefficient
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion select u).Any())
            {
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr21", new ANSYSQualifier() { Value = "Secant", Name = "Definition" }, new ANSYSQualifier() { Value = "Isotropic", Name = "Behavior" });
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr22", new ANSYSQualifier() { Value = "Secant", Name = "Definition" }, new ANSYSQualifier() { Value = "Isotropic", Name = "Behavior" });
            }


            //Physical Specific Thermal Capacity
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity select u).Any())
            {
                string tmpValue = "";
                string tmpQualifier = "";

                foreach (var prop in properties.Where(p => p.Type == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity))
                {
                    tmpQualifier += ",Dependent";
                    tmpValue += "," + Double.Parse(prop.Value).ToString();
                }
                tmpValue = tmpValue.Trim().TrimStart(',');
                tmpQualifier = tmpQualifier.Trim().TrimStart(',');


                CreatePropertyData(vkKey, properties, BulkDetails, tmpValue, "float", "pr26", new ANSYSQualifier() { Value = tmpQualifier, Name = "Variable Type" });
            }
            //Stress Strain
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PlasticStrainStress select u).Any())
            {
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr2", new ANSYSQualifier() { Value = "Multilinear", Name = "Definition" });
            }
            //Fatigue stress
            if ((from u in properties where u.Type == TMPropertyTypeEnum.FatigueStressPoints select u).Any())
            {
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr3", new ANSYSQualifier() { Value = "Mean Stress", Name = "Definition" }, new ANSYSQualifier() { Value = "Linear", Name = "Interpolation" });
            }
            //Fatigue strain
            if ((from u in properties where u.Type == TMPropertyTypeEnum.FatigueStrengthCoefficient || u.Type == TMPropertyTypeEnum.FatigueStrengthExponent || u.Type == TMPropertyTypeEnum.FatigueDuctilityCoefficient || u.Type == TMPropertyTypeEnum.FatigueDuctilityExponent || u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient || u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthExponent select u).Any())
            {
                CreatePropertyData(vkKey, properties, BulkDetails, "-", "string", "pr4", new ANSYSQualifier() { Value = "Strain-Life", Name = "Display Curve Type" });
            }
            //Mechanical Yiels
            if ((from u in properties where u.Type == TMPropertyTypeEnum.MechanicalYield select u).Any())
            {
                ExportPropertyGeneral propertyMechanicalYield = (from u in properties where u.Type == TMPropertyTypeEnum.MechanicalYield select u).FirstOrDefault();
                CreatePropertyData(vkKey, properties, BulkDetails, (Double.Parse(propertyMechanicalYield.Value) * 1000000).ToString(), "float", "pr5", new ANSYSQualifier() { Value = "Dependent", Name = "Variable Type" });
            }
            //Mechanical Tensile
            if ((from u in properties where u.Type == TMPropertyTypeEnum.MechanicalTensile select u).Any())
            {
                ExportPropertyGeneral propertyMechanicalTensile = (from u in properties where u.Type == TMPropertyTypeEnum.MechanicalTensile select u).FirstOrDefault();
                CreatePropertyData(vkKey, properties, BulkDetails, (Double.Parse(propertyMechanicalTensile.Value) * 1000000).ToString(), "float", "pr6", new ANSYSQualifier() { Value = "Dependent", Name = "Variable Type" });
            }

        }
        /// <summary>
        /// Creates the property data.
        /// </summary>
        /// <param name="BulkDetails">The bulk details.</param>
        /// <param name="dataValue">The data value.</param>
        /// <param name="dataFormat">The data format.</param>
        /// <param name="property">The property.</param>
        /// <param name="qualifiers">The qualifiers.</param>
        private static void CreatePropertyData(int vkKey, IList<ExportPropertyGeneral> properties, XElement BulkDetails, string dataValue, string dataFormat, string property, params ANSYSQualifier[] qualifiers)
        {
            XElement PropertyData = new XElement("PropertyData");
            PropertyData.SetAttributeValue("property", property);
            XElement Data = new XElement("Data");
            Data.SetAttributeValue("format", dataFormat);
            Data.Value = dataValue;
            PropertyData.Add(Data);
            foreach (var item in qualifiers)
            {
                XElement Qualifier = new XElement("Qualifier");
                Qualifier.SetAttributeValue("name", item.Name);
                Qualifier.Value = item.Value;
                PropertyData.Add(Qualifier);
            }
            // Physical properties - density
            if (property == "pr0")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa0", "float", TMPropertyTypeEnum.PhysicalDensity, "Independent");
            }
            // Physical properties
            if (property == "pr1")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa1", "float", TMPropertyTypeEnum.PhysicalModulusOfElasticity, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa2", "float", TMPropertyTypeEnum.PhysicalPoissonCoefficient, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa3", "float", TMPropertyTypeEnum.None, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa4", "float", TMPropertyTypeEnum.None, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa0", "float", TMPropertyTypeEnum.PhysicalModulusOfElasticity, "Independent");
            }
            //Stress strain
            if (property == "pr2")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa5", "float", TMPropertyTypeEnum.PlasticStrainStress, "");
                CreateParameterValue(vkKey, properties, PropertyData, "pa6", "float", TMPropertyTypeEnum.PlasticStrainStress, "");
                CreateParameterValue(vkKey, properties, PropertyData, "pa0", "float", TMPropertyTypeEnum.PlasticStrainStress, "");
            }
            //Fatigue stress
            if (property == "pr3")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa7", "float", TMPropertyTypeEnum.FatigueStressPoints, "");
                CreateParameterValue(vkKey, properties, PropertyData, "pa8", "float", TMPropertyTypeEnum.FatigueStressPoints, "");
                CreateParameterValue(vkKey, properties, PropertyData, "pa9", "float", TMPropertyTypeEnum.FatigueStressPoints, "");
            }
            // Fatigue strain
            if (property == "pr4")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa10", "float", TMPropertyTypeEnum.FatigueStrengthCoefficient, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa11", "float", TMPropertyTypeEnum.FatigueStrengthExponent, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa12", "float", TMPropertyTypeEnum.FatigueDuctilityCoefficient, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa13", "float", TMPropertyTypeEnum.FatigueDuctilityExponent, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa14", "float", TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa15", "float", TMPropertyTypeEnum.FatigueCyclicStrengthExponent, "Dependent");
            }

            //Physical Properties Thermal Conductivity
            if (property == "pr20")
            {
                string tmpQualifier = "";
                string tmpQualifierD = "";
                foreach (var prop in properties.Where(p => p.Type == TMPropertyTypeEnum.PhysicalThermalConductivity))
                {
                    tmpQualifier += ",Independent";
                    tmpQualifierD += ",Dependent";

                }
                tmpQualifier = tmpQualifier.Trim().TrimStart(',');
                tmpQualifierD = tmpQualifierD.Trim().TrimStart(',');

                CreateParameterValue(vkKey, properties, PropertyData, "pa20", "float", TMPropertyTypeEnum.PhysicalThermalConductivity, tmpQualifierD);
                CreateParameterValue(vkKey, properties, PropertyData, "pa21", "float", TMPropertyTypeEnum.PhysicalThermalConductivity, tmpQualifier);
            }

            //Physical Properties MeanCoefficient
            if (property == "pr21")
            {

                string tmpQualifier = "";
                string tmpQualifierD = "";
                foreach (var prop in properties.Where(p => p.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion))
                {
                    tmpQualifier += ",Independent";
                    tmpQualifierD += ",Dependent";

                }
                tmpQualifier = tmpQualifier.Trim().TrimStart(',');
                tmpQualifierD = tmpQualifierD.Trim().TrimStart(',');


                CreateParameterValue(vkKey, properties, PropertyData, "pa22", "float", TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, tmpQualifierD);
                CreateParameterValue(vkKey, properties, PropertyData, "pa25", "float", TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, tmpQualifier);              
            }

            //Physical Properties MeanCoefficient
            if (property == "pr22")
            {
                CreateParameterValue(vkKey, properties, PropertyData, "pa23", "float", TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "Dependent");
                CreateParameterValue(vkKey, properties, PropertyData, "pa24", "string", TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "");
            }
         

            //Physical Specific Thermal Capacity
            if (property == "pr26")
            {
                string tmpQualifier = "";
                foreach (var prop in properties.Where(p => p.Type == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity))
                {
                    tmpQualifier += ",Independent";
                 
                }
               tmpQualifier = tmpQualifier.Trim().TrimStart(',');

               CreateParameterValue(vkKey, properties, PropertyData, "pa26", "float", TMPropertyTypeEnum.PhysicalSpecificThermalCapacity, tmpQualifier);
            }



            BulkDetails.Add(PropertyData);
        }
        /// <summary>
        /// Creates the parameter value.
        /// </summary>
        /// <param name="PropertyData">The property data.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="format">The format.</param>
        /// <param name="exportType">Type of the export.</param>
        /// <param name="variableType">Type of the variable.</param>
        private static void CreateParameterValue(int vkKey, IList<ExportPropertyGeneral> properties, XElement PropertyData, string parameter, string format, TMPropertyTypeEnum exportType, string variableType)
        {
          
            XElement ParameterValue = new XElement("ParameterValue");
            ParameterValue.SetAttributeValue("parameter", parameter);
            ParameterValue.SetAttributeValue("format", format);
            XElement Data = new XElement("Data", "");

            ExportPropertyGeneral property = (from u in properties where u.Type == exportType select u).FirstOrDefault();
            if (property != null)
            {
                if (exportType == TMPropertyTypeEnum.PhysicalModulusOfElasticity)
                {
                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == exportType select u).ToList();
                    if (parameter == "pa0")
                    {                      
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                if (vkKey< 1000000 && prop.Temperature == 999)
                                {
                                    Data.Value = "23";
                                }
                                else
                                {
                                    Data.Value = prop.Temperature.ToString();
                                }
                      
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Temperature.ToString();
                            }
                        } 
                    }
                    else
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                Data.Value = (Double.Parse(prop.Value) * 1000000000).ToString();
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + (Double.Parse(prop.Value) * 1000000000).ToString();
                            }
                        }    

                    }
                }
                else if (exportType == TMPropertyTypeEnum.PhysicalPoissonCoefficient)
                {
                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity select u).ToList();

                    if (parameter == "pa2")
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                Data.Value = property.Value;
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + property.Value;
                            }
                        }
                    }
                   
                   
                }
                else if (exportType == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity)
                {
                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == exportType select u).ToList();
                    if (parameter == "pa26")
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {

                                if (vkKey < 1000000 && prop.Temperature == 999)
                                {
                                    Data.Value = "23";
                                }
                                else
                                {
                                    Data.Value = prop.Temperature.ToString();
                                }
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Temperature.ToString();
                            }                      
                        }                     
                    }
                    else
                    {

                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                Data.Value = prop.Value.ToString();
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Value.ToString();
                            }
                        }    


                    }
                }
                else if (exportType == TMPropertyTypeEnum.PhysicalThermalConductivity)
                {
                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == exportType select u).ToList();
                    if (parameter == "pa21")
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {

                                if (vkKey < 1000000 && prop.Temperature == 999)
                                {
                                    Data.Value = "23";
                                }
                                else
                                {
                                    Data.Value = prop.Temperature.ToString();
                                }
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Temperature.ToString();
                            }
                        }
                    }
                    else
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                Data.Value = prop.Value.ToString();
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Value.ToString();                         
                            }
                        }                       
                    }
                }
                else if (exportType == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion)
                {
                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == exportType select u).ToList();
                    if (parameter == "pa25")
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                if (vkKey < 1000000 && prop.Temperature == 999)
                                {
                                    Data.Value = "23";
                                }
                                else
                                {
                                    Data.Value = prop.Temperature.ToString();
                                }
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + prop.Temperature.ToString();
                            }
                        }
                    }
                    else if (parameter == "pa23")
                    {
                        if (vkKey < 1000000)
                        {
                            if (property.Temperature == 999)
                            {
                                Data.Value = "23";
                            }
                            else
                            {
                                Data.Value = property.Temperature.ToString();
                            }
                        }
                        else
                        {
                            Data.Value = "20";
                        }                      
                    }
                    else if (parameter == "pa24")
                    {
                        Data.Value = "Coefficient of Thermal Expansion";
                    }
                    else
                    {
                        foreach (var prop in list)
                        {
                            if (list.IndexOf(prop) == 0)
                            {
                                Data.Value = string.Format(CultureInfo.InvariantCulture, "{0:0.###e+00}", Double.Parse(prop.Value) / 1000000);
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + string.Format(CultureInfo.InvariantCulture, "{0:0.###e+00}", Double.Parse(prop.Value) / 1000000);
                            }
                        }
                    }
                }
                else if (exportType == TMPropertyTypeEnum.FatigueStrengthCoefficient || exportType == TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient)
                {
                    Data.Value = (Double.Parse(property.Value) * 1000000).ToString();
                }

                else if (exportType == TMPropertyTypeEnum.FatigueCyclicStrengthExponent)
                {
                    Data.Value = string.Format("{0:f4}", double.Parse(property.Value));
                }
                else if (exportType == TMPropertyTypeEnum.PhysicalDensity)
                {
                    Data.Value = "-";
                }
                else if (exportType == TMPropertyTypeEnum.PlasticStrainStress)
                {

                    IList<ExportPropertyGeneral> list = (from u in properties where u.Type == exportType select u).ToList();
                    Data.Value = "";
                    foreach (var prop in list)
                    {
                        variableType = FillStressStrainParameters(parameter, variableType, ref Data, prop);
                    }

                  
                }
                else if (exportType == TMPropertyTypeEnum.FatigueStressPoints)
                {

                    Data.Value = "";

                    foreach (var item in property.Values)
                    {
                        if (parameter == "pa8")
                        {
                            string valueX = "1";
                            for (int i = 1; i < double.Parse(item.X); i++)
                            {
                                valueX = valueX + "0";
                            }
                            if (Data.Value == "")
                            {
                                Data.Value = Data.Value + valueX;
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + valueX;
                            }

                            if (variableType == "")
                            {
                                variableType = variableType + "Independent";
                            }
                            else
                            {
                                variableType = variableType + ",Independent";
                            }
                        }
                        if (parameter == "pa7")
                        {
                            if (Data.Value == "")
                            {
                                Data.Value = Data.Value + item.Y * 1000000;
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + item.Y * 1000000;
                            }

                            if (variableType == "")
                            {
                                variableType = variableType + "Dependent";
                            }
                            else
                            {
                                variableType = variableType + ",Dependent";
                            }
                        }
                        if (parameter == "pa9")
                        {
                            if (Data.Value == "")
                            {
                                Data.Value = Data.Value + 0;
                            }
                            else
                            {
                                Data.Value = Data.Value + "," + 0;
                            }
                            if (variableType == "")
                            {
                                variableType = variableType + "Independent";
                            }
                            else
                            {
                                variableType = variableType + ",Independent";
                            }
                        }
                    }

                }
                else
                {
                    Data.Value = property.Value;
                }
            }


            XElement Qualifier = new XElement("Qualifier");
            Qualifier.SetAttributeValue("name", "Variable Type");
            Qualifier.Value = variableType;
            ParameterValue.Add(Data);
            ParameterValue.Add(Qualifier);
            PropertyData.Add(ParameterValue);
        }

        private static string FillStressStrainParameters(string parameter, string variableType, ref XElement Data, ExportPropertyGeneral property)
        {  
            foreach (var item in property.Values)
            {
                if (parameter == "pa6")
                {
                    if (Data.Value == "")
                    {
                        Data.Value = Data.Value + item.X;
                    }
                    else
                    {
                        Data.Value = Data.Value + "," + item.X;
                    }

                    if (variableType == "")
                    {
                        variableType = variableType + "Independent";
                    }
                    else
                    {
                        variableType = variableType + ",Independent";
                    }
                }
                if (parameter == "pa5")
                {
                    if (Data.Value == "")
                    {
                        Data.Value = Data.Value + item.Y * 1000000;
                    }
                    else
                    {
                        Data.Value = Data.Value + "," + item.Y * 1000000;
                    }

                    if (variableType == "")
                    {
                        variableType = variableType + "Dependent";
                    }
                    else
                    {
                        variableType = variableType + ",Dependent";
                    }
                }
                if (parameter == "pa0")
                {
                    if (Data.Value == "")
                    {
                        Data.Value = Data.Value + property.Temperature;
                    }
                    else
                    {
                        Data.Value = Data.Value + "," + property.Temperature;
                    }
                    if (variableType == "")
                    {
                        variableType = variableType + "Independent";
                    }
                    else
                    {
                        variableType = variableType + ",Independent";
                    }
                }
            }
            return variableType;
        }
        /// <summary>
        /// Fills the meta data.
        /// </summary>
        /// <param name="Metadata">The metadata.</param>
        private static void FillMetaData(XElement Metadata, IList<ExportPropertyGeneral> properties)
        {
            IList<ANSYSParameterDetail> ListOfParametersDetails = FillListOFParameterDetails();
        
            //firstly goes parameters 
            foreach (var item in from u in ListOfParametersDetails where u.IdPrefix == "pa" select u)
            {
                //For young and poison and for plastic strain we show  more matadatas (first four)
                if ((item.IdString == "pa0" || item.IdString == "pa1" || item.IdString == "pa2" || item.IdString == "pa3" || item.IdString == "pa4") && (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.PhysicalDensity select u).Any())
                {
                    AddMetaDataXMLNode(Metadata, item);
                } // For plastic we show temperature too
                else if (item.IdString == "pa0" && ((from u in properties where u.Type == TMPropertyTypeEnum.PlasticStrainStress select u).Any()))
                {
                    AddMetaDataXMLNode(Metadata, item);
                }              
                else
                {
                    //for other proeprties we show only one matadata
                    ExportPropertyGeneral prop = (from u in properties where u.Type == item.Type select u).FirstOrDefault();
                    if (prop != null)
                    {
                        AddMetaDataXMLNode(Metadata, item);
                    }
                }

            }
            //then goes properties 
            foreach (var item in from u in ListOfParametersDetails where u.IdPrefix == "pr" select u)
            {

                if ((item.Type == TMPropertyTypeEnum.None && item.Id == 1 && ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient select u).Count() > 0 || (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity select u).Count() > 0 || (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalDensity select u).Count() > 0 )) || (item.Type == TMPropertyTypeEnum.None && item.Id == 2 && (from u in properties where u.Type == TMPropertyTypeEnum.PlasticStrainStress select u).Count() > 0) || (item.Type == TMPropertyTypeEnum.None && item.Id == 3 && (from u in properties where u.Type == TMPropertyTypeEnum.FatigueStressPoints select u).Count() > 0) || (item.Type == TMPropertyTypeEnum.None && item.Id == 4 && (from u in properties where u.Type == TMPropertyTypeEnum.FatigueStrengthCoefficient select u).Count() > 0))
                {
                    AddMetaDataXMLNode(Metadata, item);
                }
                else
                {
                    ExportPropertyGeneral prop = (from u in properties where u.Type == item.Type select u).FirstOrDefault();
                    //only if I have property than should be in the list
                    if (prop != null)
                    {
                        AddMetaDataXMLNode(Metadata, item);
                    }
                }

            }

        }
        /// <summary>
        /// Adds the meta data XML node.
        /// </summary>
        /// <param name="Metadata">The metadata.</param>
        /// <param name="parameterDetail">The parameter detail.</param>
        private static void AddMetaDataXMLNode(XElement Metadata, ANSYSParameterDetail parameterDetail)
        {
            XElement ParameterDetails = new XElement("ParameterDetails");
            if (parameterDetail.IdPrefix == "pr")
            {
                ParameterDetails = new XElement("PropertyDetails");
            }
            ParameterDetails.SetAttributeValue("id", parameterDetail.IdString);
            XElement Name = new XElement("Name");
            Name.Value = parameterDetail.Name;



            if (parameterDetail.Units != null && parameterDetail.Units.Count > 0)
            {
                XElement Units = new XElement("Units");
                foreach (var item in parameterDetail.Units)
                {
                    XElement Unit = new XElement("Unit");
                    if (item.Attribite != null)
                    {
                        Unit.SetAttributeValue(item.Attribite, item.AttribiteValue);
                    }
                    XElement NameOfUnit = new XElement("Name");
                    NameOfUnit.Value = item.Name;
                    Unit.Add(NameOfUnit);
                    Units.Add(Unit);
                }
                if (parameterDetail.IdPrefix == "pr")
                {
                    ParameterDetails.Add(Units);
                    ParameterDetails.Add(Name);
                }
                else
                {
                    ParameterDetails.Add(Name);
                    ParameterDetails.Add(Units);
                }
            }
            else
            {
                if (parameterDetail.IdPrefix == "pr")
                {
                    XElement Unitless = new XElement("Unitless");
                    ParameterDetails.Add(Unitless);
                    ParameterDetails.Add(Name);
                }
                else
                {
                    ParameterDetails.Add(Name);
                    XElement Unitless = new XElement("Unitless");
                    ParameterDetails.Add(Unitless);
                }
            }

            Metadata.Add(ParameterDetails);
        }
        /// <summary>
        /// Fills the list of parameter details.
        /// </summary>
        /// <returns></returns>
        private static IList<ANSYSParameterDetail> FillListOFParameterDetails()
        {
            IList<ANSYSParameterDetail> listOfParameters = new List<ANSYSParameterDetail>();
            ANSYSParameterDetail parameterDetail = null;

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalModulusOfElasticity, "pa", 0, "Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalModulusOfElasticity, "pa", 1, "Young's Modulus", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalPoissonCoefficient, "pa", 2, "Poisson's Ratio", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PlasticStrainStress, "pa", 3, "Bulk Modulus", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PlasticStrainStress, "pa", 4, "Shear Modulus", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PlasticStrainStress, "pa", 5, "Stress", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PlasticStrainStress, "pa", 6, "Plastic Strain", new List<ANSYSUnit> { new ANSYSUnit() { Name = "m" }, new ANSYSUnit { Name = "m", Attribite = "power", AttribiteValue = "-1" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueStressPoints, "pa", 7, "Alternating Stress", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueStressPoints, "pa", 8, "Cycles", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueStressPoints, "pa", 9, "Mean Stress", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueStrengthCoefficient, "pa", 10, "Strength Coefficient", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueStrengthExponent, "pa", 11, "Strength Exponent", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueDuctilityCoefficient, "pa", 12, "Ductility Coefficient", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueDuctilityExponent, "pa", 13, "Ductility Exponent", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient, "pa", 14, "Cyclic Strength Coefficient", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.FatigueCyclicStrengthExponent, "pa", 15, "Cyclic Strain Hardening Exponent", null);
            listOfParameters.Add(parameterDetail);

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalThermalConductivity, "pa", 20, "Thermal Conductivity", new List<ANSYSUnit> { new ANSYSUnit() { Name = "W" }, new ANSYSUnit() { Name = "m", Attribite = "power", AttribiteValue = "-1" }, new ANSYSUnit() { Name = "C", Attribite = "power", AttribiteValue = "-1" } });
            listOfParameters.Add(parameterDetail);

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pa", 22, "Coefficient of Thermal Expansion", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C", Attribite = "power", AttribiteValue = "-1" }});
            listOfParameters.Add(parameterDetail);



            ///Boca
            ///
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalThermalConductivity, "pa", 21, "Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C" } });
            listOfParameters.Add(parameterDetail);

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity, "pa", 26, "Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C" } });
            listOfParameters.Add(parameterDetail);

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pa", 25, "Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C" } });
            listOfParameters.Add(parameterDetail);

            //parameterDetail = CreateParameterDetail(PropertyTypeEnum.None, "pa", 18, "Reference Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C"} });
            //listOfParameters.Add(parameterDetail);

            //parameterDetail = CreateParameterDetail(PropertyTypeEnum.None, "pa", 19, "Material Property", null);
            //listOfParameters.Add(parameterDetail);
   
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalDensity, "pr", 0, "Density", new List<ANSYSUnit> { new ANSYSUnit() { Name = "kg" }, new ANSYSUnit { Name = "m", Attribite = "power", AttribiteValue = "-3" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.None, "pr", 1, "Elasticity", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.None, "pr", 2, "Isotropic Hardening", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.None, "pr", 3, "Alternating Stress", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.None, "pr", 4, "Strain-Life Parameters", null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.MechanicalYield, "pr", 5, "Tensile Yield Strength", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.MechanicalTensile, "pr", 6, "Tensile Ultimate Strength", new List<ANSYSUnit> { new ANSYSUnit() { Name = "Pa" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalThermalConductivity, "pr", 20, "Thermal Conductivity",null);
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pr", 21, "Coefficient of Thermal Expansion", null);
            listOfParameters.Add(parameterDetail);


            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pr", 22, "Reference Temperature", null);
            listOfParameters.Add(parameterDetail);

            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pa", 23, "Reference Temperature", new List<ANSYSUnit> { new ANSYSUnit() { Name = "C" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, "pa", 24, "Material Property", null);
            listOfParameters.Add(parameterDetail);


            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.PhysicalSpecificThermalCapacity, "pr", 26, "Specific Heat", new List<ANSYSUnit> { new ANSYSUnit() { Name = "J"}, new ANSYSUnit() { Name = "kg", Attribite = "power", AttribiteValue = "-1" }, new ANSYSUnit() { Name = "C", Attribite = "power", AttribiteValue = "-1" } });
            listOfParameters.Add(parameterDetail);
            parameterDetail = CreateParameterDetail(TMPropertyTypeEnum.None, "pr", 10, "Reference Temperature", null);
            listOfParameters.Add(parameterDetail);


            return listOfParameters;
        }
        /// <summary>
        /// Creates the parameter detail.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="idPrefix">The identifier prefix.</param>
        /// <param name="Id">The identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        private static ANSYSParameterDetail CreateParameterDetail(TMPropertyTypeEnum type, string idPrefix, int Id, string Name, IList<ANSYSUnit> units)
        {
            ANSYSParameterDetail parameterDetail = new ANSYSParameterDetail();
            parameterDetail.Id = Id;
            parameterDetail.IdPrefix = idPrefix;
            parameterDetail.Type = type;
            parameterDetail.IdString = idPrefix + Id.ToString();
            parameterDetail.Name = Name;
            if (units != null && units.Count > 0)
            {
                parameterDetail.Units = units;
            }

            return parameterDetail;
        }
    }

    /// <summary>
    /// Parameter detail
    /// </summary>
    public class ANSYSParameterDetail
    {
        public TMPropertyTypeEnum Type { get; set; }
        public int Id { get; set; }
        public string IdPrefix { get; set; }
        public string IdString { get; set; }
        public string Name { get; set; }
        public IList<ANSYSUnit> Units { get; set; }
    }
    /// <summary>
    /// Unit
    /// </summary>
    public class ANSYSUnit
    {
        public string Name { get; set; }
        public string Attribite { get; set; }
        public string AttribiteValue { get; set; }
    }
    /// <summary>
    /// Qualifier
    /// </summary>
    public class ANSYSQualifier
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}


