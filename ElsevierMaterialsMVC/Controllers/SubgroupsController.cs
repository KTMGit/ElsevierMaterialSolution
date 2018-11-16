using IniCore.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterialsMVC.BL.Binders.Subgroups;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterialsMVC.Models.Home;
using ElsevierMaterials.Models;
using ElsevierMaterials.Services;
using IniCore.Web.Mvc.Html;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.Models.Subgroups;
using ElsevierMaterialsMVC.Filters;
using IniCore.Web.Mvc.Extensions;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterials.Models.Domain;
using ElsevierMaterialsMVC.Models.Shared;

namespace ElsevierMaterialsMVC.Controllers
{
    public class SubgroupsController : ElsevierController
    {
        private SubgroupsBinder _subgroupBinder;

        [SessionExpire]
        public ActionResult Subgroup(SearchSubgroupsFilters filters)
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            bool fromFullText = false;
            if (nav.LastNavigable == "FullSearch")
            {
                fromFullText = true;
            }

            filters = _subgroupBinder.BreadcrumbNavigationGetSet(filters);
            
            SubgroupsModel model = _subgroupBinder.GetSubgroupsModel(new GridDescriptor(new SortDescriptor() { PropertyName = "SourceId", Order = SortOrder.Ascending }), filters, materialContextUow);
            if (model.Materials.Count == 1 && fromFullText)
            {
                return RedirectToAction("GetMaterialDetails", "MaterialDetails", new { materialId = model.Materials[0].MaterialId, subgroupId = model.Materials[0].SubgroupId, sourceId = model.Materials[0].SourceId, sourceMaterialId = model.Materials[0].SourceMaterialId, searchText = model.Filters.FullText, tabId = "divProperties" });
            }

            model.ChemicalElsProperties = new ElsevierMaterialsMVC.BL.Binders.PropertyDescriptionBinder().GetProperties(filters.MaterialId, materialContextUow);
            
            return View("Subgroups", model);
         
        }

        [SessionExpire]
        public ActionResult SubgroupBySourceMaterialId(int sourceId, int sourceMaterialId)
        {
            SearchSubgroupsFilters filters = new SearchSubgroupsFilters();
            filters.MaterialId = materialContextUow.Materials.AllAsNoTracking.Where(m => m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).Select(m => m.MaterialId).FirstOrDefault();

            filters = _subgroupBinder.BreadcrumbNavigationGetSet(filters);
            SubgroupsModel model = _subgroupBinder.GetSubgroupsModel(new GridDescriptor(new SortDescriptor() { PropertyName = "SourceId", Order = SortOrder.Ascending }), filters, materialContextUow);

            model.ChemicalElsProperties = new ElsevierMaterialsMVC.BL.Binders.PropertyDescriptionBinder().GetProperties(filters.MaterialId, materialContextUow);
            return View("Subgroups", model);

        }

        [SessionExpire]
        public ActionResult ApplySubgroupFilters(GridDescriptor request, SearchSubgroupsFilters filters)
        {
            if (request.Pager==null)
            {
                request = new GridDescriptor(new SortDescriptor() { PropertyName = "SourceId", Order = SortOrder.Ascending });
            } 
            filters = _subgroupBinder.BreadcrumbNavigationGetSet(filters);
            SubgroupsModel model = _subgroupBinder.GetSubgroupsModel(request, filters, materialContextUow);

            return Json(ResponseStatus.Success, RenderPartialViewToString("EquivalentMaterials", model), JsonRequestBehavior.AllowGet);
        }
       
        
        
        [SessionExpire]
        public ActionResult ApplySubgroupTableFilters(GridDescriptor request, SearchSubgroupsFilters filters)
        {
            filters = _subgroupBinder.BreadcrumbNavigationGetSet(filters);
            SubgroupsModel model = null;
            if (request == null || (request != null && request.Pager == null) || (request != null && request.Sort != null && request.Sort.PropertyName == null))
            {
                model = _subgroupBinder.GetSubgroupsModel(new GridDescriptor(new SortDescriptor() { PropertyName = "SourceText", Order = SortOrder.Ascending }), filters, materialContextUow);
           
            }
            else
            {

                model = _subgroupBinder.GetSubgroupsModel(request, filters, materialContextUow);
             
            }

            return Json(ResponseStatus.Success, RenderPartialViewToString("SubgroupResults", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ClearSubgroupTableFilters(SearchSubgroupsFilters filters)
        {
            filters = _subgroupBinder.BreadcrumbNavigationGetSet(filters);
            SubgroupsModel model = _subgroupBinder.GetSubgroupsModel(new GridDescriptor(new SortDescriptor() { PropertyName = "SourceId", Order = SortOrder.Ascending }), filters, materialContextUow);
            return Json(ResponseStatus.Success, RenderPartialViewToString("EquivalentMaterials", model), JsonRequestBehavior.AllowGet);
        }

        public SubgroupsController()
        {
            _subgroupBinder = new SubgroupsBinder();
        }


    }

}
