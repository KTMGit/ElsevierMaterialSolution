using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.Models.MaterialDetails {
    public class ConditionModel {

        public ProductGroup.ProductGroupType ProductGroupId { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string MaterialDescription { get; set; }
        public string Thickness { get; set; }
        //public string TSCF { get; set; }
        
        //public string ConditionTemperature { get; set; }
        public IList<Property> Properties { get; set; }
        public IList<string> SelectedReferences { get; set; }
        public int RowId { get; set; }
        public int ProductFormId { get; set; }
        public string ConditionIdProductFormId { get; set; }
    }
}