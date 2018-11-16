using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models {
    public class CrossReferenceModel {
        public string MaterialName { get; set; }
        public int MaterialId { get; set; }
        public string CountryStandard { get; set; }
        public string EquivalenceCategory { get; set; }
    }
}