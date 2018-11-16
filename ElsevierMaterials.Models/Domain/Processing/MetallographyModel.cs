using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models
{
    public class MetallographyModel
    {
        public IList<MetallographyConditionModel> MetConditions { get; set; }
        public IList<string> AllReferences { get; set; }
        
    }
}