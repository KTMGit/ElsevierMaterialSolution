using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.Property
{
    public class PropertyConversionFactorAndUnit
    {
        public int PropertyId { get; set; }
        public string Unit { get; set; }
        public double? Factor { get; set; }
        public double? Offset { get; set; }
        public Byte? DecimalsUS { get; set; }

    }
}
