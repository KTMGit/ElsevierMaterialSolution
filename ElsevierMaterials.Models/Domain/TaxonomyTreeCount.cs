using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class TaxonomyTreeCount
    {
        public int Id { get; set; }
        public int? TypeId { get; set; }
        public int TypeCount { get; set; }
        public int? ClassId { get; set; }
        public int ClassCount { get; set; }
        public int? SubClassId { get; set; }
        public int SubClassCount { get; set; }
        public int? GroupId { get; set; }
        public int GroupCount { get; set; }
    }
}
