using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ElsevierMaterialsMVC.Models.MaterialDetails {
    public class CreepPlusModel {
        public Api.Models.CreepDataPLUS.Data Data { get; set; }
        public CreepPlusConditionModel ConditionData { get; set; }
        public IList<Api.Models.CreepDataPLUS.CreepCondition> DiagramConditions { get; set; }
        public CreepPlusModel() {
            DiagramConditions = new List<Api.Models.CreepDataPLUS.CreepCondition>();
        }
    }
}