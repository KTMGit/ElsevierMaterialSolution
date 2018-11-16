using IniCore.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterialsMVC.Models.Home;
using ElsevierMaterials.Models;
using ElsevierMaterials.Services;
using IniCore.Web.Mvc.Html;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using ElsevierMaterials.Models.Domain;
using System.Web.Caching;
using ElsevierMaterialsMVC.Models.AdvancedSearch;
using ElsevierMaterialsMVC.Models.Shared;
using ElsevierMaterialsMVC.BL.Binders;

namespace ElsevierMaterialsMVC.Controllers
{
    public class SearchController : ElsevierController
    {
        private SearchResultsBinder _searchResultsBinder;

        [SessionExpire]
        public ActionResult Search(SearchFilters filters=null, int HasSearchFilters = 0)
        {
            BaseSearchModel model = new BaseSearchModel();
            IDictionary<string, object> allFilters = BreadcrumbNavigationGetSet(filters, null);
            filters = (SearchFilters)(allFilters["filters"]);
            if (allFilters.ContainsKey("advFilters")) {
                model = SearchResults(filters, "", (AdvSearchFiltersAll)allFilters["advFilters"]);
            }
            else {
                model = SearchResults(filters, "", null);
            }
            model.HasSearchFilters = (HasSearchFiltersEnum)HasSearchFilters;
       
            return View("Search", model);
        }

      

        public ActionResult FullTextSearchOnly(SearchFilters filters)
        {
            BaseSearchModel model = new BaseSearchModel();

            model = _searchResultsBinder.GetResultsFullTextSearch(filters, new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending }), materialContextUow);
         
            model.HasSearchFilters = HasSearchFiltersEnum.Yes;


            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            if (nav.Contains("AdvancedSearch"))
            {
                AdvancedSearchNav asn = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;
                asn.PageData = null;
                asn.IsVisible = false;
            }
            if (nav.Contains("AdvancedSearchResults"))
            {
                AdvancedSearchResults asn = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
                asn.PageData = null;
                asn.IsVisible = false;
            }
            if (nav.Contains("Exporter"))
            {
                nav.Pop();
            }
            if (nav.Contains("Comparison"))
            {
                nav.Pop();
            }
            if (nav.Contains("MaterialDetails1"))
            {
                nav.Pop();
            }
            if (nav.Contains("Subgroups"))
            {
                nav.Pop();
            }
            if (nav.Contains("BrowseFacets"))
            {
                nav.Pop();
            }
            if (nav.Contains("BrowseHome"))
            {
                nav.Pop();
            }
            System.Web.HttpContext.Current.Session["AdvancedSearchFilter"] = null;

            Session["ClassificationSelection"] = null;
            Session["ClassificationIds"] = "";
            IDictionary<string, object> allFilters =new Dictionary<string, object>();

            allFilters.Add("filters", filters);
            allFilters.Add("advFilters", null);
            FullSearch fs = nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").FirstOrDefault() as FullSearch;
            if (fs==null)
            {
                fs = new FullSearch();
            }
            fs.IsVisible = true;
            fs.PageData = allFilters;
           nav.LastNavigable="FullSearch";
           nav.Push(fs);
           new TableFiltersBinder().resetAllTableFilters();
            return View("Search", model);

        }

        public JsonResult GetFilters(SearchFilters filters)
        {
            SearchResultsCondition filterModel = GetFiltersModel(filters);
            IDictionary<string, object> allFilters = BreadcrumbNavigationGetSet(filters, null);

            AdvSearchFiltersAll advFilters = new AdvSearchFiltersAll();
            if (allFilters.ContainsKey("advFilters"))
            {
                advFilters = (AdvSearchFiltersAll)allFilters["advFilters"];
            }
            IList<PropertyUnitModel> units = new List<PropertyUnitModel>();
            if (advFilters != null && advFilters.AllFilters != null)
            {
                
                foreach (var item in advFilters.AllFilters)
                {
                    var pUnits = materialContextUow.PropertyUnits.AllAsNoTracking.Where(n => n.PropertyID == item.propertyId).ToList();
                    PropertyUnitModel pum = new PropertyUnitModel() { PropertyType = item.propertyType, UniqueID = Guid.NewGuid().ToString("N"), PropertyID = item.propertyId, PropertyName = item.propertyName, Units = new List<UnitModel>() };
                    foreach (var u in pUnits.Where(i => i.UnitKey != null))
                    {
                        pum.Units.Add(new UnitModel() { Factor = (double)u.Factor, Metric = (bool)u.Metric, Offset = (double)u.Offset, UnitKey = (int)u.UnitKey, UnitLabel = u.UnitLabel });
                    }
                    pum.SelectedUnit = item.unitId;
                    pum.SelectedBinary = item.binaryOperators;
                    pum.SelectedLogical = item.logicalOperators;
                    pum.ValueFrom = item.valueFrom_orig == null ? "" : item.valueFrom_orig.ToString();
                    pum.ValueTo = item.valueTo_orig == null ? "" : item.valueTo_orig.ToString();
                    units.Add(pum);
                }
                
            }
          
            return Json(ResponseStatus.Success, new { showTitle = units.Count > 0 ? true : false, dataSearch= RenderPartialViewToString("FiltersContainerAdvProp", filterModel),dataAdv = RenderPartialViewToString("SearchConditionItemContainer", units) } , JsonRequestBehavior.AllowGet);
         }

        [SessionExpire]
        public JsonResult ApplyPager(GridDescriptor request)
        {

            if (request.Pager == null)
            {
                request = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending });
            }
            BaseSearchModel model = new BaseSearchModel();
            SearchFilters filters = new SearchFilters();
            IDictionary<string, object> allFilters = BreadcrumbNavigationGetSet(filters, null);

            filters = (SearchFilters)(allFilters["filters"]);
            if (allFilters.ContainsKey("advFilters"))
            {
                model = SearchResults(filters, "", (AdvSearchFiltersAll)allFilters["advFilters"], request);
            }
            else
            {
                model = SearchResults(filters, "", null, request);
            }
            SearchResultsCondition filterModel = GetFiltersModel(filters);
         
            model.Filter = filterModel;
            return Json(ResponseStatus.Success, RenderPartialViewToString("ResultsContainer", model), JsonRequestBehavior.AllowGet);
        }




        [SessionExpire]
        public JsonResult SearchResultsApplyInputFilter()
        {

            GridDescriptor request = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending });

            BaseSearchModel model = new BaseSearchModel();
            SearchFilters filters = new SearchFilters();
            IDictionary<string, object> allFilters = BreadcrumbNavigationGetSet(filters, null);

            filters = (SearchFilters)(allFilters["filters"]);
            if (allFilters.ContainsKey("advFilters"))
            {
                model = SearchResults(filters, "", (AdvSearchFiltersAll)allFilters["advFilters"], request);
            }
            else
            {
                model = SearchResults(filters, "", null, request);
            }
            SearchResultsCondition filterModel = GetFiltersModel(filters);

            model.Filter = filterModel;
            return Json(ResponseStatus.Success, RenderPartialViewToString("ResultsContainer", model), JsonRequestBehavior.AllowGet);
        }





 [SessionExpire]
        private SearchResultsCondition GetFiltersModel(SearchFilters filters)
        {
            var sources = materialContextUow.Sources.AllAsNoTracking.ToList().OrderBy(m => m.Id).ThenBy(n => n.Name).ToList();
            SearchResultsCondition model = new SearchResultsCondition();
            model.FullText = filters.filter;
            model.Sources = sources;
      
            IList<int> results = (IList<int>)Session["ClassificationRecordsCount"];

            //IDictionary<int, int> records = _searchResultsBinder.TreeCount(results, materialContextUow);
            IDictionary<int, int> records = _searchResultsBinder.TaxonomyTreeCount(results, materialContextUow);

            model.ClassificationTypes = materialContextUow.Trees.GetFullTreeFor(records);
            Session["ClassificationTypes"] = model.ClassificationTypes;
            model.PropertyGroups = materialContextUow.Trees.GetFullPropertyGroups(0);
            model.ClasificationId = filters.ClasificationId;
            model.ClasificationTypeId = filters.ClasificationTypeId;
            model.ShowPropertiesFilters = true;
            model.SelectedSource = filters.Source;
            model.FromBrowse = filters.FromBrowse;
   
            return  model;
        }

        [SessionExpire]
        public ActionResult BrowseSearch(SearchFilters filters)
        {
            BaseSearchModel model = new BaseSearchModel();
            filters = (SearchFilters)((IDictionary<string, object>)BreadcrumbNavigationGetSet(filters, null, true))["filters"];
            model = SearchResults(filters, "", null);

            SearchResultsCondition filterModel = new SearchResultsCondition();
            filterModel.ClasificationId = filters.ClasificationId;
            filterModel.ClasificationTypeId = filters.ClasificationTypeId;
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = filters.FromBrowse;
            filterModel.SelectedSource = filters.Source;
            model.Filter = filterModel;
         
            return View("BrowseSearch", model);
        }

       [SessionExpire]
        public JsonResult ApplyFilters(SearchFilters filters, AdvSearchFiltersAll advFilters, string classIds, int reset=0)
        {
            if (classIds == "")
            {
                Session["ClassificationIds"] = "";
            }          
            
            BaseSearchModel model = new BaseSearchModel();
            IDictionary<string, object> allFilters = BreadcrumbNavigationGetSet(filters, advFilters, false, classIds);
            filters = (SearchFilters)allFilters["filters"];
            advFilters = (AdvSearchFiltersAll)allFilters["advFilters"];
            if (reset == 1) {
                Session["ClassificationIds"] = "";
                advFilters = null;
                classIds = "";
            }
            model = SearchResults(filters, classIds, advFilters);

            SearchResultsCondition filterModel = new SearchResultsCondition();
            filterModel.ClasificationId = filters.ClasificationId;
            filterModel.ClasificationTypeId = filters.ClasificationTypeId;
            filterModel.PropertyClasificationId = filters.PropertyClasificationId;
            filterModel.PropertyClasificationTypeId = filters.PropertyClasificationTypeId;
            filterModel.FullText = filters.filter;
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = filters.FromBrowse;
            filterModel.SelectedSource = filters.Source;

            model.Filter = filterModel;

            System.Web.HttpContext.Current.Session["AdvancedSearchFilter"] = advFilters;
            ModelState.Clear();
            return Json(ResponseStatus.Success, RenderPartialViewToString("ResultsContainer", model));
        }

        private BaseSearchModel SearchResults(SearchFilters filters, string classIds, AdvSearchFiltersAll advFilters,GridDescriptor grd=null )
        {
            BaseSearchModel model;
            if (grd == null)
            {
                model = _searchResultsBinder.GetResults(filters, new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending }), materialContextUow, classIds, advFilters);
            }
            else
            {
                model = _searchResultsBinder.GetResults(filters,grd, materialContextUow, classIds, advFilters);
            }
            return model;
        }

        [SessionExpire]
        public ActionResult FullTextSearch(string filter)
        {
            IndexModel model = new IndexModel(filter);
            return PartialView("FullTextSearch", model);
        }

        [SessionExpire]
        public JsonResult GetSampleMaterials(string filter, int clId, int clTypeId, GridDescriptor request)
        {
            SearchFilters filters = new SearchFilters() { filter = filter, ClasificationId = clId, ClasificationTypeId = clTypeId };
            BaseSearchModel model = _searchResultsBinder.GetResults(filters, request, materialContextUow, "", null);

            return Json(ResponseStatus.Success, RenderPartialViewToString("ResultsContainer", model));
        }

        [SessionExpire]
        public JsonResult GetSampleMaterialsResizable(bool FromBrowse, string filter, int clId, int clTypeId, GridDescriptor request)
        {
            SearchFilters filters = new SearchFilters() { filter = filter, ClasificationId = clId, ClasificationTypeId = clTypeId, FromBrowse = FromBrowse };
            BaseSearchModel model = _searchResultsBinder.GetResults(filters, request, materialContextUow, "", null);

            return Json(ResponseStatus.Success, RenderPartialViewToString("BrowseResultsContainer", model));
        }

        public IDictionary<string, object> BreadcrumbNavigationGetSet(SearchFilters filters, AdvSearchFiltersAll advFilters, bool browse = false, string classIds = "")
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            if (nav.Contains("Exporter"))
            {
                nav.Pop();
            }
            if (nav.Contains("Comparison"))
            {
                nav.Pop();
            }
            if (nav.Contains("MaterialDetails1"))
            {
                nav.Pop();
            }
            if (nav.Contains("Subgroups"))
            {
                nav.Pop();
            }
            IDictionary<string, object> allFilters = new Dictionary<string, object>();
            foreach (var item in nav.GetOrderedItems().Where(n => n.NavigableID != "HomePage"))
            {
                item.IsVisible = false;
            }
            if (browse)
            {

               allFilters.Add("filters", filters);

                nav.GetOrderedItems().Where(n => n.NavigableID == "BrowseHome").FirstOrDefault().IsVisible = true;
               
               BrowseFacets bf = nav.GetOrderedItems().Where(n => n.NavigableID == "BrowseFacets").FirstOrDefault() as BrowseFacets;
                if (bf == null)
                {
                     
                    bf = new BrowseFacets();
                   
                    bf.PageData = allFilters;
                }
                else
                {
                    if (filters.ClasificationId == 0 ) {

                        allFilters = (Dictionary<string, object>)bf.PageData;
                    }
                    else
	                {
                            bf.PageData = allFilters;
	                }
                }
                bf.IsVisible = true;
                nav.LastNavigable = "BrowseFacets";
                nav.Push(bf);
                   

              
            }
            else
            {
                allFilters.Add("filters", filters);
                allFilters.Add("advFilters", advFilters);
            
             
                if (nav.Contains("AdvancedSearch"))
                {
                    AdvancedSearchNav asn = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;
                    asn.PageData = null;
                    asn.IsVisible = false;
                }
                if (nav.Contains("AdvancedSearchResults"))
                {
                    AdvancedSearchResults asn = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
                    asn.PageData = null;
                    asn.IsVisible = false;
                }
                FullSearch fs = nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").FirstOrDefault() as FullSearch;
                if (fs == null)
                {
                    
                    HomePage hp = new HomePage();
                    hp.PageData = 0;
                    ViewBag.TabSelected = hp.PageData;
                    nav.LastNavigable = "HomePage";
                    hp.IsVisible = true;
                    nav.Push(hp);

                    fs = new FullSearch();
                    fs.PageData = allFilters;
                }
                else
                {
                    if (filters.filter == null && (filters.Source == null || filters.Source == "0") && (advFilters == null || advFilters.AllFilters == null)) allFilters = (Dictionary<string, object>)fs.PageData;
                    else fs.PageData = allFilters;
                    if (!string.IsNullOrWhiteSpace(classIds))
                    {
                        Session["ClassificationIds"] = classIds;
                    }
                 }
               
                fs.IsVisible = true;
                nav.LastNavigable = "FullSearch";
                nav.Push(fs);
                
            }

          System.Web.HttpContext.Current.Session["Navigation"] = nav;
          return allFilters;
        }

        [SessionExpire]
        public ActionResult AddToSearch(int propertyId) {
            var pUnits = materialContextUow.PropertyUnits.AllAsNoTracking.Where(n => n.PropertyID == propertyId).ToList();
            PropertyUnitModel pum = new PropertyUnitModel() { PropertyType = ElsevierMaterials.Models.Domain.AdvancedSearch.PropertyType.Property, UniqueID = Guid.NewGuid().ToString("N"), PropertyID = pUnits[0].PropertyID, PropertyName = pUnits[0].PropertyName, Units = new List<UnitModel>() };
            foreach (var item in pUnits.Where(i => i.UnitKey != null).OrderByDescending(j => j.Metric).ThenBy(k => k.UnitLabel)) {
                pum.Units.Add(new UnitModel() { Factor = (double)item.Factor, Metric = (bool)item.Metric, Offset = (double)item.Offset, UnitKey = (int)item.UnitKey, UnitLabel = item.UnitLabel });
            }
            pum.SelectedUnit = pum.Units.Count > 0 ? (pum.Units[0] != null ? pum.Units[0].UnitKey : 0) : 0;

            return Json(ResponseStatus.Success, RenderPartialViewToString("SearchConditionItem", pum), JsonRequestBehavior.AllowGet);
        }
        
        public SearchController()
        {
            _searchResultsBinder = new SearchResultsBinder();
        }
    }
}
