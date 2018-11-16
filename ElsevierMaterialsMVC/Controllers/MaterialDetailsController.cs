using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using ElsevierMaterialsMVC.BL.Binders.MaterialDetails;
using IniCore.Web.Mvc;
using ElsevierMaterials.Models;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.BL.Notifications;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;

namespace ElsevierMaterialsMVC.Controllers
{
    public class MaterialDetailsController : ElsevierController
    {
        private MaterialDetailsBinder _materialDetailsBinder;
        public MaterialDetailsBinder MaterialDetailsBinder { get { return _materialDetailsBinder; } }

        [SessionExpire]
        public ActionResult GetMaterialDetails(int materialId = 0, int subgroupId = 0, int sourceId = 0, int sourceMaterialId = 0, string searchText = "", string tabId = "divProperties", int type=1)
        {
           
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            BreadcrumbNavigationGetSet(ref materialId, ref subgroupId, ref sourceId, ref sourceMaterialId, ref searchText, ref type);
            MaterialDetailsModel model = MaterialDetailsBinder.GetMaterial(materialId, subgroupId, sourceId, sourceMaterialId, searchText, sessionId, materialContextUow, tabId);
            model.ChemicalElsProperties = new ElsevierMaterialsMVC.BL.Binders.PropertyDescriptionBinder().GetProperties(materialId, materialContextUow);
            model.ActiveTab = tabId;
            return View("MaterialDetailsContainer", model);
        }

        [SessionExpire]
        public ActionResult FirstConditionData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId, int conditionId,int productFormId, string materialDescription,string thickness, string searchText, int type)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();

            
            ProductGroup model = MaterialDetailsBinder.DataForFirstCondition(materialId, subgroupId, sourceId, sourceMaterialId,groupId, conditionId,productFormId, materialDescription, thickness, searchText, sessionId, type, materialContextUow);

            model.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
            model.ConditionId = conditionId;
            ModelState.Clear();
            return Json(ResponseStatus.Success, RenderPartialViewToString("TestConditionsContent", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult SecondConditionData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId, int rowId, string searchText, int type)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();


            ConditionModel model = MaterialDetailsBinder.ConditionData(materialId, subgroupId, sourceId, sourceMaterialId, groupId, rowId, searchText, sessionId, type, materialContextUow);
            model.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
            return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionPropertiesContent", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]              
        public ActionResult GroupData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId, string searchText, int type, bool isChemical)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            ProductGroup model = MaterialDetailsBinder.GroupData(materialId, subgroupId, sourceId, sourceMaterialId, groupId, searchText, sessionId,type,isChemical, materialContextUow);
           
            model.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
            if (isChemical)
            {
                var jsonResult = Json(ResponseStatus.Success, RenderPartialViewToString("ChemicalGroupContent", model), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;                
            }
            else
            {
                var jsonResult = Json(ResponseStatus.Success, RenderPartialViewToString("GroupContent", model), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;  
            }
        }
        
        [SessionExpire]
        public ActionResult ConditionData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId, int conditionId, string searchText, int type)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            
            ConditionModel model = MaterialDetailsBinder.ConditionData(materialId, subgroupId, sourceId, sourceMaterialId,groupId, conditionId, searchText, sessionId, type, materialContextUow);
            model.ProductGroupId = (ProductGroup.ProductGroupType)groupId;
            model.ConditionId = conditionId;

            return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionPropertiesContent", model), JsonRequestBehavior.AllowGet);
        }
        
        [SessionExpire]
        public ActionResult GetMechanicalMetal(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId,  string searchText, int type, int conditionId=0)
        {
            if (type == 2)
            {
                type = 1;
            }

            if (type == 3)
            {
                type = 2;
            }

            IService service = new TotalMateriaService();
            IPlusService servicePLUS = new TMPlusService();

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            
            ConditionModel conditionData = new ConditionModel();
            Condition cond = new ConditionTMMetalsBinder().FillMechanicalConditionData(subgroupId, sourceMaterialId, conditionId, materialContextUow, type, sessionId);
            conditionData.ConditionId = cond.ConditionId;
            conditionData.ConditionName = cond.ConditionName;
            conditionData.Properties = cond.Properties;
            conditionData.ProductGroupId = ProductGroup.ProductGroupType.Mechanical;
            //TODO: Fill references
            conditionData.SelectedReferences = null;

            return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionPropertiesContent", conditionData), JsonRequestBehavior.AllowGet);
        }
        
        [SessionExpire]
        public ActionResult GetPhysicalMetal(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int groupId, int conditionId, string searchText, int type)
        {

            if (type == 2)
            {
                type = 1;
            }

            if (type == 3)
            {
                type = 2;
            }

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();

            ConditionModel conditionData = new ConditionModel();
            Condition cond = new ConditionTMMetalsBinder().FillPhysicalConditionData(sourceMaterialId, conditionId, materialContextUow, type, sessionId);
           
            conditionData.ConditionId = cond.ConditionId;
            conditionData.ConditionName = cond.ConditionName;
            conditionData.Properties = cond.Properties;
            conditionData.ProductGroupId = ProductGroup.ProductGroupType.Physical;
            IService service = new TotalMateriaService();
            conditionData.SelectedReferences = service.GetReferencesForSelectedConditionFromService(sessionId, sourceMaterialId, cond.ConditionId, MaterialDetailType.PhysicalProperties);
          
         

            return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionPropertiesContent", conditionData), JsonRequestBehavior.AllowGet);
        }

        private  void BreadcrumbNavigationGetSet(ref int materialId, ref int subgroupId, ref int sourceId, ref int sourceMaterialId, ref string searchText, ref int type)
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            if (nav.Contains("Exporter"))
            {
                nav.Pop();
            }
            if (nav.Contains("Comparison"))
            {
                nav.Pop();
            }
           
            MaterialDetails1 hp = nav.GetOrderedItems().Where(n => n.NavigableID == "MaterialDetails1").FirstOrDefault() as MaterialDetails1;
            if (hp == null)
            {

                hp = new MaterialDetails1();
                IDictionary<string, object> idic = new Dictionary<string, object>();
                idic.Add("materialId", materialId);
                idic.Add("subgroupId", subgroupId);
                idic.Add("sourceId", sourceId);
                idic.Add("sourceMaterialId", sourceMaterialId);
                idic.Add("searchText", searchText);
                if (sourceId == 1)
                {
                    idic.Add("unitType", 1);
                }
                else
                {
                    idic.Add("unitType", 2);
                }


                hp.PageData = idic;

            }
            else
            {
                if (materialId == 0 && subgroupId == 0 && sourceId == 0 && sourceMaterialId == 0 && searchText == "")
                {
                    IDictionary<string, object> idic = hp.PageData as Dictionary<string, object>;
                    materialId = (int)idic["materialId"];
                    subgroupId = (int)idic["subgroupId"];
                    sourceId = (int)idic["sourceId"];
                    sourceMaterialId = (int)idic["sourceMaterialId"];
                    searchText = idic["searchText"].ToString();
                    type = (int)idic["unitType"];
                }
            }
           
            nav.LastNavigable = "MaterialDetails1";
            nav.Push(hp);
            System.Web.HttpContext.Current.Session["Navigation"] = nav;
        }
        
        [SessionExpire]
        public ActionResult GetProperties(int sourceMaterialId, int subgroupId, int sourceId , string searchText)
        {                      
            return RedirectToAction("GetMaterialDetails", new { materialId = sourceMaterialId, subgroupId = subgroupId, sourceId = sourceId, sourceMaterialId = sourceMaterialId, searchText = searchText, tabId = "divProperties" });
        }
        
        [SessionExpire]
        public ActionResult GetProccesing(int sourceMaterialId, int subgroupId, int sourceId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            ProcessingModel model = new ProcessingModel();
            if (sourceId == 2)
            {
                IService service = new TotalMateriaService();
                model.HeatTreatment = service.GetHeatTreatmentFromService(sessionId, sourceMaterialId, subgroupId);
                model.Metallography = service.GetMetallographyPropertiesFromService(sessionId, sourceMaterialId, subgroupId);
                model.Machinability = service.GetMachinabilityPropertiesFromService(sessionId, sourceMaterialId);
            }
            if (sourceId == 3)
            {
                TMPlusService plusService = new TMPlusService();
                model.Manufacturing = _materialDetailsBinder.GetManufacturing(sessionId, sourceMaterialId, subgroupId, materialContextUow, plusService);
            }
       
            return Json(ResponseStatus.Success,RenderPartialViewToString("Processing", model), JsonRequestBehavior.AllowGet);
        }
        
        [SessionExpire]
        public ActionResult GetEquivalency(int sourceMaterialId, int subgroupId, int sourceId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            EquivalencyModel model = new EquivalencyModel();
            if (sourceId == 1)
            {

                model = MaterialDetailsBinder.BindEquivalencyEMS(sourceMaterialId, materialContextUow, sessionId);
            }
            if (sourceId == 2)
            {
                IService service = new TotalMateriaService();
                model = service.GetCrossReferenceFromService(sessionId, sourceMaterialId, subgroupId);
            }
            if (sourceId == 3)
            {
                TMPlusService plusService = new TMPlusService();
                var cr = plusService.GetCrossReference(sourceMaterialId);
                IList<CrossReferenceModel> eq = new List<CrossReferenceModel>();
                eq = cr.Materials.Select(v => new CrossReferenceModel()
                {
                    MaterialId = v.MaterialId,
                    CountryStandard = v.CountryStandard,
                    EquivalenceCategory = (v.Type.ToString() == "OtherSources") ? "Other Sources" : v.Type.ToString(),
                    MaterialName = v.Name
                }).ToList();
                model = new EquivalencyModel() { Equivalences = eq };
              
            }

            return Json(ResponseStatus.Success, RenderPartialViewToString("Equivalency", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult GetStressStrainTestConditionsWithDetails(int materialId, int subgroupId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            StressStrainModel model = MaterialDetailsBinder.GetTestConditionsWithData(sessionId, materialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("StressStrainGroup", model), JsonRequestBehavior.AllowGet);
            //return Json(ResponseStatus.Success, new { result = RenderPartialViewToString("StressStrainGroup", model), model = System.Web.Helpers.Json.Encode(model.MaterialConditions.ons..StressTemperatures[0])}, JsonRequestBehavior.AllowGet);
        }
                
        [SessionExpire]
        public ActionResult GetStressStrainDetails(int materialId, int subgroupId, int conditionId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            StressStrainConditionModel model = new StressStrainConditionModel();
            
            if (materialId > 1000000)
            {
                model.StressTemperatures = MaterialDetailsBinder.GetSSTemperatures(sessionId, materialId, subgroupId, conditionId);
                model.SelectedReferences = MaterialDetailsBinder.GetSSReferencesForCondition(sessionId, materialId, conditionId);
                return Json(ResponseStatus.Success, new { result = RenderPartialViewToString("StressStrain", model), model =System.Web.Helpers.Json.Encode(model.StressTemperatures[0]) }, JsonRequestBehavior.AllowGet);
            } 
            else
            {
                model.StressTemperatures = MaterialDetailsBinder.GetSSPlusTemperatures(sessionId, materialId, subgroupId, conditionId);
                model.SelectedReferences = MaterialDetailsBinder.GetSSPlusReferencesForCondition(sessionId, materialId, conditionId);
                return Json(ResponseStatus.Success, RenderPartialViewToString("StressStrainPlus", model), JsonRequestBehavior.AllowGet);
            }
           
          
            
        }
        [SessionExpire]
        public ActionResult GetStressStrainTemperatureDetails(int materialId, int subgroupId, int conditionId, double temperature, int typeSS, bool plus=false) {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            StressStrainDetails model = new StressStrainDetails();
            if (plus)
            {
                model = MaterialDetailsBinder.GetTemperaturePlusDetails(sessionId, materialId, subgroupId, conditionId, temperature, typeSS);
            }
            else
            {
                model = MaterialDetailsBinder.GetTemperatureDetails(sessionId, materialId, subgroupId, conditionId, temperature, typeSS);
            }
            return Json(ResponseStatus.Success,  RenderPartialViewToString("StressStrainDetailsTable", model), JsonRequestBehavior.AllowGet);
        }      
        [SessionExpire]
        public ActionResult GetStressStrainPlusDetails(int materialId, int subgroupId, int conditionId)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            StressStrainConditionModel model = new StressStrainConditionModel();
            model.StressTemperatures = MaterialDetailsBinder.GetSSPlusTemperatures(sessionId, materialId, subgroupId, conditionId);
            model.SelectedReferences = MaterialDetailsBinder.GetSSReferencesForCondition(sessionId, materialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("StressStrain", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult GetFatigueStrainConditionDetails(int sourceMaterialId, string materialConditionId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            FatigueCondition model = MaterialDetailsBinder.GetFatigueStrainCondition(sessionId, sourceMaterialId, materialConditionId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("FatigueCondition", model), JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult GetFatigueStressConditionDetails(int sourceMaterialId,string materialConditionId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            FatigueCondition model = MaterialDetailsBinder.GetFatigueStressCondition(sessionId, sourceMaterialId, materialConditionId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("FatigueCondition", model), JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult GetFatiguePlusConditionDetails(int sourceMaterialId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            FatiguePlusCondition model = MaterialDetailsBinder.GetFatiguePlusCondition(sessionId, sourceMaterialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("FatiguePlusCondition", model), JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult GetFatigueStrainTestConditions(int materialId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            FatigueModel model = MaterialDetailsBinder.GetFatigueStrainTestConditionsWithData(sessionId, materialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("FatigueStrainGroup", model), JsonRequestBehavior.AllowGet);
    
        }

        [SessionExpire]
        public ActionResult GetFatigueStressTestConditions(int materialId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            FatigueModel model = MaterialDetailsBinder.GetFatigueStressTestConditionsWithData(sessionId, materialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("FatigueStressGroup", model), JsonRequestBehavior.AllowGet);
     
        }

        [SessionExpire]
        public ActionResult GetCreepTestConditions(int materialId, string conditionId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            CreepDataModel model = MaterialDetailsBinder.GetCreepTestConditionsWithData(sessionId, materialId, conditionId);
            return Json(ResponseStatus.Success, RenderPartialViewToString("CreepGroup", model), JsonRequestBehavior.AllowGet);
     
        }

        

        
        [SessionExpire]
        public ActionResult GetMultipointDataTypeDiagram(int sourceMaterialId, int diagramTypeMP)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            MultipointDataDetailsModel model = MaterialDetailsBinder.GetMultipointDataTypeDiagram(sessionId, sourceMaterialId, 0, diagramTypeMP);
            return Json(ResponseStatus.Success, RenderPartialViewToString("MultipointDataDiagramDetails", model), JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult GetMultipointDataCondition(int sourceMaterialId, int diagramTypeMP,int conditionIdMP)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            MultipointDataConditionModel model = MaterialDetailsBinder.GetMultipointDataCondition(sessionId, sourceMaterialId, 0, diagramTypeMP, conditionIdMP);
            return Json(ResponseStatus.Success, RenderPartialViewToString("MultipointDataConditionDetails", model), JsonRequestBehavior.AllowGet);
        }
        [SessionExpire]
        public ActionResult GetMultipointDataDiagramLegend(int sourceMaterialId, int diagramTypeMP,int conditionIdMP, int legendIdMP)
        {

            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            MultipointDataDiagramLegendsModel model = MaterialDetailsBinder.GetMultipointDataDiagramLegend(sessionId, sourceMaterialId, 0, diagramTypeMP, conditionIdMP, legendIdMP);
            return Json(ResponseStatus.Success, RenderPartialViewToString("MultipointDataTablePoints", model), JsonRequestBehavior.AllowGet);
        }
        
         [SessionExpire]
        public ActionResult GetCreepDataMetal(int sourceMaterialId, int conditionId, string materialConditionId)
         {
               string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
             
               ElsevierMaterials.Models.Domain.CreepData.CreepDataContainer model = MaterialDetailsBinder.GetCreepData(sessionId, sourceMaterialId,materialConditionId, conditionId);
             
               return Json(ResponseStatus.Success, RenderPartialViewToString("CreepData", model), JsonRequestBehavior.AllowGet);   
    
       }
         [SessionExpire]
         public ActionResult GetCreepDataPlus(int sourceMaterialId, int conditionId) {
             string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
             
             CreepPlusConditionModel model = MaterialDetailsBinder.GetCreepPlusModel(sessionId, sourceMaterialId, conditionId);

             return Json(ResponseStatus.Success, RenderPartialViewToString("CreepPlusConditionDiagrams", model), JsonRequestBehavior.AllowGet);

         }
         [SessionExpire]
         public ActionResult GetCreepDataPlusByTemp(int sourceMaterialId, int conditionId, short temperature) {
             string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
             
             CreepPlusConditionModel model = MaterialDetailsBinder.GetCreepPlusTemperature(sessionId, sourceMaterialId, conditionId, temperature);

             return Json(ResponseStatus.Success, RenderPartialViewToString("CreepPlusConditionDiagrams", model), JsonRequestBehavior.AllowGet);

         }
         [SessionExpire]
         public ActionResult GetCreepDataPlusPoints(int sourceMaterialId, int conditionId, short temperature, double additional, int iso) {
             string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();

             IList<Api.Models.CreepDataPLUS.StressPointIso> model = MaterialDetailsBinder.GetCreepPlusPoints(sessionId, sourceMaterialId, conditionId, temperature, additional, iso);
             if (iso == 0) {
                 return Json(ResponseStatus.Success, RenderPartialViewToString("CreepDetailsTable", model), JsonRequestBehavior.AllowGet);
             }
             else {
                 return Json(ResponseStatus.Success, RenderPartialViewToString("CreepDetailsTableIso", model), JsonRequestBehavior.AllowGet);
             }

         }
         

         [SessionExpire]
         public ActionResult ConvertValues(int type)
         {
             BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
             if (nav == null)
             {
                 nav = new BreadcrumbNavigation();
             }
             MaterialDetails1 hp = nav.GetOrderedItems().Where(n => n.NavigableID == "MaterialDetails1").FirstOrDefault() as MaterialDetails1;
             IDictionary<string, object> idic = hp.PageData as Dictionary<string, object>;
             idic["unitType"] = type;          
             return Json(ResponseStatus.Success, new { sucess = true}, JsonRequestBehavior.AllowGet);
         }

         [SessionExpire]
         public ActionResult GetCitation(int cit_record_id)
         {
             string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
             ElsevierMaterials.Models.Domain.Citation model = MaterialDetailsBinder.GetCitation(cit_record_id, materialContextUow);

             return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("ShowCitation", model) }, JsonRequestBehavior.AllowGet);
         }

         [SessionExpire]
         public ActionResult GetMechnaical(int materialId = 0, int subgroupId = 0, int sourceId = 0, int sourceMaterialId = 0, string searchText = "", string tabId = "divProperties", int type = 1)
         {

             string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
             BreadcrumbNavigationGetSet(ref materialId, ref subgroupId, ref sourceId, ref sourceMaterialId, ref searchText, ref type);
             MaterialDetailsModel model = MaterialDetailsBinder.GetMaterialMechanical(materialId, subgroupId, sourceId, sourceMaterialId, searchText, sessionId, materialContextUow, tabId);
             model.ActiveTab = tabId;
             return Json(ResponseStatus.Success, RenderPartialViewToString("ConditionPropertiesContent", model), JsonRequestBehavior.AllowGet);
         }
        
        public MaterialDetailsController()
        {
            _materialDetailsBinder = new MaterialDetailsBinder();
        }
    }
}
