using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterialsMVC.Models.Shared;
using IniCore.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElsevierMaterialsMVC.Controllers
{
    public class TableFiltersController : ElsevierController
    {
        [SessionExpire]
        public JsonResult ApplySearchColumnFilters(SearchFilterColumnsAll filters)
        {
           foreach (SearchFilterColumns f in filters.AllFilters)
                {
                    f.Filter = (f.Filter == null ? "" : f.Filter);
                }
                SearchFilterColumnsAll oldFilters = null;
                if (System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"] != null)
                {
                    oldFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
                    System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"] = null;
                }
                System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"] = filters;
            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }

    

        [SessionExpire]
        public JsonResult ApplySubgroupsColumnFilters(SearchFilterColumnsAll filters, int type)
        {

            foreach (SearchFilterColumns f in filters.AllFilters)
            {
                f.Filter = (f.Filter == null ? "" : f.Filter);
            }

            switch ((FiltersGroup)type)
            {
                case FiltersGroup.None:
                    break;
                case FiltersGroup.MaterialInfo:
                    SetFiltersSessionObject(filters, "SubgroupListMaterialInfo");

                    break;
                case FiltersGroup.SubgroupList:
                    SetFiltersSessionObject(filters, "SubgroupListResults");

                    break;
                default:
                    break;
            }

            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }




        [SessionExpire]
        public JsonResult ApplyMaterialDetailsTableFilters(SearchFilterColumnsAll filters)
        {
            foreach (SearchFilterColumns f in filters.AllFilters)
            {
                f.Filter = (f.Filter == null ? "" : f.Filter);
            }
            SearchFilterColumnsAll oldFilters = null;
            if (System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"] != null)
            {
                oldFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"];
                System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"] = null;
            }
            System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"] = filters;
            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }
        

        private void SetFiltersSessionObject(SearchFilterColumnsAll filters, string sessionName)
        {
            SearchFilterColumnsAll oldFilters = null;
            if (System.Web.HttpContext.Current.Session[sessionName] != null)
            {
                oldFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session[sessionName];
                System.Web.HttpContext.Current.Session[sessionName] = null;
            }
            System.Web.HttpContext.Current.Session[sessionName] = filters;

        }







        //Reset filter for selected column
        [SessionExpire]
        public ActionResult ResetSearchResultsFilter(int columnId)
        {
            SearchFilterColumnsAll filters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
            filters.AllFilters.Where(m => m.Id == columnId).FirstOrDefault().Filter = "";
            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ResetSubgroupListResultsFilterForSelectedColumn(int columnId)
        {
            SearchFilterColumnsAll filters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListResults"];
            filters.AllFilters.Where(m => m.Id == columnId).FirstOrDefault().Filter = "";
            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }



        //Reset all filters
        [SessionExpire]
        public ActionResult ResetAllSearchResultsFilter()
        {
            SearchFilterColumnsAll filters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
            if (filters != null && filters.AllFilters != null && filters.AllFilters.Count > 0)
            {
                foreach (var item in filters.AllFilters)
                {
                    item.Filter = "";
                    item.isVisible = true;
                }
            }

            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        public ActionResult ResetAllSubgroupListResultsFilter()
        {
            SearchFilterColumnsAll filters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListResults"];
            if (filters != null && filters.AllFilters != null && filters.AllFilters.Count > 0)
            {
                foreach (var item in filters.AllFilters)
                {
                    item.Filter = "";
                    item.isVisible = true;
                }
            }
        
            return Json(ResponseStatus.Success, "", JsonRequestBehavior.AllowGet);
        }

     
        


        
    }
}
