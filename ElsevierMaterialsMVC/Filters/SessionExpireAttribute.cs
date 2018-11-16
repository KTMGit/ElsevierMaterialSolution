using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Filters;

namespace ElsevierMaterialsMVC.Filters {
    public class SessionExpireAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            HttpContext ctx = HttpContext.Current;

            // check  sessions here
            if (HttpContext.Current.Session["username"] == null) {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}