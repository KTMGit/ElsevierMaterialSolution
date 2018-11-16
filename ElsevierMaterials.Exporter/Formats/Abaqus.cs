using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ElsevierMaterials.Exporter.Models;


namespace ElsevierMaterials.Exporter.Formats
{
    public class Abaqus
    {
        private const string REVISION = "** Revision - 0";
        private const string UNITSYSTEM = "***** UNIT SYSTEM**********************************";
        private const string STRESS = "**** STRESS                            =MPa";
        private const string PRESSURE = "**** PRESSURE                          =MPa";
        private const string ELASTIC_MODULUS = "**** ELASTIC_MODULUS                   =MPa";
        private const string TEMP = "**** TEMP                              = Deg C";
        private const string Density = "**** Density                           = Tons/mm^3";
        private const string Force = "**** Force                             = Newton";
        private const string Distance = "**** Distance                          = mm";
        private const string SpecificHeat = "**** Specific heat      	       = N-mm/tons-K";
        private const string Conductivity = "**** Conductivity                      = N/sec-K";
        private const string Convection = "**** Convection                        = N/mm-sec-K";
        private const string Coefficient = "**** Coefficient of thermal expansion  =/K";
        private const string ENDCOMMNET = "*****************************************************";
        private const string TWOCOMMENTS = "**";
        private const string MATERIALNAME = "*Material, Name=";

        private const string ELASTIC = "*Elastic";
        private const string YOUNGPOISSON = "**YOUNG'S MODULUS, POISSON'S RATIO";

        private const string CONDUCTIVITY = "*Conductivity";
        private const string THERMALCONDUCTIVITY = "**THERMAL CONDUCTIVITY, TEMPERATURE";

        private const string EXPANSION = "*Expansion";
        private const string THERMALEXPANSION = "**THERMAL EXPANSION, TEMPERATURE";

        private const string SPECIFIC = "*Specific Heat";
        private const string SPECIFICHEAT = "**SPECIFIC HEAT, TEMPERATURE";

 
        private const string PLASTICITY = "*PLASTICITY";
        private const string TENSIONHARDENING = "*TENSION HARDENING";

        private const string PLASTIC = "*Plastic";
        private const string STRESSPLASTICSTRAIN = "**STRESS, PLASTIC STRAIN, TEMPERATURE";

        private const string COMPRESSIONHARDENING = "*COMPRESSION HARDENING";
        private const string STRESSPLASTICS = "*STRESS, PLASTIC STRAIN, TEMPERATURE";
        private const string DENSITY = "*DENSITY";
        private const string RIGHTZERO = ".0";
        private const string FORMATEDZERO = "0.0";
        private const string LEFTZERO = "0.";
        private const string ZERO = "0";
        /// <summary>
        /// Fills the abaqus.
        /// </summary>
        /// <returns></returns>
        public static string FillAbaqus(IList<ExportPropertyGeneral> properties, string name)
        {           
            //StringBuilder sb  = new s
            string exportString = REVISION + System.Environment.NewLine;
            exportString = exportString + UNITSYSTEM + System.Environment.NewLine;
            exportString = exportString + STRESS + System.Environment.NewLine;
            exportString = exportString + PRESSURE + System.Environment.NewLine;
            exportString = exportString + ELASTIC_MODULUS + System.Environment.NewLine;
            exportString = exportString + TEMP + System.Environment.NewLine;
            exportString = exportString + Density + System.Environment.NewLine;
            exportString = exportString + Force + System.Environment.NewLine;
            exportString = exportString + Distance + System.Environment.NewLine;
            exportString = exportString + SpecificHeat + System.Environment.NewLine;
            exportString = exportString + Conductivity + System.Environment.NewLine;
            exportString = exportString + Convection + System.Environment.NewLine;
            exportString = exportString + Coefficient + System.Environment.NewLine;
            exportString = exportString + ENDCOMMNET + System.Environment.NewLine;
            exportString = exportString + TWOCOMMENTS + System.Environment.NewLine;



            exportString = exportString + MATERIALNAME + name.Replace(" ", "_") + System.Environment.NewLine;
            exportString = exportString + ELASTIC + System.Environment.NewLine;

            exportString = exportString + YOUNGPOISSON + System.Environment.NewLine;
            exportString = exportString + FormatValue(properties, PropertyTypeEnum.PhysicalModulusOfElasticity) + ", " + FormatValue(properties, PropertyTypeEnum.PhysicalPoissonCoefficient) + System.Environment.NewLine;

            exportString = exportString + CONDUCTIVITY + System.Environment.NewLine;
            exportString = exportString + THERMALCONDUCTIVITY + System.Environment.NewLine;
            exportString = exportString + FormatThermalConductivity(properties);
            exportString = exportString + EXPANSION + System.Environment.NewLine;
            exportString = exportString + THERMALEXPANSION + System.Environment.NewLine;
            exportString = exportString + FormatThermalExpansion(properties);
            exportString = exportString + SPECIFIC + System.Environment.NewLine;
            exportString = exportString + SPECIFICHEAT + System.Environment.NewLine;
            exportString = exportString + FormatSpecificThermalCapacity(properties);
             

            ExportProperty property = (from u in properties where u.Type == PropertyTypeEnum.PlasticStrainStress select u).FirstOrDefault() as ExportProperty;
            if (property != null)
            {
                exportString = exportString + PLASTIC + System.Environment.NewLine;
                exportString = exportString + STRESSPLASTICSTRAIN + System.Environment.NewLine;    
                exportString = exportString + FormatValue(properties, PropertyTypeEnum.PlasticStrainStress);
            }
            
            exportString = exportString + DENSITY + System.Environment.NewLine;
            exportString = exportString + FormatValue(properties, PropertyTypeEnum.PhysicalDensity) + System.Environment.NewLine;
            return exportString;
        }
        /// <summary>
        /// Formats the thermal conductivity.
        /// </summary>
        /// <returns></returns>
        private static string FormatThermalConductivity(IList<ExportPropertyGeneral> adddedProperties)
        {

            List<ExportPropertyGeneral> properties = (from u in adddedProperties where u.Type == PropertyTypeEnum.PhysicalThermalConductivity select u).ToList();

            string value = LEFTZERO;
            string temperature = LEFTZERO;
            string text = value + ", \t" + temperature + System.Environment.NewLine;
            if (properties.Count > 0)
            {
                text = "";  
            }
            foreach (var item in properties)
            {                
                value = item.Value;
                temperature = item.Temperature.ToString();

                if (value != null && value != "")
                {
                   
                }
                else
                {
                    value = LEFTZERO;
                }
                if (temperature != null && temperature != "")
                {
                  
                }
                else
                {
                    temperature = LEFTZERO;
                }
                value = value + ", \t" + temperature;
                text = text + value + System.Environment.NewLine;
            }           
            return text;
        }
        /// <summary>
        /// Formats the thermal expansion.
        /// </summary>
        /// <returns></returns>
        private static string FormatThermalExpansion(IList<ExportPropertyGeneral> adddedProperties)
        {

            List<ExportPropertyGeneral> properties = (from u in adddedProperties where u.Type == PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion select u).ToList();
            string value = FORMATEDZERO;
            string temperature = FORMATEDZERO;
            string text = value + ", \t" + temperature + System.Environment.NewLine;
            if (properties.Count > 0)
            {
                text = "";
            }
            foreach (var item in properties)
            {
           
                 value = item.Value;
                 temperature = item.Temperature.ToString();

                if (value != null && value != "")
                {
                    value = string.Format("{0:f4}", double.Parse(value)) + "E-06";
                }
                else
                {
                    value = FORMATEDZERO;
                }
                if (temperature != null && temperature != "")
                {
             
                }
                else
                {
                    temperature = FORMATEDZERO;
                }           
                value = value + ", \t" + temperature;
                text = text + value + System.Environment.NewLine;
            }
            return text;
        }
        /// <summary>
        /// Formats the specific thermal capacity.
        /// </summary>
        /// <returns></returns>
        private static string FormatSpecificThermalCapacity(IList<ExportPropertyGeneral> adddedProperties)
        {

            List<ExportPropertyGeneral> properties = (from u in adddedProperties where u.Type == PropertyTypeEnum.PhysicalSpecificThermalCapacity select u).ToList();
            string value = ZERO;
            string temperature = LEFTZERO;
            string text = value + ", \t" + temperature + System.Environment.NewLine;
            if (properties.Count > 0)
            {
                text = "";
            }
            foreach (var item in properties)
            {
              
                 value = item.Value;
                 temperature = item.Temperature.ToString();
                if (value != null && value != "")
                {
                    value = string.Format("{0:f2}", double.Parse(value)) + "E+06";
                }
                else
                {
                    value = ZERO;
                }

                if (temperature != null && temperature != "")
                {
                  
                }
                else
                {
                    temperature = LEFTZERO;
                }            
                value = value + ", \t" + temperature;
                text = text + value + System.Environment.NewLine;
            }
            return text;
        }
        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="ExportType">Type of the export.</param>
        /// <param name="subtype">The subtype.</param>
        /// <returns></returns>
        private static string FormatValue(IList<ExportPropertyGeneral> properties, PropertyTypeEnum ExportType, StressStrainType subtype = StressStrainType.None)
        {

            ExportPropertyGeneral property = (from u in properties where u.Type == ExportType select u).FirstOrDefault();


            string value ="";
            string temperature = "";
            if (property != null)
            {
                 value = property.Value;
                 temperature = property.Temperature.ToString();
            }
          
             switch (ExportType)
             {
                 case PropertyTypeEnum.None:
                     break;
                 case PropertyTypeEnum.PhysicalModulusOfElasticity:
                       if (value != null && value != "")
                       {
                           value = (double.Parse(value) * 1000).ToString() + RIGHTZERO;
                       }
                       else {
                           value = FORMATEDZERO;
                       }
                     return value;
                 case PropertyTypeEnum.PhysicalPoissonCoefficient:
                     if (value == null || value == "")
                     {
                         value = FORMATEDZERO;
                     }
                     return value;
                 case PropertyTypeEnum.PhysicalDensity:
                     if (value == null || value == "")
                     {
                         value = FORMATEDZERO;
                     }
                     else
                     {
                         value = value + "e-09,";
                     }                      
                     return value;
                 case PropertyTypeEnum.PlasticStrainStress:
                     if (property!=null)
                     {
                         string table = "";
                         foreach (var item in property.Values)
                         {
                             table = table + item.Y + ", \t" + string.Format("{0:f4}", double.Parse(item.X)) + ", \t" + property.Temperature + System.Environment.NewLine;                           
                             
                         }
                         return table;
                     }
                     return "0, \t0.0000, \t0";             
                 default:
                     break;
             }
             return FORMATEDZERO;
        }

    }
}