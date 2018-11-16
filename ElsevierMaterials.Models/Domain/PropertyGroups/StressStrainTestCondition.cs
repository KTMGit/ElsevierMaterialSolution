using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class StressStrainTestCondition
    {
        public string ConditionId { get; set; }
        public string Description { get; set; }
        public int MaterialId { get; set; }
    }
}
