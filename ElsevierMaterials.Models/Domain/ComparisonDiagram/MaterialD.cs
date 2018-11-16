using ElsevierMaterials.Models.Domain.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public class MaterialD : IMaterialBasicInfo
    {     
        public int RowNumber { get; set; }
        public int SourceId { get; set; }
        public int MaterialId { get; set; }
        public int SourceMaterialId { get; set; } 
        public int SubgroupId { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public string SubgroupName { get; set; }
        public IList<ConditionD> Conditions { get; set; }
        //public ConditionD Condition { get; set; }


        public MaterialD()
        {
            Conditions = new List<ConditionD>();
            //Condition = new ConditionD();
        }
   
    }
}
