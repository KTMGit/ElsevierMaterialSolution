using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class ManufacturingModel
    {
        public string Name {get;set;}
        public int ConditionId { get; set; }
        public IList<ConditionModel> Conditions { get; set; }
        public int PropertyCount { get; set; }
        public IList<string> AllReferences { get; set; }
    }
}