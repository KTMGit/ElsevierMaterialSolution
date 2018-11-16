
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.PropertyBasic;

namespace ElsevierMaterialsMVC.BL.Binders.Comparison
{
    public class ComparisonPropertyBinder : ComparisonBasicBinder
    {
        public ComparisonPropertyBinder()
        {          
            _propertyBinder = new PropertyBinder();
        }

        private PropertyBinder _propertyBinder;


        public ElsevierMaterials.Models.Domain.Comparison.Property FillPropertyData(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, ElsevierMaterials.Models.Condition condition)
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();

            PropertyBasicInfo propertyInfo = _propertyBinder.FillPropertyBasicData(context, propertyClient, null);      

            ElsevierMaterials.Models.Domain.Comparison.Property property = new ElsevierMaterials.Models.Domain.Comparison.Property();
            property.PropertyInfo = propertyInfo;        
            property.PropertyInfo.Name = _propertyBinder.FillPropertyName(sourceMaterialId, sourceId, subgroupId, context, propertyClient, condition);
            property.PropertyInfo.Unit = _propertyBinder.FillPropertyUnit(sourceMaterialId, sourceId, subgroupId, context, propertyClient,condition);

           return property;
        }
        

        public bool RemoveProperty(int propertyId, int sourcePropertyId, int rowId)
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = GetComparison();

            if (propertyId == 0 && sourcePropertyId == 0 )
            {
                comparison.Properties.Remove(comparison.Properties.Where(m => m.PropertyInfo.RowId == rowId).FirstOrDefault());
            }
            else
            {
                comparison.Properties.Remove(comparison.Properties.Where(m => m.PropertyInfo.TypeId == propertyId && m.PropertyInfo.SourceTypeId == sourcePropertyId).FirstOrDefault());
            }
         
            return true;
        }

    }
}