using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models.Domain.Export;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using ElsevierMaterials.Exporter.Models;
using ElsevierMaterials.Exporter.Formats;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;


namespace ElsevierMaterialsMVC.BL.Binders.Exporter
{
    public class ExporterBinder
    {
           
        public Material AddMaterial(ElsevierMaterials.Models.Domain.Export.Exporter exporter, Material material, int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow materialContextUow)
        {
            if (material == null)
            {
                ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo materialBasicInfo = _materialBinder.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);

                material = new Material();
                material.MaterialInfo.MaterialId = materialId;
                material.MaterialInfo.SourceMaterialId = sourceMaterialId;
                material.MaterialInfo.SourceId = sourceId;
                material.MaterialInfo.SubgroupId = subgroupId;
                material.MaterialInfo.Name = materialBasicInfo.Name;
                material.MaterialInfo.SubgroupName = materialBasicInfo.SubgroupName;
                if (exporter.Materials.Count > 0)
                {
                    material.MaterialInfo.RowId = exporter.Materials.Max(m => m.MaterialInfo.RowId) + 1; 
                }
                else
                {
                    material.MaterialInfo.RowId = 1; 
                }

            
                material.MaterialInfo.TypeId = materialBasicInfo.TypeId;
                material.MaterialInfo.TypeName = materialBasicInfo.TypeName;
                material.MaterialInfo.Standard = materialBasicInfo.Standard;
                exporter.Materials.Add(material);
            }
            return material;
        }


        public void AddPropertiesForMaterial(int sourceMaterialId, int sourceId, int subgroupId, List<PropertyFilter> propertiesForExport, Material material, IMaterialsContextUow materialContextUow)
        {
            foreach (var propertyClient in propertiesForExport)
            {

                Property property = material.Properties.Where(m => m.ElsBasicInfo.GroupId == propertyClient.GroupId && m.ElsBasicInfo.TypeId == propertyClient.TypeId && m.ElsBasicInfo.SourceTypeId == propertyClient.SourceTypeId).FirstOrDefault();

                ElsevierMaterials.Models.Condition condition = _conditionBinder.FillCondition(sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient);

                property = AddProperty(sourceMaterialId, sourceId, subgroupId, material, property, materialContextUow, propertyClient, condition);
            }            
        }



        public ElsevierMaterials.Models.Domain.Export.Property AddProperty(int sourceMaterialId, int sourceId, int subgroupId, ElsevierMaterials.Models.Domain.Export.Material material, ElsevierMaterials.Models.Domain.Export.Property property, IMaterialsContextUow materialContextUow, PropertyFilter propertyClient, ElsevierMaterials.Models.Condition condition)
        {            
            if (property == null)
            {
                property = new ElsevierMaterials.Models.Domain.Export.Property();
                PropertyBasicInfo propertyInfo = _propertyBinder.FillPropertyBasicData(materialContextUow, propertyClient, null);
                property.ElsBasicInfo = propertyInfo;
                property.ElsBasicInfo.Name = _propertyBinder.FillPropertyName(sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);
                property.ElsBasicInfo.Unit = _propertyBinder.FillPropertyUnit(sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);      
                material.Properties.Add(property);
            }         

            property.Value = _propertyBinder.FillPropertyValue(material.MaterialInfo.MaterialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);
            property.Temperature = _propertyBinder.FillPropertyTemperature(material.MaterialInfo.MaterialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, propertyClient, condition);
            return property;
        }


        public int RemovePropertyFromMaterial(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int propertyId, int sourcePropertyId, int rowId)
        {
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = GetExporter();

            ElsevierMaterials.Models.Domain.Export.Material material = exporter.Materials.Where(m => m.MaterialInfo.MaterialId == materialId && m.MaterialInfo.SourceMaterialId == sourceMaterialId && m.MaterialInfo.SourceId == sourceId && m.MaterialInfo.SubgroupId == subgroupId).FirstOrDefault();

          material.Properties.Remove(material.Properties.Where(m => m.ElsBasicInfo.TypeId == propertyId && m.ElsBasicInfo.SourceTypeId == sourcePropertyId && m.ElsBasicInfo.RowId == rowId).FirstOrDefault());

          Material materialForDelete = null;
         
          foreach (var materialobj in exporter.Materials)
          {
              if (materialobj.Properties.Count == 0)
              {
                  materialForDelete = materialobj;
                  break;
              }
          }

          int materialRowId = -1;
          if (materialForDelete != null)
          {
              materialRowId = materialForDelete.MaterialInfo.RowId;
              exporter.Materials.Remove(materialForDelete);
          }

            System.Web.HttpContext.Current.Session["Exporter"] = exporter;
            return materialRowId;

        }

        public bool RemoveMaterials(int[] materials)
        {
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = GetExporter();
            foreach (var rowId in materials)
            {
                exporter.Materials.Remove(exporter.Materials.Where(m => m.MaterialInfo.RowId == rowId).FirstOrDefault());
            }
            System.Web.HttpContext.Current.Session["Exporter"] = exporter;
            return true;
        }


   
        public ElsevierMaterials.Models.Domain.Export.Exporter GetExporter()
        {
            //TODO: Ogar predlaze dase cuva u bazi. mozda
            ElsevierMaterials.Models.Domain.Export.Exporter exporter = System.Web.HttpContext.Current.Session["Exporter"] as ElsevierMaterials.Models.Domain.Export.Exporter;
            if (exporter != null)
            {
                return exporter;
            }
            else
            {
                exporter = new ElsevierMaterials.Models.Domain.Export.Exporter();
                System.Web.HttpContext.Current.Session["Exporter"] = exporter;
                return exporter;
            }
        }

        public ExporterBinder()
        {
            _conditionBinder = new ConditionBinder();   
            _materialBinder = new MaterialBinder();
            _propertyBinder = new PropertyBinder();

        }

        private ConditionBinder _conditionBinder;
        private MaterialBinder _materialBinder;
        private PropertyBinder _propertyBinder;
        


    }
}