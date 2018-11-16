using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    //TODO:Material and test conditions: Ne znam sta da radim sa ovim modelom jer se svuda koristi.
    public class StressStrainConditionModel {
        public string comment { get; set; }
        public string Condition { get; set; }
        public string engineering { get; set; }
        public int? FormId { get; set; }
        public int? HeatTreatmentId { get; set; }
        public string LinearCoefComment { get; set; }
        public int No { get; set; }
        public string sr { get; set; }
        public string test_temperature { get; set; }
        public byte? TestingType { get; set; }

        public IList<StressStrainTemperature> StressTemperatures { get; set; }
        public IList<string> SelectedReferences { get; set; }

        public int ConditionId { get; set; }
        public string Description { get; set; }
        public int MaterialId { get; set; }
         
    }
}