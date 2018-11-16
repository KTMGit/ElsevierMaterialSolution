using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class EquivalentMaterial
    {
       public int MaterialId { get; set; } 
       public int SourceMaterialId { get; set; }
       public int SourceId { get; set; } 
    }
}
