using ElsevierMaterials.Models.Domain.Comparison;
using ElsevierMaterialsMVC.BL.Binders.MaterialDetails;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterialsMVC.Filters;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElsevierMaterials.EF.MaterialsContextUow;
using IniCore.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using ElsevierMaterialsMVC.BL.Binders.Comparison;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.ComparisonDiagram;
using ElsevierMaterials.Models.Domain.ComparisonDiagram;
using ElsevierMaterialsMVC.Models.Comparison;

namespace ElsevierMaterialsMVC.Controllers
{
    public class ComparisonController : ElsevierController
    {        
        [SessionExpire]
        public ActionResult Materials()
        {
            BreadcrumbNavigationGetSet(); 
            Comparison comparison = _basicBinder.GetComparison();
            comparison.ComparisonD = new ComparisonDiagramControllerBinder().GetComparisonDiagramModel();
            return View("Comparison", comparison);                     
        }
        
        [SessionExpire]
        public ActionResult RemoveMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId)
        {
            ComparisonD comparison1 = _ComparisonDiagramBinder.GetComparisonDiagramModel();

            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = _materialBinder.RemoveMaterial(materialId, sourceMaterialId, sourceId, subgroupId);
            ComparisonD comparisonD = _ComparisonDiagramBinder.RemoveMaterial(materialId,sourceMaterialId, sourceId, subgroupId);
            comparison.ComparisonD = comparisonD;
            return Json(ResponseStatus.Success, new { hasProperties = comparison.Properties.Count > 0 ? true : false, data = RenderPartialViewToString("PropertiesWithDiagrams", comparison), plot = RenderPartialViewToString("Plot", comparison), model = Newtonsoft.Json.JsonConvert.SerializeObject(comparison) }, JsonRequestBehavior.AllowGet);
        }
        
        [SessionExpire]
        public ActionResult RemoveProperty(int propertyId = 0, int sourcePropertyId = 0, int rowId = 0)
        {
            bool sucess = _binder.RemoveProperty(propertyId, sourcePropertyId, rowId);
            Comparison comparison = _basicBinder.GetComparison();
            return Json(ResponseStatus.Success, new { hasProperties = comparison.Properties.Count > 0 ? true : false, data = RenderPartialViewToString("PropertiesWithDiagrams", comparison), plot = RenderPartialViewToString("Plot", comparison), model = Newtonsoft.Json.JsonConvert.SerializeObject(comparison) }, JsonRequestBehavior.AllowGet);
        }
                           
        [SessionExpire]
        public ActionResult AddPropertiesForMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, List<PropertyFilter> properties)
        {
            bool success = _materialBinder.AddMaterialToMaterialNamesList(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);

            if (success)
            {
                foreach (var propertyClient in properties)
                {

                    Comparison comparison = _basicBinder.GetComparison();

                    ElsevierMaterials.Models.Domain.Comparison.Property propertyComparison = _binder.GetProperty(propertyClient, comparison);

                    if (propertyComparison != null)
                    {
                        _materialBinder.ChangePropertyData(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, ref propertyComparison);
                    }
                    else
                    {
                         _binder.AddProperty(materialId, sourceMaterialId, sourceId, subgroupId, propertyClient,  ref  propertyComparison, materialContextUow);
                         _binder.AddMaterial(materialId, sourceMaterialId, sourceId, subgroupId, propertyClient, propertyComparison, materialContextUow);

                    }
                }
            }                 

            return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }
        
        [SessionExpire]
        public ActionResult ShowDigramForProperty(int propertyId, int sourcePropertyId)
        {
         
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();

            string imageName = _diagramBinder.GenerateChart(propertyId, sourcePropertyId, ref chart);
            chart.SaveImage(Server.MapPath("~/Temp/" + imageName), ChartImageFormat.Png);

            return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("Diagram", imageName) }, JsonRequestBehavior.AllowGet);
        }
        
        private void BreadcrumbNavigationGetSet()
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            ExporterNav en = nav.GetOrderedItems().Where(n => n.NavigableID == "Exporter").FirstOrDefault() as ExporterNav;
            if (en != null)
            {
                en.IsVisible = false;
            }
            ComparisonNav cn = nav.GetOrderedItems().Where(n => n.NavigableID == "Comparison").FirstOrDefault() as ComparisonNav;
            if (cn == null)
            {
                cn = new ComparisonNav();
            }
            else
            {
               
            }
            cn.IsVisible = true;
            nav.LastNavigable = "Comparison";
            nav.Push(cn);
            System.Web.HttpContext.Current.Session["Navigation"] = nav;
           
        }

        

        [SessionExpire]
        public ActionResult AddChemicalDiagramToComparison(int materialId, int subgroupId, int conditionId, int propertyId, string groupName)
        {
            ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddChemicalDiagramToComparison(materialId, subgroupId, conditionId, "curveName", 1, materialContextUow, propertyId);
           return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddChemicalDiagramsToComparison(List<ChemicalPropertiesForComparison> listOfChemicalProperties)
        {
            ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddChemicalDiagramsToComparison(listOfChemicalProperties, "curveName", 1, materialContextUow);
            return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
            return null;
        }
        

        [SessionExpire]
        public ActionResult AddMechanicalDiagramToComparison(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int conditionId, int sourcePropertyId, int propertyId)
        {
           ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddMechanicalDiagramToComparison(materialId, sourceMaterialId, sourceId, subgroupId, conditionId, "curveName", 1, materialContextUow, sourcePropertyId, propertyId);
           return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddPhysicalDiagramToComparison(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int conditionId, int sourcePropertyId, int propertyId)
        {
            ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddPhysicalDiagramToComparison(materialId, sourceMaterialId, sourceId, subgroupId, conditionId, "curveName", 1, materialContextUow, sourcePropertyId, propertyId);
            return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddStressStrain(int materialId, int sourceMaterialId, int sourceId, int subgroupId, double temperature, string materialConditionId = "", int testConditionId = 0, bool addForAllTemperatues = false)
        {
            ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddToComparisonTrueStressStrainDiagram(materialId, sourceMaterialId, sourceId, subgroupId, materialConditionId, testConditionId, temperature, materialContextUow, addForAllTemperatues);
           return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddSSToComparison(int materialId, int subgroupId, int sourceId, int sourceMaterialId, List<double> temperatures, string materialConditionId = "", int testConditionId = 0)
        {
            ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddToComparisonTrueSSDiagram(materialId, sourceMaterialId, sourceId, subgroupId, materialConditionId, testConditionId, temperatures,materialContextUow);
            return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult AddFatigue(int materialId, int sourceMaterialId, int sourceId, int subgroupId,  string selectedCurveName, string materialConditionId = "", string testConditionId = "")
        {
            ElsevierMaterials.Models.FatigueType fatigueType = ElsevierMaterials.Models.FatigueType.StrainLife;
             int curveType = -2;
             switch (selectedCurveName)
             {
                 case "plastic":
                     curveType = -2;
                     break;
                 case  "total":
                     curveType = -3;
                     break;
                 case "elastic":
                     curveType = -4;
                     break;
                 case "S-N curve":
                     curveType = -5;
                     fatigueType = ElsevierMaterials.Models.FatigueType.StressLife;
                     break;
                 default:
                     curveType = -5;                   
                     fatigueType = ElsevierMaterials.Models.FatigueType.StressLife;
                     break;
             }

             ComparisonSuccessMessage success = _ComparisonDiagramBinder.AddToComparisonFatigueDiagram(materialId, sourceMaterialId, sourceId, subgroupId, materialConditionId, testConditionId, fatigueType, curveType, selectedCurveName, materialContextUow);
             return Json(ResponseStatus.Success, new { success = success }, JsonRequestBehavior.AllowGet);
          
        }



        

        [SessionExpire]
        public ActionResult RemoveCurveFromInteractiveDiagram(int propertyid, int curveId)
        {
           ComparisonD comparison = _ComparisonDiagramBinder.GetComparisonDiagramModel();

           PropertyD property = comparison.Properties.Where(m => m.Id == propertyid).FirstOrDefault();

           int materialIdForRemoving = -1;
           foreach (var material in property.Materials)
           {
       
               string conditionIdForRemoving = null;

               foreach (var condition in material.Conditions)
               {
                   InterativeCurve curve = condition.Temperatures.Where(m => m.Id == curveId).FirstOrDefault();
                   if (curve != null)
                   {
                       condition.Temperatures.Remove(curve);
                       if (condition.Temperatures.Count == 0)
                       {
                           conditionIdForRemoving = condition.ConditionId;
                       }               
                   }
                  
               }
               if (conditionIdForRemoving != null)
               {
                   material.Conditions.Remove(material.Conditions.Where(m => m.ConditionId == conditionIdForRemoving).FirstOrDefault());
               }

               if (material.Conditions.Count == 0)
               {
                   materialIdForRemoving = material.MaterialId;
               }       
           }

        
           property.Materials.Remove(property.Materials.Where(m => m.MaterialId == materialIdForRemoving).FirstOrDefault());


           if (property.Materials.Count == 0)
           {
               comparison.Properties.Remove(property);

               if (comparison.Properties.Count == 0)
               {
                   return Json(ResponseStatus.Success, new { removedProperrty = true, comparisonHasPropeties = false }, JsonRequestBehavior.AllowGet);

           
               }
               else
               {

                   Comparison comparison1 = _basicBinder.GetComparison();
                   return Json(ResponseStatus.Success, new
                   {                       
                       data = RenderPartialViewToString("PropertiesWithDiagrams", comparison1),                    
                       removedProperrty = true, 
                       comparisonHasPropeties = true
                   }, JsonRequestBehavior.AllowGet);

                  
               }
           
           }
           else
           {
               int rowNumberDiagram = 0;
               foreach (var material1 in property.Materials)
               {
                   foreach (var condition1 in material1.Conditions)
                   {
                       foreach (var temperature1 in condition1.Temperatures)
                       {
                           rowNumberDiagram += 1;
                           temperature1.Id = rowNumberDiagram;
                       }
                   }
               }

           }



           return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("InteractiveDiagram", property), removedProperrty = false, comparisonHasPropeties = true}, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ShowInterctiveDiagramForProperty(int diagramId)
        {
            ComparisonD comparison = _ComparisonDiagramBinder.GetComparisonDiagramModel();
            PropertyD property = comparison.Properties.Where(m => m.Id == diagramId).FirstOrDefault();

            property.MaxYValue = _ComparisonDiagramBinder.GetMaxYValueForAllSeries(property);
            property.MaxXValue = _ComparisonDiagramBinder.GetMaxXValueForAllSeries(property);
            property.MinYValue = _ComparisonDiagramBinder.GetMinYValueForAllSeries(property);
            property.MinXValue = _ComparisonDiagramBinder.GetMinXValueForAllSeries(property);

            return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("InteractiveDiagram", property) }, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire]
        public ActionResult ShowInterctiveDiagramSelectedForProperty(int diagramId, string selectedXValue)
        {
            ComparisonD comparison = _ComparisonDiagramBinder.GetComparisonDiagramModel();
            PropertyD property = comparison.Properties.Where(m => m.Id == diagramId).FirstOrDefault();
            property.SelectedXName = selectedXValue;

            property.MaxYValue = _ComparisonDiagramBinder.GetMaxYValueForAllSeries(property);
            property.MaxXValue = _ComparisonDiagramBinder.GetMaxXValueForAllSeries(property);
            property.MinYValue = _ComparisonDiagramBinder.GetMinYValueForAllSeries(property);
            property.MinXValue = _ComparisonDiagramBinder.GetMinXValueForAllSeries(property);

            return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("InteractiveDiagramsSelected", property) }, JsonRequestBehavior.AllowGet);
        }


        [SessionExpire]
        public ActionResult RemoveInteractiveDiagram(int diagramId)
        {
            ComparisonD comparison1 = _ComparisonDiagramBinder.GetComparisonDiagramModel();
            comparison1.Properties.Remove(comparison1.Properties.Where(m => m.Id == diagramId).FirstOrDefault());
            Comparison comparison = _basicBinder.GetComparison();
            comparison.ComparisonD = comparison1;


            return Json(ResponseStatus.Success, new { 
                hasProperties = comparison.Properties.Count > 0 ? true : false,
                data = RenderPartialViewToString("PropertiesWithDiagrams", comparison),
                plot = RenderPartialViewToString("Plot", comparison), 
                model = Newtonsoft.Json.JsonConvert.SerializeObject(comparison) 
            }, JsonRequestBehavior.AllowGet);
        }
                

        [SessionExpire]
        public ActionResult SelectTemperaturesForStressStrain(List<float> arrayTemperatures)
        {
            return Json(ResponseStatus.Success, new { data = RenderPartialViewToString("SSSelectTemperatures", arrayTemperatures) }, JsonRequestBehavior.AllowGet);
        }
        public ComparisonController()
        {
            _binder = new ComparisonControllerBinder();
            _materialBinder = new ComparisonMaterialBinder();
            _basicBinder = new ComparisonBasicBinder();
            _diagramBinder = new ComparisonDiagramBinder();
            _ComparisonDiagramBinder = new ComparisonDiagramControllerBinder();
        }

        private ComparisonDiagramControllerBinder _ComparisonDiagramBinder { get; set; }
        private ComparisonControllerBinder _binder;
        private ComparisonMaterialBinder _materialBinder;
        private ComparisonBasicBinder _basicBinder;
        private ComparisonDiagramBinder _diagramBinder;
        
    }
}
