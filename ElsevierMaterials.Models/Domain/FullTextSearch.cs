using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class FullTextSearch
    {
        public int Id { get; set; }
        public string material_designation { get; set; }
        public string material_type { get; set; }
        public string material_group { get; set; }
        public string material_class { get; set; }
        public string material_subClass { get; set; }
        public string properties { get; set; }
        public int? type_ID { get; set; }
        public int? group_ID { get; set; }
        public int? class_ID { get; set; }
        public int? subClass_ID { get; set; }
        public string prop_IDs { get; set; }
        public string UNS { get; set; }
        public string CAS_RN { get; set; }
        public string source_IDs { get; set; }
        public string databook_IDs { get; set; }
        public string structure_image { get; set; }
    }
}
