using Api.Models.Plus;
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
    public class ConditionTMPlusBinder
    {
        public Condition FillConditionData(int subgroupId, int sourceMaterialId, int sourceId, int groupId, int conditionId, IMaterialsContextUow context)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            IService service = new TotalMateriaService();
            IPlusService servicePLUS = new TMPlusService();
            Condition condition = null;


            if (groupId == 806)
            {
                PropertiesContainer mechanical = servicePLUS.GetMechanicalPLUSPropertiesFromService(sessionId, sourceMaterialId, subgroupId);
                IList<TabConditionAndData> mechanicalCondition = mechanical.Model.Where(m => m.ConditionId == conditionId).ToList();
                condition = new Condition();
                condition.Properties = new List<Property>();
                foreach (var item in mechanicalCondition)
                {
                    condition.ConditionName = item.Condition;
                    condition.ConditionId = item.ConditionId;
                    foreach (var prop in item.DataForCondition)
                    {
                        foreach (var row in prop.Rows)
                        {
                            string tempString = "";

                            if (!string.IsNullOrEmpty(row.Item3))
                            {
                                if (tempString != "")
                                {
                                    tempString += "; " + row.Item3;
                                }
                                else
                                {
                                    tempString += row.Item3;
                                }
                            }

                            if (tempString != "")
                            {
                                tempString += "; " + row.Item2;
                            }
                            else
                            {
                                tempString += row.Item2;
                            }
                            condition.Properties.Add(new Property() { SourcePropertyId = prop.PropertyId, PropertyName = prop.Header.ToLower(), OrigUnit = prop.Unit.Replace("(", "").Replace(")", ""), OrigValue = row.Item1, OrigValueText = tempString });
                        }
                    }

                    condition.Properties = condition.Properties.OrderBy(s => s.PropertyName).ToList();

                    int count = 0;
                    foreach (var prop in condition.Properties)
                    {
                        var st = context.EquivalentProperties.AllAsNoTracking.Where(s => s.SourcePropertyId == prop.SourcePropertyId && s.SourceId == 3).FirstOrDefault();
                        if (st != null)
                        {
                            prop.PropertyId = st.PropertyId;
                            prop.SourcePropertyId = st.SourcePropertyId;
                            prop.PropertyName = st.Name;
                            prop.ValueId = count;
                            count = count + 1;
                        }
                        else
                        {
                            prop.ValueId = count;
                            count = count + 1;
                        }
                    }
                }

            }
            else
            {

                PropertiesContainer physical = servicePLUS.GetPhysicalPropertiesPLUSFromService(sessionId, sourceMaterialId);
                IList<TabConditionAndData> physicalCondition = physical.Model.Where(m => m.ConditionId == conditionId).ToList();

                condition = new Condition();
                condition.Properties = new List<Property>();
                foreach (var item in physicalCondition)
                {
                    condition.ConditionName = item.Condition;
                    condition.ConditionId = item.ConditionId;
                    foreach (var prop in item.DataForCondition)
                    {
                        foreach (var row in prop.Rows)
                        {
                            string tempString = "";

                            if (!string.IsNullOrEmpty(row.Item3))
                            {
                                if (tempString != "")
                                {
                                    tempString += "; " + row.Item3;
                                }
                                else
                                {
                                    tempString += row.Item3;
                                }
                            }

                            if (tempString != "")
                            {
                                tempString += "; " + row.Item2;
                            }
                            else
                            {
                                tempString += row.Item2;
                            }
                            condition.Properties.Add(new Property() { SourcePropertyId = prop.PropertyId, PropertyName = prop.Header.ToLower(), OrigUnit = prop.Unit.Replace("(", "").Replace(")", ""), OrigValue = row.Item1, OrigValueText = tempString });
                        }
                    }

                    condition.Properties = condition.Properties.OrderBy(s => s.PropertyName).ToList();

                    int count = 0;
                    foreach (var prop in condition.Properties)
                    {
                        var st = context.EquivalentProperties.AllAsNoTracking.Where(s => s.SourcePropertyId == prop.SourcePropertyId && s.SourceId == 3).FirstOrDefault();
                        if (st != null)
                        {
                            prop.PropertyId = st.PropertyId;
                            prop.SourcePropertyId = st.SourcePropertyId;
                            prop.PropertyName = st.Name;
                            prop.ValueId = count;
                            count = count + 1;
                        }
                        else
                        {
                            prop.ValueId = count;
                            count = count + 1;
                        }
                    }
                }
            }
            return condition;
        }

    }
}