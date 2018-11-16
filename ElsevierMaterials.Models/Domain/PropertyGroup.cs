using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElsevierMaterials.Models {
    public class PropertyGroup {
        public int PGId { get; set; }
        public int MaterialId { get; set; }
        public int SubgroupId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }

        //public IList<MaterialProperty> Properties { get; set; } 
    }
}
