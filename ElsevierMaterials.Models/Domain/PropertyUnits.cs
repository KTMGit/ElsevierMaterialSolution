using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models
{
    public class PropertyUnit
    {
        public int? UnitKey { get; set; }
        public string UnitLabel { get; set; }
        public double? Factor { get; set; }
        public double? Offset { get; set; }
        public bool? Metric { get; set; }
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }
    }


}
