using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class Classification
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int? Parent { get; set; }
        public string Name { get; set; }
        public IDictionary<ClassificationType, string> ClassificationNames { get; set; }
        public ICollection<Material> Materials { get; set; }
        public ICollection<SampleMaterial> SampleMaterials { get; set; }

        public Classification() {
            Materials = new HashSet<Material>();
            SampleMaterials = new HashSet<SampleMaterial>();
            ClassificationNames = new Dictionary<ClassificationType, string>();
        }
    }
}
