using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.Models.Plus;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Services;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonTMBinder: ComparisonGeneralBinder
    {
        public ElsevierMaterials.Models.Domain.Comparison.Material AddMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ElsevierMaterials.Models.Domain.Comparison.Property property, ref ElsevierMaterials.Models.Domain.Comparison.Material materialForProperty, Condition condition)
        {


          new ComparisonMaterialBinder().FillMaterialBasicData(materialId, context, sourceMaterialId, propertyClient.ConditionId, sourceId, subgroupId, ref materialForProperty);
           ElsevierMaterials.Models.Property prop = condition.Properties.Where(m => m.SourcePropertyId == propertyClient.SourceTypeId && m.ValueId == propertyClient.RowId).FirstOrDefault();
           if (sourceId == 2)
           {
               materialForProperty.Value = _binderPropertyTMMetals.GetPropertyValue(propertyClient, condition);

           } 
           else if(sourceId == 3)
           {
               materialForProperty.Value = _binderPropertyTMPLUS.GetPropertyValue(propertyClient, condition);
           }
       
            //TODO: material with e value
           //if (materialForProperty.Value.Contains("E"))
           //{
           //    materialForProperty.Value = double.Parse(materialForProperty.Value).ToString();
           //}

           materialForProperty.Condition = condition.ConditionName + (!string.IsNullOrEmpty(prop.OrigValueText) ? " " + prop.OrigValueText : "");            
           materialForProperty.ConditionId = condition.ConditionId;
           return materialForProperty;

        }


        public ComparisonTMBinder()
        {           
            _binderPropertyTMMetals = new PropertyTMMetalsBinder();
            _binderPropertyTMPLUS = new PropertyTMPlusBinder();
        }
  
        private PropertyTMMetalsBinder _binderPropertyTMMetals;
        public PropertyTMPlusBinder _binderPropertyTMPLUS { get; set; }

      

      
    }
}