using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.ConditionBasic
{
    public class ConditionTMMetalsBinder
    {
        public Condition FillCondition(int subgroupId, int sourceMaterialId, int sourceId,int groupId, int conditionId, IMaterialsContextUow context, int unitType =1)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            IService service = new TotalMateriaService();
            Condition condition = null;

            if (groupId == (int)(ElsevierMaterialsMVC.Models.MaterialDetails.ProductGroup.ProductGroupType.Mechanical))
            {
                condition = FillMechanicalConditionData(subgroupId, sourceMaterialId, conditionId, context, unitType, sessionId);

            }
            else if (groupId == (int)(ElsevierMaterialsMVC.Models.MaterialDetails.ProductGroup.ProductGroupType.Physical))
            {

                condition = FillPhysicalConditionData(sourceMaterialId, conditionId, context, unitType, sessionId);
            }
            else if (groupId == (int)(ElsevierMaterialsMVC.Models.MaterialDetails.ProductGroup.ProductGroupType.Chemical))
            {
                condition = service.GetChemicalCompositionFromService(sessionId, sourceMaterialId, subgroupId).Where(m => m.ConditionId == conditionId).OrderBy(m => m.ConditionName).FirstOrDefault();
            }

            return condition;
        }

        public Condition FillPhysicalConditionData(int sourceMaterialId, int conditionId, IMaterialsContextUow context, int unitType, string sessionId)
        {
            IService service = new TotalMateriaService();
            Condition condition = new Condition();
            condition.Properties = new List<Property>();
            Condition physicalCondition = service.GetPhysicalPropertiesFromService(sessionId, sourceMaterialId).Where(m => m.ConditionId == conditionId).FirstOrDefault();
            condition.Properties = new ConditionTMMetalsBinder().FillPhysicalConditionData(context, unitType, physicalCondition);
            condition.ConditionId = conditionId;
            for (int i = 0; i < condition.Properties.ToList().Count; i++)
            {
                condition.Properties[i].ValueId = i;
            }
            condition.ConditionName = physicalCondition.ConditionName;
            IList<string> selectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, physicalCondition.ConditionId, MaterialDetailType.PhysicalProperties);
            return condition;
        }

        public Condition FillMechanicalConditionData(int subgroupId, int sourceMaterialId, int conditionId, IMaterialsContextUow context, int unitType, string sessionId)
        {
            IService service = new TotalMateriaService();
            Condition condition = new Condition();
            IList<Condition> mechanical = service.GetMechanicalRoomPropertiesFromService(sessionId, sourceMaterialId, subgroupId, unitType).OrderBy(m => m.ConditionName).ToList();
            IList<Condition> mechanicalHigh = service.GetMechanicalHighLowPropertiesFromService(sessionId, sourceMaterialId, subgroupId, Api.Models.Mechanical.MechanicalGroupEnum.High, unitType).OrderBy(m => m.ConditionName).ToList();
            IList<Condition> mechanicalLow = service.GetMechanicalHighLowPropertiesFromService(sessionId, sourceMaterialId, subgroupId, Api.Models.Mechanical.MechanicalGroupEnum.Low, unitType).OrderBy(m => m.ConditionName).ToList();

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

            if (conditionId == 0)
            {
                conditionId = mechanical.Select(c=>c.ConditionId).FirstOrDefault();
            }            
            condition = mechanical.Where(m => m.ConditionId == conditionId).FirstOrDefault();


            string unit = "°C";
            if (unitType == 2)
            {
                unit = "°F";
            }

            foreach (var pro in condition.Properties)
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
                         
            condition.Properties = condition.Properties.OrderBy(s => s.PropertyName).ThenBy(m => m.Temperature).ToList();

            int rowId = 0;
            foreach (var pro in condition.Properties)
            {
                pro.ValueId = rowId;
                rowId = rowId + 1;
            }
            return condition;
        }
        
        public IList<Property> FillPhysicalConditionData(IMaterialsContextUow context, int type, Condition physicalCondition)
        {

            IList<PropertyConversionFactorAndUnit> propertiesWithConversionFacorsAndUnits = context.PropertiesWithConversionFactorsAndUnits.GetPropertiesWithConversionFactorsAndUnits();
            IList<Property> physicalProperties = new List<Property>();
            physicalProperties = physicalCondition.Properties.OrderBy(s => s.PropertyName).ThenBy(m => m.Temperature).ToList();

            int rowId = 0;
            foreach (var physicalProperty in physicalProperties)
            {
                var st = context.EquivalentProperties.AllAsNoTracking.Where(s => s.SourcePropertyId == physicalProperty.SourcePropertyId && s.SourceId == 2).FirstOrDefault();
                if (st != null)
                {

                    physicalProperty.PropertyId = st.PropertyId;
                    physicalProperty.PropertyName = st.Name;

                    if (type == 2)
                    {
                        PropertyConversionFactorAndUnit propConversionData = propertiesWithConversionFacorsAndUnits.Where(m => m.PropertyId == physicalProperty.PropertyId).FirstOrDefault();


                        if (physicalProperty.OrigValue.Contains("-"))
                        {
                            IList<string> values = physicalProperty.OrigValue.Split('-');
                            if (values.Count > 0)
                            {
                                physicalProperty.OrigValue = values[0];
                            }
                        }
                        double value = 0;
                        if (double.TryParse(physicalProperty.OrigValue, out value))
                        {
                            if (propConversionData.Factor != null)
                            {
                                if (propConversionData.Offset == null)
                                {
                                    propConversionData.Offset = 0;
                                }

                                double valueTemp = (value - (double)propConversionData.Offset) / (double)propConversionData.Factor;

                                if (propConversionData.DecimalsUS != null)
                                {
                                    physicalProperty.OrigValue = Math.Round(valueTemp, (int)propConversionData.DecimalsUS).ToString();
                                }
                                else
                                {
                                    physicalProperty.OrigValue = valueTemp.ToString();
                                }

                                physicalProperty.OrigUnit = propConversionData.Unit;
                            }
                        }



                    }
                    string unit = "°C";
                  
                    physicalProperty.OrigValueText = !string.IsNullOrEmpty(physicalProperty.OrigValueText) ? physicalProperty.OrigValueText : "";
                    if (!string.IsNullOrEmpty(physicalProperty.AdditionalCondition) && physicalProperty.Temperature != null)
                    {
                        physicalProperty.OrigValueText += physicalProperty.AdditionalCondition + "; " + physicalProperty.Temperature.ToString() + unit;
                    }
                    if (!string.IsNullOrEmpty(physicalProperty.AdditionalCondition) && physicalProperty.Temperature == null)
                    {
                        physicalProperty.OrigValueText += physicalProperty.AdditionalCondition;
                    }
                    if (string.IsNullOrEmpty(physicalProperty.AdditionalCondition) && physicalProperty.Temperature != null)
                    {
                        physicalProperty.OrigValueText += physicalProperty.Temperature.ToString() + unit;
                    }
     
                    physicalProperty.ValueId = rowId;
                    rowId = rowId + 1;
                }
                else
                {
                    physicalProperty.ValueId = rowId;
                }
            }
            return physicalProperties;
        }

        public MaterialCondition GetStressStrainConditionData(int sourceMaterialId, int conditionId, IMaterialsContextUow context, int unitType, string sessionId)
        {
            IService service = new TotalMateriaService();
            Condition condition = new Condition();
            IList<MaterialCondition> materialConditions = service.GetStressStrainMaterialConditionsFromService(sessionId, sourceMaterialId, 1);


            return null;
        }


    }
}