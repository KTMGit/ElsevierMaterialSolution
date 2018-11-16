using ElsevierMaterials.Models.Domain.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.Comparison
{
      
    public class Material : MaterialBasicInfo
    {      
        public string Value { get; set; }  
        public string Condition { get; set; }
        public int ConditionId { get; set; }   
   
    }
}
