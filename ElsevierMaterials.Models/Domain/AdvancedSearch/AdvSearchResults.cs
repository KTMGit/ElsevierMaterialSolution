using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
    public class AdvSearchResults
    {
        public int MaterialID { get; set; }
        public int PropertyID { get; set; }
        public double? Value { get; set; }
        public double? ValueMin { get; set; }
        public double? ValueMax { get; set; }
    }
}