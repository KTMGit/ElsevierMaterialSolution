using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterialsMVC.BL.Helpers;

namespace ElsevierMaterialsMVC.BL.Binders.ConditionBasic
{
    public class ConditionElsBinder
    {
        public IList<ConditionModel> GetGroupMaterialConditions(int materialId, int subgroupId, int groupId, IMaterialsContextUow context)
        {
            IList<ConditionModel> conditionsList = new List<ConditionModel>();
            IList<GroupMaterialCondition> conditions = context.GroupMaterialConditions.AllAsNoTracking.Where(n => n.MaterialID == materialId && n.SubgroupID == subgroupId && n.GroupId == groupId).ToList();

            foreach (var record in conditions)
            {
                ConditionModel materialCondition = new ConditionModel();

                materialCondition.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
                materialCondition.Properties = new List<ElsevierMaterials.Models.Property>();
                materialCondition.ConditionId = record.ConditionId;
                materialCondition.ProductFormId = record.ProductFormId;
                materialCondition.ConditionIdProductFormId = record.ConditionId + ";" + record.ProductFormId;
                materialCondition.ConditionName = "";
                materialCondition.MaterialDescription = record.MaterialDescription;
                materialCondition.Thickness = record.Thickness;

                if (record.Condition != null && record.Condition != "") { 
                    materialCondition.ConditionName = record.Condition;
                }
                
                if (record.ProductForm != null && record.ProductForm != "") { 

                    materialCondition.ConditionName = materialCondition.ConditionName + ((materialCondition.ConditionName != "") ? "; " + record.ProductForm : record.ProductForm);
                }

                if (record.Thickness != null && record.Thickness != "")
                {
                    materialCondition.ConditionName = materialCondition.ConditionName + ((materialCondition.ConditionName != "") ? "; " + record.Thickness : record.Thickness);
                }

                if (record.MaterialDescription != null && record.MaterialDescription != "")
                {
                    materialCondition.ConditionName = materialCondition.ConditionName + ((materialCondition.ConditionName != "") ? "; " + record.MaterialDescription : record.MaterialDescription);
                }

                if (record.Phase != null && record.Phase != "")
                {
                    materialCondition.ConditionName = materialCondition.ConditionName + ((materialCondition.ConditionName != "") ? "; " + record.Phase : record.Phase);
                }


                if (materialCondition.ConditionName == null || materialCondition.ConditionName == "")
                {
                    materialCondition.ConditionName = "All";
                }

                materialCondition.ConditionName = materialCondition.ConditionName.TrimStart(';').TrimStart();

                conditionsList.Add(materialCondition);
            }

            conditionsList = conditionsList.OrderBy(c => c.ConditionName).ToList();
            return conditionsList;
        }

        public IList<ConditionModel> GetGroupTestConditions(int materialId, int subgroupId, int groupId, int conditionId, int productFormId, string materialDescription, string thickness, IMaterialsContextUow context)
        {
            IList<ConditionModel> conditionsList = new List<ConditionModel>();
            var conditions = context.GroupTestConditions.AllAsNoTracking.Where(n => n.MaterialID == materialId && n.SubgroupID == subgroupId && n.GroupId == groupId && n.ConditionId == conditionId && n.ProductFormId == productFormId);


            IList<GroupTestCondition> testConditions = conditions.Where(m => m.MaterialDesc.Trim() == materialDescription.Trim() && m.Thickness.Trim() == thickness.Trim()).ToList();

            foreach (var record in testConditions)
            {
                ConditionModel cm = GetDataForSlectedTestCondition(materialId, subgroupId, groupId, record.RowID, context);
                conditionsList.Add(cm);
            }
            conditionsList = conditionsList.OrderBy(c => c.ConditionName).ToList();
            return conditionsList;
        }

        public ConditionModel GetDataForSlectedTestCondition(int materialId, int subgroupId, int groupId, int rowId, IMaterialsContextUow context)
        {
            GroupTestCondition slectedTestCondition = context.GroupTestConditions.AllAsNoTracking.Where(n => n.MaterialID == materialId && n.SubgroupID == subgroupId && n.GroupId == groupId && n.RowID == rowId).FirstOrDefault();

            ConditionModel conditionModel = new ConditionModel();
            conditionModel.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
            conditionModel.Properties = new List<ElsevierMaterials.Models.Property>();
            conditionModel.RowId = rowId;

            conditionModel.ConditionName = "";

            if (slectedTestCondition.Temperature != null && slectedTestCondition.Temperature != "") {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; " + slectedTestCondition.Temperature : slectedTestCondition.Temperature);
            }

            if (slectedTestCondition.Basis != null && slectedTestCondition.Basis != "") { 
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Basis: " + slectedTestCondition.Basis : "Basis: " + slectedTestCondition.Basis); 
            }


            if (slectedTestCondition.TestType != null && slectedTestCondition.TestType != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; " + slectedTestCondition.TestType : slectedTestCondition.TestType);
            }

            if (slectedTestCondition.StressRatio != null && slectedTestCondition.StressRatio != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Stress ratio: " + slectedTestCondition.StressRatio : "Stress ratio: " + slectedTestCondition.StressRatio);
            }

            if (slectedTestCondition.SpecimenOrientation != null && slectedTestCondition.SpecimenOrientation != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Specimen orientation: " + slectedTestCondition.SpecimenOrientation : "Specimen orientation: " + slectedTestCondition.SpecimenOrientation);
            }

            if (slectedTestCondition.HoldingTemperature != null && slectedTestCondition.HoldingTemperature != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Holding temperature: " + slectedTestCondition.HoldingTemperature : "Holding temperature: " + slectedTestCondition.HoldingTemperature);
            }

            if (slectedTestCondition.HoldingTime != null && slectedTestCondition.HoldingTime != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Holding time: " + slectedTestCondition.HoldingTime : "Holding time: " + slectedTestCondition.HoldingTime);
            }

            if (slectedTestCondition.SpecimenType != null && slectedTestCondition.SpecimenType != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; " + slectedTestCondition.SpecimenType : slectedTestCondition.SpecimenType);
            }

            if (slectedTestCondition.Comment != null && slectedTestCondition.Comment != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; " + slectedTestCondition.Comment : slectedTestCondition.Comment);
            }


            if (slectedTestCondition.TSCF != null && slectedTestCondition.TSCF != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; Theoretical stress concentration factor: " + slectedTestCondition.TSCF : "Theoretical stress concentration factor: " + slectedTestCondition.TSCF);
            }


            if (slectedTestCondition.Pressure != null && slectedTestCondition.Pressure != "")
            {
                conditionModel.ConditionName = conditionModel.ConditionName + ((conditionModel.ConditionName != "") ? "; " + slectedTestCondition.Pressure : slectedTestCondition.Pressure);
            }


            if (conditionModel.ConditionName == null || conditionModel.ConditionName == "")
            {  
                conditionModel.ConditionName = "All";
            }

            conditionModel.ConditionName = conditionModel.ConditionName.TrimStart(';').TrimStart();

            IList<PropertyWithConvertedValues> propertiesPerCond = context.PropertiesWithConvertedValues.All.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.GroupId == slectedTestCondition.GroupId && m.RowId == slectedTestCondition.RowID).ToList();

            foreach (var prop in propertiesPerCond)
            {
                ElsevierMaterials.Models.Property propertyObj = new PropertyElsBinder().GetPropertyDataNew(materialId, subgroupId, slectedTestCondition, prop);

                conditionModel.Properties.Add(propertyObj);
            }

            conditionModel.Properties = conditionModel.Properties.OrderBy(m => m.PropertyId).ToList();
            return conditionModel;
        }

        public ConditionModel FillElsCondition(int materialId, int subgroupId, IMaterialsContextUow context, RecordLink conditionObj, MaterialProperty prop, int type)
        {
            ConditionModel condition = new ConditionModel();
            condition.ConditionId = (int)conditionObj.RowID;
            condition.Properties = new List<Property>();
            condition.ConditionName = FillElsConditionName(conditionObj);
            string unitName = prop.OrigUnit == null ? "" : context.Units.Find(n => n.UnitId == prop.OrigUnit).UnitText;
            ElsevierMaterials.Models.Property property = context.PropertiesWithConvertedValues.GetPropertyInfoForMaterialForSelectedMetric(context.PreferredNames.Find(o => o.PN_ID == prop.PropertyId).PN, materialId, subgroupId, conditionObj.RowID, prop.PropertyId, prop.ValueId, type, unitName);
            condition.Properties.Add(property);
            return condition;
        }

        public string FillElsConditionName(RecordLink conditionObj)
        {
            string name = "";
            if (conditionObj.Condition != null && conditionObj.Condition != "")
            {
                name = conditionObj.Condition;
            }

            if (conditionObj.ProductForm != null && conditionObj.ProductForm != "")
            {
                name = name + ((name != "") ? "; " + conditionObj.ProductForm : conditionObj.ProductForm);
            }

            if (conditionObj.Thickness != null && conditionObj.Thickness != "")
            {
                name = name + ((name != "") ? "; " + conditionObj.Thickness : conditionObj.Thickness);
            }

            if (conditionObj.Temperature != null && conditionObj.Temperature != "")
            {
                name = name + ((name != "") ? "; " + conditionObj.Temperature : conditionObj.Temperature);
            }
            if (name == null)
            {
                name = "As received";
            }

            return name;
        }

        public IList<ChemicalPropertyModel> GetChemicalPropertiesData(int materialId, int subgroupId, int groupId,IMaterialsContextUow context)
        {
            IList<ChemicalPropertyModel> model = new List<ChemicalPropertyModel>();
            var prop = context.PropertiesWithConvertedValues.AllAsNoTracking.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId).Select(m=>new {m.GroupId,m.PropertyId,m.ConvValue, m.ConvValueMax, m.ConvValueMin, m.DefaultUnit, m.DefaultUnitId, m.OriginalUnit, m.OrigUnitId, m.OrigValue, m.OrigValueMax, m.OrigValueMin, m.UsUnit, m.Temperature, m.UsValue, m.UsValueMax, m.UsValueMin, m.ValueId, m.RowId});
            ChemicalPropertyModel item=new ChemicalPropertyModel();           
            IList<GroupChemicalPropertyAll> groupChemicalProperties = context.GroupChemicalPropertyAll.AllAsNoTracking.Where(m => m.MaterialID == materialId && m.SubgroupID == subgroupId && m.GroupId==groupId).OrderBy(m=>m.PropertyID).ThenBy(m=>m.Phase).ThenBy(m=>m.X_unit_lbl).ThenBy(m=>m.Condition).ToList();
            IList<string> temperatureReplace = new List<string>();
            //IList<int> propIds=groupChemicalProperties.Select(p=>p.PropertyID).Distinct().ToList();
            //IList<string> phasesList = groupChemicalProperties.Select(p => p.Phase).Distinct().ToList();
         
            //var propNames= context.PreferredNames.AllAsNoTracking.Where(p=>propIds.Contains(p.PN_ID) ).Select(p=>new {Key=p.PN_ID, Value=p.PN});
            var condModel=new ChemicalConditionModel();
            string prevPhase = "";
            string prevPropName = "";

            foreach (GroupChemicalPropertyAll itemProp in groupChemicalProperties)
            {   
      
               var propData = prop.Where(p => p.PropertyId == itemProp.PropertyID && p.RowId == itemProp.RowID && p.GroupId == itemProp.GroupId && p.ValueId == itemProp.ValueID).FirstOrDefault();              
                   if ((prevPhase == "" || prevPropName == "") || (prevPhase != itemProp.Phase || prevPropName != itemProp.PropertyName))
                   {
                       item = new ChemicalPropertyModel();
                       item.GroupId = itemProp.GroupId;
                       item.PropertyId = itemProp.PropertyID;
                       //item.PropertyName = propNames.Where(p => p.Key == itemProp.PropertyID).Select(p => p.Value).FirstOrDefault();
                       item.Phase = itemProp.Phase;
                       item.PhaseId = itemProp.PhaseId;
                       item.PropertyName = itemProp.PropertyName;
                       item.PNameAndPhase = itemProp.PropertyName + ", " + itemProp.Phase;
                       if (item.PNameAndPhase.Trim().EndsWith(","))
                       {
                           item.PNameAndPhase=ElsevierMaterialsMVC.BL.Helpers.TransformFunctions.RemoveLast(item.PNameAndPhase.Trim(),",");
                       }
                       prevPhase = itemProp.Phase;
                       prevPropName = itemProp.PropertyName;
                       item.ConditionList = new List<ChemicalConditionModel>();
                       temperatureReplace = new List<string>();
                       item.DiagramID = itemProp.DiagramID;

                       condModel = SetChemicalConditionModel(itemProp, temperatureReplace, context);

                       if (itemProp.DiagramID!=null)
                       {
                           condModel.PropertyUnits = itemProp.Y_unit_lbl;
                           condModel.USPropertyUnits = itemProp.Y_unit_lbl;
                           condModel.DefaultPropertyUnits = itemProp.Y_unit_lbl;
                           condModel.VariableUnits = itemProp.X_unit_lbl;
                           condModel.DefaultVariableUnits = itemProp.X_unit_lbl;
                           condModel.USVariableUnits = itemProp.X_unit_lbl;
                           condModel.Notes = itemProp.cit_record_id != null ? "note" : "";
                           condModel.cit_record_id = itemProp.cit_record_id;

                               if (condModel.TemperatureList == null)
                               {
                                   condModel.TemperatureList = new List<ChemicalConditionPointModel>();
                               }

                               var diagPoints = context.DiagramPoints.AllAsNoTracking.Where(p => p.DiagramID == itemProp.DiagramID).OrderBy(p=>p.PointOrder);

                               foreach (var point in diagPoints)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Origin });
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Default });
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Us });
                               }
                               
                           

                               condModel.PropertyRange = itemProp.Y_range;
                               condModel.VariableRange = itemProp.X_range;  
                               //condModel.VariableUnits = "";

                               if (condModel.TemperatureList != null && condModel.TemperatureList.Count > 1)
                               {
                                   condModel.HasMultipleTemperatures = true;

                                   foreach (string s in temperatureReplace)
                                   {
                                       condModel.Name = condModel.Name.Replace(s, "");
                                   }
                               }
                       }
                       else
                       {                      
                           if (propData.OriginalUnit != null)
                           {
                               condModel.PropertyUnits = propData.OriginalUnit;
                           }
                           if (propData.UsUnit != null)
                           {
                               condModel.USPropertyUnits = propData.UsUnit;
                           }
                           if (propData.DefaultUnit != null)
                           {
                               condModel.DefaultPropertyUnits = propData.DefaultUnit;
                           }
                           if (itemProp.Temperature != null)
                           {
                               if (condModel.TemperatureList == null)
                               {
                                   condModel.TemperatureList = new List<ChemicalConditionPointModel>();
                               }
                               if (propData.OrigValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.OrigValue, type = ChemicalUnitValuesType.Origin });
                               }
                               if (propData.ConvValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.ConvValue, type = ChemicalUnitValuesType.Default });
                               }
                               if (propData.UsValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.UsValue, type = ChemicalUnitValuesType.Us });
                               }
                           }

                           condModel.PropertyRange = propData.OrigValue == null ? "" : ((double)(propData.OrigValue)).ToString("0.####");
                           condModel.DefaultPropertyRange = propData.ConvValue == null ? "" : ((double)propData.ConvValue).ToString("0.####");
                           condModel.USPropertyRange = propData.UsValue == null ? "" : ((double)propData.UsValue).ToString("0.####");
                           condModel.DefaultVariableUnits = "°C";

                           condModel.VariableUnits = "";
                           condModel.Notes = "";
                       }  
                       item.ConditionList.Add(condModel);
                       model.Add(item);
                   }
                   else
                   {
                       ChemicalPropertyModel property = model.Where(m => m.PropertyId == itemProp.PropertyID && m.Phase == itemProp.Phase).FirstOrDefault();

                       if (!property.ConditionList.Where(c => c.RowId == itemProp.RowID).Any())
                       {
                           condModel = SetChemicalConditionModel(itemProp, temperatureReplace, context);
                           property.ConditionList.Add(condModel);
                       }

                       var condModelPrev = new ChemicalConditionModel();
                       var condModelNew = new ChemicalConditionModel();
                       var newCondition = false;

                       condModelPrev = property.ConditionList.Where(c => c.RowId == itemProp.RowID).FirstOrDefault();
                       condModelNew = SetChemicalConditionModel(itemProp, temperatureReplace, context);
                       if (condModelPrev.Name != condModelNew.Name)
                       {
                           newCondition = true;
                           condModel = condModelNew;
                       }
                       else
                       {
                           condModel = condModelPrev;
                       }

                       if (itemProp.DiagramID != null)
                       {
                           condModel.PropertyUnits = itemProp.Y_unit_lbl;
                           condModel.USPropertyUnits = itemProp.Y_unit_lbl;
                           condModel.DefaultPropertyUnits = itemProp.Y_unit_lbl;
                           condModel.VariableUnits = itemProp.X_unit_lbl;
                           condModel.DefaultVariableUnits = itemProp.X_unit_lbl;
                           condModel.USVariableUnits = itemProp.X_unit_lbl;
                           condModel.Notes = itemProp.cit_record_id != null ? "note" : "";
                           condModel.cit_record_id = itemProp.cit_record_id;

                           if (condModel.TemperatureList == null)
                           {
                               condModel.TemperatureList = new List<ChemicalConditionPointModel>();
                           }

                           var diagPoints = context.DiagramPoints.AllAsNoTracking.Where(p => p.DiagramID == itemProp.DiagramID).OrderBy(p => p.PointOrder);

                           foreach (var point in diagPoints)
                           {
                               condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Origin });
                               condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Default });
                               condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)point.X_value, Y = (double)point.Y_value, type = ChemicalUnitValuesType.Us });
                           }                           

                           condModel.PropertyRange = itemProp.Y_range;
                           condModel.VariableRange = itemProp.X_range;  

                           if (condModel.TemperatureList != null && condModel.TemperatureList.Count > 1)
                           {
                               condModel.HasMultipleTemperatures = true;

                               foreach (string s in temperatureReplace)
                               {
                                   condModel.Name = condModel.Name.Replace(s, "");
                               }
                           }
                       }
                       else
                       {
                           if (propData.OriginalUnit != null)
                           {
                               condModel.PropertyUnits = propData.OriginalUnit;
                           }
                           if (propData.UsUnit != null)
                           {
                               condModel.USPropertyUnits = propData.UsUnit;
                           }
                           if (propData.DefaultUnit != null)
                           {
                               condModel.DefaultPropertyUnits = propData.DefaultUnit;
                           }
                           if (itemProp.Temperature != null)
                           {
                               if (condModel.TemperatureList == null)
                               {
                                   condModel.TemperatureList = new List<ChemicalConditionPointModel>();
                               }
                               if (propData.OrigValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.OrigValue, type = ChemicalUnitValuesType.Origin });
                               }
                               if (propData.ConvValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.ConvValue, type = ChemicalUnitValuesType.Default });
                               }
                               if (propData.UsValue != null)
                               {
                                   condModel.TemperatureList.Add(new ChemicalConditionPointModel() { X = (double)itemProp.Temperature, Y = (double)propData.UsValue, type = ChemicalUnitValuesType.Us });
                               }
                           }

                           condModel.PropertyRange = propData.OrigValue == null ? "" : ((double)(propData.OrigValue)).ToString("0.####");
                           condModel.DefaultPropertyRange = propData.ConvValue == null ? "" : ((double)propData.ConvValue).ToString("0.####");
                           condModel.USPropertyRange = propData.UsValue == null ? "" : ((double)propData.UsValue).ToString("0.####");
                           condModel.DefaultVariableUnits = "°C";

                           condModel.VariableUnits = "";
                           condModel.Notes = "";

                           if (condModel.TemperatureList != null && condModel.TemperatureList.Count > 1)
                           {
                               condModel.HasMultipleTemperatures = true;

                               foreach (string s in temperatureReplace)
                               {
                                   condModel.Name = condModel.Name.Replace(s, "");
                               }
                           }
                       }
                       if (newCondition)
                       {
                           item.ConditionList.Add(condModel);
                       }                       
                   }               
           }
           double propMin = double.MaxValue;
           double propMax =double.MinValue;
           double valMin =double.MaxValue;
           double valMax =double.MinValue;
           for(int i=0; i<model.Count; i++)
           {
               for(int j=0; j<model[i].ConditionList.Count; j++)
               {
                   if (model[i].ConditionList[j].Name == "")
                   {
                       model[i].ConditionList[j].Name = "As received";
                   }
                   if (model[i].ConditionList[j].TemperatureList != null && (model[i].ConditionList[j].TemperatureList.Where(p=>p.type==ChemicalUnitValuesType.Origin).Count()) > 1)
                   {
                       model[i].ConditionList[j].TemperatureList = model[i].ConditionList[j].TemperatureList.OrderBy(p=>p.type).ThenBy(p => p.X).ThenBy(P => P.Y).ToList();
                       foreach (var k in model[i].ConditionList[j].TemperatureList.Where(p => p.type == ChemicalUnitValuesType.Origin))
                       {

                           if (valMin > k.X)
                           {
                               valMin = k.X;
                           }
                           if (valMax < k.X)
                           {
                               valMax = k.X;
                           }
                           if (propMin > k.Y)
                           {
                               propMin = k.Y;
                           }
                           if (propMax < k.Y)
                           {
                               propMax = k.Y;
                           }
                       }
                       if (model[i].DiagramID==null)
                       {
                           model[i].ConditionList[j].VariableRange = valMin.ToString("0.####") + " &mdash; " + valMax.ToString("0.####");                      
                           model[i].ConditionList[j].PropertyRange = propMin.ToString("0.####") + " &mdash; " + propMax.ToString("0.####");
                       }
                       model[i].ConditionList[j].VariableUnits = model[i].ConditionList[j].VariableUnits!=""?model[i].ConditionList[j].VariableUnits:"°C";
                       model[i].ConditionList[j].PropertyUnits = model[i].ConditionList[j].PropertyUnits;

                       propMin = double.MaxValue;
                       propMax = double.MinValue;
                       foreach (var k in model[i].ConditionList[j].TemperatureList.Where(p => p.type == ChemicalUnitValuesType.Default))
                       {
                           if (propMin > k.Y)
                           {
                               propMin = k.Y;
                           }
                           if (propMax < k.Y)
                           {
                               propMax = k.Y;
                           }
                       }
                       
                       model[i].ConditionList[j].DefaultPropertyRange = propMin.ToString("0.####") + " &mdash; " + propMax.ToString("0.####");                     
                       propMin = double.MaxValue;
                       propMax = double.MinValue;
                       foreach (var k in model[i].ConditionList[j].TemperatureList.Where(p => p.type == ChemicalUnitValuesType.Us))
                       {
                           if (propMin > k.Y)
                           {
                               propMin = k.Y;
                           }
                           if (propMax < k.Y)
                           {
                               propMax = k.Y;
                           }
                       }

                       model[i].ConditionList[j].USPropertyRange = propMin.ToString("0.####") + " &mdash; " + propMax.ToString("0.####");
                   }
               }
           }
            return model;
        }

        private ChemicalConditionModel SetChemicalConditionModel(GroupChemicalPropertyAll itemProp, IList<string> temperatureReplace, IMaterialsContextUow context)
        {
            ChemicalConditionModel condModel = new ChemicalConditionModel();
            condModel.RowId = itemProp.RowID;
            condModel.ValueId = itemProp.ValueID;
            condModel.HasMultipleTemperatures = false;
            condModel.Name = "";
            if (itemProp.Condition != null && itemProp.Condition != "")
            {
                condModel.Name = ElsevierMaterialsMVC.BL.Helpers.TransformFunctions.RemoveLast(itemProp.Condition,";");
            }  
            return condModel;
        }

       
    }
}