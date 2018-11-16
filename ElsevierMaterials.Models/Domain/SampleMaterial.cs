using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class SampleMaterial
    {
        public int Id { get; set; } //ems mat Id
        public string Name { get; set; } //PN iz prefered names taxonomiId=null
        public int ClassificationId { get; set; }//PN iz prefered names taxonomiId=(ne znamo)
        public Classification Classification { get; set; }
      //  public int EquivalenceId { get; set; }
       
    }
}
