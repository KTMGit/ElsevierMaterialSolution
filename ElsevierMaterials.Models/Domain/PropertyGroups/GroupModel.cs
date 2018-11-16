using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class PropertyGroupModel {
        public int PropertyGroupModelId { get; set; }
        public string PropertyGroupModelName { get; set; }
        public IList<PropertyModel> Properties { get; set; }
    }
}