using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
  public  class AdvStructureSearch
    {
        public string Query { get; set; }
        public string Exactsearch { get; set; }
        public string Stereo { get; set; }
    }
}
