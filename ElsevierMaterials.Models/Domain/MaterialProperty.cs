using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class MaterialProperty {
        public int MaterialId { get; set; }
        public int SubgroupId { get; set; }
        public int RowId { get; set; }
        public int PropertyId { get; set; }
        public int ValueId { get; set; }
        public string AdditionalCondition { get; set; }
        public double? Temperature { get; set; }
        public double? OrigValue { get; set; }
        public double? OrigValueMin { get; set; }
        public double? OrigValueMax { get; set; }
        public string OrigValueText { get; set; }
        public int? OrigUnit { get; set; }
        public double? ConvValue { get; set; }
        public double? ConvValueMin { get; set; }
        public double? ConvValueMax { get; set; }
    }
}
