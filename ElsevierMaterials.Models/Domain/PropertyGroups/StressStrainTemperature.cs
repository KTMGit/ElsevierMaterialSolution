using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class StressStrainTemperature {
        public int? LegendDesc { get; set; }
        public string LegendDescText { get; set; }
        public double? RT { get; set; }
        public double Temperature { get; set; }
        public string TemperatureText { get; set; }
        public StressStrainDetails TrueDetails  { get; set; }
        public StressStrainDetails EngineeringDetails { get; set; }
    }
}
