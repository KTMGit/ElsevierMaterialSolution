using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class MetallographyDetailModel {
        public IList<MetallographyCCModel> CCList { get; set; }
        public string Comment { get; set; }
        public string EtchingMedium { get; set; }
        public string HeatTreatment { get; set; }
        public string MicroStructure { get; set; }
        public IList<MetallographyMicrostructureModel> MSList { get; set; }
        public IList<string> SelectedReferences { get; set; }
    }
}
