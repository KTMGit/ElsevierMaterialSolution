using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SoftArtisans.OfficeWriter.ExcelWriter;
using System.Text.RegularExpressions;

namespace ElsevierMaterials.Exporter.Formats
{
    public class KTMXLS
    {

        public static System.IO.MemoryStream FillKTMXLS(IList<Models.ExportPropertyGeneral> properties)
        {
 
            System.IO.MemoryStream memoryStreamKTMXls = new System.IO.MemoryStream();
            ExcelApplication excel = new ExcelApplication();
            
            Workbook book;
            book = excel.Create(ExcelApplication.FileFormat.Xls);

            Worksheet sheet = book.Worksheets[0];
            //property
            ColumnProperties colProps0 = sheet.GetColumnProperties(0);
            colProps0.Width = 180;
            //temperature
            ColumnProperties colProps1 = sheet.GetColumnProperties(1);
            colProps1.Width = 60;
            //value
            ColumnProperties colProps2 = sheet.GetColumnProperties(2);
            colProps2.Width = 150;
            //unit
            ColumnProperties colProps3 = sheet.GetColumnProperties(3);
            colProps3.Width = 50;
            //note
            ColumnProperties colProps4 = sheet.GetColumnProperties(4);
            colProps4.Width = 350;          


            sheet.Name = "export";
            Palette pal = book.Palette;
            Color clrYellow = pal.GetClosestColor(255, 255, 0);
            sheet[0, 0].Value = "Property";
            sheet[0, 0].Style.BackgroundColor = clrYellow;
            sheet[0, 1].Value = "T (°C)";
            sheet[0, 1].Style.BackgroundColor = clrYellow;
            sheet[0, 2].Value = "Value";
            sheet[0, 2].Style.BackgroundColor = clrYellow;
            sheet[0, 3].Value = "Unit";
            sheet[0, 3].Style.BackgroundColor = clrYellow;
            sheet[0, 4].Value = "Note";
            sheet[0, 4].Style.BackgroundColor = clrYellow;


       

            int count = 0;
            for (int i = 0; i < properties.Count; i++)
            {             
                    count = count + 1;
                    sheet[count, 0].Value = properties[i].Name;
                    sheet[count, 0].Style.WrapText = true;
                    if (properties[i].Temperature == 999)
                    {
                        sheet[count, 1].Value = "-";
                    }
                    else
                    {
                        sheet[count, 1].Value = properties[i].Temperature;
                    }
               

                    if (properties[i].Values != null && properties[i].Values.Count > 0)
                    {
                        string val = "";
                        foreach (var item in properties[i].Values)
                        {
                            val = val + item.X.Trim() + ", " + item.Y.ToString().Trim() + "; ";
                        }
                        sheet[count, 2].Value = val;

                    }
                    else
                    {
                        sheet[count, 2].Value = properties[i].Value;
                    }
                    sheet[count, 2].Style.WrapText = true;
                    sheet[count, 3].Value = properties[i].Unit;
                    if (properties[i].Note != null)
                    {                     
                        sheet[count, 4].Value = HtmlRemoval.StripTagsRegex(properties[i].Note);
                        sheet[count, 4].Style.WrapText = true;

                    }                       
             
            }

            excel.Save(book, memoryStreamKTMXls);

            return memoryStreamKTMXls;
        }

    }

    public static class HtmlRemoval
    {
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}