using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class TypeClass {
        public int TypeClassId { get; set; }
        public string TypeClassName { get; set; }
        public int TypeClassCount { get; set; }
        public IList<ClassModel> Classes { get; set; }
    }
}