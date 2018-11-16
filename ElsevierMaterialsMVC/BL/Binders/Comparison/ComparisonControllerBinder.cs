using Api.Models.Plus;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Comparison;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;


namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonControllerBinder : ComparisonBasicBinder
    {
        private ComparisonPropertyBinder _propertyBinder;
        private ComparisonMaterialBinder _materialBinder;
        private ConditionBinder _conditionBinder; 
        public ComparisonControllerBinder()
        {
            _conditionBinder = new ConditionBinder();
            _propertyBinder = new ComparisonPropertyBinder();
            _materialBinder = new ComparisonMaterialBinder();
        }


        public void AddMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, PropertyFilter propertyClient, ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison, IMaterialsContextUow materialContextUow)
        {
            ElsevierMaterials.Models.Domain.Comparison.Material material = _materialBinder.FillMaterialData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, propertyComparison, null);
            propertyComparison.Materials.Add(material);
        }

        public void AddProperty(int materialId, int sourceMaterialId, int sourceId, int subgroupId, PropertyFilter propertyClient, ref ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison, IMaterialsContextUow materialContextUow)
        {
            ElsevierMaterials.Models.Condition condition = _conditionBinder.FillCondition(sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient);
            propertyComparison = _propertyBinder.FillPropertyData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();
            comparison.Properties.Add(propertyComparison);
 
        }

        public void AddProperties(int materialId, int sourceMaterialId, int sourceId, int subgroupId, List<PropertyFilter> properties, IMaterialsContextUow materialContextUow)
        {            
                foreach (var propertyClient in properties)
                {
                    ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();
                    ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison = GetProperty(propertyClient, comparison);

                    if (propertyComparison != null)
                    {
                        _materialBinder.ChangePropertyData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, ref propertyComparison);
                    }
                    else
                    {
                        ElsevierMaterials.Models.Condition condition = _conditionBinder.FillCondition(sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient);
                        propertyComparison = _propertyBinder.FillPropertyData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);
                        comparison.Properties.Add(propertyComparison);

                        ElsevierMaterials.Models.Domain.Comparison.Material material = _materialBinder.FillMaterialData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, propertyComparison, null);
                        propertyComparison.Materials.Add(material);

                    }
                }
          
        }


        public ElsevierMaterials.Models.Domain.Comparison.Property GetProperty(PropertyFilter propertyClient, ElsevierMaterials.Models.Domain.Comparison.Comparison comparison)
        {
            ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison = null;

            if (propertyClient.TypeId == 0 && propertyClient.SourceTypeId > 0)
            {
                propertyComparison = comparison.Properties.Where(m => m.PropertyInfo.SourceTypeId == propertyClient.SourceTypeId).FirstOrDefault();
            }
            else if (propertyClient.TypeId > 0)
            {
                propertyComparison = comparison.Properties.Where(m => m.PropertyInfo.TypeId == propertyClient.TypeId).FirstOrDefault();
            }
            else
            {
                propertyComparison = comparison.Properties.Where(m => m.PropertyInfo.RowId == propertyClient.RowId && m.PropertyInfo.ConditionId == propertyClient.ConditionId && m.PropertyInfo.GroupId == propertyClient.GroupId).FirstOrDefault();
            }
            return propertyComparison;
        }


        public bool RemoveProperty(int propertyId, int sourcePropertyId, int rowId)
        {
           return _propertyBinder.RemoveProperty(propertyId, sourcePropertyId, rowId);              
        }

       
    }
}