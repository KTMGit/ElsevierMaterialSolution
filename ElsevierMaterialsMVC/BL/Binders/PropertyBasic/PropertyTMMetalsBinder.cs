using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.PropertyBasic
{
    //TODO: Nparaviti interfejs koji ce imati metode GetPropertyValue, GetPropertyTemperature i koji ce ova klasa implementirati
    public class PropertyTMMetalsBinder
    {
        
         public string GetPropertyValue(PropertyFilter propertyClient, Condition condition)
         {
             ElsevierMaterials.Models.Property prop = condition.Properties.Where(m => m.SourcePropertyId == propertyClient.SourceTypeId && m.ValueId == propertyClient.RowId).FirstOrDefault();
             if (prop!= null)
             {
                 return prop.OrigValue;
             }

             return null;
         }

         public double GetPropertyTemperature(PropertyFilter propertyClient, Condition condition)
         {
             ElsevierMaterials.Models.Property prop = condition.Properties.Where(m => m.SourcePropertyId == propertyClient.SourceTypeId && m.ValueId == propertyClient.RowId).FirstOrDefault();
             if (prop.Temperature != null)
             {
                 return (double)prop.Temperature;
             }
             else
             {                 
                 return 20;
             }

         }
                 
    }
}