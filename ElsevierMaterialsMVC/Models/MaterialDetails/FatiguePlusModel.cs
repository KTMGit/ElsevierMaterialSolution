using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class FatiguePlusModel
    {
        public IList<SelectListItem> ConditionList { get; set; }
        public FatiguePlusCondition ConditionPlus { get; set; }
        public FatiguePlusModel()
        {
            ConditionList = new List<SelectListItem>();
        }
    }
}