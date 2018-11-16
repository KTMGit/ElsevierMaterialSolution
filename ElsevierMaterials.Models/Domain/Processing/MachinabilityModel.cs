using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models.Machinability;

namespace ElsevierMaterials.Models {
   public class MachinabilityModel {
       public IList<string> AllReferences { get; set; }
       public IList<MachinabilityCondition> MachinabilityData { get; set; }
    }
}
