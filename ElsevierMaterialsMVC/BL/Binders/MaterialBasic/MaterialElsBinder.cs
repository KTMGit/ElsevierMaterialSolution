using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.MaterialBasic
{
    public class MaterialElsBinder
    {

        public ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo GetMaterialInfo(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context)
        {
            return context.Materials.AllAsNoTracking.Where(s => s.MaterialId == materialId && s.SourceMaterialId == sourceMaterialId && s.SourceId == sourceId && s.SubgroupId == subgroupId).Select(m => new ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo { MaterialId = m.MaterialId, Name = m.Name, SourceId = m.SourceId, SourceMaterialId = m.SourceMaterialId, Standard = m.Standard, SubgroupId = m.SubgroupId, SubgroupName = (m.SourceText == "-" ? "" : m.SourceText.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Standard == "-" ? "" : m.Standard.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Specification == "-" ? "" : m.Specification.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ")) }).FirstOrDefault();

        }

        public void FillElsMaterial(int materialId, int subgroupId, int sourceMaterialId, IMaterialsContextUow context, MaterialDetailsModel model, int type)
        {


            int setId = (int)context.RecordLinks.All.Where(m => m.MaterialID == materialId && m.SubgroupID == subgroupId).FirstOrDefault().SetID;

            IList<RecordLink> conditions = context.RecordLinks.All.Where(n => n.MaterialID == materialId && n.SubgroupID == subgroupId).ToList();

            IDictionary<ProductGroup.ProductGroupType, ProductGroup> listOfGroups = new Dictionary<ProductGroup.ProductGroupType, ProductGroup>();

            IList<ConditionModel> conditionWithProperties = new List<ConditionModel>();

            foreach (var conditionObj in conditions)
            {
                IList<MaterialProperty> propertiesPerCond = context.MaterialProperties.All.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == conditionObj.RowID).ToList();
                foreach (var prop in propertiesPerCond)
                {

                    ProductGroup.ProductGroupType groupId = (ProductGroup.ProductGroupType)context.Trees.Find(n => n.PN_ID == prop.PropertyId).BT_ID;

                    if (!listOfGroups.ContainsKey(groupId))
                    {
                        conditionWithProperties = new List<ConditionModel>();

                        ConditionModel condition = new ConditionElsBinder().FillElsCondition(materialId, subgroupId, context, conditionObj, prop, type);
                        conditionWithProperties.Add(condition);

                        ProductGroup productGroup = FillElsProductGroup(conditionWithProperties, groupId, context);

                        productGroup.MaterialConditions = productGroup.MaterialConditions.OrderBy(c => c.ConditionName).ToList();
                        listOfGroups.Add(groupId, productGroup);
                    }
                    else
                    {

                        ProductGroup selectedGroup = listOfGroups[groupId];

                        if (!selectedGroup.MaterialConditions.Any(v => v.ConditionId == conditionObj.RowID))
                        {
                            ConditionModel condition = new ConditionElsBinder().FillElsCondition(materialId, subgroupId, context, conditionObj, prop, type);
                            selectedGroup.MaterialConditions.Add(condition);
                        }
                        else
                        {
                            ConditionModel condition = selectedGroup.MaterialConditions.Where(v => v.ConditionId == conditionObj.RowID).FirstOrDefault();

                            string unitName = prop.OrigUnit == null ? "" : context.Units.Find(n => n.UnitId == prop.OrigUnit).UnitText;
                            ElsevierMaterials.Models.Property property = context.PropertiesWithConvertedValues.GetPropertyInfoForMaterialForSelectedMetric(context.PreferredNames.Find(o => o.PN_ID == prop.PropertyId).PN, materialId, subgroupId, conditionObj.RowID, prop.PropertyId, prop.ValueId, type, unitName);
                            condition.Properties.Add(property);
                        }
                    }

                    listOfGroups[groupId].PropertyCount++;
                }

            }


            model.Properties = new PropertiesModel();
            OrderPropertiesGroups(model, listOfGroups);


        }


        public ProductGroup FillElsProductGroup(IList<ConditionModel> conditionWithProperties, ProductGroup.ProductGroupType groupId, IMaterialsContextUow context)
        {

            string nameP = context.PreferredNames.Find(p => p.PN_ID == (int)groupId).PN;

            ProductGroup productGroup = new ProductGroup();
            productGroup.ProductGroupId = groupId;
            productGroup.ProductGroupName = nameP;
            productGroup.Conditions = conditionWithProperties.OrderBy(c => c.ConditionName).ToList();
            productGroup.ConditionId = conditionWithProperties[0].ConditionId;
            return productGroup;
        }

        public void OrderPropertiesGroups(MaterialDetailsModel model, IDictionary<ProductGroup.ProductGroupType, ProductGroup> listOfGroups)
        {

            ProductGroup orderedGroups = new ProductGroup();
            IList<ProductGroup.ProductGroupType> list = orderedGroups.GetOrder();

            foreach (var item in list)
            {
                if (listOfGroups.ContainsKey(item)) model.Properties.ProductGroups.Add(item, listOfGroups[item]);
            }


        }

      

    }
}