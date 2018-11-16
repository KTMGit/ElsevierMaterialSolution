using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class GroupChemicalPropertyAll
    {
        public int MaterialID { get; set; }
        public int SubgroupID { get; set; }
        public int RowID { get; set; }
        public int ValueID { get; set; }

        public int GroupId { get; set; }
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }
        public string Condition { get; set; }
        public string Phase { get; set; }
        public int PhaseId { get; set; }
        public double? Temperature { get; set; }
        public int? DiagramID { get; set; }
        public string X_unit_lbl { get; set; }
        public string Y_unit_lbl { get; set; }
        public string X_range { get; set; }
        public string Y_range { get; set; }
        public int databook_id { get; set; }       
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
        public int? cit_record_id { get; set; }
    }
}
