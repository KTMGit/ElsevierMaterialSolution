using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class EquivalentProperty {
        public int PropertyId { get; set; }
        public int SourcePropertyId { get; set; }
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string DefaultUnitName { get; set; }
        public int? DefaultUnitId { get; set; }
    }
}
