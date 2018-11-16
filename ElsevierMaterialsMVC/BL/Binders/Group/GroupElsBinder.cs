using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.Group
{
    public class GroupElsBinder
    {

        public IDictionary<ProductGroup.ProductGroupType, ProductGroup> GetGroups(int materialId, int subgroupId,bool isChemical,  IMaterialsContextUow context)
        {
            IList<PropertyGroup> groups = context.PropertyGroups.AllAsNoTracking.Where(s => s.MaterialId == materialId && s.SubgroupId == subgroupId).ToList();
            IDictionary<ProductGroup.ProductGroupType, ProductGroup> listOfGroups = new Dictionary<ProductGroup.ProductGroupType, ProductGroup>();

            foreach (var group in groups)
            {
                ProductGroup groupObj = new ProductGroup();

                if (groups.IndexOf(group) == 0)
                {
                    groupObj = new GroupElsBinder().GetGroupData(materialId, subgroupId, group.PGId,isChemical, context);

                }
                else
                {
                    PropertyGroup group1 = GetMaterialGroup(materialId, subgroupId, group.PGId, context);
                    groupObj = GetMaterialGroupBasicData(group1, isChemical);
                }

                listOfGroups.Add((ProductGroup.ProductGroupType)group.PGId, groupObj);
            }
            return listOfGroups;
        }      

        public PropertyGroup GetMaterialGroup(int materialId, int subgroupId, int groupId, IMaterialsContextUow context)
        {
            return context.PropertyGroups.AllAsNoTracking.Where(s => s.MaterialId == materialId && s.SubgroupId == subgroupId && s.PGId == groupId).FirstOrDefault();

        }
        
        public ProductGroup GetMaterialGroupBasicData(PropertyGroup group, bool isChemical)
        {
            ProductGroup groupObj = new ProductGroup() { ProductGroupId = (ElsevierMaterialsMVC.Models.MaterialDetails.ProductGroup.ProductGroupType)group.PGId, ProductGroupName = group.Name, PropertyCount = group.Count };
            if (isChemical)
            {
                groupObj.ChemicalProperties = new List<ChemicalPropertyModel>();
            }
            else
            {
                groupObj.MaterialConditions = new List<ConditionModel>();
            }
            groupObj.PropertyCount = group.Count;
            return groupObj;
        }


        public ProductGroup GetGroupData(int materialId, int subgroupId,int groupId,bool isChemical, IMaterialsContextUow context)
        {
            PropertyGroup group = GetMaterialGroup(materialId, subgroupId, groupId, context);
            ProductGroup groupObj = GetMaterialGroupBasicData(group, isChemical);
            ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder condBinder= new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder();
            if (isChemical)
            {
                groupObj.ChemicalProperties = condBinder.GetChemicalPropertiesData(materialId, subgroupId,groupId, context).OrderBy(p=>p.PropertyId).ToList();
            }
            else
            {
                groupObj.MaterialConditions = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder().GetGroupMaterialConditions(materialId, subgroupId, groupId, context);

                if (groupObj.MaterialConditions.Count > 0)
                {
                    groupObj.ConditionIdProductFormId = groupObj.MaterialConditions[0].ConditionIdProductFormId;
                    groupObj.ConditionId = groupObj.MaterialConditions[0].ConditionId;
                    groupObj.TestConditions = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder().GetGroupTestConditions(materialId, subgroupId, groupId, groupObj.MaterialConditions[0].ConditionId, groupObj.MaterialConditions[0].ProductFormId, groupObj.MaterialConditions[0].MaterialDescription, groupObj.MaterialConditions[0].Thickness, context);
                }

                if (groupObj.TestConditions.Count > 0)
                {
                    groupObj.RowId = groupObj.TestConditions[0].RowId;
                }
            }
            return groupObj;
        }

    }
}