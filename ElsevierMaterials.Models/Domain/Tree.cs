using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class Tree
    {  
        //public int Id { get; set; } //ems id
        public int PN_ID { get; set; }// SourceMaterialId 
        public int? BT_ID { get; set; }
        public string Name { get; set; }
        public bool Root { get; set; }
        public int taxonomy_id { get; set; }
        public bool? metals { get; set; }
        public bool? plastics { get; set; }
        public bool? chemicals { get; set; }
     }
}
