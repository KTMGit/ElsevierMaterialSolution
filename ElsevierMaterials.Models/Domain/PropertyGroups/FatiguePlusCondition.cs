using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class FatiguePlusCondition
    {
        public Api.Models.FatiguePLUS.Condition Condition { get; set; }
        public Api.Models.FatiguePLUS.ConditionDetails Details { get; set; }
        public ImageSource Diagram { get; set; }
     
    }
}
