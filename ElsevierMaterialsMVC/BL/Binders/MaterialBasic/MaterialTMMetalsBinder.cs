using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.MaterialBasic
{
    public class MaterialTMMetalsBinder
    {

        public ProductGroup FillMechanicalGroup(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service, int type)
        {

            IList<Condition> mechanical = FillMechanicalConditions(sourceMaterialId, sessionId, subgroupId, service, type);



            string unit = "°C";
            if (type == 2)
            {
                unit = "°F";
            }

            if (mechanical.Count > 0)
            {

                IList<ConditionModel> mechanicalConditionsGlobal = new List<ConditionModel>();

                foreach (var itemC in mechanical)
                {

                    foreach (var pro in itemC.Properties)
                    {

                        pro.OrigValueText = !string.IsNullOrEmpty(pro.OrigValueText) ? pro.OrigValueText : "";

                        if (!string.IsNullOrEmpty(pro.AdditionalCondition) && pro.Temperature != null)
                        {
                            pro.OrigValueText += pro.AdditionalCondition + "; " + pro.Temperature.ToString() + unit;
                        }
                        if (!string.IsNullOrEmpty(pro.AdditionalCondition) && pro.Temperature == null)
                        {
                            pro.OrigValueText += pro.AdditionalCondition;
                        }
                        if (string.IsNullOrEmpty(pro.AdditionalCondition) && pro.Temperature != null)
                        {
                            pro.OrigValueText += pro.Temperature.ToString() + unit;
                        }


                        var st = context.EquivalentProperties.AllAsNoTracking.Where(s => s.SourcePropertyId == pro.SourcePropertyId && s.SourceId == 2).FirstOrDefault();
                        if (st != null)
                        {
                            pro.PropertyId = st.PropertyId;
                            pro.PropertyName = st.Name;
                        }

                    }

                    itemC.Properties = itemC.Properties.OrderBy(s => s.PropertyName).ThenBy(m => m.Temperature).ToList();
                    int rowId = 0;
                    foreach (var pro in itemC.Properties)
                    {
                        pro.ValueId = rowId;
                        rowId = rowId + 1;
                    }


                    ConditionModel condModel = new ConditionModel();
                    condModel.ConditionId = itemC.ConditionId;
                    condModel.ConditionName = itemC.ConditionName;
                    condModel.Properties = itemC.Properties;

                    mechanicalConditionsGlobal.Add(condModel);

                }

                //string nameM = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Mechanical).PN;
                //propertiesGroups.Add(ProductGroup.ProductGroupType.Mechanical, new ProductGroup() { ProductGroupId = ProductGroup.ProductGroupType.Mechanical, ProductGroupName = nameM, Conditions = mechanicalConditionsGlobal, ConditionId = chemcialConditionsGlobal[0].ConditionId, PropertyCount = tempCount });

                ProductGroup propertyGroup = FillPropertyGroup(mechanicalConditionsGlobal, ProductGroup.ProductGroupType.Mechanical, context);
                return propertyGroup;
            }

            return null;

        }

        public IList<Condition> FillMechanicalConditions(int sourceMaterialId, string sessionId, int subgroupId, IService service, int type)
        {

            IList<Condition> mechanical = service.GetMechanicalRoomPropertiesFromService(sessionId, sourceMaterialId, subgroupId, type).OrderBy(m => m.ConditionName).ToList();
            IList<Condition> mechanicalHigh = service.GetMechanicalHighLowPropertiesFromService(sessionId, sourceMaterialId, subgroupId, Api.Models.Mechanical.MechanicalGroupEnum.High, type).OrderBy(m => m.ConditionName).ToList();
            IList<Condition> mechanicalLow = service.GetMechanicalHighLowPropertiesFromService(sessionId, sourceMaterialId, subgroupId, Api.Models.Mechanical.MechanicalGroupEnum.Low, type).OrderBy(m => m.ConditionName).ToList();




            if (mechanicalHigh.Count > 0 && mechanicalLow.Count > 0)
            {
                foreach (var item in mechanicalHigh)
                {
                    if (mechanicalLow.Where(c => c.ConditionId == item.ConditionId).Any())
                    {
                        mechanical.Add(new Condition() { ConditionId = 5000 + item.ConditionId, ConditionName = item.ConditionName, Properties = item.Properties.Concat(mechanicalLow.Where(c => c.ConditionId == item.ConditionId).FirstOrDefault().Properties).ToList() });
                    }
                    else
                    {
                        mechanical.Add(new Condition() { ConditionId = 3000 + item.ConditionId, ConditionName = item.ConditionName, Properties = item.Properties });
                    }


                }
                foreach (var item in mechanicalLow)
                {
                    if (!mechanicalHigh.Where(c => c.ConditionId == item.ConditionId).Any())
                    {
                        mechanical.Add(new Condition() { ConditionId = 1000 + item.ConditionId, ConditionName = item.ConditionName, Properties = item.Properties });
                    }

                }
            }
            if (mechanicalHigh.Count > 0 && mechanicalLow.Count == 0)
            {
                foreach (var item in mechanicalHigh)
                {

                    mechanical.Add(new Condition() { ConditionId = 3000 + item.ConditionId, ConditionName = item.ConditionName, Properties = item.Properties });
                }
            }
            if (mechanicalHigh.Count == 0 && mechanicalLow.Count > 0)
            {
                foreach (var item in mechanicalHigh)
                {

                    mechanical.Add(new Condition() { ConditionId = 1000 + item.ConditionId, ConditionName = item.ConditionName, Properties = item.Properties });
                }
            }


            return mechanical;
        }
               
        public ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo GetMaterialInfo(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context)
        {
            FullTextSearch fts = context.FullTextSearch.GetMaterialById(materialId);           
            ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo material = null;

            IService service = new TotalMateriaService();
            material = service.GetMaterialSubgroupListFromService(System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString(), materialId, sourceMaterialId).Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId).Select(m => new ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo { MaterialId = m.MaterialId, Name = m.Name, SourceId = m.SourceId, SourceMaterialId = m.SourceMaterialId, Standard = m.Standard, SubgroupId = m.SubgroupId, SubgroupName = (m.SourceText == "-" ? "" : m.SourceText.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Standard == "-" ? "" : m.Standard.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Specification == "-" ? "" : m.Specification.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ")) }).FirstOrDefault();

            if (material != null)
            {
                material.Name = fts.material_designation;
            }

            return material;
        }
        
        public Dictionary<ProductGroup.ProductGroupType, ProductGroup> FillPhysicalMechanicalChemicalGroups(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, string tabId, IService service, int type)
        {

            Dictionary<ProductGroup.ProductGroupType, ProductGroup> productGroups = new Dictionary<ProductGroup.ProductGroupType, ProductGroup>();

            ProductGroup propertyGroup = FillChemicalGroup(subgroupId, sourceMaterialId, sessionId, context, service);

            if (propertyGroup != null)
            {

                productGroups.Add(ProductGroup.ProductGroupType.Chemical, propertyGroup);
            }

            propertyGroup = FillMechanicalGroup(subgroupId, sourceMaterialId, sessionId, context, service, type);

            if (propertyGroup != null)
            {
                productGroups.Add(ProductGroup.ProductGroupType.Mechanical, propertyGroup);
            }


            propertyGroup = FillPhysicalGroup(subgroupId, sourceMaterialId, sessionId, context, service, type);
            if (propertyGroup != null)
            {
                propertyGroup.AllReferences = service.GetAllReferencesFromService(sessionId, sourceMaterialId, MaterialDetailType.PhysicalProperties);
                productGroups.Add(ProductGroup.ProductGroupType.Physical, propertyGroup);
            }


            return productGroups;
        }
      
        public ProductGroup FillChemicalGroup(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service)
        {

            IList<ConditionModel> chemcialConditionsGlobal = new List<ConditionModel>();

            IList<Condition> chemicalConditions = FillChemicalData(subgroupId, sourceMaterialId, sessionId, service);

            if (chemicalConditions.Count > 0 && chemicalConditions[0].Properties.Count > 0)
            {
                foreach (var chemicalCondition in chemicalConditions)
                {
                    foreach (var chemicalElement in chemicalCondition.Properties)
                    {
                        var propertyEls = context.PreferredNames.AllAsNoTracking.Where(p => p.PN.ToLower() == chemicalElement.PropertyName.ToLower() && p.taxonomy_id == 2).FirstOrDefault();
                        if (propertyEls != null)
                        {
                            chemicalElement.PropertyId = propertyEls.PN_ID;
                          
                        }
                    }
                }


                ConditionModel cond = FillConditionModel(chemicalConditions, ProductGroup.ProductGroupType.Chemical);
                chemcialConditionsGlobal.Add(cond);


                ProductGroup propertyGroup = FillPropertyGroup(chemcialConditionsGlobal, ProductGroup.ProductGroupType.Chemical, context);
                return propertyGroup;

            }
            return null;
        }

        public IList<Condition> FillChemicalData(int subgroupId, int sourceMaterialId, string sessionId, IService service)
        {
            IList<Condition> chemicalConditions = service.GetChemicalCompositionFromService(sessionId, sourceMaterialId, subgroupId).ToList();
            return chemicalConditions;
        }
        
        public ProductGroup FillPhysicalGroup(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service, int type)
        {
            IList<ConditionModel> physicalConditionsGlobal = new List<ConditionModel>();
                       
            IList<Condition> physicalConditions = service.GetPhysicalPropertiesFromService(sessionId, sourceMaterialId).OrderBy(m => m.ConditionName).ToList();                    
            
            foreach (var physicalCondition in physicalConditions)
            {
                IList<Property> physicalProperties = new ConditionTMMetalsBinder().FillPhysicalConditionData(context, type, physicalCondition);
                IList<string> selectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, physicalCondition.ConditionId, MaterialDetailType.PhysicalProperties);
               
                physicalConditionsGlobal.Add(new ConditionModel() { ConditionId = physicalCondition.ConditionId, SelectedReferences = selectedReferences, ConditionName = physicalCondition.ConditionName, Properties = physicalProperties });
            }

            if (physicalConditions.Count > 0)
            {
                //string nameP = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Physical).PN;
                //propertiesGroups.Add(ProductGroup.ProductGroupType.Physical, new ProductGroup() { ProductGroupId = ProductGroup.ProductGroupType.Physical, ProductGroupName = nameP, Conditions = chemcialConditionsGlobal, ConditionId = chemcialConditionsGlobal[0].ConditionId, PropertyCount = tempCount });
                ProductGroup propertyGroup = FillPropertyGroup(physicalConditionsGlobal, ProductGroup.ProductGroupType.Physical, context);
                return propertyGroup;
            }

            return null;
        }
             
        public FatigueModel FillFatigueStrain(int sourceMaterialId, string sessionId, IService service, int type = 1)
        {

           FatigueModel fatigueStrain = new FatigueModel();
           IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetFatigueMaterialConditionsFromService(sessionId, sourceMaterialId, 1, type);
           fatigueStrain.MaterialConditions = materialConditions.Select(m => new System.Web.Mvc.SelectListItem() { Value = m.ConditionId, Text = m.Description}).ToList();
           if ( fatigueStrain.MaterialConditions.Count > 0)
           {
               IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, fatigueStrain.MaterialConditions[0].Value, 1, type);
                  if (tempConds.Count > 0)
                   {

                       foreach (var item in tempConds)
                       {
                           System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                           fatigueStrain.ConditionList.Add(tempListItem);
                       }
                       string no = fatigueStrain.ConditionList[0].Value;
                       fatigueStrain.Condition = new FatigueCondition();
                       fatigueStrain.Condition.UnitType = type;
                       fatigueStrain.Condition.Condition = tempConds[0];
                       fatigueStrain.Condition.Details = service.GetFatigueStrainLifeConditionDetailsFromService(sessionId, sourceMaterialId, no, type);
                       fatigueStrain.Condition.Diagram = service.GetFatigueStrainSNCurveDiagramFromService(sessionId, sourceMaterialId, no, type);
                       fatigueStrain.Condition.Points = service.GetFatigueStrainSNCurveDataFromService(sessionId, sourceMaterialId, no, type);
                       fatigueStrain.Condition.PointsForDiagram = service.GetFatigueStrainSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, no, type);
                       fatigueStrain.Condition.Type = FatigueType.StrainLife;
                       fatigueStrain.Condition.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, no, ElsevierMaterials.Models.MaterialDetailType.FatigueData);
                   }

           }       
           
               return fatigueStrain;

         
            //return fatigueStrain;
        }
        
        public FatigueModel FillFatigueStress(int sourceMaterialId, string sessionId, IService service, int type = 1)
        {
            FatigueModel fatigueStress = new FatigueModel();           

            IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetFatigueMaterialConditionsFromService(sessionId, sourceMaterialId, 2, type);
            fatigueStress.MaterialConditions = materialConditions.Select(m => new System.Web.Mvc.SelectListItem() { Value = m.ConditionId, Text = m.Description }).ToList();

            if (fatigueStress.MaterialConditions.Count > 0)
            {
                  IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, fatigueStress.MaterialConditions[0].Value, 2, type);
            
                if (tempConds.Count > 0)
                {
                    foreach (var item in tempConds)
                    {
                        System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                        fatigueStress.ConditionList.Add(tempListItem);
                    }

                    string no = fatigueStress.ConditionList[0].Value;
                    fatigueStress.Condition = new FatigueCondition();
                    fatigueStress.Condition.Condition = tempConds[0];
                    fatigueStress.Condition.UnitType = type;
                    fatigueStress.Condition.Details = service.GetFatigueStressLifeConditionDetailsFromService(sessionId, sourceMaterialId, no, type);                   
                    fatigueStress.Condition.Points = service.GetFatigueStressSNCurveDataFromService(sessionId, sourceMaterialId, no, type);
                    fatigueStress.Condition.PointsForDiagram = service.GetFatigueStressSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, no, type);
                    if (fatigueStress.Condition.PointsForDiagram.Curves.Count()>0)
                    {
                        fatigueStress.Condition.Diagram = service.GetFatigueStressSNCurveDiagramFromService(sessionId, sourceMaterialId, no, type);
                    }  
                    fatigueStress.Condition.Type = FatigueType.StressLife;
                    fatigueStress.Condition.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, no, ElsevierMaterials.Models.MaterialDetailType.FatigueData);
                    return fatigueStress;
                }
      
            }
            return fatigueStress;
        }
        
        public ConditionModel FillConditionModel(IList<Condition> conditions, ProductGroup.ProductGroupType productGroup)
        {
            ConditionModel cond = new ConditionModel();
            cond.ConditionId = conditions[0].ConditionId;
            cond.ConditionName = conditions[0].ConditionName;
            cond.Properties = conditions[0].Properties;
            cond.ProductGroupId = productGroup;
            return cond;
        }

        public ProductGroup FillPropertyGroup(IList<ConditionModel> conditions, ProductGroup.ProductGroupType propertyGroupType, IMaterialsContextUow context)
        {

            string propertyGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)propertyGroupType).PN;

            ProductGroup propertyGroup = new ProductGroup();
            if (System.Configuration.ConfigurationManager.AppSettings["DeveloperSite"].ToLower() == "true")
            {
                if (conditions.Count() > 0)
                {
                    propertyGroup.ProductGroupId = propertyGroupType;
                    propertyGroup.ProductGroupName = propertyGroupName;
                    propertyGroup.Conditions = conditions;
                    propertyGroup.ConditionId = conditions[0].ConditionId;
                    int propertyCount = 0;
                    foreach (var item in conditions)
                    {
                        propertyCount = propertyCount + item.Properties.Count();
                    }
                    propertyGroup.PropertyCount = propertyCount;
                }
            }
            else
            {
                propertyGroup.ProductGroupId = propertyGroupType;
                propertyGroup.ProductGroupName = propertyGroupName;
                propertyGroup.Conditions = conditions;
                propertyGroup.ConditionId = conditions[0].ConditionId;
                int propertyCount = 0;
                foreach (var item in conditions)
                {
                    propertyCount = propertyCount + item.Properties.Count();
                }
                propertyGroup.PropertyCount = propertyCount;
            
            }
            return propertyGroup;

        }


        //procedure for loading only Mechanical conditions for first material screen without seeing any details
        public ProductGroup FillMechanicalGroupOnlyConditions(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service, int type)
        {

            //IList<Condition> mechanical = FillMechanicalConditions(sourceMaterialId, sessionId, subgroupId, service, type);

            //string unit = "°C";
            //if (type == 2)
            //{
            //    unit = "°F";
            //}

            //if (mechanical.Count > 0)
            //{

            //    IList<ConditionModel> mechanicalConditionsGlobal = new List<ConditionModel>();

            //    foreach (var itemC in mechanical)
            //    {

            //        ConditionModel condModel = new ConditionModel();
            //        condModel.ConditionId = itemC.ConditionId;
            //        condModel.ConditionName = itemC.ConditionName;
            //        condModel.Properties = new List<Property>();

            //        mechanicalConditionsGlobal.Add(condModel);
            //    }

            //    ProductGroup propertyGroup = FillPropertyGroup(mechanicalConditionsGlobal, ProductGroup.ProductGroupType.Mechanical, context);
            //    return propertyGroup;
            //}

            //return null;

            IList<ConditionModel> mechanicalConditionsGlobal = new List<ConditionModel>();
            ProductGroup propertyGroup = FillPropertyGroup(mechanicalConditionsGlobal, ProductGroup.ProductGroupType.Mechanical, context);
            return propertyGroup;

        }

        //procedure for loading only Physical conditions for first material screen without seeing any details
        public ProductGroup FillPhysicalGroupOnlyConditions(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service, int type)
        {
            //IList<Condition> physicalConditions = service.GetPhysicalPropertiesFromService(sessionId, sourceMaterialId).OrderBy(m => m.ConditionName).ToList();
            //IList<ConditionModel> physicalConditionsGlobal = new List<ConditionModel>();

            //foreach (var physicalCondition in physicalConditions)
            //{
            //    //IList<Property> physicalProperties = new ConditionTMMetalsBinder().FillPhysicalConditionData(context, type, physicalCondition);

            //    IList<PropertyConversionFactorAndUnit> propertiesWithConversionFacorsAndUnits = context.PropertiesWithConversionFactorsAndUnits.GetPropertiesWithConversionFactorsAndUnits();
            //    IList<Property> physicalProperties = new List<Property>();
            //    IList<string> selectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, physicalCondition.ConditionId, MaterialDetailType.PhysicalProperties);

            //    physicalConditionsGlobal.Add(new ConditionModel() { ConditionId = physicalCondition.ConditionId, SelectedReferences = selectedReferences, ConditionName = physicalCondition.ConditionName, Properties = physicalProperties });
            //}

            //if (physicalConditions.Count > 0)
            //{
            //    ProductGroup propertyGroup = FillPropertyGroup(physicalConditionsGlobal, ProductGroup.ProductGroupType.Physical, context);
            //    return propertyGroup;
            //}

            //return null;
            IList<ConditionModel> physicalConditionsGlobal = new List<ConditionModel>();
            ProductGroup propertyGroup = FillPropertyGroup(physicalConditionsGlobal, ProductGroup.ProductGroupType.Physical, context);
            return propertyGroup;
        }

        //procedure for loading only Chemical conditions for first material screen without seeing any details
        public ProductGroup FillChemicalGroupOnlyConditions(int subgroupId, int sourceMaterialId, string sessionId, IMaterialsContextUow context, IService service, int type)
        {

            //IList<ConditionModel> chemcialConditionsGlobal = new List<ConditionModel>();

            //IList<Condition> chemicalConditions = FillChemicalData(subgroupId, sourceMaterialId, sessionId, service);

            //if (chemicalConditions.Count > 0 && chemicalConditions[0].Properties.Count > 0)
            //{
                
            //    ConditionModel cond = FillConditionModel(chemicalConditions, ProductGroup.ProductGroupType.Chemical);
            //    chemcialConditionsGlobal.Add(cond);

            //    ProductGroup propertyGroup = FillPropertyGroup(chemcialConditionsGlobal, ProductGroup.ProductGroupType.Chemical, context);
            //    return propertyGroup;

            //}
            //return null;
            IList<ConditionModel> chemcialConditionsGlobal = new List<ConditionModel>();
            ProductGroup propertyGroup = FillPropertyGroup(chemcialConditionsGlobal, ProductGroup.ProductGroupType.Chemical, context);
            return propertyGroup;
        }

        //procedure for loading only Fatigue strain conditions for first material screen without seeing any details
        public FatigueModel FillFatigueStrainOnlyConditions(int sourceMaterialId, string sessionId, IService service, int type = 1)
        {

            FatigueModel fatigueStrain = new FatigueModel();
            IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetFatigueMaterialConditionsFromService(sessionId, sourceMaterialId, 1, type);
            fatigueStrain.MaterialConditions = materialConditions.Select(m => new System.Web.Mvc.SelectListItem() { Value = m.ConditionId, Text = m.Description }).ToList();
            if (fatigueStrain.MaterialConditions.Count > 0)
            {
                IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, fatigueStrain.MaterialConditions[0].Value, 1, type);
                if (tempConds.Count > 0)
                {
                    foreach (var item in tempConds)
                    {
                        System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                        fatigueStrain.ConditionList.Add(tempListItem);
                    }
                    string no = fatigueStrain.ConditionList[0].Value;
                    fatigueStrain.Condition = new FatigueCondition();
                    fatigueStrain.Condition.UnitType = type;
                    fatigueStrain.Condition.Condition = tempConds[0];                    
                    fatigueStrain.Condition.Type = FatigueType.StrainLife;                    
                }
            }
            return fatigueStrain;
        }
      
        //procedure for loading only Fatigue stress conditions for first material screen without seeing any details
        public FatigueModel FillFatigueStressOnlyConditions(int sourceMaterialId, string sessionId, IService service, int type = 1)
        {
            FatigueModel fatigueStress = new FatigueModel();           

            IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetFatigueMaterialConditionsFromService(sessionId, sourceMaterialId, 2, type);
            fatigueStress.MaterialConditions = materialConditions.Select(m => new System.Web.Mvc.SelectListItem() { Value = m.ConditionId, Text = m.Description }).ToList();

            if (fatigueStress.MaterialConditions.Count > 0)
            {
                  IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, fatigueStress.MaterialConditions[0].Value, 2, type);
            
                if (tempConds.Count > 0)
                {
                    foreach (var item in tempConds)
                    {
                        System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                        fatigueStress.ConditionList.Add(tempListItem);
                    }

                    string no = fatigueStress.ConditionList[0].Value;
                    fatigueStress.Condition = new FatigueCondition();
                    fatigueStress.Condition.Condition = tempConds[0];
                    fatigueStress.Condition.UnitType = type;                    
                    fatigueStress.Condition.Type = FatigueType.StressLife;                   
                    return fatigueStress;
                }
      
            }
            return fatigueStress;
        }
    }
}