using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.Models.AdvancedSearch
{
    public class AdvancedSearch
    {
        public SearchCondition SearchCondition { get; set; }
        //public AdvSearchFiltersAll Filters { get; set; }
        public IList<PropertyUnitModel> PropertyUnits { get; set; }
        public BaseSearchModel SearchModel { get; set; }
        public bool IsChemical { get; set; }
    }
}