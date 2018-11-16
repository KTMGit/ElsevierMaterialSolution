using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Exporter.Models;



namespace ElsevierMaterials.Exporter.Formats
{
    public  static class AltairHyperworks
    {
        private const int Octet = 8;
        private const string EmptyOctet = "        ";
        private const string PLUS = "+       ";
        private const string STATIC = "STATIC  ";
        private const string ZERO = "0.0     ";
        private const string METRIC = "     MPA";
        private const string BIGINBULK = "BEGIN BULK";
        private const string HMNAME = "$HMNAME ";
        private const string MAT = "MAT     ";
        private const string ONE = "       1";
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


        //MATHF
        private const string HYPERFORM = "@HyperForm";
        private const string DOLAR = "$";
        private const string HFSOLVER = "   Template:  HFSolver";
        private const string SCREENOUT = "SCREEN OUT";
        private const string SUBCASE = "subcase 1";
        private const string SPC = "spc = 0";
        private const string PARAMCOEFFC = "PARAM, COEFFC, 0.125";
        private const string COMMENT = "$------+-------+-------+-------+-------+-------+-------+-------+-------+";
        private const string COMMENT2 = "$------+-------+-------+-------+-------+-------+-------+-------+";
        private const string COMMENT3 = "$------+-------+-------+-------+-------+-------+-------+";
        private const string HFCURVES = "$HFNAME CURVES                                                       stress_strain_curve";
        private const string TABLES = "TABLES1,       1";

        private const string ENDCOMMA = ",ENDT";
        private const string HFNAMEMATS = "$HFNAME MATS  ";

        private const string ENDNUMMBERS = "             1.0     1.0     1.0       1";
        private const string ENDNUMMBERSWITHOUTSS = "             1.0     1.0     1.0        ";

        /// <summary>
        /// Mas the t1 export.
        /// </summary>
        /// <returns></returns>
        public static string MAT1Export(IList<ExportPropertyGeneral> properties, string name, string standard)
        {


            MaterialTypePart = "\"" + name + " " + standard + "\" \"" + AltairExportGroupType.MAT1.ToString() + "\"";         
            string exportString = BIGINBULK + System.Environment.NewLine;           
            exportString = exportString +  HMNAME + MAT + EMPTYOCTET + ONE + MaterialTypePart + System.Environment.NewLine;                 
            exportString = exportString + MAT1 + ONE;

            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity).FirstOrDefault(),TypeOfOctet.Left) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalDensity).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).FirstOrDefault(), TypeOfOctet.Left);     
            exportString = exportString + EMPTYOCTET + EMPTYOCTET + System.Environment.NewLine;
            exportString = exportString + ENDDATA;          
            return exportString;
        }
        /// <summary>
        /// Mas the t4 export.
        /// </summary>
        /// <returns></returns>
        public static string MAT4Export(IList<ExportPropertyGeneral> properties, string name, string standard)
        {

            MaterialTypePart = "\"" + name + " " + standard + "\" \"" + AltairExportGroupType.MAT4.ToString() + "\"";     
            string exportString = BIGINBULK + System.Environment.NewLine;
            exportString = exportString + HMNAME + MAT + EMPTYOCTET + ONE + MaterialTypePart + System.Environment.NewLine;
            exportString = exportString + MAT4 + ONE;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalThermalConductivity).FirstOrDefault(), TypeOfOctet.Right) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalDensity).FirstOrDefault(), TypeOfOctet.Right);          
            exportString = exportString + EMPTYOCTET + EMPTYOCTET + EMPTYOCTET + System.Environment.NewLine;
            exportString = exportString + ENDDATA;
            return exportString;
        }
        /// <summary>
        /// Matfatsns the export.
        /// </summary>
        /// <returns></returns>
        public static string MATFATSNExport(IList<ExportPropertyGeneral> properties, string name, string standard)
        {

            MaterialTypePart = "\"" + name + " " + standard + "\" \"" + AltairExportGroupType.MAT1.ToString() + "\"";
            string exportString = BIGINBULK + System.Environment.NewLine;
            exportString = exportString + HMNAME + MAT + EMPTYOCTET + ONE + MaterialTypePart + System.Environment.NewLine;
            exportString = exportString + MAT1 + ONE;

            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity).FirstOrDefault(), TypeOfOctet.Left) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalDensity).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).FirstOrDefault(), TypeOfOctet.Left);    
          
     
            exportString = exportString + EMPTYOCTET + EMPTYOCTET + System.Environment.NewLine;       
            exportString = exportString + MATFAT + ONE + METRIC + System.Environment.NewLine;  

            exportString = exportString + PLUS + STATIC + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.MechanicalYield).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.MechanicalTensile).FirstOrDefault(), TypeOfOctet.Left) + System.Environment.NewLine;
            exportString = exportString + PLUS + SN + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueA).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueB).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueNf_maxSNDiagram).FirstOrDefault(), TypeOfOctet.Left) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueLimit).FirstOrDefault(), TypeOfOctet.Left)  +System.Environment.NewLine;
        
            exportString = exportString + ENDDATA;
            return exportString;
        }
        /// <summary>
        /// Matfatens the export.
        /// </summary>
        /// <returns></returns>
        public static string MATFATENExport(IList<ExportPropertyGeneral> properties, string name, string standard)
        {



            MaterialTypePart = "\"" + name + " " + standard + "\" \"" + AltairExportGroupType.MAT1.ToString() + "\"";
            string exportString = BIGINBULK + System.Environment.NewLine;
            exportString = exportString + HMNAME + MAT + EMPTYOCTET + ONE + MaterialTypePart + System.Environment.NewLine;

            exportString = exportString + MAT1 + ONE;

            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity).FirstOrDefault(), TypeOfOctet.Left) + EMPTYOCTET;
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalDensity).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion).FirstOrDefault(), TypeOfOctet.Left);
  

            exportString = exportString + EMPTYOCTET + EMPTYOCTET + System.Environment.NewLine;

          
            exportString = exportString + MATFAT + ONE + METRIC + System.Environment.NewLine;

            exportString = exportString + PLUS + STATIC + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.MechanicalYield).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.MechanicalTensile).FirstOrDefault(), TypeOfOctet.Left) + System.Environment.NewLine;

            exportString = exportString + PLUS + EN + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueStrengthCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueStrengthExponent).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueDuctilityExponent).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueDuctilityCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthExponent).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient).FirstOrDefault(), TypeOfOctet.Left);
            exportString = exportString + FormatOctet(properties.Where(u => u.Type == TMPropertyTypeEnum.FatigueNf_maxENDiagram).FirstOrDefault(), TypeOfOctet.Left) + System.Environment.NewLine;
            
     
            exportString = exportString + PLUS + EMPTYOCTET + ZERO + ZERO + System.Environment.NewLine;
            exportString = exportString + ENDDATA;

            return exportString;
        }


        public static string MATHFExport(IList<ExportPropertyGeneral> properties, string name, string standard)
        {



            MaterialTypePart = name + " " + standard;

            System.Text.StringBuilder str = new System.Text.StringBuilder();
            str.Append(HYPERFORM);
            str.Append(System.Environment.NewLine);
            str.Append(DOLAR);
            str.Append(System.Environment.NewLine);
            str.Append(DOLAR);
            str.Append(HFSOLVER);
            str.Append(System.Environment.NewLine);
            str.Append(DOLAR);
            str.Append(System.Environment.NewLine);
            str.Append(SCREENOUT);
            str.Append(System.Environment.NewLine);
            str.Append(SUBCASE);
            str.Append(System.Environment.NewLine);
            str.Append(SPC);
            str.Append(System.Environment.NewLine);
            str.Append(BIGINBULK);
            str.Append(System.Environment.NewLine);




            ExportPropertyGeneral property = properties.Where(u => u.Type == TMPropertyTypeEnum.StressStrain).FirstOrDefault();

            if (property != null)
            {
                if (property.Values != null)
                {
                    str.Append(PARAMCOEFFC);
                    str.Append(System.Environment.NewLine);
                    str.Append(COMMENT);
                    str.Append(System.Environment.NewLine);
                    str.Append(HFCURVES);
                    str.Append(System.Environment.NewLine);
                    str.Append(TABLES);
                    str.Append(System.Environment.NewLine);

                    foreach (var item in property.Values)
                    {
                        str.Append("+,      ");

                        str.Append(FillEmptySpace(double.Parse(item.X), TypeOfOctet.Right, 8));
                        str.Append(",");
                        str.Append(FillEmptySpace(item.Y, TypeOfOctet.Right, 7));
                        str.Append(System.Environment.NewLine);
                    }

                    str.Append(ENDCOMMA);
                    str.Append(System.Environment.NewLine);
                }

            }


            str.Append(COMMENT2);
            str.Append(System.Environment.NewLine);
            str.Append(HFNAMEMATS);
            str.Append(MaterialTypePart.PadLeft(75));
            str.Append(System.Environment.NewLine);

            str.Append(MATHF);
            str.Append(1.ToString().PadLeft(8));       

            str.Append(FormatOctetMATHF(TMPropertyTypeEnum.PhysicalModulusOfElasticity, TypeOfOctet.Right, properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity).FirstOrDefault()));
            str.Append(FormatOctetMATHF(TMPropertyTypeEnum.PhysicalPoissonCoefficient, TypeOfOctet.Right, properties.Where(u => u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient).FirstOrDefault()));
            str.Append(FormatOctetMATHF(TMPropertyTypeEnum.MechanicalYield, TypeOfOctet.Right, properties.Where(u => u.Type == TMPropertyTypeEnum.MechanicalYield).FirstOrDefault()));
            str.Append(EMPTYOCTET);
            str.Append(EMPTYOCTET);
            str.Append(EMPTYOCTET);
            str.Append(System.Environment.NewLine);

            if (property == null)
            {
                str.Append(ENDNUMMBERSWITHOUTSS);
            }
            else
            {
                str.Append(ENDNUMMBERS);
            }

            str.Append(System.Environment.NewLine);
            str.Append(COMMENT3);
            str.Append(System.Environment.NewLine);
            str.Append(ENDDATA);
            return str.ToString();
          
        }


        private static string FormatOctetMATHF(TMPropertyTypeEnum exportPropertyType, TypeOfOctet octetType, ExportPropertyGeneral property)
        {
            double Num;
            string propertyValue = null;
            double value = 0;

                   

            if (property != null)
            {
                propertyValue = property.Value;
                value = double.Parse(propertyValue);
                if (exportPropertyType == TMPropertyTypeEnum.PhysicalModulusOfElasticity)
                {
                    value = value * 1000;
                }
            }
            string emptyOctet = EMPTYOCTET;
            if (exportPropertyType != TMPropertyTypeEnum.PhysicalPoissonCoefficient)
            {
                emptyOctet = FillEmptySpace(0, octetType, 8);
            }


            return (propertyValue != null && double.TryParse(propertyValue, out Num) ? FillEmptySpace(value, octetType, 8) : emptyOctet);

        }


        private static String FillEmptySpace(double value, TypeOfOctet type, int reservedSpace)
        {
            string stringValue = value.ToString();
            int stringLength = stringValue.Length;
            int freeSpaces = reservedSpace - stringValue.Length;
            if (stringLength > reservedSpace)
            {
                if (stringValue.Split('.').Count() > 1 && stringValue.Split('.')[1] != null)
                {
                    int beforeComma = stringValue.Split('.')[0].Count();
                    int afterComma = stringValue.Split('.')[1].Count();
                    string returnValue = Math.Round(value, reservedSpace - beforeComma - 1).ToString();
                    if (returnValue.Length == reservedSpace)
                    {
                        return returnValue;
                    }
                    freeSpaces = reservedSpace - returnValue.Length;
                    return AddEmptySpaces(returnValue, freeSpaces, type);

                }
                else
                {
                    stringValue = stringValue.Substring(0, 1) + ".0+" + (stringValue.Length - 1).ToString();
                    freeSpaces = reservedSpace - stringValue.Length;
                    return AddEmptySpaces(stringValue, freeSpaces, type);
                    // return stringValue + ".0";
                }
            }
            if (stringValue.IndexOf(".") > -1 && stringValue.Split('.')[1] != null)
            {
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }
            if (stringValue.Length > reservedSpace - 2)
            {
                stringValue = "1.0+" + (stringValue.Length - 1).ToString();
                freeSpaces = reservedSpace - stringValue.Length;
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }
            else
            {
                stringValue = stringValue + ".0";
                freeSpaces = freeSpaces - 2;
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }
        }

        /// <summary>
        /// Formats the octet.
        /// </summary>
        /// <param name="export">The export.</param>
        /// <param name="exportType">Type of the export.</param>
        /// <returns></returns>
        private static string FormatOctet(ExportPropertyGeneral property, TypeOfOctet octetType)
        {
                  double Num;
                  string propertyValue = null;
               
                  if (property != null)
                  {              
                      propertyValue = property.Value;
                  }                            
                  return (propertyValue != null && double.TryParse(propertyValue, out Num) ? FillOctet(double.Parse(propertyValue), octetType) : EMPTYOCTET);
           
        }        
        /// <summary>
        /// Fills the octet with values according to if value is double, int, or value which lenght is bigger then 8 characters. 
        /// Also we must consider value align in octet.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String FillOctet(double value, TypeOfOctet type) 
        {      
            string stringValue = value.ToString();
            int stringLength = stringValue.Length;
            int freeSpaces = Octet - stringValue.Length;
            if (stringLength > Octet)
            {
                if ( stringValue.Split('.').Count() > 1 && stringValue.Split('.')[1] != null)
                {
                    int beforeComma = stringValue.Split('.')[0].Count();
                    int afterComma = stringValue.Split('.')[1].Count();
                    string returnValue = Math.Round(value, Octet - beforeComma - 1).ToString();
                    if (returnValue.Length == 8)
                    {
                        return returnValue;
                    }
                        freeSpaces = Octet - returnValue.Length;
                        return AddEmptySpaces(returnValue, freeSpaces, type);                   
                  
                }
                else
                {
                    stringValue = stringValue.Substring(0, 1) + ".0+" + (stringValue.Length - 1).ToString();
                    freeSpaces = Octet - stringValue.Length;
                    return AddEmptySpaces(stringValue, freeSpaces, type);
                   // return stringValue + ".0";
                }             
            }           
            if (stringValue.IndexOf(".") > -1 && stringValue.Split('.')[1] != null)
            {
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }               
            if (stringValue.Length > Octet - 2)
            {                      
                stringValue = "1.0+" +(stringValue.Length - 1).ToString();
                freeSpaces = Octet - stringValue.Length;
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }
            else
            {                    
                stringValue = stringValue + ".0";
                freeSpaces = freeSpaces - 2;
                return AddEmptySpaces(stringValue, freeSpaces, type);
            }     
        }
        /// <summary>
        /// Adds the empty spaces according to type Of octet.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="freeSpaces">The free spaces.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static string AddEmptySpaces(string stringValue, int freeSpaces, TypeOfOctet type)
        {
            if (type == TypeOfOctet.Left)
            {
                for (int i = 0; i < freeSpaces; i++)
                {
                    stringValue = stringValue + " ";
                }
            }
            else
            {
                for (int i = 0; i < freeSpaces; i++)
                {
                    stringValue = " " + stringValue;
                }
            }          
            return stringValue;
        }


        public static bool HasAtLeastOnePropertyMAT1(IList<ExportPropertyGeneral> properties)
        {
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.PhysicalDensity || u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion select u).Any())
            {
                return true;
            }
            return false;
        }

        public static bool HasAtLeastOnePropertyMAT4(IList<ExportPropertyGeneral> properties)
        {
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalThermalConductivity || u.Type == TMPropertyTypeEnum.PhysicalDensity select u).Any())
            {
                return true;
            }
            return false;
        }

        public static bool HasAtLeastOnePropertyMATFAT_EN(IList<ExportPropertyGeneral> properties)
        {
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.PhysicalDensity || u.Type == TMPropertyTypeEnum.MechanicalYield || u.Type == TMPropertyTypeEnum.MechanicalTensile || u.Type == TMPropertyTypeEnum.FatigueStrengthCoefficient || u.Type == TMPropertyTypeEnum.FatigueStrengthExponent || u.Type == TMPropertyTypeEnum.FatigueDuctilityExponent || u.Type == TMPropertyTypeEnum.FatigueDuctilityCoefficient || u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthExponent || u.Type == TMPropertyTypeEnum.FatigueCyclicStrengthCoefficient || u.Type == TMPropertyTypeEnum.FatigueNf_maxENDiagram select u).Any())
            {
                return true;
            }
            return false;
        }

        public static bool HasAtLeastOnePropertyMATFAT_SN(IList<ExportPropertyGeneral> properties)
        {
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.PhysicalDensity || u.Type == TMPropertyTypeEnum.PhysicalMeanCoeffThermalExpansion || u.Type == TMPropertyTypeEnum.MechanicalYield || u.Type == TMPropertyTypeEnum.MechanicalTensile || u.Type == TMPropertyTypeEnum.FatigueA || u.Type == TMPropertyTypeEnum.FatigueB || u.Type == TMPropertyTypeEnum.FatigueNf_maxSNDiagram || u.Type == TMPropertyTypeEnum.FatigueLimit select u).Any())
            {
                return true;
            }
            return false;
        }

        public static bool HasAtLeastOnePropertyMATHF(IList<ExportPropertyGeneral> properties)
        {
            if ((from u in properties where u.Type == TMPropertyTypeEnum.PhysicalModulusOfElasticity || u.Type == TMPropertyTypeEnum.PhysicalPoissonCoefficient || u.Type == TMPropertyTypeEnum.MechanicalYield || u.Type == TMPropertyTypeEnum.StressStrain select u).Any())
            {
                return true;
            }
            return false;
        }


     
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