using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
   public class RecordLink {
       public int MaterialID { get; set; }
       public int SubgroupID { get; set; }
       public int RowID { get; set; }
       public int? RecordID { get; set; }
       public int? SetID { get; set; }
       public int? ConditionID { get; set; }
       public int? ProductFormID { get; set; }
       public string Thickness { get; set; }
       public string Temperature { get; set; }
       public string Condition { get; set; }
       public string ProductForm { get; set; }
    }
}
