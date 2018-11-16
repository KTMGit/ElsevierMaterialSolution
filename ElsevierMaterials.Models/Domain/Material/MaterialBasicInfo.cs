using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models.Domain.Material;

namespace ElsevierMaterials.Models.Domain.Material
{
    public class MaterialBasicInfo : IMaterialBasicInfo
    {
        public int SourceId { get; set; }
        public int MaterialId { get; set; }
        public int SourceMaterialId { get; set; }
        public string SubgroupName { get; set; }
        public string Name { get; set; }
        public string  ShortName { get; set; }
        public string Standard { get; set; }

        public int SubgroupId { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public int RowId { get; set; }
  
    }
}
