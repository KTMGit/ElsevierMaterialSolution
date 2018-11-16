using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Shared
{
    public class PropertyDescription
    {
        public PropertyDescriptionEnum Type { get; set; }
        public string  Text { get; set; }
        public string Name { get; set; }
    }


}