using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IniCore.Web.Mvc.Html;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Search;

namespace ElsevierMaterialsMVC.Models.Subgroups
{
    public class SubgroupsModel
    {
        public SubgroupsModel() {
            ChemicalElsProperties = new List<ElsevierMaterialsMVC.Models.Shared.PropertyDescription>();
        }
        public string Name { get; set; }
        public IList<Material> Materials { get; set; }
        public Material MaterialInfo { get; set; }
        public IList<ElsevierMaterialsMVC.Models.Shared.PropertyDescription> ChemicalElsProperties { get; set; }   
        public SearchSubgroupCondition Filters { get; set; }
        public GridDescriptor Descriptor { get; set; }
    }
}