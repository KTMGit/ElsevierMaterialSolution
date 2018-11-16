using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.Filters;

using IniCore.Web.Mvc;
using ElsevierMaterials.Models.Domain.Export;

using System.Web.Script.Serialization;
using System.Globalization;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;
using ElsevierMaterialsMVC.BL.Binders.Exporter;
using ElsevierMaterialsMVC.BL.Global;

namespace ElsevierMaterialsMVC.Controllers
{
    public class ExporterController : ElsevierController
    {

        public ExporterController()
        {
            _binder = new ExporterBinder();
            _formaterBinder = new ExportFormaterBinder();
        }
        private ExporterBinder _binder;
        private ExportFormaterBinder _formaterBinder;

        [SessionExpire]
        public ActionResult Materials()
        {
            BreadcrumbNavigationGetSet();
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();
            foreach (var material in exporter.Materials)
            {
                material.Properties = material.Properties.OrderBy(m => m.ElsBasicInfo.Name).ToList();
            }
            return View("Exporter", exporter);
        }


        [SessionExpire]
        public ActionResult AddPropertiesForMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, List<PropertyFilter> properties)
        {            
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();

            List<PropertyFilter> propertiesForExport;
            string message = _formaterBinder.GetPropertiesForExport(sourceMaterialId, sourceId, subgroupId, properties, exporter, out propertiesForExport, materialContextUow);

            if (propertiesForExport.Count > 0)
            {
                Material material = exporter.Materials.Where(m => m.MaterialInfo.MaterialId == materialId && m.MaterialInfo.SourceMaterialId == sourceMaterialId && m.MaterialInfo.SourceId == sourceId && m.MaterialInfo.SubgroupId == subgroupId).FirstOrDefault();
                material = _binder.AddMaterial(exporter, material, materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);


                _binder.AddPropertiesForMaterial(sourceMaterialId, sourceId, subgroupId, propertiesForExport, material, materialContextUow);
            }
      
            return Json(ResponseStatus.Success, new { success = true, message = message }, JsonRequestBehavior.AllowGet);
        }
             
             

        [SessionExpire]
        public ActionResult ChangeMaterial(int rowId)
        {

            //TODO:-Progress indicator ChangeMaterial         
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();
            ElsevierMaterials.Models.Domain.Export.Material material = exporter.Materials.Where(m => m.MaterialInfo.RowId == rowId).FirstOrDefault();
            return Json(ResponseStatus.Success, RenderPartialViewToString("MaterialProperties", material), JsonRequestBehavior.AllowGet);

        }

        [SessionExpire]
        public ActionResult RemovePropertyFromMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int propertyId, int sourcePropertyId, int rowId)
        {
            //TODO:-na Layout home napravi partial views za popupove koje pozivas
            
          int materialRowIdForDeleting = _binder.RemovePropertyFromMaterial(materialId, sourceMaterialId, sourceId, subgroupId, propertyId, sourcePropertyId, rowId);

            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();

            ElsevierMaterials.Models.Domain.Export.Material material = exporter.Materials.Where(m => m.MaterialInfo.MaterialId == materialId && m.MaterialInfo.SourceMaterialId == sourceMaterialId && m.MaterialInfo.SourceId == sourceId && m.MaterialInfo.SubgroupId == subgroupId).FirstOrDefault();

            return Json(ResponseStatus.Success, new { hasMaterialsAdded = exporter.Materials.Count > 0 ? true: false , materialRowIdForDeleting = materialRowIdForDeleting, data = RenderPartialViewToString("MaterialProperties", material) }, JsonRequestBehavior.AllowGet);

        }

          [SessionExpire]
        public ActionResult RemoveMaterials(string materials)
        {
            var jsSer = new JavaScriptSerializer();
            int[] materialsInt = jsSer.Deserialize<int[]>(materials);
            bool hasMaterials = _binder.RemoveMaterials(materialsInt);

            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();

            return View("Exporter", exporter);

        }

    
        [SessionExpire]
        public ActionResult ExportData(string types, string materials)
        {
            var jsSer = new JavaScriptSerializer();
            string[] typesString = jsSer.Deserialize<string[]>(types);
            int[] materialsInt = jsSer.Deserialize<int[]>(materials);

            if (typesString.Length == 0)
            {
                return Json(IniCore.Web.Mvc.ResponseStatus.Error, new { message = "Please select at least one export type." });
            }
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = _binder.GetExporter();
            MemoryStream zipStream = _formaterBinder.ExportData(typesString, materialsInt, exporter);
            return File(zipStream.ToArray(), "application/octet-stream", "Export " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss",
                               CultureInfo.InvariantCulture) + ".zip");           
        }

        private void BreadcrumbNavigationGetSet()
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
            if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
            ComparisonNav cn = nav.GetOrderedItems().Where(n => n.NavigableID == "Comparison").FirstOrDefault() as ComparisonNav;
            if (cn != null)
            {
                cn.IsVisible = false;
            }
           
            ExporterNav en = nav.GetOrderedItems().Where(n => n.NavigableID == "Exporter").FirstOrDefault() as ExporterNav;
            if (en == null)
            {
                en = new ExporterNav();
            }
            else
            {

            }
            en.IsVisible = true;
            nav.LastNavigable = "Exporter";
            nav.Push(en);
            System.Web.HttpContext.Current.Session["Navigation"] = nav;

        }
    }
}
