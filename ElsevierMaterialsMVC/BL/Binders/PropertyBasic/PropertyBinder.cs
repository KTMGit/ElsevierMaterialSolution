using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.PropertyBasic
{
    public class PropertyBinder
    {
        public PropertyBasicInfo FillPropertyBasicData(IMaterialsContextUow context, PropertyFilter propertyClient, PropertyBasicInfo property)
        {
            if (property == null)
            {
                property = new PropertyBasicInfo();
            }
            property.TypeId = propertyClient.TypeId;
            property.SourceTypeId = propertyClient.SourceTypeId;
            property.GroupId = propertyClient.GroupId;
            property.RowId = propertyClient.RowId;
            property.ConditionId = propertyClient.ConditionId;
            property.Group = context.PreferredNames.Find(p => p.PN_ID == propertyClient.GroupId).PN;

            return property;
        }
        
        public string FillPropertyName(int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, Condition condition)
        {

            string name = "";
            EquivalentProperty eq = context.EquivalentProperties.AllAsNoTracking.Where(m => m.PropertyId == propertyClient.TypeId && m.SourceId == sourceId).FirstOrDefault();

            //TODO: Source stavi u Enum: 1- els, 2-TMMetals, 3-TMPlus
            switch (sourceId)
            {
                case 1:
                    name =  FillMappedPropertyName(context, propertyClient);
                    break;

                case 2:

                    if (eq != null)
                    {
                        name =  FillMappedPropertyName(context, propertyClient);
                    }
                    else
                    {                     
                        ElsevierMaterials.Models.Property propertyTm = FillNotMappedProperty(propertyClient, condition);
                        name =  propertyTm.PropertyName;
                    }
                    break;
                case 3:

                    if (eq != null)
                    {
                        name =  FillMappedPropertyName(context, propertyClient);
                    }

                    if (propertyClient.TypeId == 0 || eq == null)
                    {                       
                        ElsevierMaterials.Models.Property propertyTm = FillNotMappedProperty(propertyClient, condition);
                        if (propertyTm != null)
                        {
                            name = propertyTm.PropertyName;
                        }
                   
                    }                  
                    break;

                default:                  
                    break;

            }
            return name;

        }
        
        public string FillPropertyUnit(int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, Condition condition)
        {
            string unit = "";

            EquivalentProperty eq = context.EquivalentProperties.AllAsNoTracking.Where(m => m.PropertyId == propertyClient.TypeId && m.SourceId == sourceId).FirstOrDefault();

            switch (sourceId)
            {
                case 1:

                    unit = FillMappedPropertyUnit(eq);
                    break;
                case 2:

                    if (eq != null)
                    {

                        unit = FillMappedPropertyUnit(eq);
                    }
                    else
                    {
            
                        ElsevierMaterials.Models.Property propertyTm = FillNotMappedProperty(propertyClient, condition);
                        unit = propertyTm.OrigUnit;

                    }
                    break;
                case 3:

                    if (eq != null)
                    {

                        unit = FillMappedPropertyUnit(eq);
                    }

                    else if (propertyClient.TypeId == 0 || eq == null)
                    {
                      
                        ElsevierMaterials.Models.Property propertyTm = FillNotMappedProperty(propertyClient, condition);
                        if (propertyTm != null)
                        {
                            unit = propertyTm.OrigUnit;
                        }
                   

                    }
                    else
                    {
                        unit = "";
                    }

                    break;
                default:

                    unit = "";
                    break;

            }

            return unit.Replace("<sup>", "^").Replace("</sup>", "").Replace("<sub>", "").Replace("</sub>", "").Replace("&deg;", "°");

        }

        public string FillPropertyValue(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, Condition condition)
        {
            string value = "";
            switch (sourceId)
            {
                case 1:
                    value = _propertyElsBinder.GetPropertyValue(materialId, subgroupId, context, propertyClient);

                    break;
                case 2:

                    value = _propertyTMMetalsBinder.GetPropertyValue(propertyClient, condition);
                    break;

                case 3:

                    value = _propertyTMPlusBinder.GetPropertyValue(propertyClient, condition);
                    break;

                default:
                    break;
            }

            //TODO: formatiranje vrednosti izbaciti u utillity klasu
            if (value.Contains("<hr"))
            {
                value = value.Split(new string[] { "<hr" }, StringSplitOptions.None)[0];
            }

            if (value.Contains("&GreaterEqual;"))
            {
                value = value.Replace("&GreaterEqual;", "");
            }

            if (value.Contains("&le;"))
            {
                value = value.Replace("&le;", "");
            }

            if (value.Contains("-") && !value.Contains("E"))
            {
                if (value.Split('-')[0] != "")
                {
                    value = value.Split('-')[0];
                }               
            }



            return value;
        }

        public string FillMappedPropertyUnit(EquivalentProperty eq)
        {
            if (eq.DefaultUnitName != null)
            {
                return eq.DefaultUnitName;
            }
            else
            {
                return "";
            }
        }

        public double FillPropertyTemperature(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient, Condition condition)
        {
            double temperature = 20;
            switch (sourceId)
            {
                case 1:
                    temperature = _propertyElsBinder.GetPropertyTemperature(materialId, subgroupId, context, propertyClient);
                    break;
                case 2:
                    temperature = _propertyTMMetalsBinder.GetPropertyTemperature(propertyClient, condition);
                    break;
                case 3:
                    temperature = _propertyTMPlusBinder.GetPropertyTemperature(propertyClient, condition);
                    break;
                default:

                    break;
            }
            return temperature;
        }
        
        public string FillMappedPropertyName(IMaterialsContextUow context, PropertyFilter propertyClient)
        {
            //TODO: verovatno treba da je taxonomy id =2 i uradjen innerjoin sa proerties???
            return context.PreferredNames.Find(p => p.PN_ID == propertyClient.TypeId).PN;
        }

        public Property FillNotMappedProperty(PropertyFilter propertyClient, Condition condition)
        {
            ElsevierMaterials.Models.Property propertyTm = null;

            if (propertyClient.SourceTypeId > 0)
            {
                propertyTm = condition.Properties.Where(m => m.SourcePropertyId == propertyClient.SourceTypeId).FirstOrDefault();
            }
            else
            {
                //Chemical composition
                propertyTm = condition.Properties.Where(m => m.ValueId == propertyClient.RowId).FirstOrDefault();
            }
            return propertyTm;
        }

        
        public PropertyBinder()
        {          
            _propertyElsBinder = new PropertyElsBinder();
            _propertyTMMetalsBinder = new PropertyTMMetalsBinder();
            _propertyTMPlusBinder = new PropertyTMPlusBinder();
        }
                
        private PropertyElsBinder _propertyElsBinder;
        private PropertyTMMetalsBinder _propertyTMMetalsBinder;
        private PropertyTMPlusBinder _propertyTMPlusBinder;

    }
}