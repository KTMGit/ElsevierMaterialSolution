using Api.Models.StressStrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public  class InterativeCurve
    {
        public int Id { get; set; }
        public int CounterId { get; set; }

        public string XUnit { get; set; }
        public string YUnit { get; set; }
        public string XName { get; set; }
        public string YName { get; set; }

        public int? LegendDesc { get; set; }
        public string LegendDescText { get; set; }
        public double? RT { get; set; }

        public double Temperature { get; set; }
        public string TemperatureText { get; set; }


        public IList<PointD> PointsForDiagram { get; set; }
        public IList<PointD> Points { get; set; }
        

        public InterativeCurve()
        {
            Points = new List<PointD>();
            PointsForDiagram = new List<PointD>();
        }
     
    }
}
