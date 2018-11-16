using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public class ConditionD
    {
      
        public string Condition { get; set; }
        public string ConditionId { get; set; }
        public IList<PropertyD> Properties { get; set; }
        public IList<InterativeCurve> Temperatures { get; set; }

        public ConditionD() {
           Temperatures = new List<InterativeCurve>();
           Properties = new List<PropertyD>();
        }
        
    }
}
