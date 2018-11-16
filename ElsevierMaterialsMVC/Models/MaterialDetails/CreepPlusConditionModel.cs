using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.Models.MaterialDetails {
    public class CreepPlusConditionModel {
        public IList<TemperatureItem> ConditionTemperatures { get; set; }
        public TemperatureItem SelectedConditionTemperature { get; set; }
        public IList<CreepPlusTimeStresses> TimesIso { get; set; }
        public IList<CreepPlusTimeStresses> Stresses { get; set; }
        public IList<Api.Models.CreepDataPLUS.StressPointIso> Points { get; set; }
        public ImageSource Diagram { get; set; }
    }
}