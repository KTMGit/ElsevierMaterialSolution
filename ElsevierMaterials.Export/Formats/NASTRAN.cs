using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class NASTRAN
    {

        private const int Octet = 8;
        private const string EmptyOctet = "        ";
        private const string PLUS = "+       ";
        private const string STATIC = "STATIC  ";
        private const string ZERO = "0.0     ";
        private const string METRIC = "     MPA";
        private const string BIGINBULK = "BEGIN BULK";
        private const string HMNAME = "$";
        private const string MAT = "MAT     ";
        private const string ONE = "1       ";
        private const string ALL = "       1";
        private const string MAT1 = "MAT1    ";
        private const string MAT4 = "MAT4    ";
        private const string MATFAT = "MATFAT  ";
        private const string SN = "SN      ";
        private const string EN = "EN      ";
        private const string MATHF = "MATHF   ";
        private const string ENDDATA = "ENDDATA";
        private static string MaterialTypePart = "";
        private const string EMPTYOCTET = "        ";

        public static string FillNASTRAN(int vkKey, string standard, string name,  IList<ExportPropertyGeneral> properties)
        {

            MaterialTypePart = name + " " + standard;
            string exportString = BIGINBULK + System.Environment.NewLine;
            exportString = exportString + HMNAME + MaterialTypePart + System.Environment.NewLine;          
            exportString = exportString + MAT1 + vkKey.ToString().PadRight(8);
            exportString = exportString + FormatOctet(properties, TMPropertyTypeEnum.PhysicalModulusOfElasticity, TypeOfOctet.Left) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties, TMPropertyTypeEnum.PhysicalPoissonCoefficient, TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties, TMPropertyTypeEnum.PhysicalDensity, TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties, TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion, TypeOfOctet.Left);
            exportString = exportString + ZERO  + System.Environment.NewLine;
            exportString = exportString + ENDDATA;
            return exportString;
        }

             /// <summary>
        /// Formats the octet.
        /// </summary>
        /// <param name="export">The export.</param>
        /// <param name="exportType">Type of the export.</param>
        /// <returns></returns>
        private static string FormatOctet( IList<ExportPropertyGeneral> properties,  TMPropertyTypeEnum exportType, TypeOfOctet octetType)
        {
                  double Num;
                  string propertyValue = null;
                  ExportPropertyGeneral property = (from u in properties where u.Type == exportType select u).FirstOrDefault();
                  if (property != null)
                  {                     
                      propertyValue = property.Value;
                      return (propertyValue != null && double.TryParse(propertyValue, out Num) ? FillOctet(double.Parse(propertyValue), octetType, exportType) : EMPTYOCTET);
                  }
                  else
                  {
                      return ZERO;
                  }
        }        
        /// <summary>
        /// Fills the octet with values according to if value is double, int, or value which lenght is bigger then 8 characters. 
        /// Also we must consider value align in octet.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String FillOctet(double value, TypeOfOctet type, TMPropertyTypeEnum exportType) 
        {
            string formatedValue = "";  
            switch (exportType)
            {
                case TMPropertyTypeEnum.None:
                    break;
                case TMPropertyTypeEnum.PhysicalModulusOfElasticity:  
                    formatedValue = (String.Format(CultureInfo.InvariantCulture, "{0:0.###E+0}", value * 1000000)).Replace("E", "");
                    formatedValue = formatedValue.PadRight(8);
                    break;
                case TMPropertyTypeEnum.PhysicalPoissonCoefficient:
                    formatedValue = value.ToString().PadRight(8);
                    break;
                case TMPropertyTypeEnum.PhysicalDensity:
                    formatedValue = value.ToString().PadRight(8);
                    break;
                case TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion:           
                    formatedValue = (String.Format(CultureInfo.InvariantCulture,"{0:0.###E+0}", value/1000000)).Replace("E", "");
                    formatedValue = formatedValue.PadRight(8);
                    break;
                default:
                    break;
            }
            return formatedValue;
        
        }   
    
    /// <summary>
    /// Position of string
    /// </summary>
    public enum TypeOfOctet 
    {
    None = 0,
    Left = 1,
    Right = 2
    }
    }

   
}