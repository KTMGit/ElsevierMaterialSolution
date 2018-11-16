using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.Property
{
   public class PropertyWithConvertedValues
    {

        //public int RowId { get; set; }
        //public int PropertyId { get; set; }
        //public string PropertyName { get; set; }
        //public int? DefaultUnitId { get; set; }
        //public int SourcePropertyId { get; set; }
        //public int SourceId { get; set; }
        //public int MaterialId { get; set; }
        //public int SubgroupId { get; set; }

        //public int ValueId { get; set; }
        //public string AdditionalCondition { get; set; }
        //public double? Temperature { get; set; }
        //public string OrigValue { get; set; }
        //public string OrigValueText { get; set; }
        //public string OrigUnit { get; set; }
        //public string ConvValue { get; set; }
        //public string ChemicalIdentityId { get; set; }




        //preostali proeprties        
        //public int? DefaultUnitId { get; set; }
        //public int SourcePropertyId { get; set; }
        //public int SourceId { get; set; }       
        //public string ConvValue { get; set; }
        //public string ChemicalIdentityId { get; set; }

        public string PropertyName { get; set; }
        public int MaterialId { get; set; } //       MaterialID
        public int SubgroupId { get; set; }//SubgroupID    
        public int RowId { get; set; }//RowID
        public int PropertyId { get; set; }//PropertyID
        public int ValueId { get; set; }//ValueID
        public int GroupId { get; set; }
        public string AdditionalCondition { get; set; }//AdditionalCondition
        public string SpecimenOrientation { get; set; }//AdditionalCondition

        public double? Temperature { get; set; }//Temperature



        public double? OrigValue { get; set; }//OrigValue
       public double? OrigValueMin { get; set; }
       public double? OrigValueMax { get; set; }

       public string OrigValueText { get; set; }//OrigValueText
       public int? OrigUnitId { get; set; }//OrigUnit      

       public double? UsValue { get; set; }
       public double? UsValueMin { get; set; }
       public double? UsValueMax { get; set; }


        public double? ConvValue { get; set; }
        public double? ConvValueMin { get; set; }
        public double? ConvValueMax { get; set; }


        public int? DefaultUnitId { get; set; }
        public string DefaultUnit { get; set; }
        public string OriginalUnit { get; set; }
        public string UsUnit { get; set; }






       
       // public ElsevierMaterials.Models.Property Property { get; set; }
    }
}
