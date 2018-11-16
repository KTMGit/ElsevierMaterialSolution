using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Exporter.Models;

namespace ElsevierMaterials.Exporter.Formats
{
    public class LsDyna
    {
        /// <summary>
        /// mat_1.key constants
        /// </summary>
        private const int DEKTET = 10;
        private const string EMPTYOCTET = "          ";
        private const string MATELASTICTITLE = "*MAT_ELASTIC";
        private const string MAT1 = "mat1";
        private const string DOLAR = "$";
        private const string ZERRO = "       0.0";
        /// <summary>
        /// mat_24.key  constants
        /// </summary>
        private const string KEYWORD = "*KEYWORD";
        private const string DOLARLESS = "$>";
        private const string PLASTICTITLE = "*MAT_PIECEWISE_LINEAR_PLASTICITY_TITLE";
        private const string MAT24 = "mat24(static)";
        private const string DOLARTARABA = "$#";
        private const string MID = "mid";
        private const string RO = "ro";
        private const string E = "e";
        private const string PR = "pr";
        private const string SIGY = "sigy";
        private const string ETAN = "etan";
        private const string FAIL = "fail";
        private const string TDEL = "tdel";
        private const string ONEUNFORMATED = "1";
        private const string CONST185 = "185";
        private const string C = "c";
        private const string P = "p";
        private const string LCSS = "lcss";
        private const string LCSR = "lcsr";
        private const string VP = "vp";
        private const string CONST11 = "11";
        private const string ZERROUNFORMATED = "0";
        private const string EPS = "eps";
        private const string ES = "es";
        private const string DEFINECURVE = "*DEFINE_CURVE_TITLE";
        private const string SS1 = "ss1";
        private const string CROSSREFERENCE = "$: Cross-reference summary for Load-curve 11";
        private const string LINE = "$: -----------------------------------------------";
        private const string STRUCTURALMATERIAL = "$: Structural material 1 : Eff stress vs Eff plastic strain";
        private const string EFFECTIVEPLASTICSTRAIN = "$: X axis : Effective plastic strain   (Units: Strain)";
        private const string EFFECTIVESTRESS = "$: Y axis : Effective stress           (Units: Stress)";
        private const string TABLELOADCURVE = "$: Table 100 : Table load curve";
        private const string XAXIS = "$: X axis : X-axis                     (Units: Scalar, no units)";
        private const string YAXIS = "$: Y axis : Y_axis                     (Units: Scalar, no units)";
        private const string TRANSIENTANALISIS = "$: Usage: Transient analysis";
        private const string LCID = "lcid";
        private const string SIDR = "sidr";
        private const string SFA = "sfa";
        private const string SFO = "sfo";
        private const string OFFA = "offa";
        private const string OFFO = "offo";
        private const string DATTYP = "dattyp";
        private const string ONE = "1.0";
        private const string A1 = "a1";
        private const string O1 = "o1";

        /// <summary>
        /// Fills the mat1.
        /// </summary>
        /// <param name="exportConatiner">The export conatiner.</param>
        /// <returns></returns>
        public static string FillMat1(int vkKey, IList<ExportPropertyGeneral> properties)
        {
            string exportString = DOLAR + System.Environment.NewLine;
            exportString = exportString + MATELASTICTITLE + System.Environment.NewLine;
            exportString = exportString + MAT1 + System.Environment.NewLine;
            exportString = exportString + FormatDektetText(vkKey.ToString());
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalDensity, properties);
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalModulusOfElasticity, properties);
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalPoissonCoefficient, properties);
            exportString = exportString + FormatDektetText(ZERRO);
            exportString = exportString + FormatDektetText(ZERRO);
            exportString = exportString + FormatDektetText(ZERRO) + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            return exportString;
        }

        /// <summary>
        /// Fills the mat24.
        /// </summary>
        /// <param name="exportConatiner">The export conatiner.</param>
        /// <returns></returns>
        public static string FillMat24(IList<ExportPropertyGeneral> properties)
        {
            string exportString = KEYWORD + System.Environment.NewLine;
            exportString = exportString + DOLARLESS + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + PLASTICTITLE + System.Environment.NewLine;
            exportString = exportString + MAT24 + System.Environment.NewLine;
            exportString = exportString + DOLARTARABA;
            exportString = exportString + FormatOktetText(MID);
            exportString = exportString + FormatDektetText(RO);
            exportString = exportString + FormatDektetText(E);
            exportString = exportString + FormatDektetText(PR);
            exportString = exportString + FormatDektetText(SIGY);
            exportString = exportString + FormatDektetText(ETAN);
            exportString = exportString + FormatDektetText(FAIL);
            exportString = exportString + FormatDektetText(TDEL) + System.Environment.NewLine;
            exportString = exportString + FormatDektetText(ONEUNFORMATED);
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalDensity, properties);
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalModulusOfElasticity, properties);
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PhysicalPoissonCoefficient, properties);
            IList<Value> values = (from u in properties where u.Type == PropertyTypeEnum.PlasticStrainStress select u.Values).FirstOrDefault();
            double firsCooordinateY = 0;
            if (values != null && values.Count() > 0)
            {
                firsCooordinateY = values[0].Y;
            }
            if (firsCooordinateY > 0)
            {
                exportString = exportString + FormatDektetText(CONST185);
            }
            else
            {
                exportString = exportString + ZERRO;
            }
            exportString = exportString + FormatDektetText(ZERRO);
            exportString = exportString + FormatDektetText(ZERRO);
            exportString = exportString + FormatDektetText(ZERRO) + System.Environment.NewLine;
            exportString = exportString + DOLARTARABA;
            exportString = exportString + FormatOktetText(C);
            exportString = exportString + FormatDektetText(P);
            exportString = exportString + FormatDektetText(LCSS);
            exportString = exportString + FormatDektetText(LCSR);
            exportString = exportString + FormatDektetText(VP) + System.Environment.NewLine;
            exportString = exportString + FormatOktetText(ZERRO);
            exportString = exportString + FormatDektetText(ZERRO);
            exportString = exportString + FormatDektetText(CONST11);
            exportString = exportString + FormatDektetText(ZERROUNFORMATED);
            exportString = exportString + FormatDektetText(ZERRO) + System.Environment.NewLine;
            exportString = exportString + DOLARTARABA;
            for (int i = 1; i < 9; i++)
            {
                if (i == 1)
                {
                    exportString = exportString + FormatOktetText(EPS + i);
                }
                else
                {
                    exportString = exportString + FormatDektetText(EPS + i);
                }
            }
            exportString = exportString + System.Environment.NewLine;
            for (int i = 1; i < 9; i++)
            {
                exportString = exportString + FormatDektetText(ZERRO);
            }
            exportString = exportString + System.Environment.NewLine;

            exportString = exportString + DOLARTARABA;
            for (int i = 1; i < 9; i++)
            {
                if (i == 1)
                {
                    exportString = exportString + FormatOktetText(ES + i);
                }
                else
                {
                    exportString = exportString + FormatDektetText(ES + i);
                }
            }
            exportString = exportString + System.Environment.NewLine;
            for (int i = 1; i < 9; i++)
            {
                exportString = exportString + FormatDektetText(ZERRO);
            }
            exportString = exportString + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DEFINECURVE + System.Environment.NewLine;
            exportString = exportString + SS1 + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + CROSSREFERENCE + System.Environment.NewLine;
            exportString = exportString + LINE + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + STRUCTURALMATERIAL + System.Environment.NewLine;
            exportString = exportString + EFFECTIVEPLASTICSTRAIN + System.Environment.NewLine;
            exportString = exportString + EFFECTIVESTRESS + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + TABLELOADCURVE + System.Environment.NewLine;
            exportString = exportString + XAXIS + System.Environment.NewLine;
            exportString = exportString + YAXIS + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + TRANSIENTANALISIS + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DOLAR + System.Environment.NewLine;
            exportString = exportString + DOLARTARABA;
            exportString = exportString + FormatOktetText(LCID);
            exportString = exportString + FormatDektetText(SIDR);
            exportString = exportString + FormatDektetText(SFA);
            exportString = exportString + FormatDektetText(SFO);
            exportString = exportString + FormatDektetText(OFFA);
            exportString = exportString + FormatDektetText(OFFO);
            exportString = exportString + FormatDektetText(DATTYP) + System.Environment.NewLine;
            exportString = exportString + FormatDektetText(CONST11);
            exportString = exportString + FormatDektetText(ZERROUNFORMATED);
            exportString = exportString + FormatDektetText(ONE);
            exportString = exportString + FormatDektetText(ONE);
            exportString = exportString + ZERRO;
            exportString = exportString + ZERRO;
            exportString = exportString + FormatDektetText(ZERROUNFORMATED) + System.Environment.NewLine;
            exportString = exportString + DOLARTARABA;
            exportString = exportString + FormatOktetText("");
            exportString = exportString + FormatDektetText(A1);
            exportString = exportString + FormatDektetText("");
            exportString = exportString + FormatDektetText(O1) + System.Environment.NewLine;
            exportString = exportString + FormatDektetProperty(PropertyTypeEnum.PlasticStrainStress, properties);
            exportString = exportString + DOLAR;
            return exportString;
        }

        /// <summary>
        /// Formats the dektet property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="exportConatiner">The export conatiner.</param>
        /// <returns></returns>
        private static string FormatDektetProperty(PropertyTypeEnum type,  IList<ExportPropertyGeneral>  properties)
        {
            string value = "";
            var property = (from u in properties where u.Type == type select u).FirstOrDefault();

            if (property == null)
            {
                value = FormatOktetText(ZERRO);
                return value;
            }
            if (type == PropertyTypeEnum.PlasticStrainStress)
            {
                foreach (var item in property.Values)
                {
                    value = value + FormatDoubleDektetText(item.X.ToString()) + FormatDoubleDektetText(item.Y.ToString()) + FormatDektetText("") + FormatDektetText("") + FormatDektetText("") + System.Environment.NewLine;
                }
                return value;
            }
            else
            {
                if (property.Value != null)
                {
                    switch (type)
                    {
                        case PropertyTypeEnum.None:
                            break;
                        case PropertyTypeEnum.PhysicalModulusOfElasticity:
                            value = (double.Parse(property.Value) * 1000).ToString("f1", System.Globalization.CultureInfo.InvariantCulture);
                            break;
                        case PropertyTypeEnum.PhysicalPoissonCoefficient:
                            value = property.Value;
                            break;
                        case PropertyTypeEnum.PhysicalDensity:                        
                         value =  String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.###E-0}", double.Parse(property.Value) / 1000000000);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    value = ZERRO;
                }
                return FormatDektetText(value);
            }
        }
        /// <summary>
        /// Formats the dektet text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string FormatDektetText(string text)
        {
            string formatedString = "";
            for (int i = 0; i < 10 - text.Length; i++)
            {
                formatedString = formatedString + " ";
            }
            return formatedString + text;
        }
        /// <summary>
        /// Formats the oktet text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string FormatOktetText(string text)
        {
            string formatedString = "";
            for (int i = 0; i < 8 - text.Length; i++)
            {
                formatedString = formatedString + " ";
            }
            return formatedString + text;
        }
        /// <summary>
        /// Formats the double dektet text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string FormatDoubleDektetText(string text)
        {
            string formatedString = "";
            for (int i = 0; i < 20 - text.Length; i++)
            {
                formatedString = formatedString + " ";
            }
            return formatedString + text;
        }
    }

}