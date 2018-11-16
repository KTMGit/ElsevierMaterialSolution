using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class StressStrainModel {

        public StressStrainModel() {
            MaterialConditions = new List<MaterialCondition>();
        }

        public IList<MaterialCondition> MaterialConditions { get; set; }
        public IList<StressStrainConditionModel> TestConditions { get; set; }
        public IList<StressStrainConditionModel> StressConditions { get; set; }
        
        public IList<string> AllReferences { get; set; }
       
    }
}