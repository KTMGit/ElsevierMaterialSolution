using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
    public class AdvSearchEquivalentProperty
    {
        public int PropertyId { get; set; }
        public int SourcePropertyId { get; set; }
        public int SourceId { get; set; }
    }
}
