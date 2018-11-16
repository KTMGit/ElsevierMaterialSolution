using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class Condition {
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public IList<Property> Properties { get; set; }

        public Condition() {
            Properties = new List<Property>();
        }

    }
}
