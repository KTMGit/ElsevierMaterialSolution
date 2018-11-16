using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Filters;

namespace ElsevierMaterialsMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}