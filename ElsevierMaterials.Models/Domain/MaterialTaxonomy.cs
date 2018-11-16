using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class MaterialTaxonomy
    {
        public int ID { get; set; }
        public string MaterialName { get; set; }
        public int? Level1 { get; set; }
        public string Level1Name { get; set; }
        public int? Level2 { get; set; }
        public string Level2Name { get; set; }
        public int? Level3 { get; set; }
        public string Level3Name { get; set; }
        public int? Level4 { get; set; }
        public string Level4Name { get; set; }
        public string source_IDs { get; set; }
        public string databook_IDs { get; set; }
    }
}
