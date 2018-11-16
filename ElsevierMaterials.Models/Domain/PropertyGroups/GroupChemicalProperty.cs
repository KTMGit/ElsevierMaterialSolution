using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class GroupChemicalProperty
    {
        public int MaterialID {get; set;}
        public int SubgroupID { get; set; }
        public int RowID { get; set; }  
       public int ValueID { get; set; }


        public int GroupId { get; set; }
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }
        public string ProductForm { get; set; }
        public string condition { get; set; }
        public string temperatureCond { get; set; }
        public string thickness { get; set; }
        public string MaterialDesc { get; set; }
        public string basis { get; set; }
        public int databook_id { get; set; }
        public double? Temperature { get; set; }
        public double? OrigValue { get; set; }
        public double? OrigValueMin { get; set; }
        public double? OrigValueMax { get; set; }
        public string OrigValueText { get; set; }
        public int? OrigUnit { get; set; }
        public double? ConvValue { get; set; }
        public double? ConvValueMin { get; set; }
        public double? ConvValueMax { get; set; }
        public int? ConvUnit { get; set; }
        public double? UsValue { get; set; }
        public double? UsValueMin { get; set; }
        public double? UsValueMax { get; set; }
        public int? UsUnit { get; set; }
        public string specimen_orientation { get; set; }
        public string stress_ratio { get; set; }
        public string test_type { get; set; }
        public string holding_temperature { get; set; }
        public string holding_time { get; set; }
        public string comment { get; set; }
        public string specimen_type { get; set; }
        public string theoretical_stress_concentration_factor { get; set; }
        public string pressure { get; set; }
        public string phase { get; set; }
        public string concentration { get; set; }
        public string exposure_strain { get; set; }
        public string exposure_stress { get; set; }
        public string exposure_time { get; set; }
        public string wavelength { get; set; }




    }
}
