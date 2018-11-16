using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;


namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonMaterialBinder: ComparisonBasicBinder
    {



        public bool AddMaterialToMaterialNamesList(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow materialContextUow)
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();


            if (!comparison.MaterialNames.Where(m => m.MaterialId == materialId && m.SourceMaterialId == sourceMaterialId && m.SourceId == sourceId && m.SubgroupId == subgroupId).Any())
            {
                if (comparison.MaterialNames.Count == 5)
                {
                    return false;
                }

                ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo material = _binderMaterial.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);

                comparison.MaterialNames.Add(new ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo { MaterialId = material.MaterialId, SourceId = sourceId, SourceMaterialId = sourceMaterialId, SubgroupId = subgroupId, Name = material.Name, SubgroupName = material.SubgroupName });

            }

            return true;
        }

        public ElsevierMaterials.Models.Domain.Comparison.Comparison RemoveMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId)
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();

            comparison.MaterialNames.Remove(comparison.MaterialNames.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).FirstOrDefault());

            foreach (var prop in comparison.Properties)
            {
                prop.Materials.Remove(prop.Materials.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).FirstOrDefault());
            }

            IList<ElsevierMaterials.Models.Domain.Comparison.Property> propertiesForDeleting = new List<ElsevierMaterials.Models.Domain.Comparison.Property>();

            foreach (var prop in comparison.Properties)
            {
                if (prop.Materials.Count == 0)
                {
                    propertiesForDeleting.Add(prop);
                }
            }
            foreach (var prop in propertiesForDeleting)
            {
                comparison.Properties.Remove(prop);
            }
            
            return comparison;
          
        }


        public ElsevierMaterials.Models.Domain.Comparison.Material FillMaterialBasicData(int materialId, IMaterialsContextUow context, int sourceMaterialId, int conditionId, int sourceId, int subgroupId, ref ElsevierMaterials.Models.Domain.Comparison.Material materialForProperty)
        {

            if (materialForProperty == null)
            {
                materialForProperty = new ElsevierMaterials.Models.Domain.Comparison.Material();
            }
            ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo basicInfo =  _binderMaterial.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, context);
            materialForProperty.Name = basicInfo.Name;

            materialForProperty.MaterialId = materialId;
            materialForProperty.SourceMaterialId = sourceMaterialId;
            materialForProperty.ConditionId = conditionId;
            materialForProperty.SourceId = sourceId;
            materialForProperty.SubgroupId = subgroupId;
            materialForProperty.SubgroupName = basicInfo.SubgroupName;

            return materialForProperty;
        }
   

   
        public ElsevierMaterials.Models.Domain.Comparison.Material FillMaterialData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ElsevierMaterials.Models.Domain.Comparison.Property property, ElsevierMaterials.Models.Domain.Comparison.Material propertyMaterial)
        {

            if (propertyMaterial == null)
            {
                propertyMaterial = new ElsevierMaterials.Models.Domain.Comparison.Material();
            }

            Condition condition = null;
            switch (sourceId)
            {
                case 1:
                    _binderEls.AddMaterial(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, property, ref propertyMaterial);             
                    break;
                case 2:
                    //TODO: ConditionId umsero rowId.. ??
                    condition = _binderConditionTMMetals.FillCondition(subgroupId, sourceMaterialId, sourceId, propertyClient.GroupId, propertyClient.ConditionId, context);
                    _binderTM.AddMaterial(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, property, ref propertyMaterial, condition);                
                    break;

                case 3:
                    //TODO: ConditionId umsero rowId.. ?? grilon TS Fr  ima problem jer ne vrati properties u okviru conditions.
                    condition = _binderConditionTMPlus.FillConditionData(subgroupId, sourceMaterialId, sourceId, propertyClient.GroupId, propertyClient.ConditionId, context);
                    _binderTM.AddMaterial(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, property, ref propertyMaterial, condition);              
                    break;

                default:
                    break;
            }

            propertyMaterial.Value = _binderProperty.FillPropertyValue(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, condition);

            return propertyMaterial;
        }

    


       


        public void ChangePropertyData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ref ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison)
        {
            ElsevierMaterials.Models.Domain.Comparison.Material materialInComparison = propertyComparison.Materials.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).FirstOrDefault();

            if (materialInComparison != null)
            {
                ElsevierMaterials.Models.Domain.Comparison.Material material = FillMaterialData(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, propertyComparison, materialInComparison);
            }
            else
            {
                ElsevierMaterials.Models.Domain.Comparison.Material material = FillMaterialData(materialId, sourceMaterialId, sourceId, subgroupId, context, propertyClient, propertyComparison, materialInComparison);

                propertyComparison.Materials.Add(material);
            }
        }


        public ComparisonMaterialBinder()
        {
            _binderEls = new ComparisonELSBinder();         
            _binderTM = new ComparisonTMBinder();
            _binderMaterial = new MaterialBinder();
            _binderPropertyEls = new PropertyElsBinder();
            _binderPropertyTMMetals = new PropertyTMMetalsBinder();
            _binderPropertyTMPlus = new PropertyTMPlusBinder();
            _binderProperty = new PropertyBinder();

            _binderConditionTMPlus = new ConditionTMPlusBinder();
            _binderConditionTMMetals = new ConditionTMMetalsBinder();


        }

        private ComparisonELSBinder _binderEls;   
        private ComparisonTMBinder _binderTM;

        private MaterialBinder _binderMaterial;

        private PropertyBinder _binderProperty;
        private PropertyElsBinder _binderPropertyEls;
        private PropertyTMMetalsBinder _binderPropertyTMMetals;
        private PropertyTMPlusBinder _binderPropertyTMPlus;

        private ConditionTMPlusBinder _binderConditionTMPlus;
        private ConditionTMMetalsBinder _binderConditionTMMetals;   
      
        
     
    }
}