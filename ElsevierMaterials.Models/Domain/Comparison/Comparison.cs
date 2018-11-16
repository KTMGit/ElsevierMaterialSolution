using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models.Domain.Material;

namespace ElsevierMaterials.Models.Domain.Comparison
{
    public class Comparison
    {        
        public Comparison() {

            MaterialNames = new List<MaterialBasicInfo>();
            Properties = new List<Property>();
            ComparisonD = new ElsevierMaterials.Models.Domain.ComparisonDiagram.ComparisonD();
        }
        public ElsevierMaterials.Models.Domain.ComparisonDiagram.ComparisonD ComparisonD { get; set; }
        public IList<MaterialBasicInfo> MaterialNames { get; set; }
        public IList<Property> Properties { get; set; }

    }
}
