using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterials.Models.Domain.Export
{
    public class Property
    {
        public Property() {
            //BasicInfo = new PropertyBasicInfo();
            ElsBasicInfo = new PropertyBasicInfo();

        }
        //public PropertyBasicInfo BasicInfo { get; set; }
        public PropertyBasicInfo ElsBasicInfo { get; set; }
        public string Value { get; set; }
        public double Temperature { get; set; } 
    }
}
