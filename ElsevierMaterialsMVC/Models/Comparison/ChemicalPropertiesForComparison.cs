using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Comparison
{
    public class ChemicalPropertiesForComparison
    {
        public int MaterialId { get; set; }
        public int SubgroupId { get; set; }
        public int ConditionId { get; set; }
        public int PropertyId { get; set; }
        public string Variable { get; set; }
    }
}