using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonBasicBinder
    {
        public ElsevierMaterials.Models.Domain.Comparison.Comparison GetComparison()
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = (ElsevierMaterials.Models.Domain.Comparison.Comparison)System.Web.HttpContext.Current.Session["ComparisonContainer"];
            if (comparison != null)
            {
                return comparison;
            }
            else
            {
                comparison = new ElsevierMaterials.Models.Domain.Comparison.Comparison();
                System.Web.HttpContext.Current.Session["ComparisonContainer"] = comparison;
                return comparison;
            }
        }

    }
}