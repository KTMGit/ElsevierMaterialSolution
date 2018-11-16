using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class SubclassModel {
        public int SubclassModelId { get; set; }
        public string SubclassName { get; set; }
        public int SubclassCount { get; set; }
        public IList<GroupModel> Groups { get; set; }
    }
}