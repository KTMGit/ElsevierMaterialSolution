using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterialsMVC.Models.AdvancedSearch;
using ElsevierMaterialsMVC.Models.Search;
using IniCore.Web.Mvc;
using IniCore.Web.Mvc.Html;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.BL.Binders;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using ElsevierMaterials.Models;
using ElsevierMaterials.EF.MaterialsContextUow;

namespace ElsevierMaterialsMVC.Controllers
{
    public class AdvSearchController : ElsevierController
    {
        private SearchResultsBinder _searchResultsBinder;

        public AdvSearchController()
        {
            _searchResultsBinder = new SearchResultsBinder();
        }
        [SessionExpire]
        public ActionResult AdvSearch(bool initial = false, bool isChemical = false, int groupId = 0)
        {
            BreadcrumbNavigation nav = System.Web.HttpContext.Current.Session["Navigation"] as BreadcrumbNavigation;
            if (initial)
            {
                nav = new BreadcrumbNavigation();
                System.Web.HttpContext.Current.Session["Navigation"] = nav;
            }
            else
            {
                //if (nav != null)
                //{
                //    AdvancedSearchResults res = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
                //    if (res != null)
                //    {
                //        AdvSearchFiltersAll filters = res.PageData as AdvSearchFiltersAll;
                //        if (filters != null && filters.StructureSearch != null && (!string.IsNullOrEmpty(filters.StructureSearch.Exactsearch) || !string.IsNullOrEmpty(filters.StructureSearch.Query) || !string.IsNullOrEmpty(filters.StructureSearch.Stereo)))
                //        {
                //            isChemical = true;
                //        }
                //    }
                //}
                isChemical = (bool)Session["isChemical"];
            }
            AdvancedSearch model = new AdvancedSearch() { SearchCondition = new SearchCondition(), PropertyUnits = new List<PropertyUnitModel>() };
            var sources = materialContextUow.Sources.AllAsNoTracking.ToList();
            model.SearchCondition.FullText = "";
            model.SearchCondition.Sources = sources;
            model.SearchCondition.ClassificationTypes = materialContextUow.Trees.GetFullTreeFor();

            if (groupId != 1 && groupId != 0)
            {
                model.SearchCondition.ClassificationTypes = model.SearchCondition.ClassificationTypes.Where(m => m.TypeClassId == groupId).ToList();
            }
            if (isChemical)
            {
                model.SearchCondition.ClassificationTypes = model.SearchCondition.ClassificationTypes.Where(m => m.TypeClassId == 609397 || m.TypeClassId == 609441).ToList();
            }

            model.SearchCondition.PropertyGroups = materialContextUow.Trees.GetFullPropertyGroups(groupId);


            model.SearchModel = new BaseSearchModel() { Descriptor = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending }) };
            model.PropertyUnits = BreadcrumbNavigationGetSet();
            model.IsChemical = isChemical;
            Session["isChemical"] = isChemical;
            new TableFiltersBinder().resetAllTableFilters();
            return View("AdvSearch", model);
        }


        //Path to the Structure search: DDLs libinchi.dll, StructureHandler.dll and StructureHandler.ini must be on "C:\StructureSearchDLL" location for all servers 
        //dbxmss.dll from  same folder has to be in C:\Windows\System32
        //  [DllImport("D:\\WebSites\\KTM\\Elsevier\\ElsevierMaterialsSolution\\ElsevierMaterialsMVC\\BL\\DLL\\StructureHandler.dll")]
        [DllImport("C:\\EMSStructureSearch\\BL\\DLL\\StructureHandler.dll")]
        //[DllImport("C:\\KTM\\Apps\\ElsevierMaterialsSolution_Test\\BL\\DLL\\StructureHandler.dll")]
        public static extern int StructureSearch(byte[] NamelessParameter1, StringBuilder NamelessParameter2, int NamelessParameter3);


        [SessionExpire]
        public ActionResult AdvStructureSearch(string query, string exactsearch, string stereo, GridDescriptor request)
        {
            System.Web.HttpContext.Current.Session["AdvancedSearchFilter"] = null;
            var tmp = BreadcrumbNavigationGetSet();
            AdvSearchFiltersAll filters = new AdvSearchFiltersAll();
            if (filters.StructureSearch == null)
            {

                filters.StructureSearch = new AdvStructureSearch();
            }
            filters.StructureSearch.Query = query;
            filters.StructureSearch.Exactsearch = exactsearch;
            filters.StructureSearch.Stereo = stereo;
            filters = BreadcrumbNavigationGetSet(filters);

            StringBuilder sb = new StringBuilder(524288);

            byte[] bytesData = Encoding.ASCII.GetBytes("query=" + query + ";exactsearch=" + exactsearch + ";stereo=" + stereo + "\0");
            int result = StructureSearch(bytesData, sb, sb.Capacity);


            string recordIds = sb.Replace(";", ",").ToString().Trim().TrimEnd(',');
            ViewBag.ErrorMsg = "result: " + result + "sb: " + sb;
            BaseSearchModel model = new BaseSearchModel();
            model.IsStructureSearch = true;
            if (request.Pager == null)
            {
                request = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending });
            }
            model.Descriptor = request;

            //kada proradi raskomentarisati
            if (!string.IsNullOrEmpty(recordIds) && result > 0)
            {
                model.ListOfMaterials = _searchResultsBinder.GetResultsStructureAdvSearch(recordIds, request, materialContextUow);
            }

            SearchResultsCondition filterModel = new SearchResultsCondition();

            filterModel.FullText = "";
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = false;
            filterModel.FromAdvanced = true;
            model.Filter = filterModel;


            ModelState.Clear();
            return View("AdvSearchResults", model);

        }



        [SessionExpire]
        public JsonResult ApplyAdvSearchSource(AdvSearchFiltersAll filters, GridDescriptor request)
        {
            filters = BreadcrumbNavigationGetSet(filters);

            return Json(ResponseStatus.Success, new { url = Url.Action("AdvSearchResults", "AdvSearch") }, JsonRequestBehavior.AllowGet);
        }



        [SessionExpire]
        public JsonResult ApplyAdvSearchFilters(AdvSearchFiltersAll filters, GridDescriptor request)
        {
            // Check for null valuse in conditionFilters
            foreach (AdvSearchFilters f in filters.AllFilters)
            {
                if (f.PropertyConditions == null)
                    f.PropertyConditions = new List<PropertyConditionModel>();
            }


            filters = BreadcrumbNavigationGetSet(filters);

            return Json(ResponseStatus.Success, new { url = Url.Action("AdvSearchResults", "AdvSearch") }, JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult AdvSearchResults(GridDescriptor request)
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            var createNav = BreadcrumbNavigationGetSet();
            AdvSearchFiltersAll filters = new AdvSearchFiltersAll();

            AdvancedSearchResults res = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
            if (res != null)
            {
                res.IsVisible = true;
                filters = res.PageData as AdvSearchFiltersAll;
            }



            nav.LastNavigable = "AdvancedSearchResults";
            System.Web.HttpContext.Current.Session["Navigation"] = nav;

            BaseSearchModel model = new BaseSearchModel();
            if (request.Pager == null)
            {
                request = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending });
            }
            model.Descriptor = request;
            if (filters.StructureSearch != null)
            {
                StringBuilder sb = new StringBuilder(524288);

                byte[] bytesData = Encoding.ASCII.GetBytes("query=" + filters.StructureSearch.Query + ";exactsearch=" + filters.StructureSearch.Exactsearch + ";stereo=" + filters.StructureSearch.Stereo + "\0");

                var result = StructureSearch(bytesData, sb, sb.Capacity);
                string recordIds = sb.Replace(";", ",").ToString().Trim().TrimEnd(',');

                if (!string.IsNullOrEmpty(recordIds))
                {
                    model.ListOfMaterials = _searchResultsBinder.GetResultsStructureAdvSearch(recordIds, request, materialContextUow);
                }
                model.IsStructureSearch = true;

            }
            else
            {
                model.ListOfMaterials = _searchResultsBinder.GetResultsAdvSearch(filters, request, materialContextUow);
            }
            SearchResultsCondition filterModel = new SearchResultsCondition();

            filterModel.FullText = "";
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = false;
            filterModel.FromAdvanced = true;
            // add Sources to model
            filterModel.Sources = materialContextUow.Sources.AllAsNoTracking.OrderBy(m => m.Id).ThenBy(n => n.Name).ToList();
            filterModel.SelectedSource = filters.SelectedSource;
            model.Filter = filterModel;

            System.Web.HttpContext.Current.Session["AdvancedSearchFilter"] = filters;
            ModelState.Clear();

            return View("AdvSearchResults", model);
        }


        [SessionExpire]
        public JsonResult ClearAdvSearchFilters()
        {
            IList<PropertyUnitModel> model = new List<PropertyUnitModel>();
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            AdvancedSearchResults res = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
            if (res != null)
            {
                nav.Pop();
            }

            AdvancedSearchNav adv = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;
            adv.PageData = new List<PropertyUnitModel>();
            nav.Push(adv);
            nav.LastNavigable = "AdvancedSearch";


            System.Web.HttpContext.Current.Session["Navigation"] = nav;

            return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionsContainer", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public JsonResult ApplyPager(GridDescriptor request)
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            AdvSearchFiltersAll filters = new AdvSearchFiltersAll();

            AdvancedSearchResults res = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
            if (res != null)
            {
                filters = res.PageData as AdvSearchFiltersAll;
            }
            nav.LastNavigable = "AdvancedSearchResults";
            System.Web.HttpContext.Current.Session["Navigation"] = nav;

            BaseSearchModel model = new BaseSearchModel();
            if (request.Pager == null)
            {
                request = new GridDescriptor(new SortDescriptor() { PropertyName = "Name", Order = SortOrder.Ascending });
            }
            model.Descriptor = request;
            if (filters.StructureSearch != null)
            {
                StringBuilder sb = new StringBuilder(65536);

                byte[] bytesData = Encoding.ASCII.GetBytes("query=" + filters.StructureSearch.Query + ";exactsearch=" + filters.StructureSearch.Exactsearch + ";stereo=" + filters.StructureSearch.Stereo + "\0");

                var result = StructureSearch(bytesData, sb, sb.Capacity);
                string recordIds = sb.Replace(";", ",").ToString().Trim().TrimEnd(',');

                if (!string.IsNullOrEmpty(recordIds))
                {
                    model.ListOfMaterials = _searchResultsBinder.GetResultsStructureAdvSearch(recordIds, request, materialContextUow);
                }
                model.IsStructureSearch = true;

            }
            else
            {
                model.ListOfMaterials = _searchResultsBinder.GetResultsAdvSearch(filters, request, materialContextUow);
            }
            SearchResultsCondition filterModel = new SearchResultsCondition();
            //filterModel.ClasificationId = filters.ClasificationId;
            //filterModel.ClasificationTypeId = filters.ClasificationTypeId;
            filterModel.FullText = "";
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = false;
            filterModel.FromAdvanced = true;
            // add Sources to model
            filterModel.Sources = materialContextUow.Sources.AllAsNoTracking.OrderBy(m => m.Id).ThenBy(n => n.Name).ToList();
            filterModel.SelectedSource = filters.SelectedSource;
            model.Filter = filterModel;
            //BreadcrumbNavigationGetSet(filters);

            return Json(ResponseStatus.Success, RenderPartialViewToString("AdvResultsContainer", model), JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        public ActionResult AddToSearch(int propertyId)
        {
            var pUnits = materialContextUow.PropertyUnits.AllAsNoTracking.Where(n => n.PropertyID == propertyId).ToList();
            PropertyUnitModel pum = new PropertyUnitModel()
            {
                PropertyType = ElsevierMaterials.Models.Domain.AdvancedSearch.PropertyType.Property,
                UniqueID = Guid.NewGuid().ToString("N"),
                PropertyID = pUnits[0].PropertyID,
                PropertyName = pUnits[0].PropertyName,
                Units = new List<UnitModel>()
            };
            pum.PropertyConditions = FillPropertyConditions(propertyId);
            foreach (var item in pUnits.Where(i => i.UnitKey != null).OrderByDescending(j => j.Metric).ThenBy(k => k.UnitLabel))
            {
                pum.Units.Add(new UnitModel() { Factor = (double)item.Factor, Metric = (bool)item.Metric, Offset = (double)item.Offset, UnitKey = (int)item.UnitKey, UnitLabel = item.UnitLabel });
            }
            pum.SelectedUnit = pum.Units.Count > 0 ? (pum.Units[0] != null ? pum.Units[0].UnitKey : 0) : 0;

            return Json(ResponseStatus.Success, RenderPartialViewToString("AdvSearchConditionItem", pum), JsonRequestBehavior.AllowGet);
        }

        // Fill PropertyConditions for selected property
        public IList<PropertyConditionModel> FillPropertyConditions(int propertyId)
        {
            IList<PropertyConditionModel> ret = new List<PropertyConditionModel>();
            IList<AdvSearchPropertyConditions> pcs = materialContextUow.AdvSearchPropertyConditionsAll.AllAsNoTracking.Where(p => p.PropertyID == propertyId).ToList();

            foreach (AdvSearchPropertyConditions pc in pcs)
            {
                PropertyConditionModel pcm = new PropertyConditionModel();
                pcm.Condition = new AdvSearchPropertyConditions()
                {
                    PropertyID = pc.PropertyID,
                    UnitGroup = pc.UnitGroup,
                    X_label = pc.X_label.Trim()
                };
                pcm.FillUnits();

                ret.Add(pcm);
            }
            return ret;
        }


        [SessionExpire]
        public ActionResult AddMaterialToSearch(int materialId, string title, int nodetype)
        {
            string propertyName = "";

            if (nodetype == 1)
            {
                propertyName = "Type";
            }
            else if (nodetype == 2)
            {
                propertyName = "Class";
            }
            else if (nodetype == 3)
            {
                propertyName = "Subclass";
            }
            else if (nodetype == 4)
            {
                propertyName = "Group";
            }

            PropertyUnitModel pum = new PropertyUnitModel()
            {
                PropertyType = ElsevierMaterials.Models.Domain.AdvancedSearch.PropertyType.Material,
                UniqueID = Guid.NewGuid().ToString("N"),
                PropertyID = materialId,
                PropertyName = string.Concat(propertyName, ": ", title),
                Units = new List<UnitModel>()
            };

            return Json(ResponseStatus.Success, RenderPartialViewToString("AdvSearchConditionItem", pum), JsonRequestBehavior.AllowGet);
        }

        public IList<PropertyUnitModel> BreadcrumbNavigationGetSet()
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            IList<PropertyUnitModel> retVal;
            foreach (var item in nav.GetOrderedItems().Where(n => n.NavigableID != "HomePage"))
            {
                item.IsVisible = false;
            }
            if (nav.Contains("FullSearch"))
            {
                FullSearch fs = nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").FirstOrDefault() as FullSearch;
                fs.IsVisible = false;
                //fs.PageData = null;
            }


            AdvancedSearchNav adv = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;
            if (adv == null)
            {
                //  nav = new BreadcrumbNavigation();
                HomePage hp = new HomePage();
                hp.PageData = 0;
                ViewBag.TabSelected = hp.PageData;
                nav.LastNavigable = "HomePage";
                nav.Push(hp);
                adv = new AdvancedSearchNav();
                retVal = new List<PropertyUnitModel>();
            }
            else
            {
                retVal = (IList<PropertyUnitModel>)adv.PageData == null ? new List<PropertyUnitModel>() : (IList<PropertyUnitModel>)adv.PageData;
            }
            nav.LastNavigable = "AdvancedSearch";
            adv.IsVisible = true;
            nav.Push(adv);

            System.Web.HttpContext.Current.Session["Navigation"] = nav;
            return retVal;
        }

        public AdvSearchFiltersAll BreadcrumbNavigationGetSet(AdvSearchFiltersAll filters)
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
            if (nav.Contains("BrowseFacets"))
            {
                nav.Pop();
            }
            if (nav.Contains("BrowseHome"))
            {
                nav.Pop();
            }



            AdvancedSearchNav adv = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;

            if (adv == null)
            {
                adv.IsVisible = true;
                adv = new AdvancedSearchNav();
            }


            IList<PropertyUnitModel> units = new List<PropertyUnitModel>();
            foreach (var item in nav.GetOrderedItems().Where(n => n.NavigableID != "HomePage" && n.NavigableID != "AdvancedSearch"))
            {
                item.IsVisible = false;
            }
            if (nav.Contains("FullSearch"))
            {
                FullSearch fs = nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").FirstOrDefault() as FullSearch;
                fs.IsVisible = false;
                fs.PageData = null;
            }

            AdvancedSearchResults res = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearchResults").FirstOrDefault() as AdvancedSearchResults;
            if (res == null)
            {
                AdvancedSearchResults hp = new AdvancedSearchResults();
                hp.PageData = filters;
                nav.Push(hp);
            }
            else
            {
                if (filters == null
                    || (filters != null && filters.AllFilters == null && filters.StructureSearch == null)
                    || (filters != null && filters.AllFilters != null && filters.AllFilters.Count == 0 && filters.StructureSearch == null)
                    )
                {
                    string selectedSource = filters.SelectedSource;
                    filters = res.PageData as AdvSearchFiltersAll;
                    if (selectedSource != null)
                    {
                        filters.SelectedSource = selectedSource;
                    }
                }
                else
                {
                    res.PageData = filters;
                    res.IsVisible = true;
                    nav.Push(res);
                }
            }

            nav.LastNavigable = "AdvancedSearchResults";

            if (filters != null && filters.AllFilters != null)
            {
                foreach (var item in filters.AllFilters)
                {
                    var pUnits = materialContextUow.PropertyUnits.AllAsNoTracking.Where(n => n.PropertyID == item.propertyId).ToList();
                    PropertyUnitModel pum = new PropertyUnitModel()
                    {
                        PropertyType = item.propertyType,
                        UniqueID = Guid.NewGuid().ToString("N"),
                        PropertyID = item.propertyId,
                        PropertyName = item.propertyName,
                        IsPropertyConditionsActive = item.isPropertyConditionsActive,
                        Units = new List<UnitModel>()
                    };
                    foreach (var u in pUnits.Where(i => i.UnitKey != null))
                    {
                        pum.Units.Add(new UnitModel() { Factor = (double)u.Factor, Metric = (bool)u.Metric, Offset = (double)u.Offset, UnitKey = (int)u.UnitKey, UnitLabel = u.UnitLabel });
                    }
                    pum.SelectedUnit = item.unitId;
                    pum.SelectedBinary = item.binaryOperators;
                    pum.SelectedLogical = item.logicalOperators;
                    pum.ValueFrom = item.valueFrom_orig == null ? "" : item.valueFrom_orig.ToString();
                    pum.ValueTo = item.valueTo_orig == null ? "" : item.valueTo_orig.ToString();

                    // Fill property conditions
                    pum.PropertyConditions = new List<PropertyConditionModel>();

                    foreach (var cond in item.PropertyConditions)
                    {
                        PropertyConditionModel oneCondition = new PropertyConditionModel()
                        {
                            Condition = new AdvSearchPropertyConditions()
                            {
                                PropertyID = cond.Condition.PropertyID,
                                X_label = cond.Condition.X_label,
                                UnitGroup = cond.Condition.UnitGroup
                            },
                            SelectedLogical = cond.SelectedLogical,
                            ValueFrom = cond.ValueFrom,
                            ValueTo = cond.ValueTo
                        };
                        oneCondition.FillUnits();

                        pum.PropertyConditions.Add(oneCondition);
                    }


                    units.Add(pum);
                }

                adv.PageData = units;
            }



            System.Web.HttpContext.Current.Session["Navigation"] = nav;
            return filters;
        }

        private AdvSearchFiltersAll GenerateFilters()
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            AdvancedSearchNav adv = nav.GetOrderedItems().Where(n => n.NavigableID == "AdvancedSearch").FirstOrDefault() as AdvancedSearchNav;

            IList<PropertyUnitModel> units = (IList<PropertyUnitModel>)adv.PageData;

            AdvSearchFiltersAll ret = new AdvSearchFiltersAll() { AllFilters = new List<AdvSearchFilters>() };

            foreach (var item in units)
            {
                AdvSearchFilters fil = new AdvSearchFilters() { propertyId = item.PropertyID, propertyName = item.PropertyName, propertyType = item.PropertyType, unitId = item.SelectedUnit };
                if (item.SelectedUnit != -1) fil.unitName = item.Units.FirstOrDefault(i => i.UnitKey == item.SelectedUnit).UnitLabel;

                //fil.valueFrom = Decimal.Parse(item.ValueFrom);
                //fil.valueTo = Decimal.Parse(item.ValueTo);
                //ret.AllFilters.Add(fil);

                decimal vFrom;
                bool isDecimalFrom = decimal.TryParse(item.ValueFrom, out vFrom);
                vFrom = isDecimalFrom ? vFrom : 0;
                ;
                decimal vTo;
                bool isDecimalTo = decimal.TryParse(item.ValueFrom, out vTo);
                vTo = isDecimalTo ? vTo : 0;
                ;
                fil.valueFrom = vFrom;
                fil.valueTo = vTo;
                ret.AllFilters.Add(fil);
            }
            return ret;
        }
    }
}
