using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class UnitModel
    {
        public int UnitKey { get; set; }
        public string UnitLabel { get; set; }
        public double Factor { get; set; }
        public double Offset { get; set; }
        public bool Metric { get; set; }
    }
}
