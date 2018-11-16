using ElsevierMaterials.Models.Domain.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.Comparison
{
 
    public class Property
    {
        public Property()
        {
            PropertyInfo = new PropertyBasicInfo();
            Materials = new List<Material>();

        }


        public PropertyBasicInfo PropertyInfo { get; set; }
        public IList<Material> Materials { get; set; }


    }
}
