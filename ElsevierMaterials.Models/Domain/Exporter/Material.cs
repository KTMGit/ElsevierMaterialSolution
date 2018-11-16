using ElsevierMaterials.Models.Domain.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.Export
{
    public class Material
    {
            public Material()
        {

            MaterialInfo = new MaterialBasicInfo();
            Properties = new List<Property>();
        }
        public MaterialBasicInfo MaterialInfo { get; set; }
        public IList<Property> Properties { get; set; }

       
  
    }
}
