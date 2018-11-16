using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterials.Common.Interfaces;

using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Home;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterialsMVC.Models.Shared;
using ElsevierMaterialsMVC.BL.Binders;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Reflection;
namespace ElsevierMaterialsMVC.Controllers
{
    public class HomeController : ElsevierController
    {
        public BreadcrumbNavigation Navigation { get; set; }

        public HomeController()
            : base()
        {

            string sessionId = "";
            if (System.Web.HttpContext.Current.Session["TotalMateriaSession"] == null)
            {
                sessionId = new TotalMateriaService().GetSessionFromService();
                System.Web.HttpContext.Current.Session["TotalMateriaSession"] = sessionId;
            }
            else
            {
                sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            }


        }
        //import DLL from Delphi
       
     

        [SessionExpire]
        public ActionResult Index()
        {
            ViewBag.Title = "Elsevier :: Home";
           
         
            ViewBag.TabSelected = BreadcrumbNavigationGetSet();
            return View("Index", new IndexModel());
           // return View("IndexNewDesign", new IndexModel());
        }


        [SessionExpire]
        public ActionResult Browse(HasSearchFiltersEnum HasSearchFilters=HasSearchFiltersEnum.No, int groupId = 0)
        {

            System.Web.HttpContext.Current.Session["BrowsedDb"] = groupId;

            ViewBag.Title = "Elsevier :: Browse";
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
            foreach (var item in nav.GetOrderedItems().Where(n => n.NavigableID != "HomePage"))
            {
                item.IsVisible = false;
            }


            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
                BrowseHome bh = new BrowseHome();

                nav.LastNavigable = "BrowseHome";
                bh.IsVisible = true;
                nav.Push(bh);

            }
            else
            {
                if (nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").Any())
                {
                    nav.GetOrderedItems().Where(n => n.NavigableID == "FullSearch").FirstOrDefault().IsVisible = false;
                    HasSearchFilters = HasSearchFiltersEnum.Yes;
                }
               
                BrowseHome bh = nav.GetOrderedItems().Where(n => n.NavigableID == "BrowseHome").FirstOrDefault() as BrowseHome;
                if (bh == null)
                {
                 
                    bh = new BrowseHome();

                }
                bh.IsVisible = true;
                nav.LastNavigable = "BrowseHome";
                nav.Push(bh);

            }
            System.Web.HttpContext.Current.Session["Navigation"] = nav;

            new TableFiltersBinder().resetAllTableFilters();
            return View("Browse",HasSearchFilters);
        }


        

        [SessionExpire]
        public ActionResult GetFiltersMaterials()
        {
            var sources = materialContextUow.Sources.AllAsNoTracking.ToList();
            SearchCondition model = new SearchCondition("", sources);

            model.ClassificationTypes = materialContextUow.Trees.GetFullTreeFor();
            if (System.Web.HttpContext.Current.Session["BrowsedDb"] != null && (int)System.Web.HttpContext.Current.Session["BrowsedDb"] != 0)
            {
                int groupId = (int)System.Web.HttpContext.Current.Session["BrowsedDb"];
                if (groupId != 1)
                {
                    model.ClassificationTypes = model.ClassificationTypes.Where(m => m.TypeClassId == groupId).ToList();
                }
                else
                {
                    model.ClassificationTypes = model.ClassificationTypes.Where(m => m.TypeClassId == 609397 || m.TypeClassId == 609441).ToList();
                }
               
            }
       
            model.PropertyGroups = materialContextUow.Trees.GetFullPropertyGroups(0);
            ViewBag.TabSelected = 1;
            return PartialView("FiltersContainerMaterials", model);
        }


        [SessionExpire]
        public ActionResult GetFiltersResizable(SearchFilters filters)
        {
            var sources = materialContextUow.Sources.AllAsNoTracking.ToList();
            SearchResultsCondition model = new SearchResultsCondition();
            model.FullText = "";
            model.Sources = sources;
            model.ClassificationTypes = materialContextUow.Trees.GetFullTreeFor();
            model.PropertyGroups = materialContextUow.Trees.GetFullPropertyGroups(0);
            model.ClasificationId = filters.ClasificationId;
            model.ClasificationTypeId = filters.ClasificationTypeId;
            model.ShowPropertiesFilters = true;
            return PartialView("FiltersResizable", model);
        }



        public int BreadcrumbNavigationGetSet()
        {
           // BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            BreadcrumbNavigation nav = new BreadcrumbNavigation();
            int ret = 0;
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
                HomePage hp = new HomePage();
                hp.PageData = 0;

                nav.LastNavigable = "HomePage";
                nav.Push(hp);
                ret = (int)hp.PageData;
            }
            else
            {
                HomePage hp = nav.GetOrderedItems().Where(n => n.NavigableID == "HomePage").FirstOrDefault() as HomePage;
                if (hp == null)
                {
                    hp = nav.GetOrderedItems().Where(n => n.NavigableID == "HomePage").FirstOrDefault() as HomePage;
                    hp = new HomePage();
                    hp.PageData = 0;
                }

                nav.LastNavigable = "HomePage";
                nav.Push(hp);
                ret = (int)hp.PageData;
            }
            System.Web.HttpContext.Current.Session["Navigation"] = nav;
            return ret;
        }





    }
}
