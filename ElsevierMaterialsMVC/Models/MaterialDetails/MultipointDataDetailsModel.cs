using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class MultipointDataDetailsModel
    {   
         public IList<Api.Models.PLUS.MultiPointData.Condition> Conditions { get; set; }
         public MultipointDataConditionModel SelectedCondition { get; set; }
         public MultipointDataDetailsModel()
         {
             Conditions = new List<Api.Models.PLUS.MultiPointData.Condition>();
         }
    }
}