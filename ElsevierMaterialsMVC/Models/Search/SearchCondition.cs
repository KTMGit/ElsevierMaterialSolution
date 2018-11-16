using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.Models.Search
{
 
    public class SearchCondition
    {
       public string FullText { get; set; }
     
       public bool ShowPropertiesFilters { get; set; }

       public IList<Source> Sources { get; set; }
       public IList<TypeClass> ClassificationTypes { get; set; }
       public IList<PropertyGroupModel> PropertyGroups { get; set; }
       public bool FromBrowse { get; set; }
       public bool FromAdvanced { get; set; }

       public SearchCondition()
       {
           Sources = new List<Source>();
       }
       public SearchCondition(string _fullText, IList<Source> _sources)
       {
           Sources = _sources;
           FullText = _fullText;
           FromBrowse = false;
       }
       public SearchCondition(string _fullText, IList<Source> _sources, IList<TypeClass> _classification) {
           Sources = _sources;
           FullText = _fullText;
           ClassificationTypes = _classification;
           FromBrowse = false;
       }
       public SearchCondition(string _fullText)
       {
           Sources = new List<Source>();
           FullText = _fullText;
           FromBrowse = false;
       }
    }

    public class SearchResultsCondition : SearchCondition
    {

        public int ClasificationId { get; set; }
        public int ClasificationTypeId { get; set; }

        public int PropertyClasificationId { get; set; }
        public int PropertyClasificationTypeId { get; set; }

        public string SelectedSource { get; set; }

    }

    public class SearchSubgroupCondition : SearchCondition
    {
        public int SourceId { get; set; }
        public string StandardId { get; set; }
        public string Specification { get; set; }
        public int MaterialId { get; set; }

        public string SelectedSource { get; set; }
    }
}