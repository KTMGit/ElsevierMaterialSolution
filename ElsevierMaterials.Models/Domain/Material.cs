using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class Material 
    {
        public int MaterialId { get; set; } //ems id
        public int SourceMaterialId { get; set; }// SourceMaterialId 
        public int SubgroupId { get; set; } 
        public string Name { get; set; } // PN 
        public string SourceText { get; set; } //databookname 
        public string Standard { get; set; }
        public string Specification { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }
    
        public int ClassificationId { get; set; }
        public Classification Classification { get; set; }
        public int DatabookId { get; set; }

        public int NumProperties { get; set; } 
        public int NumEquivalency { get; set; }
        public int NumProcessing { get; set; }

        public string Manufacturer { get; set; }
        public string Filler { get; set; }

        public string UNSNo { get; set; }
        public string CASRN { get; set; }
     
    }
}
