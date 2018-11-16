using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Models
{
    public class MetallographyConditionModel
    {
        public int ConditionId { get; set; }
        public string Name { get; set; }
        public MetallographyDetailModel  Details { get; set; }
        public IList<string> SelectedReferences { get; set; }
    }
}