using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Shared;
using IniCore.Web.Mvc.Html;

namespace ElsevierMaterialsMVC.Models.Search
{
    public class BaseSearchModel
    {
        public IEnumerable<SampleMaterialModel> ListOfMaterials { get; set; }
        public GridDescriptor Descriptor { get; set; }
        public SearchResultsCondition Filter { get; set; }
        public HasSearchFiltersEnum HasSearchFilters { get; set; }
        public bool IsStructureSearch { get; set; }

      public  BaseSearchModel()
      {
          ListOfMaterials = new HashSet<SampleMaterialModel>();
          Descriptor = null;
          Filter = new SearchResultsCondition();
      }
      public BaseSearchModel(List<SampleMaterialModel> _list, GridDescriptor _desc, SearchResultsCondition _filter)
      {
          ListOfMaterials = _list;
          Descriptor = _desc;
          Filter = _filter;
      }
    }
}