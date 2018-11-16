using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class ClassModel {
        public int ClassModelId { get; set; }
        public string ClassModelName { get; set; }
        public int ClassCount { get; set; }
        public IList<SubclassModel> Subclasses { get; set; }

    }
}