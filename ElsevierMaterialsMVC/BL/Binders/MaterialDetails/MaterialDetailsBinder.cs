using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterialsMVC.BL.Binders.Subgroups;
using IniCore.Web.Mvc.Html;
using Api.Models;
using Api.Models.Plus;
using Api.Models.HeatTreatment;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;
using ElsevierMaterialsMVC.BL.Binders.Group;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterials.Models.Domain.CreepData;
using ElsevierMaterialsMVC.Models.MaterialDetails;

namespace ElsevierMaterialsMVC.BL.Binders.MaterialDetails
{
    public class MaterialDetailsBinder
    {

        public CreepDataModel GetCreepTestConditionsWithData(string sessionId, int sourceMaterialId, string condCreepId)
        {
            int type = GetSelectedUnitForTMMaterials();
            IService service = new TotalMateriaService();

            CreepDataModel model = new CreepDataModel();
            model.Conditions = service.GetCreepTestConditionsFromService(sessionId, sourceMaterialId, condCreepId);

          
            var condCreep = model.Conditions.Where(m => m.ConditionId == condCreepId.ToString()).FirstOrDefault();


            CreepDataContainer modelCreep = new CreepDataContainer();
            modelCreep.UnitType = type;
            modelCreep.Data = service.GetCreepDataFromService(sessionId, sourceMaterialId,int.Parse (model.Conditions[0].ConditionId), type);
            modelCreep.SelectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, int.Parse(model.Conditions[0].ConditionId), MaterialDetailType.CreepData);
            model.Data = modelCreep;
            return model;
        }

        public ElsevierMaterials.Models.Domain.CreepData.CreepDataContainer GetCreepData(string sessionId, int sourceMaterialId,string materialConditionId,  int condCreepId)
        {
            int type = GetSelectedUnitForTMMaterials();
            IService service = new TotalMateriaService();

            Api.Models.CreepData.CreepData model = service.GetCreepDataFromService(sessionId, sourceMaterialId, condCreepId, type);
            ElsevierMaterials.Models.Domain.CreepData.CreepDataContainer modelContainer = new ElsevierMaterials.Models.Domain.CreepData.CreepDataContainer();
            modelContainer.UnitType = type;
            modelContainer.Data = model;
            modelContainer.SelectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, condCreepId, MaterialDetailType.CreepData);
            return modelContainer;
        }

        public CreepPlusConditionModel GetCreepPlusModel(string sessionId, int sourceMaterialId, int condCreepId)
        {

            CreepPlusConditionModel condModel = new CreepPlusConditionModel();
            IPlusService plusService = new TMPlusService();
            bool IsIsochronous = plusService.GetCreepDiagramConditions(sessionId, sourceMaterialId).Where(n => n.ConditionId == condCreepId).FirstOrDefault().IsIsochronous;
            if (IsIsochronous)
            {
                condModel.ConditionTemperatures = plusService.GetCreepConditionTemperaturesIso(sessionId, sourceMaterialId, condCreepId);
                condModel.TimesIso = plusService.GetCreepTimesIso(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value).Select(h => new CreepPlusTimeStresses() { Value = h.Value, Text = h.Value.ToString() + "h", Unit = h.Unit }).ToList();
                condModel.Points = plusService.GetCreepStressPointsIso(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value, condModel.TimesIso[0].Unit, condModel.TimesIso[0].Value);
                condModel.Diagram = plusService.GetCreepDiagramIso(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value);
                condModel.SelectedConditionTemperature = condModel.ConditionTemperatures[0];
            }
            else
            {
                condModel.ConditionTemperatures = plusService.GetCreepConditionTemperatures(sessionId, sourceMaterialId, condCreepId);
                condModel.Stresses = plusService.GetCreepStresses(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value).Select(h => new CreepPlusTimeStresses() { Value = h.stress, Text = h.stress.ToString() + " MPa", Unit = "" }).ToList();
                condModel.Points = plusService.GetCreepStressPoints(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value, condModel.Stresses[0].Value);
                condModel.Diagram = plusService.GetCreepDiagram(sessionId, sourceMaterialId, condCreepId, condModel.ConditionTemperatures[0].Value);
                condModel.SelectedConditionTemperature = condModel.ConditionTemperatures[0];
            }
            return condModel;
        }

        public CreepPlusConditionModel GetCreepPlusTemperature(string sessionId, int sourceMaterialId, int condCreepId, short temperature)
        {

            CreepPlusConditionModel condModel = new CreepPlusConditionModel();
            IPlusService plusService = new TMPlusService();
            bool IsIsochronous = plusService.GetCreepDiagramConditions(sessionId, sourceMaterialId).Where(n => n.ConditionId == condCreepId).FirstOrDefault().IsIsochronous;
            if (IsIsochronous)
            {
                condModel.ConditionTemperatures = plusService.GetCreepConditionTemperaturesIso(sessionId, sourceMaterialId, condCreepId);
                condModel.SelectedConditionTemperature = condModel.ConditionTemperatures.FirstOrDefault(n => n.Value == temperature);
                condModel.TimesIso = plusService.GetCreepTimesIso(sessionId, sourceMaterialId, condCreepId, temperature).Select(h => new CreepPlusTimeStresses() { Value = h.Value, Text = h.Value.ToString() + "h", Unit = h.Unit }).ToList();
                condModel.Points = plusService.GetCreepStressPointsIso(sessionId, sourceMaterialId, condCreepId, temperature, condModel.TimesIso[0].Unit, condModel.TimesIso[0].Value);
                condModel.Diagram = plusService.GetCreepDiagramIso(sessionId, sourceMaterialId, condCreepId, temperature);
            }
            else
            {
                condModel.ConditionTemperatures = plusService.GetCreepConditionTemperatures(sessionId, sourceMaterialId, condCreepId);
                condModel.SelectedConditionTemperature = condModel.ConditionTemperatures.FirstOrDefault(n => n.Value == temperature);
                condModel.Stresses = plusService.GetCreepStresses(sessionId, sourceMaterialId, condCreepId, temperature).Select(h => new CreepPlusTimeStresses() { Value = h.stress, Text = h.stress.ToString() + " MPa", Unit = "" }).ToList();
                condModel.Points = plusService.GetCreepStressPoints(sessionId, sourceMaterialId, condCreepId, temperature, condModel.Stresses[0].Value);
                condModel.Diagram = plusService.GetCreepDiagram(sessionId, sourceMaterialId, condCreepId, temperature);
            }
            return condModel;
        }

        public IList<Api.Models.CreepDataPLUS.StressPointIso> GetCreepPlusPoints(string sessionId, int sourceMaterialId, int condCreepId, short temperature, double additional, int iso)
        {

            IList<Api.Models.CreepDataPLUS.StressPointIso> points = new List<Api.Models.CreepDataPLUS.StressPointIso>();
            IPlusService plusService = new TMPlusService();
            if (iso == 1)
            {
                Api.Models.CreepDataPLUS.Time time = plusService.GetCreepTimesIso(sessionId, sourceMaterialId, condCreepId, temperature).FirstOrDefault(b => b.Value == additional);
                points = plusService.GetCreepStressPointsIso(sessionId, sourceMaterialId, condCreepId, temperature, time.Unit, time.Value);
            }
            else
            {
                Api.Models.CreepDataPLUS.StressPoint stress = plusService.GetCreepStresses(sessionId, sourceMaterialId, condCreepId, temperature).FirstOrDefault(n => n.stress == additional);
                points = plusService.GetCreepStressPoints(sessionId, sourceMaterialId, condCreepId, temperature, stress.stress);
            }
            return points;
        }
        
        public FatigueModel GetFatigueStrainTestConditionsWithData(string sessionId, int sourceMaterialId, string no)
        {
            int type = GetSelectedUnitForTMMaterials();
            FatigueModel fatigueStrain = new FatigueModel();
            IService service = new TotalMateriaService();

            IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, no, 1, type);
            foreach (var item in tempConds)
            {
                System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                fatigueStrain.ConditionList.Add(tempListItem);
            }

            fatigueStrain.Condition = new FatigueCondition();
            fatigueStrain.Condition.UnitType = type;
            fatigueStrain.Condition.Condition = tempConds[0];
            fatigueStrain.Condition.Details = service.GetFatigueStrainLifeConditionDetailsFromService(sessionId, sourceMaterialId, fatigueStrain.ConditionList[0].Value, type);
            fatigueStrain.Condition.Diagram = service.GetFatigueStrainSNCurveDiagramFromService(sessionId, sourceMaterialId, fatigueStrain.ConditionList[0].Value, type);
            fatigueStrain.Condition.Points = service.GetFatigueStrainSNCurveDataFromService(sessionId, sourceMaterialId, fatigueStrain.ConditionList[0].Value, type);
            fatigueStrain.Condition.PointsForDiagram = service.GetFatigueStrainSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, fatigueStrain.ConditionList[0].Value, type);
            fatigueStrain.Condition.Type = FatigueType.StrainLife;
            fatigueStrain.Condition.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, fatigueStrain.ConditionList[0].Value, MaterialDetailType.FatigueData);

            return fatigueStrain;
        }

        public FatigueModel GetFatigueStressTestConditionsWithData(string sessionId, int sourceMaterialId, string no)
        {
            int type = GetSelectedUnitForTMMaterials();
            FatigueModel fatigueStress = new FatigueModel();
            IService service = new TotalMateriaService();

            IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, no, 2, type);
            foreach (var item in tempConds)
            {
                System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.ConditionId, Text = item.Description };
                fatigueStress.ConditionList.Add(tempListItem);
            }

            fatigueStress.Condition = new FatigueCondition();
            fatigueStress.Condition.UnitType = type;
            fatigueStress.Condition.Condition = tempConds[0];
            fatigueStress.Condition.Details = service.GetFatigueStressLifeConditionDetailsFromService(sessionId, sourceMaterialId, fatigueStress.ConditionList[0].Value, type);
            fatigueStress.Condition.Diagram = service.GetFatigueStressSNCurveDiagramFromService(sessionId, sourceMaterialId, fatigueStress.ConditionList[0].Value, type);
            fatigueStress.Condition.Points = service.GetFatigueStressSNCurveDataFromService(sessionId, sourceMaterialId, fatigueStress.ConditionList[0].Value, type);
            fatigueStress.Condition.PointsForDiagram = service.GetFatigueStressSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, fatigueStress.ConditionList[0].Value, type);
            fatigueStress.Condition.Type = FatigueType.StressLife;
            fatigueStress.Condition.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, fatigueStress.ConditionList[0].Value, MaterialDetailType.FatigueData);
            return fatigueStress;
        }

        public FatigueCondition GetFatigueStrainCondition(string sessionId, int sourceMaterialId, string materialConditionId, string no)
        {
            FatigueCondition model = new FatigueCondition();

            int type = GetSelectedUnitForTMMaterials();
            
            model.UnitType = GetSelectedUnitForTMMaterials();  
            IService service = new TotalMateriaService();
            Api.Models.TestCondition tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, materialConditionId, 1, type).Where(m => m.ConditionId == no).FirstOrDefault();
            model.Condition = tempConds;
            model.Details = service.GetFatigueStrainLifeConditionDetailsFromService(sessionId, sourceMaterialId, no, type);
            model.Diagram = service.GetFatigueStrainSNCurveDiagramFromService(sessionId, sourceMaterialId, no, type);
            model.Points = service.GetFatigueStrainSNCurveDataFromService(sessionId, sourceMaterialId, no, type);
            model.PointsForDiagram = service.GetFatigueStrainSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, no, type);
            model.Type = FatigueType.StrainLife;
            model.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, no, MaterialDetailType.FatigueData);

            return model;
        }

        public FatigueCondition GetFatigueStressCondition(string sessionId, int sourceMaterialId, string materialConditionId, string no)
        {
            FatigueCondition model = new FatigueCondition();

            int type = GetSelectedUnitForTMMaterials();

            IService service = new TotalMateriaService();

            Api.Models.Fatigue.FatigueCondition tempCond = new Api.Models.Fatigue.FatigueCondition();
        
         
            model.UnitType = type;
            Api.Models.TestCondition tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, materialConditionId, 2, type).Where(m => m.ConditionId == no).FirstOrDefault();

            model.Condition = tempConds;    

            model.Details = service.GetFatigueStressLifeConditionDetailsFromService(sessionId, sourceMaterialId, no, type);
            model.Diagram = service.GetFatigueStressSNCurveDiagramFromService(sessionId, sourceMaterialId, no, type);
            model.Points = service.GetFatigueStressSNCurveDataFromService(sessionId, sourceMaterialId, no, type);
            model.PointsForDiagram = service.GetFatigueStressSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, no, type);
            model.Type = FatigueType.StressLife;
            model.SelectedReferences = service.GetReferencesForSelectedConditionFatigueFromService(sessionId, sourceMaterialId, no, MaterialDetailType.FatigueData);
            return model;
        }
        
        public MaterialDetailsModel GetMaterial(int materialId, int subgroupId, int sourceId, int sourceMaterialId, string searchText, string sessionId, IMaterialsContextUow context, string tabId)
        {

            MaterialDetailsModel model = new MaterialDetailsModel();



            FullTextSearch fts = context.FullTextSearch.GetMaterialById(materialId);
            Classification cl = new Classification();
            bool isChemical = false;
            model.IsChemical = false;
            if (fts.material_type != null) cl.ClassificationNames.Add(ClassificationType.Type, fts.material_type);
            if (fts.material_group != null) cl.ClassificationNames.Add(ClassificationType.Group, fts.material_group);
            if (fts.material_class != null) cl.ClassificationNames.Add(ClassificationType.Class, fts.material_class);
            if (fts.material_subClass != null) cl.ClassificationNames.Add(ClassificationType.Subclass, fts.material_subClass);

            if (sourceId == 1)
            {
                model.Material = context.Materials.AllAsNoTracking.Where(s => s.MaterialId == materialId && s.SubgroupId == subgroupId && s.SourceMaterialId == sourceMaterialId).FirstOrDefault();
                model.Material.Classification = cl;
                //Check if material is chemical
                if (model.Material.DatabookId == 1187 || model.Material.DatabookId == 5083 || model.Material.DatabookId == 8403)
                {
                    model.IsChemical = true;
                    isChemical = true;
                }
                if (tabId == "divProperties")
                {
                    IDictionary<ProductGroup.ProductGroupType, ProductGroup> listOfGroups = _groupElsBinder.GetGroups(materialId, subgroupId, isChemical, context);
                    ProductGroup tempG1 = new ProductGroup();
                    IList<ProductGroup.ProductGroupType> list = tempG1.GetOrder();
                    foreach (var item in list)
                    {
                        if (listOfGroups.ContainsKey(item)) model.Properties.ProductGroups.Add(item, listOfGroups[item]);
                    }
                }

                if (tabId == "divEquivalency")
                {
                    model.Equivalency = BindEquivalencyEMS(materialId, context, sessionId);
                }

                model.Material.NumEquivalency = GetEquivalencyCountEMS(materialId, context, sessionId);

            }
            else
            {
                IService service = new TotalMateriaService();
                IPlusService servicePLUS = new TMPlusService();
                if (sourceId == 2)
                {
                    model.Material = service.GetMaterialSubgroupListFromService(sessionId, materialId, sourceMaterialId).Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId).FirstOrDefault();
                }
                else
                {
                    model.Material = servicePLUS.GetMaterialSubgroupPLUSListFromService(sessionId, materialId, sourceMaterialId).Where(m => m.SubgroupId == subgroupId).FirstOrDefault();

                }

                model.Material.Classification = cl;
                model.Material.Name = fts.material_designation;

                if (sourceId == 2)
                {

                    if (System.Configuration.ConfigurationManager.AppSettings["DeveloperSite"].ToLower() == "true")
                    {
                        int type = 1;

                        Api.Models.MaterialCounters oneSubgroupCounter = service.GetMetalsMaterialsCountersFromService(sessionId, sourceMaterialId).Where(r => r.MaterialId == model.Material.SourceMaterialId && r.KeyNum == model.Material.SubgroupId).FirstOrDefault();

                        model.MaterialCounters = new List<MaterialCountersModel>();

                        MaterialCountersModel item1 = new MaterialCountersModel();
                        item1.PropGroupType = ProductGroup.ProductGroupType.Mechanical;
                        item1.PropGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Mechanical).PN;
                        item1.Counter = (int)oneSubgroupCounter.NoMechanicalProperties;
                        model.MaterialCounters.Add(item1);
                        //ProductGroup propertyGroupMech = new BL.Binders.MaterialBasic.MaterialTMMetalsBinder().FillMechanicalGroupOnlyConditions(subgroupId, sourceMaterialId, sessionId, context, service, type);
                        //if (propertyGroupMech != null)
                        //{
                        //    propertyGroupMech.ConditionId = propertyGroupMech.Conditions.Select(r => r.ConditionId).FirstOrDefault();
                        //    model.Properties.ProductGroups.Add(ProductGroup.ProductGroupType.Mechanical, propertyGroupMech);
                        //}                      
                      
                        
                        MaterialCountersModel item2 = new MaterialCountersModel();
                        item2.PropGroupType = ProductGroup.ProductGroupType.Physical;
                        item2.PropGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Physical).PN;
                        item2.Counter = (int)oneSubgroupCounter.NoPhysicalTotal;
                        model.MaterialCounters.Add(item2);
                        //ProductGroup propertyGroupPhys = new BL.Binders.MaterialBasic.MaterialTMMetalsBinder().FillPhysicalGroupOnlyConditions(subgroupId, sourceMaterialId, sessionId, context, service, type);
                        //if (propertyGroupPhys != null)
                        //{
                        //    propertyGroupPhys.ConditionId = propertyGroupPhys.Conditions.Select(r => r.ConditionId).FirstOrDefault();
                        //    model.Properties.ProductGroups.Add(ProductGroup.ProductGroupType.Physical, propertyGroupPhys);
                        //}      
                        
                        MaterialCountersModel item3 = new MaterialCountersModel();
                        item3.PropGroupType = ProductGroup.ProductGroupType.StressStrain;
                        item3.PropGroupName = "stress strain";
                        item3.Counter = (int)oneSubgroupCounter.NoStressStrain;
                        model.MaterialCounters.Add(item3);
                        //IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetStressStrainMaterialConditionsFromService(sessionId, sourceMaterialId, type);
                        //if (materialConditions.Count > 0)      
                        //{
                        //    StressStrainModel ssModel=new StressStrainModel();                            
                        //    ssModel = service.GetStressStrainOnlyTestConditionsFromService(sessionId, sourceMaterialId, materialConditions[0].ConditionId, type);
                        //    ssModel.MaterialConditions = materialConditions;
                        //    model.Properties.StressStrain = ssModel;
                        //}
                        //model.Properties.StressStrain.MaterialConditions = new List<ElsevierMaterials.Models.MaterialCondition>();   

                        MaterialCountersModel item4 = new MaterialCountersModel();
                        item4.PropGroupType = ProductGroup.ProductGroupType.CreepData;
                        item4.PropGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.CreepData).PN;
                        item4.Counter = (int)oneSubgroupCounter.NoCreep;
                        model.MaterialCounters.Add(item4);
                        //model.Properties.CreepData.MaterialConditions = service.GetCreepMaterialConditionsFromService(sessionId, sourceMaterialId);
                        //if (model.Properties.CreepData.MaterialConditions.Count > 0)
                        //{
                        //    model.Properties.CreepData.Conditions = service.GetCreepTestConditionsFromService(sessionId, sourceMaterialId, model.Properties.CreepData.MaterialConditions[0].ConditionId);

                        //    var condCreep = model.Properties.CreepData.Conditions.FirstOrDefault();
                        //    int condCreepId = -1;
                        //    if (condCreep != null)
                        //    {
                        //        condCreepId = int.Parse(condCreep.ConditionId);
                        //    }

                        //    CreepDataContainer modelCreep = new CreepDataContainer();
                        //    modelCreep.UnitType = type;                           
                        //    model.Properties.CreepData.Data = modelCreep;
                        //}
                        //model.Properties.CreepData.MaterialConditions = new List<ElsevierMaterials.Models.MaterialCondition>();   



                        MaterialCountersModel item5 = new MaterialCountersModel();
                        item5.PropGroupType = ProductGroup.ProductGroupType.FatigueData;
                        item5.PropGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.FatigueData).PN;
                        item5.Counter = (int)oneSubgroupCounter.NoFatigue;
                        model.MaterialCounters.Add(item5);
                        //model.Properties.FatigueStrain = _materialTMMetalsBinder.FillFatigueStrainOnlyConditions(sourceMaterialId, sessionId, service, type);
                        //model.Properties.FatigueStress = _materialTMMetalsBinder.FillFatigueStressOnlyConditions(sourceMaterialId, sessionId, service, type);
                        //model.Properties.FatigueStrain.MaterialConditions = new List<System.Web.Mvc.SelectListItem>();
                        //model.Properties.FatigueStress.MaterialConditions = new List<System.Web.Mvc.SelectListItem>();   


                        MaterialCountersModel item6 = new MaterialCountersModel();
                        item6.PropGroupType = ProductGroup.ProductGroupType.Chemical;
                        item6.PropGroupName = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Chemical).PN;
                        item6.Counter = (int)oneSubgroupCounter.NoChemicalComposition;
                        model.MaterialCounters.Add(item6);
                        //ProductGroup propertyGroupChem = new BL.Binders.MaterialBasic.MaterialTMMetalsBinder().FillChemicalGroupOnlyConditions(subgroupId, sourceMaterialId, sessionId, context, service, type);
                        //if (propertyGroupChem != null)
                        //{
                        //    propertyGroupChem.ConditionId = propertyGroupChem.Conditions.Select(r => r.ConditionId).FirstOrDefault();
                        //    model.Properties.ProductGroups.Add(ProductGroup.ProductGroupType.Chemical, propertyGroupChem);
                        //}          

                       
                    }
                    else
                    {
                        int type = 1;

                        model.Properties.ProductGroups = _materialTMMetalsBinder.FillPhysicalMechanicalChemicalGroups(subgroupId, sourceMaterialId, sessionId, context, tabId, service, type);

                        IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetStressStrainMaterialConditionsFromService(sessionId, sourceMaterialId, type);
                        if (materialConditions.Count > 0)
                        {
                            model.Properties.StressStrain = service.GetStressStrainTestConditionsWithDataFromService(sessionId, sourceMaterialId, materialConditions[0].ConditionId, type);
                            model.Properties.StressStrain.MaterialConditions = materialConditions;
                        }



                        model.Properties.FatigueStrain = _materialTMMetalsBinder.FillFatigueStrain(sourceMaterialId, sessionId, service, type);

                        model.Properties.FatigueStress = _materialTMMetalsBinder.FillFatigueStress(sourceMaterialId, sessionId, service, type);
                   
                        

                    }

                    if (tabId == "divEquivalency")
                    {
                        model.Equivalency = service.GetCrossReferenceFromService(sessionId, sourceMaterialId, model.Material.SubgroupId);
                    }
                    if (tabId == "divProcessing")
                    {
                        model.Processing = new ProcessingModel();
                        model.Processing.HeatTreatment = service.GetHeatTreatmentFromService(sessionId, sourceMaterialId, subgroupId);
                        model.Processing.Metallography = service.GetMetallographyPropertiesFromService(sessionId, sourceMaterialId, subgroupId);
                        model.Processing.Machinability = service.GetMachinabilityPropertiesFromService(sessionId, sourceMaterialId);
                    }

                }
                else
                {

                    int tempCount = 0;
                    IList<ConditionModel> condModels = new List<ConditionModel>();
                    IDictionary<ProductGroup.ProductGroupType, ProductGroup> listOfGroups1 = new Dictionary<ProductGroup.ProductGroupType, ProductGroup>();
                    condModels = new List<ConditionModel>();


                    /*plastic*/
                    tempCount = 0;
                    //if (tabId == "divProperties")
                    //{
                    PropertiesContainer mechanical = servicePLUS.GetMechanicalPLUSPropertiesFromService(sessionId, sourceMaterialId, subgroupId);
                    //  IList<Condition> condListMechanical = new List<Condition>();

                    bool isNewCondition = true;
                    foreach (var item in mechanical.Model)
                    {
                        isNewCondition = false;
                        ConditionModel cond = condModels.Where(c => c.ConditionName == item.Condition).FirstOrDefault();
                        if (cond == null)
                        {
                            cond = new ConditionModel() { ConditionId = item.ConditionId, ConditionName = item.Condition };
                            cond.Properties = new List<Property>();

                            isNewCondition = true;
                        }

                        foreach (var prop in item.DataForCondition)
                        {
                            //string[] tmp = prop.Header.Split('(');
                            //tmp[1] = tmp[1].Replace("(", "").Replace(")", "");


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

                                cond.Properties.Add(new Property() { SourcePropertyId = prop.PropertyId, PropertyName = prop.Header.ToLower(), OrigUnit = prop.Unit.Replace("(", "").Replace(")", ""), OrigValue = row.Item1, OrigValueText = tempString });
                                tempCount++;
                            }

                        }

                        cond.Properties = cond.Properties.OrderBy(s => s.PropertyName).ToList();

                        int count = 0;
                        foreach (var prop in cond.Properties)
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

                        if (isNewCondition) condModels.Add(cond);



                    }


                    if (mechanical.Model.Count > 0)
                    {
                        string nameM = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Mechanical).PN;
                        listOfGroups1.Add(ProductGroup.ProductGroupType.Mechanical, new ProductGroup() { ProductGroupId = ProductGroup.ProductGroupType.Mechanical, ProductGroupName = nameM, Conditions = condModels, ConditionId = condModels[0].ConditionId, PropertyCount = tempCount, AllReferences = mechanical.AllReferences });

                    }


                    tempCount = 0;
                    IList<ConditionModel> condModelsp = new List<ConditionModel>();
                    ConditionModel condp = new ConditionModel();
                    PropertiesContainer physical = servicePLUS.GetPhysicalPropertiesPLUSFromService(sessionId, sourceMaterialId);
                    //  IList<Condition> condListMechanical = new List<Condition>();

                    isNewCondition = true;
                    foreach (var item in physical.Model)
                    {
                        isNewCondition = false;
                        condp = condModelsp.Where(c => c.ConditionName == item.Condition).FirstOrDefault();
                        if (condp == null)
                        {
                            condp = new ConditionModel() { ConditionId = item.ConditionId, ConditionName = item.Condition };
                            condp.Properties = new List<Property>();

                            isNewCondition = true;
                        }



                        foreach (var prop in item.DataForCondition)
                        {
                            //string[] tmp = prop.Header.Split('(');
                            //tmp[1] = tmp[1].Replace("(", "").Replace(")", "");


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


                                if (!string.IsNullOrEmpty(row.Item2) && row.Item2 != "-")
                                {
                                    if (tempString != "")
                                    {
                                        tempString += "; " + row.Item2;
                                    }
                                    else
                                    {
                                        tempString += row.Item2;
                                    }
                                }

                                condp.Properties.Add(new Property() { SourcePropertyId = prop.PropertyId, PropertyName = prop.Header.ToLower(), OrigUnit = prop.Unit.Replace("(", "").Replace(")", ""), OrigValue = row.Item1, OrigValueText = tempString });
                                tempCount++;
                            }

                        }

                        condp.Properties = condp.Properties.OrderBy(s => s.PropertyName).ToList();

                        int count = 0;
                        foreach (var prop in condp.Properties)
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

                        if (isNewCondition) condModelsp.Add(condp);



                    }


                    if (physical.Model.Count > 0)
                    {
                        string nameM = context.PreferredNames.Find(p => p.PN_ID == (int)ProductGroup.ProductGroupType.Physical).PN;
                        listOfGroups1.Add(ProductGroup.ProductGroupType.Physical, new ProductGroup() { ProductGroupId = ProductGroup.ProductGroupType.Physical, ProductGroupName = nameM, Conditions = condModelsp, ConditionId = condModelsp[0].ConditionId, PropertyCount = tempCount, AllReferences = physical.AllReferences });

                    }



                    model.Properties = new PropertiesModel() { ProductGroups = listOfGroups1 };
                    IPlusService plusService = new TMPlusService();


                    model.Properties.MultipointData = new MultipointDataModel();

                    model.Properties.MultipointData.DiagramTypes = plusService.GetMPDiagramTypesFromPLUSService(sessionId, sourceMaterialId, subgroupId);
                    if (model.Properties.MultipointData.DiagramTypes != null && model.Properties.MultipointData.DiagramTypes.Count > 0)
                    {
                        int diagramTypeMP = model.Properties.MultipointData.DiagramTypes.FirstOrDefault().Type;
                        model.Properties.MultipointData.SelectedDiagram = new MultipointDataDetailsModel();
                        model.Properties.MultipointData.SelectedDiagram.Conditions = plusService.GetMPConditionsForDiagramTypeFromPLUSService(sessionId, sourceMaterialId, subgroupId, diagramTypeMP);
                        foreach (var it in model.Properties.MultipointData.SelectedDiagram.Conditions)
                        {
                            if (string.IsNullOrEmpty(it.Name))
                            {
                                it.Name = "As received";
                            }
                        }
                        int countMP = 0;
                        foreach (var item in model.Properties.MultipointData.DiagramTypes)
                        {
                            countMP += plusService.GetMPConditionsForDiagramTypeFromPLUSService(sessionId, sourceMaterialId, subgroupId, item.Type).Count;
                        }
                        model.Properties.MultipointData.Count = countMP;

                        

                        if (model.Properties.MultipointData.SelectedDiagram != null && model.Properties.MultipointData.SelectedDiagram.Conditions.Count > 0)
                        {
                            int conditionIdMP = model.Properties.MultipointData.SelectedDiagram.Conditions.FirstOrDefault().Id;                            


                            model.Properties.MultipointData.SelectedDiagram.SelectedCondition = new MultipointDataConditionModel();
                     
                            model.Properties.MultipointData.SelectedDiagram.SelectedCondition.DiagramLegends = plusService.GetMPLegendsForConditionFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);
                            model.Properties.MultipointData.SelectedDiagram.SelectedCondition.SelectedDiagramLegend = new MultipointDataDiagramLegendsModel();


                            if (model.Properties.MultipointData.SelectedDiagram.SelectedCondition != null && model.Properties.MultipointData.SelectedDiagram.SelectedCondition.DiagramLegends.Count > 0)
                            {
                                int legendId = model.Properties.MultipointData.SelectedDiagram.SelectedCondition.DiagramLegends.FirstOrDefault().Id;
                                model.Properties.MultipointData.SelectedDiagram.SelectedCondition.SelectedDiagramLegend.TablePoints = plusService.GetMPTablePointsFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP, legendId);
                                model.Properties.MultipointData.SelectedDiagram.SelectedCondition.Diagram = plusService.GetMultipointDataDiagramFromPlusService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);

                            }
                        }
                    }

                    model.Properties.StressStrain = plusService.GetStressStrainFromPLUSService(sourceMaterialId, subgroupId);



                    FatiguePlusModel fatigue = new FatiguePlusModel();
                    IList<Api.Models.FatiguePLUS.Condition> tempConds = plusService.GetFatigueConditionsFromPLUSService(sessionId, sourceMaterialId);

                    string no = "";
                    if (tempConds.Count > 0)
                    {
                        foreach (var item in tempConds)
                        {
                            string tempText = "";
                            if (!string.IsNullOrEmpty(item.condition_product) && item.condition_product != "-")
                            {
                                tempText = item.condition_product;
                            }
                            if (!string.IsNullOrEmpty(item.condition_experiment) && item.condition_experiment != "-")
                            {
                                if (!string.IsNullOrEmpty(tempText)) tempText += "; ";
                                tempText += item.condition_experiment;
                            }
                            if (!string.IsNullOrEmpty(item.condition_specimen) && item.condition_specimen != "-")
                            {
                                if (!string.IsNullOrEmpty(tempText)) tempText += "; ";
                                tempText += item.condition_specimen;
                            }
                            System.Web.Mvc.SelectListItem tempListItem = new System.Web.Mvc.SelectListItem() { Value = item.NoNo1Temperature, Text = tempText };
                            fatigue.ConditionList.Add(tempListItem);
                        }
                        no = tempConds[0].NoNo1Temperature;
                        fatigue.ConditionPlus = new FatiguePlusCondition();

                        Api.Models.FatiguePLUS.ConditionDetails cd = plusService.GetFatigueConditionDetailsFromPLUSService(sessionId, sourceMaterialId, no);

                        fatigue.ConditionPlus.Condition = tempConds[0];
                        
                        fatigue.ConditionPlus.Details = cd;
                        
                        try
                        {
                            fatigue.ConditionPlus.Diagram = plusService.GetFatigueDiagramFromPLUSService(sessionId, sourceMaterialId, no);
                        }
                        catch (Exception)
                        {
                            fatigue.ConditionPlus.Diagram = null;
                        }
                        


                        model.Properties.FatiguePlus = fatigue;
                    }
                    model.Properties.CreepPlusData = new CreepPlusModel();
                    model.Properties.CreepPlusData.Data = plusService.GetCreepDataFromService(sessionId, sourceMaterialId);
                    model.Properties.CreepPlusData.DiagramConditions = plusService.GetCreepDiagramConditions(sessionId, sourceMaterialId);
                    if (model.Properties.CreepPlusData.DiagramConditions != null && model.Properties.CreepPlusData.DiagramConditions.Count > 0)
                    {
                        model.Properties.CreepPlusData.ConditionData = new MaterialDetailsBinder().GetCreepPlusModel(sessionId, sourceMaterialId, model.Properties.CreepPlusData.DiagramConditions[0].ConditionId);
                    }

                    //  }
                    if (tabId == "divEquivalency")
                    {


                        var cr = plusService.GetCrossReference(sourceMaterialId);
                        IList<CrossReferenceModel> eq = new List<CrossReferenceModel>();
                        eq = cr.Materials.Select(v => new CrossReferenceModel()
                        {
                            MaterialId = v.MaterialId,
                            CountryStandard = v.CountryStandard,
                            EquivalenceCategory = (v.Type.ToString() == "OtherSources") ? "Other Sources" : v.Type.ToString(),
                            MaterialName = v.Name
                        }).ToList();
                        model.Equivalency = new EquivalencyModel() { Equivalences = eq };
                    }
                    if (tabId == "divProcessing")
                    {

                        TMPlusService plusService1 = new TMPlusService();
                        model.Processing = new ProcessingModel();
                        model.Processing.Manufacturing = GetManufacturing(sessionId, sourceMaterialId, subgroupId, context, plusService1);
                    }
                }
            }



            model.TypeName = model.Material.Classification.ClassificationNames.ContainsKey(ClassificationType.Type) ? model.Material.Classification.ClassificationNames[ClassificationType.Type] : "";
            model.GroupName = model.Material.Classification.ClassificationNames.ContainsKey(ClassificationType.Group) ? model.Material.Classification.ClassificationNames[ClassificationType.Group] : "";
            model.ClassName = model.Material.Classification.ClassificationNames.ContainsKey(ClassificationType.Class) ? model.Material.Classification.ClassificationNames[ClassificationType.Class] : "";
            model.SubClassName = model.Material.Classification.ClassificationNames.ContainsKey(ClassificationType.Subclass) ? model.Material.Classification.ClassificationNames[ClassificationType.Subclass] : ""; ;
            model.Filter = new SearchCondition(searchText);

            Material material = GetMaterialInfoData(materialId, sessionId, context);

            if (material != null)
            {
                model.CASRN = material.CASRN;
                model.UNSNo = material.UNSNo;
            }
            System.Web.HttpContext.Current.Session["materialInfoData"] = model;

            return model;

           
            
        }

        private Material GetMaterialInfoData(int materialId, string sessionId, IMaterialsContextUow context)
        {
            FullTextSearch fulltext = context.FullTextSearch.GetMaterialById(materialId);
            Material material = context.Materials.GetMaterialByEquivalence(materialId, fulltext, sessionId).FirstOrDefault();      
          
            return material;
           
        }
        
        public ProductGroup GroupData(int materialId, int subgroupId, int sourceId, int sourceMaterialId, int groupId, string searchText, string sessionId, int unitType, bool isChemical,IMaterialsContextUow context)
        {
            //TODO: Treba zameniti ime modela, Product group 
            ProductGroup groupObj = new ProductGroup();
            if (sourceId == 1)
            {
                groupObj = _groupElsBinder.GetGroupData(materialId, subgroupId, groupId,isChemical, context);
            }
            else if (sourceId == 2)
            {

            }
            else if (sourceId == 3)
            {

            }


            return groupObj;
        }

        public ConditionModel ConditionData(int materialId, int subgroupId, int sourceId, int sourceMaterialId, int groupId, int conditionId, string searchText, string sessionId, int unitType, IMaterialsContextUow materialContextUow)
        {
            ConditionModel conditionData = new ConditionModel();

            if (sourceId == 1)
            {
            
              conditionData = new ConditionElsBinder().GetDataForSlectedTestCondition(materialId, subgroupId, groupId, conditionId, materialContextUow);

            }
            else if (sourceId == 2)
            {
                if (unitType == 2)
                {
                    unitType = 1;
                }

                if (unitType == 3)
                {
                    unitType = 2;
                }
                //Only for Mechanical and Physical
                Condition cond = new ConditionTMMetalsBinder().FillCondition(subgroupId, sourceMaterialId, sourceId, groupId, conditionId, materialContextUow, unitType);
                conditionData.ConditionId = cond.ConditionId;
                conditionData.ConditionName = cond.ConditionName;
                conditionData.Properties = cond.Properties;

                //TODO: Fill references
                conditionData.SelectedReferences = null;
            }
            else if (sourceId == 3)
            {

                Condition cond = new ConditionTMPlusBinder().FillConditionData(subgroupId, sourceMaterialId, sourceId, groupId, conditionId, materialContextUow);
                conditionData.ConditionId = cond.ConditionId;
                conditionData.ConditionName = cond.ConditionName;
                conditionData.Properties = cond.Properties;
                conditionData.SelectedReferences = null;
            }

            return conditionData;
        }

        private int GetEquivalencyCountEMS(int materialId, IMaterialsContextUow context, string sessionId)
        {
            SubgroupsBinder _subgroupsBinder = new SubgroupsBinder();
            Material eqTM = _subgroupsBinder.GetEquivalentMaterials(materialId, new GridDescriptor(new SortDescriptor() { PropertyName = "SourceText", Order = SortOrder.Ascending }), context).Where(m => m.SourceId != 1).FirstOrDefault();
            int count = 0;

            if (eqTM != null)
            {
                IService service = new TotalMateriaService();
                IPlusService servicePLUS = new TMPlusService();
                if (eqTM.SourceId == 2)
                {

                    count = service.GetMaterialSubgroupListFromService(sessionId, eqTM.MaterialId, eqTM.SourceMaterialId).FirstOrDefault().NumEquivalency;
                }
                if (eqTM.SourceId == 3)
                {

                    count = servicePLUS.GetMaterialSubgroupPLUSListFromService(sessionId, materialId, eqTM.SourceMaterialId).FirstOrDefault().NumEquivalency;
                }
            }

            return count;
        }

        public EquivalencyModel BindEquivalencyEMS(int materialId, IMaterialsContextUow context, string sessionId)
        {
            EquivalencyModel model = new EquivalencyModel() { Equivalences = new List<CrossReferenceModel>() };
            IService service = new TotalMateriaService();
            SubgroupsBinder _subgroupsBinder = new SubgroupsBinder();
            Material eqTM = _subgroupsBinder.GetEquivalentMaterials(materialId, new GridDescriptor(new SortDescriptor() { PropertyName = "SourceText", Order = SortOrder.Ascending }), context).Where(m => m.SourceId != 1).FirstOrDefault();
            if (eqTM != null)
            {
                model = service.GetCrossReferenceFromService(sessionId, eqTM.SourceMaterialId, eqTM.SubgroupId);
            }

            return model;
        }
        
        public StressStrainModel GetTestConditionsWithData(string sessionId, int materialId, string conditionId)
        {
            IService service = new TotalMateriaService();
            int type = GetSelectedUnitForTMMaterials();
            return service.GetStressStrainTestConditionsWithDataFromService(sessionId, materialId, conditionId, type);

        }

        public IList<StressStrainTemperature> GetSSTemperatures(string sessionId, int materialId, int subgroupId, int conditionId)
        {

            int type = GetSelectedUnitForTMMaterials();
            IService service = new TotalMateriaService();
            return service.GetStressStrainTemperatures(sessionId, materialId, conditionId, type);

        }

        public IList<StressStrainTemperature> GetSSPlusTemperatures(string sessionId, int materialId, int subgroupId, int conditionId)
        {

            int type = GetSelectedUnitForTMMaterials();
            IPlusService plusService = new TMPlusService();
            return plusService.GetStressStrainTemperaturesPLUS(sessionId, materialId, subgroupId, conditionId, 1).Union(plusService.GetStressStrainTemperaturesPLUS(sessionId, materialId, subgroupId, conditionId, 2)).ToList();

        }

        public IList<string> GetSSReferencesForCondition(string sessionId, int materialId, int conditionId)
        {
            IService service = new TotalMateriaService();
            return service.GetReferencesForSelectedConditionFromService(sessionId, materialId, conditionId, MaterialDetailType.StressStrain);
        }

        public IList<string> GetSSPlusReferencesForCondition(string sessionId, int materialId, int conditionId)
        {
            IPlusService plusService = new TMPlusService();
            return plusService.GetReferencesForSelectedConditionForSS(sessionId, materialId, conditionId);
        }

        private static int GetSelectedUnitForTMMaterials()
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            int type=0;
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            else
            {
                MaterialDetails1 hp = nav.GetOrderedItems().Where(n => n.NavigableID == "MaterialDetails1").FirstOrDefault() as MaterialDetails1;
                IDictionary<string, object> idic = hp.PageData as Dictionary<string, object>;
                type = (int)idic["unitType"];

                switch (type)
                {
                    case 2:
                        type = 1;
                        break;
                    case 3:
                        type = 2;
                        break;
                    default:
                        break;
                }
            }
            return type;
        }

        public StressStrainDetails GetTemperatureDetails(string sessionId, int materialId, int subgroupId, int conditionId, double temperature, int typeSS)
        {

            int type = GetSelectedUnitForTMMaterials();
            IService service = new TotalMateriaService();
            return service.GetStressStrainDetails(sessionId, materialId, conditionId, temperature, typeSS, type);

        }

        public StressStrainDetails GetTemperaturePlusDetails(string sessionId, int materialId, int subgroupId, int conditionId, double temperature, int typeSS)
        {
            IPlusService plusService = new TMPlusService();
            return plusService.GetStressStrainPLUSDetails(sessionId, materialId, subgroupId, conditionId, temperature, typeSS);

        }

        public HeatTreatment GetHeatTreatment(string sessionId, int materialId, int subgroupId)
        {
            IService service = new TotalMateriaService();

            return service.GetHeatTreatmentFromService(sessionId, materialId, subgroupId);
        }

        public MetallographyModel GetMetallography(string sessionId, int materialId, int subgroupId)
        {
            IService service = new TotalMateriaService();
            return service.GetMetallographyPropertiesFromService(sessionId, materialId, subgroupId);


        }

        public ManufacturingModel GetManufacturing(string sessionId, int sourceMaterialId, int subgroupId, IMaterialsContextUow context, TMPlusService plusService)
        {

            int tempCount = 0;
            ManufacturingModel model = new ManufacturingModel();
            IList<ConditionModel> condModels = new List<ConditionModel>();
            PropertiesContainer manufacturing = plusService.GetManufactoringProcesses(sourceMaterialId, subgroupId);

            bool isNewCondition = true;
            foreach (var item in manufacturing.Model)
            {
                isNewCondition = false;
                ConditionModel cond = condModels.Where(c => c.ConditionName == item.Condition).FirstOrDefault();
                if (cond == null)
                {
                    cond = new ConditionModel() { ConditionId = item.ConditionId, ConditionName = item.Condition };
                    cond.Properties = new List<Property>();

                    isNewCondition = true;
                }

                foreach (var prop in item.DataForCondition)
                {
                    //string[] tmp = prop.Header.Split('(');
                    //tmp[1] = tmp[1].Replace("(", "").Replace(")", "");


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

                        if (!string.IsNullOrEmpty(row.Item2) && row.Item2!="-")
                        {
                            if (tempString != "")
                            {
                                tempString += "; " + row.Item2;
                            }
                            else
                            {
                                tempString += row.Item2;
                            }
                        }

                        cond.Properties.Add(new Property() { SourcePropertyId = prop.PropertyId, PropertyName = prop.Header.ToLower(), OrigUnit = prop.Unit.Replace("(", "").Replace(")", ""), OrigValue = row.Item1, OrigValueText = tempString });
                        tempCount++;
                    }

                }

                cond.Properties = cond.Properties.OrderBy(s => s.PropertyName).ToList();

                int count = 0;
                foreach (var prop in cond.Properties)
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

                if (isNewCondition) condModels.Add(cond);



            }


            if (manufacturing.Model.Count > 0)
            {
                model = new ManufacturingModel() { Name = "manufacturing", ConditionId = condModels[0].ConditionId, Conditions = condModels, PropertyCount = tempCount, AllReferences = manufacturing.AllReferences };
            }
            return model;
        }
              
        public MultipointDataDetailsModel GetMultipointDataTypeDiagram(string sessionId, int sourceMaterialId, int subgroupId, int diagramTypeMP)
        {
            IPlusService plusService = new TMPlusService();

            MultipointDataDetailsModel model = new MultipointDataDetailsModel();

            model.Conditions = plusService.GetMPConditionsForDiagramTypeFromPLUSService(sessionId, sourceMaterialId, subgroupId, diagramTypeMP);
            foreach (var it in model.Conditions)
            {
                if (string.IsNullOrEmpty(it.Name))
                {
                    it.Name = "As received";
                }
            }
            int conditionIdMP = model.Conditions.FirstOrDefault().Id;

            model.SelectedCondition = new MultipointDataConditionModel();
            model.SelectedCondition.DiagramLegends = plusService.GetMPLegendsForConditionFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);
            model.SelectedCondition.SelectedDiagramLegend = new MultipointDataDiagramLegendsModel();
            int legendId = model.SelectedCondition.DiagramLegends.FirstOrDefault().Id;
            model.SelectedCondition.SelectedDiagramLegend.TablePoints = plusService.GetMPTablePointsFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP, legendId);
            model.SelectedCondition.Diagram = plusService.GetMultipointDataDiagramFromPlusService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);
            return model;
        }

        public MultipointDataConditionModel GetMultipointDataCondition(string sessionId, int sourceMaterialId, int subgroupId, int diagramTypeMP, int conditionIdMP)
        {
            IPlusService plusService = new TMPlusService();

            MultipointDataConditionModel model = new MultipointDataConditionModel();

            model.DiagramLegends = plusService.GetMPLegendsForConditionFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);
            model.SelectedDiagramLegend = new MultipointDataDiagramLegendsModel();
            int legendId = model.DiagramLegends.FirstOrDefault().Id;
            model.SelectedDiagramLegend.TablePoints = plusService.GetMPTablePointsFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP, legendId);
            model.Diagram = plusService.GetMultipointDataDiagramFromPlusService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP);
            return model;
        }

        public MultipointDataDiagramLegendsModel GetMultipointDataDiagramLegend(string sessionId, int sourceMaterialId, int subgroupId, int diagramTypeMP, int conditionIdMP, int legendId)
        {
            IPlusService plusService = new TMPlusService();

            MultipointDataDiagramLegendsModel model = new MultipointDataDiagramLegendsModel();

            model.TablePoints = plusService.GetMPTablePointsFromPLUSService(sessionId, sourceMaterialId, subgroupId, conditionIdMP, diagramTypeMP, legendId);
            return model;
        }

        public FatiguePlusCondition GetFatiguePlusCondition(string sessionId, int sourceMaterialId, string no)
        {
            IPlusService service = new TMPlusService();

            Api.Models.FatiguePLUS.Condition tempCond = new Api.Models.FatiguePLUS.Condition();
            tempCond = service.GetFatigueConditionsFromPLUSService(sessionId, sourceMaterialId).Where(fs => fs.NoNo1Temperature == no).FirstOrDefault();

            FatiguePlusCondition model = new FatiguePlusCondition();

            model.Condition = tempCond;
            model.Details = service.GetFatigueConditionDetailsFromPLUSService(sessionId, sourceMaterialId, no);
            try
            {
                model.Diagram = service.GetFatigueDiagramFromPLUSService(sessionId, sourceMaterialId, no);
            }
            catch (Exception)
            {
                model.Diagram = null;
            }
            
            return model;
        }

        public ProductGroup DataForFirstCondition(int materialId, int subgroupId, int sourceId, int sourceMaterialId, int groupId, int conditionId, int productFormId, string materialDescription, string thickness, string searchText, string sessionId, int type,IMaterialsContextUow context)
        {
            PropertyGroup group = new GroupElsBinder().GetMaterialGroup(materialId, subgroupId, groupId, context);
            ProductGroup groupObj = new GroupElsBinder().GetMaterialGroupBasicData(group,false);

            //groupObj.Conditions = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder().GetGroupConditionsFirst(materialId, subgroupId,groupId, context);

            // groupObj.ConditionsFirst = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder().GetGroupConditionsFirst(materialId, subgroupId, groupId, context);

            //   int productFormId = groupObj.ConditionsFirst.Where(m => m.ConditionId == conditionId).Select(m => m.ProductFormId).FirstOrDefault();

            //if (groupObj.ConditionsFirst.Count > 0)
            //{
            groupObj.ConditionId = conditionId;
            groupObj.TestConditions = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder().GetGroupTestConditions(materialId, subgroupId, groupId, conditionId, productFormId, materialDescription, thickness, context);
            //}

            groupObj.TestConditions = groupObj.TestConditions.OrderBy(m => m.ConditionName).ToList();
            if (groupObj.TestConditions.Count > 0)
            {

                groupObj.RowId = groupObj.TestConditions[0].RowId;
            }

            return groupObj;




        }

        public ElsevierMaterials.Models.Domain.Citation GetCitation(int cit_record_id, IMaterialsContextUow context)
        {           
            ElsevierMaterials.Models.Domain.Citation modelCitation = new ElsevierMaterials.Models.Domain.Citation();
            Citation citation = context.Citations.AllAsNoTracking.Where(s => s.record_id == cit_record_id).FirstOrDefault();

            modelCitation.RecordId = citation.record_id;
            modelCitation.MaterialName = citation.MaterialName;
            modelCitation.InChIKey = citation.InChIKey;
            modelCitation.Property = citation.Property;
            modelCitation.MeasuredData = citation.MeasuredData;
            modelCitation.PredictedData = citation.PredictedData;
            modelCitation.EquationTxt = citation.EquationTxt;
            modelCitation.EquationLink = citation.EquationLink;

            return modelCitation;
        }
        
        public MaterialTMMetalsBinder _materialTMMetalsBinder { get; set; }
        public GroupElsBinder _groupElsBinder { get; set; }

        public MaterialDetailsBinder()
        {
            _groupElsBinder = new GroupElsBinder();
            _materialTMMetalsBinder = new MaterialTMMetalsBinder();
        }      
    }
}