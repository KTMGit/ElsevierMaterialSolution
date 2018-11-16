using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonDiagramBinder: ComparisonGeneralBinder
    {
        public string GenerateChart(int propertyId, int sourcePropertyId, ref System.Web.UI.DataVisualization.Charting.Chart chart)
        {

        

            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();

            ElsevierMaterials.Models.Domain.Comparison.Property property = comparison.Properties.Where(m => m.PropertyInfo.TypeId == propertyId && m.PropertyInfo.SourceTypeId == sourcePropertyId).Select(x => x).FirstOrDefault();
            IList<ElsevierMaterials.Models.Domain.Comparison.Material> materialsForProeprty = property.Materials;

            foreach (var y in materialsForProeprty)
            {
                if (y.Value.Contains("&GreaterEqual;"))
                {
                    y.Value = y.Value.Replace("&GreaterEqual;", "");
                }


                if (y.Value.Contains("&le;"))
                {
                    y.Value = y.Value.Replace("&le;", "");
                }

                if (y.Value.Contains("-") && !y.Value.Contains("E"))  
                {
                    if (y.Value.Split('-')[0]!="")
                    {
                        y.Value = y.Value.Split('-')[0];
                    }                    
                }

                if (y.Value.Contains(">") || y.Value.Contains(">=") ||  y.Value.Contains("<") ||  y.Value.Contains("<=") )
                {
                    y.Value = y.Value.Replace(">", "").Replace(">=", "").Replace("<", "").Replace("<=", "").Replace(" ", "").Trim();
                }
                if (y.Value.Contains("E"))
                {
                    double d = double.Parse(y.Value, System.Globalization.NumberStyles.Float);
                    y.Value = d.ToString();                  
                }


            }

            IList<string> yAxesValues = materialsForProeprty.Select(x => x.Value).ToList();
            IList<string> xAxesValues = property.Materials.Select(x => x.Name).ToList();

            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BackColor = Color.White;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;

            chart.Width = 850;
            chart.Height = 500;

            ChartArea area = new ChartArea("Main");
            area.BorderColor = Color.FromArgb(64, 64, 64, 64);
            area.BorderDashStyle = ChartDashStyle.Solid;
            area.BackColor = Color.White;
            area.BackSecondaryColor = Color.White;
            area.ShadowColor = Color.Transparent;
            area.BackGradientStyle = GradientStyle.TopBottom;
            area.Area3DStyle.Rotation = 25;
            area.Area3DStyle.Perspective = 9;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            area.Area3DStyle.Inclination = 40;
            area.Area3DStyle.IsRightAngleAxes = false;
            area.Area3DStyle.WallWidth = 3;
            area.Area3DStyle.IsClustered = false;
            area.Area3DStyle.Enable3D = false;

            Font axesTitleFont = new Font("Arial", (float)13, FontStyle.Regular);
            area.AxisY.TitleFont = axesTitleFont;

            if (property.PropertyInfo.Unit != "" && property.PropertyInfo.Unit != null)
            {
                area.AxisY.Title = property.PropertyInfo.Name + " (" + property.PropertyInfo.Unit.Replace("<sup>", "^").Replace("</sup>", "").Replace("<sub>", "").Replace("</sub>", "").Replace("&deg;", "°") + ")";
            }
            else
            {
                area.AxisY.Title = property.PropertyInfo.Name;
            }

            chart.ChartAreas.Add(area);
            chart.Series.Add("pie");
            chart.Series["pie"].Points.DataBindXY(xAxesValues, yAxesValues);
            chart.Series["pie"].ChartType = SeriesChartType.Column;
            chart.DataBind();
            string imageName = Guid.NewGuid().ToString() + ".png";
            return imageName;
        }
    }
}