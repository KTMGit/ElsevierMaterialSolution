using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ElsevierMaterials.Common.Interfaces;
using IniCore.Web.Mvc;
using ElsevierMaterials.EF.MaterialsContextUow;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Filters;

namespace ElsevierMaterialsMVC
{
    public class ElsevierController : IniController
    {
        //private IExceptionHelper _exceptionHelper;
        private IMaterialsContextUow _materialContextUow;
        private IService _service; 
        public IMaterialsContextUow materialContextUow { get { return _materialContextUow; } }
      
        public ElsevierController() {
            _materialContextUow = new MaterialsContextUow();
         

        }
                [SessionExpire]
        public ActionResult Breadcrumb(string name) {
            ElsevierMaterialsMVC.BL.Global.BreadcrumbNavigation navigation = ((ElsevierMaterialsMVC.BL.Global.BreadcrumbNavigation)Session["Navigation"]);
            ElsevierMaterialsMVC.BL.Global.BaseNavigablePage page = (ElsevierMaterialsMVC.BL.Global.BaseNavigablePage)(from p in navigation.GetOrderedItems() where p.NavigableID == name select p).FirstOrDefault();
            return RedirectToAction(page.Action, page.Controller, page.PageData);
        }
                [SessionExpire]
        public ActionResult BreadcrumbView() {
            //ElsevierMaterialsMVC.BL.Global.BreadcrumbNavigation navigation = ((ElsevierMaterialsMVC.BL.Global.BreadcrumbNavigation)Session["Navigation"]);
            return PartialView("Breadcrumb");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) { _materialContextUow.Dispose(); }
            base.Dispose(disposing);
        }

    }
}
