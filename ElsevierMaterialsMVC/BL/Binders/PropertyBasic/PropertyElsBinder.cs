using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.Property;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.PropertyBasic
{

    //TODO: Nparaviti interfejs koji ce imati metode GetPropertyValue, GetPropertyTemperature i koji ce ova klasa implementirati
    public class PropertyElsBinder
    {         


        //TODO: Ova metoda moze bolje da se napise!
         public string GetPropertyValue(int materialId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient)
         {
             PropertyWithConvertedValues prop = context.PropertiesWithConvertedValues.All.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.GroupId == propertyClient.GroupId && m.RowId == propertyClient.ConditionId && m.PropertyId == propertyClient.TypeId).FirstOrDefault();

             string value = "";
             string unit = "";
             string note = "";
             string text = "";
             FillValueForPropertyUnitType(prop.ConvValue, prop.ConvValueMin, prop.ConvValueMax, prop.OrigValueText, text, prop.Temperature, prop.DefaultUnit, out value, out note, out unit);
             return value;
         }

         public double GetPropertyTemperature(int materialId, int subgroupId, IMaterialsContextUow context, PropertyFilter propertyClient)
         {
             MaterialProperty propCond = context.MaterialProperties.All.Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId && m.RowId == propertyClient.ConditionId && m.PropertyId == propertyClient.TypeId && m.ValueId == propertyClient.RowId).FirstOrDefault();

             if (propCond.Temperature != null)
             {
                 return (double)propCond.Temperature;
             }
             else
             {              
                 return 20;
             }
         }

         public Property GetPropertyDataNew(int materialId, int subgroupId, GroupTestCondition record, PropertyWithConvertedValues prop)
         {

             Property propertyObj = new Property()
             {
                 PropertyId = prop.PropertyId,
                 SourcePropertyId = 0,
                 SourceId = 1,
                 DefaultUnitId = 1,
                 MaterialId = materialId,
                 SubgroupId = subgroupId,
                 RowId = record.RowID,
                 ValueId = prop.ValueId
             };


             propertyObj.PropertyName = prop.PropertyName;


             string value = "";
             string unit = "";
             string note = "";
             string text = "";
             if (prop.AdditionalCondition != null && prop.SpecimenOrientation != null)
             {
                 text = prop.AdditionalCondition + "; specimen orientation: " + prop.SpecimenOrientation;
             }

             if (prop.AdditionalCondition == null && prop.SpecimenOrientation != null)
             {
                 text = "specimen orientation: " + prop.SpecimenOrientation;
             }

             if (prop.AdditionalCondition != null && prop.SpecimenOrientation == null)
             {
                 text = prop.AdditionalCondition;
             }

             FillValueForPropertyUnitType(prop.OrigValue, prop.OrigValueMin, prop.OrigValueMax, prop.OrigValueText, text, prop.Temperature, prop.OriginalUnit, out value, out note, out unit);
             propertyObj.OrigValue = value;
             propertyObj.OrigUnit = unit;
             propertyObj.OrigValueText = note;

             //TODO: prop.OrigValueText
             FillValueForPropertyUnitType(prop.ConvValue, prop.ConvValueMin, prop.ConvValueMax, prop.OrigValueText, text, prop.Temperature, prop.DefaultUnit, out value, out note, out unit);
             propertyObj.DeafaultValue = value;
             propertyObj.DeafaultUnit = unit;
             propertyObj.DeafaultValueText = note;

             //TODO: prop.OrigValueText
             FillValueForPropertyUnitType(prop.UsValue, prop.UsValueMin, prop.UsValueMax, prop.OrigValueText, text, prop.Temperature, prop.UsUnit, out value, out note, out unit);
             propertyObj.USValue = value;
             propertyObj.UStUnit = unit;
             propertyObj.USValueText = note;

             //TODO: Treba izbaciti, mislim da je visak
             if (prop.ConvValue == null)
             {
                 if (prop.ConvValueMin != null && prop.ConvValueMax != null) propertyObj.ConvValue = ((double)prop.ConvValueMin).ToString("0.####") + "-" + ((double)prop.ConvValueMax).ToString("0.##");
                 if (prop.ConvValueMin != null && prop.ConvValueMax == null) propertyObj.ConvValue = "&le;" + ((double)prop.ConvValueMin).ToString("0.####");
                 if (prop.ConvValueMax != null && prop.ConvValueMin == null) propertyObj.ConvValue = "&GreaterEqual;" + ((double)prop.ConvValueMax).ToString("0.####");
                 if (prop.ConvValueMax == null && prop.ConvValueMin == null) propertyObj.ConvValue = "";
             }
             else
             {
                 propertyObj.ConvValue = prop.ConvValue.ToString();
             }

             return propertyObj;
         }             

                
         //public Property GetPropertyData(int materialId, int subgroupId, GroupCondition record, PropertyWithConvertedValues prop)
         //{

         //    Property propertyObj = new Property() {
         //        PropertyId = prop.PropertyId,
         //        SourcePropertyId = 0, 
         //        SourceId = 1, 
         //        DefaultUnitId = 1, 
         //        MaterialId = materialId, 
         //        SubgroupId = subgroupId,
         //        RowId = record.RowID,
         //        ValueId = prop.ValueId 
         //    };
           
             
         //    propertyObj.PropertyName = prop.PropertyName;       
        

         //    string value = "";
         //    string unit = "";
         //    string note = "";
         //    string text = "";
         //    if (prop.AdditionalCondition != null && prop.SpecimenOrientation != null)
         //    {
         //        text = prop.AdditionalCondition + "; specimen orientation: " + prop.SpecimenOrientation;
         //    }

         //    if (prop.AdditionalCondition == null && prop.SpecimenOrientation != null)
         //    {
         //        text = "specimen orientation: " + prop.SpecimenOrientation;
         //    }

         //    if (prop.AdditionalCondition != null && prop.SpecimenOrientation == null)
         //    {
         //        text = prop.AdditionalCondition;
         //    }
                        
         //    FillValueForPropertyUnitType(prop.OrigValue, prop.OrigValueMin, prop.OrigValueMax, prop.OrigValueText, text, prop.Temperature, prop.OriginalUnit, out value, out note, out unit);
         //    propertyObj.OrigValue = value;
         //    propertyObj.OrigUnit = unit;
         //    propertyObj.OrigValueText = note;

         //    //TODO: prop.OrigValueText
         //    FillValueForPropertyUnitType(prop.ConvValue, prop.ConvValueMin, prop.ConvValueMax, prop.OrigValueText, text, prop.Temperature, prop.DefaultUnit, out value, out note, out unit);
         //    propertyObj.DeafaultValue = value;
         //    propertyObj.DeafaultUnit = unit;
         //    propertyObj.DeafaultValueText = note;

         //    //TODO: prop.OrigValueText
         //    FillValueForPropertyUnitType(prop.UsValue, prop.UsValueMin, prop.UsValueMax, prop.OrigValueText, text, prop.Temperature, prop.UsUnit, out value, out note, out unit);
         //    propertyObj.USValue = value;
         //    propertyObj.UStUnit = unit;
         //    propertyObj.USValueText = note;

         //    //TODO: Treba izbaciti, mislim da je visak
         //    if (prop.ConvValue == null)
         //    {
         //        if (prop.ConvValueMin != null && prop.ConvValueMax != null) propertyObj.ConvValue = ((double)prop.ConvValueMin).ToString("0.####") + "-" + ((double)prop.ConvValueMax).ToString("0.##");
         //        if (prop.ConvValueMin != null && prop.ConvValueMax == null) propertyObj.ConvValue = "&le;" + ((double)prop.ConvValueMin).ToString("0.####");
         //        if (prop.ConvValueMax != null && prop.ConvValueMin == null) propertyObj.ConvValue = "&GreaterEqual;" + ((double)prop.ConvValueMax).ToString("0.####");
         //        if (prop.ConvValueMax == null && prop.ConvValueMin == null) propertyObj.ConvValue = "";
         //    }
         //    else
         //    {
         //        propertyObj.ConvValue = prop.ConvValue.ToString();
         //    }
           
         //    return propertyObj;
         //}             

         private void FillValueForPropertyUnitType(double? value, double? valuMin, double? valueMax, string valueText,  string additionalCondition, double? temperature, string unit, out string propValue, out string propertyNote, out string propertyUnit)
         {
              propValue = "";
              propertyNote = "";
              propertyUnit = "";

             if (value == null && valueMax == null && valuMin == null)
             {
                 if (!string.IsNullOrEmpty(valueText))
                 {
                     propValue = valueText;
                     if (!string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = additionalCondition + " ;" + temperature.ToString() + "°C";
                     if (!string.IsNullOrEmpty(additionalCondition) && temperature == null) propertyNote = additionalCondition;
                     if (string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = temperature.ToString() + "°C";
                 }
             }
             else
             {
                 if (!string.IsNullOrEmpty(valueText))
                 {

                     if (!string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = valueText + " ;" + additionalCondition + " ;" + temperature.ToString() + "°C";
                     if (!string.IsNullOrEmpty(additionalCondition) && temperature == null) propertyNote = valueText + " ;" + additionalCondition;
                     if (string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = valueText + " ;" + temperature.ToString() + "°C";
                 }
                 else
                 {

                     if (!string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = additionalCondition + " ;" + temperature.ToString() + "°C";
                     if (!string.IsNullOrEmpty(additionalCondition) && temperature == null) propertyNote = additionalCondition;
                     if (string.IsNullOrEmpty(additionalCondition) && temperature != null) propertyNote = temperature.ToString() + "°C";
                 }


                 if (value == null)
                 {
                     if (valuMin != null && valueMax != null) propValue = ((double)valuMin).ToString("0.####") + "-" + ((double)valueMax).ToString("0.##");
                     if (valuMin != null && valueMax == null) propValue = "&GreaterEqual;" + ((double)valuMin).ToString("0.####");
                     if (valueMax != null && valuMin == null) propValue = "&le;" + ((double)valueMax).ToString("0.####");
                     if (valueMax == null && valuMin == null) propValue = "";
                 }
                 else
                 {
                     propValue = value.ToString();
                 }
             }


             propertyUnit = unit;        

         }
            
    }
}