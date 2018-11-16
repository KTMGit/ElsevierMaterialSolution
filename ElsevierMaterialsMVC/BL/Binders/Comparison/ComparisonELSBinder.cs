using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonELSBinder: ComparisonGeneralBinder
    {
              
        public Condition FillConditionData(int materialId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ref ElsevierMaterials.Models.Domain.Comparison.Material materialForProperty)
        {

            Condition condition = new Condition();

            ElsevierMaterials.Models.RecordLink conditionRecord = context.RecordLinks.All.Where(n => n.MaterialID == materialId && n.SubgroupID == subgroupId && n.RowID == propertyClient.ConditionId).FirstOrDefault();

            condition.ConditionId = propertyClient.ConditionId;

            if (conditionRecord.ProductForm != null && conditionRecord.ProductForm != "")
            {
                condition.ConditionName = conditionRecord.Condition + "; " + conditionRecord.ProductForm;
            }
           
          
            else
            {
                condition.ConditionName = conditionRecord.Condition;
            }


            //TODO:-Material je mogao da zadrzi model condition
            materialForProperty.Condition = condition.ConditionName;
            materialForProperty.ConditionId = condition.ConditionId;


            MaterialProperty propCond = context.MaterialProperties.All.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == propertyClient.ConditionId && m.PropertyId == propertyClient.TypeId && m.ValueId == propertyClient.RowId).FirstOrDefault();
            if (propCond.OrigValue == null && propCond.OrigValueMax == null && propCond.OrigValueMin == null)
            {
                if (!string.IsNullOrEmpty(propCond.OrigValueText))
                {

                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.AdditionalCondition + " ;" + propCond.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature == null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.AdditionalCondition;
                    if (string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.Temperature.ToString() + "°C";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(propCond.OrigValueText))
                {

                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.OrigValueText + " ;" + propCond.AdditionalCondition + " ;" + propCond.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature == null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.OrigValueText + " ;" + propCond.AdditionalCondition;
                    if (string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.OrigValueText + " ;" + propCond.Temperature.ToString() + "°C";
                }
                else
                {

                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.AdditionalCondition + " ;" + propCond.Temperature.ToString() + "°C";
                    if (!string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature == null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.AdditionalCondition;
                    if (string.IsNullOrEmpty(propCond.AdditionalCondition) && propCond.Temperature != null) materialForProperty.Condition = materialForProperty.Condition + "; " + propCond.Temperature.ToString() + "°C";
                }

            }

            return condition;
        }


 


        public ElsevierMaterials.Models.Domain.Comparison.Material AddMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ElsevierMaterials.Models.Domain.Comparison.Property property, ref ElsevierMaterials.Models.Domain.Comparison.Material materialForProperty)
        {
            if (materialForProperty == null)
            {
                materialForProperty = new ElsevierMaterials.Models.Domain.Comparison.Material();
            }

            Condition condition = FillConditionData(materialId, subgroupId, context, propertyClient, ref materialForProperty);

            new ComparisonMaterialBinder().FillMaterialBasicData(materialId, context, sourceMaterialId, propertyClient.ConditionId, sourceId, subgroupId, ref materialForProperty);

            return materialForProperty;

        }

 
    }
}