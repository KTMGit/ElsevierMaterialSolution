using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.BL.Binders.PropertyBasic
{
    public class Utility
    {
        public string FormatOperator(LogicalOperators type) {
            switch (type)
            {
                case LogicalOperators.NotDefined:
                    break;
                case LogicalOperators.Exists:
                    return "exists";
                
                case LogicalOperators.Eq:
                    return "=";
                case LogicalOperators.Lte:
                    return "&#8804;";
                case LogicalOperators.Gte:
                    return "&#8805;";
                case LogicalOperators.Between:
                    return "is between";
                default:
                    break;
            }
            return "";
        }
    }
}