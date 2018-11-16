using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class FatigueModel
    {

    public IList<SelectListItem> MaterialConditions { get; set; }
    public IList<SelectListItem> ConditionList {get; set;}
    public FatigueCondition Condition { get; set; }
    public IList<string> AllReferences { get; set; }
    public IList<string> SelectedReferences { get; set; }
    public  FatigueModel()
    {
        ConditionList = new List<SelectListItem>();
        MaterialConditions = new List<SelectListItem>();
    }
    }
}