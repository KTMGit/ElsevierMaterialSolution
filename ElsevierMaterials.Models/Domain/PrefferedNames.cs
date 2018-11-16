using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class PrefferedNames
    {
       public int PN_ID { get; set; }
       public string PN { get; set; }
       public int? taxonomy_id { get; set; }
    }
}
