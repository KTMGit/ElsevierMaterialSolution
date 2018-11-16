using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models.Domain.Material;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public class ComparisonD
    {
        public ComparisonD()
        {        
            Properties = new List<PropertyD>();
        }

        public IList<PropertyD> Properties { get; set; }

    }
}
