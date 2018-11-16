using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class CreepConditionModel {
        public string Condition { get; set; }
        public int no { get; set; }
        public long? RowNumber { get; set; }
        //public IList<string> SelectedReferences { get; set; }
    }
}
