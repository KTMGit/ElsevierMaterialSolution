using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class FEMAP
    {
        private const string VERSION = "FEMAP Version 10.3 Material Library";
        private const string ENGINEERING = " Engineering Materials Library for FEMAP v10.3 in Metric Units";
        private const string UPDATEDENSITY = " Revised 5-February-2013 to Update Density of Stainless Steels";
        private const string CORRECTSPECIFIC = " Revised 5-February-2013 to Correct Specific Heat Units";
        private const string FOLLOWINGUNITS = " Units are as follows:";
        private const string E = " E - Pa (N/m^2)";
        private const string G = " G - Pa";
        private const string DIMENSIONLESS = " nu - dimensionless";
        private const string EXPANSIONCOEFFICIENT = " a - Alpha - Expansion Coefficient - m/m/degC";
        private const string CONDUCTUIVITI = " k - Conductivity - Watt/m-degK";
        private const string SPECIFICHEAT = " Cp - Specific Heat - J/Kg-degK";
        private const string STRESSLIMITS = " Stress Limits - Pa";
        private const string MASSDENSISTY = " Mass Density - Kg/m^3";
        private const string REFERENCETEMPERATURE = " Reference Temperature - degK";
        private const string DOLAR = "$";
        private const string COMMA = ",";
        private const string POINT = ".";
        private const string ZERRO = "0.";
        private const string BR1 = "37544204.";
        private const string BR2 = "15768142.";
        private const string BR3 = "10898031.";
        private const string BR4 = "9111969.1";
        private const string BR5 = "30898031.";
        private const string ZERRODECTED = "0,0,0,0,0,0,0,0,0,0,";
        private const string ZERRODECTEDFORMATED = "0.,0.,0.,0.,0.,0.,0.,0.,0.,0.,";
        private const string ZERROOCTETORMATED = "0.,0.,0.,0.,0.,0.,0.,0.,";
        private const string ZERRODEKTERFORMATET = "0.,0.,";
        private const string COM = "$COM     0   11     ";
        private const string CONSTMINUS1 = "   -1";
        private const string CONST601 = "   601";
        private const string CONST601104 = "0,-601,104,0,0,1,0,";
        private const string CONST10 = "10,";
        private const string CONST25 = "25,";
        private const string ZERROQUINTET = "0,0,0,0,0,";
        private const string CONST200 = "200,";
        private const string CONST50 = "50,";
        private const string CONST70 = "70,";
        /// <summary>
        /// Fills the femap.
        /// </summary>
        /// <returns></returns>
        public static string FillFEMAP(string standard, string name, IList<ExportPropertyGeneral> properties)
        {           
          
            string exportString = VERSION + System.Environment.NewLine;
            exportString = exportString + DOLAR + ENGINEERING + System.Environment.NewLine;
            exportString = exportString + DOLAR + UPDATEDENSITY + System.Environment.NewLine;
            exportString = exportString + DOLAR + CORRECTSPECIFIC + System.Environment.NewLine;
            exportString = exportString + DOLAR + FOLLOWINGUNITS + System.Environment.NewLine;
            exportString = exportString + DOLAR + E + System.Environment.NewLine;
            exportString = exportString + DOLAR + G + System.Environment.NewLine;
            exportString = exportString + DOLAR + DIMENSIONLESS + System.Environment.NewLine;
            exportString = exportString + DOLAR + EXPANSIONCOEFFICIENT + System.Environment.NewLine;
            exportString = exportString + DOLAR + CONDUCTUIVITI + System.Environment.NewLine;
            exportString = exportString + DOLAR + SPECIFICHEAT + System.Environment.NewLine;
            exportString = exportString + DOLAR + STRESSLIMITS + System.Environment.NewLine;
            exportString = exportString + DOLAR + MASSDENSISTY + System.Environment.NewLine;
            exportString = exportString + DOLAR + REFERENCETEMPERATURE + System.Environment.NewLine;
            exportString = exportString + COM + name + System.Environment.NewLine;
            exportString = exportString + CONSTMINUS1 + System.Environment.NewLine;
            exportString = exportString + CONST601 + System.Environment.NewLine;
            exportString = exportString + CONST601104 + System.Environment.NewLine;
            exportString = exportString + name + System.Environment.NewLine;
            exportString = exportString + CONST10 + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + CONST25 + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERROQUINTET + System.Environment.NewLine;
            exportString = exportString + CONST200 + System.Environment.NewLine;
            ExportPropertyGeneral young = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity     select u).FirstOrDefault();
            ExportPropertyGeneral poison = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient   select u).FirstOrDefault();
            ExportPropertyGeneral expansion = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion   select u).FirstOrDefault();
            ExportPropertyGeneral conductivity = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalThermalConductivity select u).FirstOrDefault();
            ExportPropertyGeneral spoecificHeat = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalSpecificThermalCapacity   select u).FirstOrDefault();
            ExportPropertyGeneral density = (from u in properties where u.Type == TMPropertyTypeEnum.PhysicalDensity   select u).FirstOrDefault();
            exportString = exportString + FormatPropertyValue(young) + COMMA + FormatPropertyValue(young) + COMMA + FormatPropertyValue(young) + COMMA + ZERRO + COMMA + ZERRO + COMMA + ZERRO + COMMA + FormatPropertyValue(poison) + COMMA + FormatPropertyValue(poison) + COMMA + FormatPropertyValue(poison) + COMMA + BR1 + COMMA + System.Environment.NewLine;
            exportString = exportString + BR2 + COMMA + BR2 + COMMA + ZERRO + COMMA + ZERRO + COMMA + ZERRO + COMMA + BR1 + COMMA + BR2 + COMMA + ZERRO + COMMA + ZERRO + COMMA + ZERRO + COMMA + System.Environment.NewLine;
            exportString = exportString +  BR1 + COMMA + ZERRO + COMMA + ZERRO + COMMA + ZERRO + COMMA + BR3 + COMMA + ZERRO + COMMA + ZERRO + COMMA + BR3 + COMMA + ZERRO + COMMA + BR3 + COMMA + System.Environment.NewLine;
            exportString = exportString + BR5 + COMMA + BR4 + COMMA + ZERRO + COMMA + BR5 + COMMA + ZERRO + COMMA + BR3 + COMMA + FormatPropertyValue(expansion) + COMMA + ZERRO + COMMA + ZERRO + COMMA + FormatPropertyValue(expansion) + COMMA + System.Environment.NewLine;
            exportString = exportString + ZERRO + COMMA + FormatPropertyValue(expansion) + COMMA + FormatPropertyValue(conductivity) + COMMA + ZERRO + COMMA + ZERRO + COMMA + FormatPropertyValue(conductivity) + COMMA + ZERRO + COMMA + FormatPropertyValue(conductivity) + COMMA + FormatPropertyValue(spoecificHeat) + COMMA + FormatPropertyValue(density) + COMMA + System.Environment.NewLine;
             string temperature = "";          
             if ( young!= null && young.Temperature > 0)
             {
                 temperature = (young.Temperature + 273.15).ToString();
             }
             else if (poison != null && poison.Temperature > 0)
             {
                 temperature = (poison.Temperature + 273.15).ToString();
             }
             else if (expansion != null && expansion.Temperature > 0)
             {
                 temperature = (expansion.Temperature + 273.15).ToString();
             }
             else if (conductivity != null && conductivity.Temperature > 0)
             {
                 temperature = (conductivity.Temperature + 273.15).ToString();
             }
             else if (spoecificHeat != null && spoecificHeat.Temperature > 0)
             {
                 temperature = (spoecificHeat.Temperature + 273.15).ToString();
             }
             else if (density != null && density.Temperature > 0)
             {
                 temperature = (density.Temperature + 273.15).ToString();
             }
             exportString = exportString + ZERRO + COMMA + temperature + COMMA + ZERROOCTETORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTEDFORMATED + System.Environment.NewLine;
            exportString = exportString + CONST50 + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + CONST70 + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + ZERRODECTED + System.Environment.NewLine;
            exportString = exportString + CONSTMINUS1 + System.Environment.NewLine; 
            return exportString;
        }
        /// <summary>
        /// Formats the property value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private static string FormatPropertyValue(ExportPropertyGeneral property)
        {
            if (property !=null)
            {
                double propDouble = double.Parse(property.Value);
                bool isInt = propDouble == (int)propDouble;
                switch (property.Type)
                {
                    case TMPropertyTypeEnum.None:
                        break;
                    case TMPropertyTypeEnum.PhysicalModulusOfElasticity:
                        return (double.Parse(property.Value) * 1000000000).ToString() + POINT;
                    case TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                        if (isInt)
                        {
                            return property.Value + POINT;
                        }
                        else
                        {
                            return property.Value;
                        }
                    case TMPropertyTypeEnum.PhysicalDensity:
                        return (double.Parse(property.Value) * 1000).ToString() + POINT;
                    case TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:
                        return (double.Parse(property.Value) / 1000000).ToString("F8", System.Globalization.CultureInfo.InvariantCulture);
                    case TMPropertyTypeEnum.PhysicalThermalConductivity:
                        if (isInt)
                        {
                            return property.Value + POINT;
                        }
                        else
                        {
                            return property.Value;
                        }

                    case TMPropertyTypeEnum.PhysicalSpecificThermalCapacity:

                        if (isInt)
                        {
                            return property.Value + POINT;
                        }
                        else
                        {
                            return property.Value;
                        }

                    default:
                        return "0.";
                }
            }
           
          return "0.";
        }
    }    
}