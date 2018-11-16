using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class Source
    {
       public int Databook_id { get; set; }
       public int Id { get; set; }
       public string Name { get; set; }
       public string  SourceUrl { get; set; }

       public ICollection<Material> Materials { get; set; }

       public Source()
       {
           Materials = new HashSet<Material>();
       }
    }
}
