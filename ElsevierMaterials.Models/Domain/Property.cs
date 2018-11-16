using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class Property {


        public int RowId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int? DefaultUnitId { get; set; }
        public int SourcePropertyId { get; set; }
        public int SourceId { get; set; }
        public int MaterialId { get; set; }
        public int SubgroupId { get; set; }
       
        public int ValueId { get; set; }
        public string AdditionalCondition { get; set; }
        public double? Temperature { get; set; }
   
        public string ConvValue { get; set; }
        public string ChemicalIdentityId { get; set; }


        public string OrigValue { get; set; }
        public string OrigValueText { get; set; }
        public string OrigUnit { get; set; }


        public string DeafaultValue { get; set; }
        public string DeafaultValueText { get; set; }
        public string DeafaultUnit { get; set; }


        public string USValue { get; set; }
        public string USValueText { get; set; }
        public string UStUnit { get; set; }
     
       

    }
}
