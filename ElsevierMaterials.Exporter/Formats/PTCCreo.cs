using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class PTCCreo
    {


        private const string DENSITYOPEN = "{"  + "\n" +
                                             "  Name = PTC_MASS_DENSITY"  + "\n" +
                                             "  Type = Real"  + "\n" +
                                             "  Default = ";
        private const string YOUNGOPEN = "{" + "\n" +
                                        "  Name = PTC_YOUNG_MODULUS" + "\n" +
                                        "  Type = Real" + "\n" +
                                        "  Default = ";
        private const string POISONOPEN = "{" + "\n" +
                                   "  Name = PTC_POISSON_RATIO" + "\n" +
                                   "  Type = Real" + "\n" +
                                   "  Default = ";
        private const string THERMALEXPANSIONOPEN = "{" + "\n" +
                            "  Name = PTC_THERMAL_EXPANSION_COEF" + "\n" +
                            "  Type = Real" + "\n" +
                            "  Default = ";

        private const string REFTEMPERATURE = "{" + "\n" +
                                                  "  Name = THERM_EXPANSION_REF_TEMPERATURE" + "\n" +
                                                  "  Type = Real" + "\n" +
                                                  "  Default = 2.000000e+01" + "\n" +
                                                  "  Access = Full" + "\n" +
                                                "},";

        private const string THERMALCONDUCTIVITYOPEN = "{" + "\n" +
                      "  Name = PTC_THERMAL_CONDUCTIVITY" + "\n" +
                      "  Type = Real" + "\n" +
                      "  Default = ";
        private const string SPECIFICHEATOPEN = "{" + "\n" +
              "  Name = PTC_SPECIFIC_HEAT" + "\n" +
              "  Type = Real" + "\n" +
              "  Default = ";

        private const string CLOSE = "  Access = Full" + "\n" +
                                   "}";



        private const string ND_RelParSet_K01 = "ND_RelParSet_K01 = {" + "\n"  + "\n" + "Name = ";

        private const string PARAMETERS = "PARAMETERS =" + "\n" +
"{" + "\n" +
  "  Name = PTC_MATERIAL_DESCRIPTION" + "\n" +
  "  Type = String" + "\n" +
  "  Default = ''" + "\n" +
  "  Access = Full" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = TEMPERATURE" + "\n" +
  "  Type = Real" + "\n" +
  "  Default = 0.000000e+00 C" + "\n" +
  "  Access = Full" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = PTC_BEND_TABLE" + "\n" +
  "  Type = String" + "\n" +
  "  Default = ''" + "\n" +
  "  Access = Full" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = PTC_MATERIAL_TYPE" + "\n" +
  "  Type = Integer" + "\n" +
  "  Default = 9" + "\n" +
  "  Access = Locked" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = PTC_FAILURE_CRITERION_TYPE" + "\n" +
  "  Type = String" + "\n" +
  "  Default = 'NONE'" + "\n" +
  "  Access = Locked" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = PTC_FATIGUE_TYPE" + "\n" +
  "  Type = String" + "\n" +
  "  Default = 'NONE'" + "\n" +
  "  Access = Locked" + "\n" +
"}," + "\n" +
"{" + "\n" +
  "  Name = PTC_MATERIAL_SUB_TYPE" + "\n" +
  "  Type = String" + "\n" +
  "  Default = 'LINEAR'" + "\n" +
  "  Access = Locked" + "\n" + "}," + "\n" ;

        private const string EMPTYSPACE = " ";
        private const string MPA = "MPa";
        private const string DENSITYUNIT = "kg/dm^3";
        private const string MEANCOEFFUNIT = "/C";
        private const string THERMALCONDUCTIVITYUNIT = "W/(m C)";
        private const string SPECIFICHEATUNIT = "J/(kg C)";


        private const string OPENBRACKET = "PARAMETERS";
        public static string FillPTCCreo(string standard, string name, IList<Models.ExportPropertyGeneral> properties)
        {
           
            string fileStr = "";
            fileStr = ND_RelParSet_K01 + name.Replace(".", "_").Replace(" ", "_") + "_" + standard.Replace(" ", "_") + Environment.NewLine + Environment.NewLine + PARAMETERS;



            foreach (var item in properties)
            {
             
                    string constStr="";
                    string value = "";
                    switch (item.Type)
                    {
                       
                        case PropertyTypeEnum.PhysicalModulusOfElasticity:
                            constStr = YOUNGOPEN;
                            value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value) * 1000).Replace("e+00", "e+0").Replace("e-00", "e-0")) + EMPTYSPACE + MPA;                        
                            break;
                        case PropertyTypeEnum.PhysicalPoissonCoefficient:
                            constStr = POISONOPEN;
                            value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value)).Replace("e+00", "e+0").Replace("e-00", "e-0"));
                            break;
                        case PropertyTypeEnum.PhysicalDensity:
                             constStr = DENSITYOPEN;
                             value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value)).Replace("e+00", "e+0").Replace("e-00", "e-0")) + EMPTYSPACE + DENSITYUNIT;
                         
                            break;
                        case PropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:                         
                            constStr = THERMALEXPANSIONOPEN;
                            value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value) / 1000000).Replace("e+00", "e+0").Replace("e-00", "e-0")) + EMPTYSPACE + MEANCOEFFUNIT;
                              bool last = false;
                              if (properties.IndexOf(item) == properties.Count - 1)
                                {
                                    last = true;
                                    fileStr = fileStr + createPropertyFormated(value, constStr, last);
                                    fileStr = fileStr + REFTEMPERATURE + "\n}";
                                    return fileStr;
                                }
                                else
                                {
                                    fileStr = fileStr + createPropertyFormated(value, constStr, false);
                                    fileStr = fileStr + REFTEMPERATURE + "\n";
                                    continue;
                                }
                      
                       
                        case PropertyTypeEnum.PhysicalThermalConductivity:
                            constStr = THERMALCONDUCTIVITYOPEN;
                            value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value)).Replace("e+00", "e+0").Replace("e-00", "e-0")) + EMPTYSPACE + THERMALCONDUCTIVITYUNIT;
                            break;
                        case PropertyTypeEnum.PhysicalSpecificThermalCapacity:
                            constStr = SPECIFICHEATOPEN;
                            value = (String.Format(CultureInfo.InvariantCulture, "{0:e}", Double.Parse(item.Value)).Replace("e+00", "e+0").Replace("e-00", "e-0")) + EMPTYSPACE + SPECIFICHEATUNIT;

                            break;     
                        default:
                            break;
                    }
                    bool last1 = false;
                  
                    if (properties.IndexOf(item) == properties.Count() - 1)
                    {
                        last1 = true;                        
                    }
                    fileStr = fileStr + createPropertyFormated(value, constStr, last1);
           
            }
            return fileStr + "\n}";
        }

        private static string createPropertyFormated(string valuestr, string constStr, bool last)
        {
            string value = "";
            value = constStr + valuestr + Environment.NewLine + CLOSE + (last== true ? "" : ",") + Environment.NewLine;

            return value;
        }


       
    }
}