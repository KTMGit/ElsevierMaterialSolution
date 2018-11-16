using Api.Models.StressStrain;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.ComparisonDiagram;
using ElsevierMaterials.Models.Domain.Material;
using ElsevierMaterials.Services;
using ElsevierMaterialsMVC.BL.Binders.Comparison;
using ElsevierMaterialsMVC.BL.Binders.ConditionBasic;
using ElsevierMaterialsMVC.BL.Binders.MaterialBasic;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using ElsevierMaterialsMVC.Models.Comparison;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.ComparisonDiagram
{

    public enum ComparisonSuccessMessage {     
        None = 0,
        Success=1,
        AddedMaxNumberOfMaterials = 2,
        AddedMaxNumberOfCurves = 3
    }

    public class ComparisonDiagramControllerBinder
    {
        public ComparisonDiagramControllerBinder()
        {
            _materialBinder = new MaterialBinder();
            _conditionTMBinder = new ConditionTMMetalsBinder();
        }
        private MaterialBinder _materialBinder;
        private ConditionTMMetalsBinder _conditionTMBinder;


        public ComparisonSuccessMessage AddMechanicalDiagramToComparison(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int conditionId, string selectedCurveName, int curveType, IMaterialsContextUow materialContextUow, int sourcePropertyId, int propertyId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();    
            Condition cond = new ConditionTMMetalsBinder().FillMechanicalConditionData(subgroupId, sourceMaterialId, conditionId, materialContextUow, 1, sessionId);
            string groupName =  "mechanical properties";
            IList<Property> properties = cond.Properties.Where(m => m.SourcePropertyId == sourcePropertyId).ToList();
            curveType = properties[0].SourcePropertyId;
            string propertyUnit = properties[0].OrigUnit;
            string propertyName = properties[0].PropertyName;

            return AddToComparisonProperty(materialId, sourceMaterialId, sourceId, subgroupId, conditionId.ToString(), cond.ConditionName, curveType, materialContextUow, properties, groupName,GroupTypeEnum.Mechanical, propertyUnit, propertyName);            
        }

        public ComparisonSuccessMessage AddPhysicalDiagramToComparison(int materialId, int sourceMaterialId, int sourceId, int subgroupId, int conditionId, string selectedCurveName, int curveType, IMaterialsContextUow materialContextUow, int sourcePropertyId, int propertyId)
        {
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();  
            Condition cond = new ConditionTMMetalsBinder().FillPhysicalConditionData(sourceMaterialId, conditionId, materialContextUow, 1, sessionId);
            string groupName = "physical properties";
            IList<Property> properties = cond.Properties.Where(m => m.SourcePropertyId == sourcePropertyId).ToList();
            curveType = properties[0].SourcePropertyId;
            string propertyUnit = properties[0].OrigUnit;
            string propertyName = properties[0].PropertyName;

            return AddToComparisonProperty(materialId, sourceMaterialId, sourceId, subgroupId, conditionId.ToString(), cond.ConditionName, curveType, materialContextUow, properties, groupName, GroupTypeEnum.Physical, propertyUnit, propertyName);            
          
        }


        private ComparisonSuccessMessage AddToComparisonProperty(int materialId, int sourceMaterialId, int sourceId, int subgroupId, string conditionId, string conditionName, int curveType, IMaterialsContextUow materialContextUow, IList<Property> properties, string groupName,GroupTypeEnum groupId, string propertyUnit, string propertyName)
        {
            ComparisonD comparison = GetComparisonDiagramModel();
            if (comparison != null && IsAddedMaxNumberOfMaterilas(materialId, sourceMaterialId, sourceId) == true)
            {
                return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;
            }
            else
            {
                if (comparison == null)
                {
                    comparison = AddComparisonDiagramModel();
                }

                IService service = new TotalMateriaService();

                PropertyD property = GetPropertyObject(curveType, comparison, propertyUnit, propertyName, groupName, groupId);

                if (IsAddedMaxNumberOfCurves(property) == false)
                {

                    MaterialD material = GetMaterialObject(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow, comparison, property);

                    ConditionD condition = GetConditionObject(material, conditionId, conditionName);

                    InterativeCurve curveObj = GetCurveObject(propertyUnit, propertyName, condition,"°C");

                    curveObj.PointsForDiagram = properties.Select(m => new PointD { X = m.Temperature.ToString(), Y = m.OrigValue.Replace("≥", "").Replace("&LessEqual;", "").Replace("&GreaterEqual;", "").Replace("≤", "").Trim() }).ToList();
                    curveObj.Points = curveObj.PointsForDiagram;

                    SetCurvesIdsForProperty(property);

                    System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;

                    return ComparisonSuccessMessage.Success;
                }
                else
                {
                    return ComparisonSuccessMessage.AddedMaxNumberOfCurves;
                }
            }
        }

        private void SetCurvesIdsForProperty(PropertyD property)
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

        private InterativeCurve GetCurveObject(string propertyUnit, string propertyName, ConditionD condition, string variable)
        {
            InterativeCurve temperatureObj = null;
            if (!IsAddedTemperature(condition, 20, variable))
            {
                string xName = "";
                switch (variable)
	            {
		        case "K":
                        xName = "temperature";
                        break;
	            case "°C":
                        xName = "temperature";
                        break;
	            case "kPa":
                        xName = "pressure";
                        break;
	            case "nm":	 
                        xName = "wavelength";
                        break;
	            default:
                        xName = "temperature";
                        break;
	            } 
                string yName = propertyName;
                string xUnit = variable;
                string yUnit = propertyUnit;

                temperatureObj = AddTemperature(condition, 20, xName, yName, xUnit, yUnit);
            }
            else
            {
                temperatureObj = GetTemperature(condition, 20);
            }
            return temperatureObj;
        }

        private ConditionD GetConditionObject(MaterialD material, string conditionId, string conditionName)
        {
            ConditionD condition = null;

            if (!IsAddedCondition(material, conditionId))
            {

                condition = AddCondition(material, "0", conditionId, conditionName);
            }
            else
            {
                condition = GetCondition(material, conditionId);
            }
            return condition;
        }

        private MaterialD GetMaterialObject(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow materialContextUow, ComparisonD comparison, PropertyD property)
        {
            MaterialD material = null;
            if (!IsAddedMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId))
            {
                material = AddMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
            }
            else
            {
                material = GetMaterial(property, materialId, sourceMaterialId, sourceId, subgroupId);
            }
            return material;
        }

        private PropertyD GetPropertyObject(int curveType, ComparisonD comparison, string propertyUnit, string propertyName, string groupName, GroupTypeEnum groupId)
        {
            PropertyD property = null;
            if (!IsAddedProperty(comparison, curveType))
            {
                property = AddProperty(comparison, curveType, propertyName, propertyUnit, groupName, groupId);
            }
            else
            {
                property = GetProperty(comparison, curveType);
            }
            return property;
        }


        public ComparisonSuccessMessage AddChemicalDiagramToComparison(int materialId, int subgroupId, int conditionId, string selectedCurveName, int curveType, IMaterialsContextUow materialContextUow, int propertyId)
        {

            ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder condBinder = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder();

            var findGroupId = materialContextUow.Trees.AllAsNoTracking.Where(m => m.PN_ID == propertyId).Select(m => m.BT_ID).FirstOrDefault();
            int groupId = findGroupId == null ? 139994 : (int)findGroupId;

            IList<ChemicalPropertyModel> chemicalProperties = condBinder.GetChemicalPropertiesData(materialId, subgroupId, groupId, materialContextUow);
            ChemicalPropertyModel ChemicalProperty = chemicalProperties.Where(p => p.PropertyId == propertyId).FirstOrDefault();
            if (ChemicalProperty != null)
            {
                ChemicalConditionModel cond = ChemicalProperty.ConditionList.Where(m => m.RowId == conditionId).FirstOrDefault();

                string groupName = "thermal properties";

                curveType = propertyId;
                string propertyUnit = "";
                string propertyName = ChemicalProperty.PropertyName;

                ComparisonD comparison = GetComparisonDiagramModel();
                if (comparison != null && IsAddedMaxNumberOfMaterilas(materialId, materialId, 1) == true)
                {
                    return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;
                }
                else
                {
                    if (comparison == null)
                    {
                        comparison = AddComparisonDiagramModel();
                    }

                    IService service = new TotalMateriaService();

                    PropertyD property = GetPropertyObject(curveType, comparison, propertyUnit, propertyName, groupName, GroupTypeEnum.Thermal);

                    if (IsAddedMaxNumberOfCurves(property) == false)
                    {

                        MaterialD material = GetMaterialObject(materialId, materialId, 1, subgroupId, materialContextUow, comparison, property);

                        ConditionD condition = GetConditionObject(material, conditionId.ToString(), cond.Name);

                        InterativeCurve curveObj = GetCurveObject(propertyUnit, propertyName, condition, "°C");

                        curveObj.PointsForDiagram = cond.TemperatureList.Where(p => p.type == ChemicalUnitValuesType.Origin).Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                        curveObj.Points = curveObj.PointsForDiagram;

                        SetCurvesIdsForProperty(property);

                        System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;

                        return ComparisonSuccessMessage.Success;
                    }
                    else
                    {
                        return ComparisonSuccessMessage.AddedMaxNumberOfCurves;
                    }
                }

                return ComparisonSuccessMessage.Success;
            }
            return ComparisonSuccessMessage.None;
        }

        public ComparisonSuccessMessage AddChemicalDiagramsToComparison(List<ChemicalPropertiesForComparison> listOfChemicalProperties, string selectedCurveName, int curveType, IMaterialsContextUow materialContextUow)
        {

            bool success = true;

            foreach (var item in listOfChemicalProperties)
            {
                ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder condBinder = new ElsevierMaterialsMVC.BL.Binders.ConditionBasic.ConditionElsBinder();

                var findGroupId = materialContextUow.Trees.AllAsNoTracking.Where(m => m.PN_ID == item.PropertyId).Select(m => m.BT_ID).FirstOrDefault();
                int groupId = findGroupId == null ? 139994 : (int)findGroupId;
                
                string groupName = findGroupId == null ? "thermal properties": materialContextUow.PreferredNames.AllAsNoTracking.Where(p => p.PN_ID == groupId && p.taxonomy_id == 2).Select(p=>p.PN).FirstOrDefault();

                IList<ChemicalPropertyModel> chemicalProperties = condBinder.GetChemicalPropertiesData(item.MaterialId, item.SubgroupId, groupId, materialContextUow);
                ChemicalPropertyModel ChemicalProperty = chemicalProperties.Where(p => p.PropertyId == item.PropertyId).FirstOrDefault();
                if (ChemicalProperty != null)
                {
                    ChemicalConditionModel cond = ChemicalProperty.ConditionList.Where(m => m.RowId == item.ConditionId).FirstOrDefault();

                    curveType = item.PropertyId;
                    string propertyUnit = cond.DefaultPropertyUnits;
                    string propertyName = ChemicalProperty.PropertyName;

                    ComparisonD comparison = GetComparisonDiagramModel();
                    if (comparison != null && IsAddedMaxNumberOfMaterilas(item.MaterialId, item.MaterialId, 1) == true)
                    {
                        return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;
                    }
                    else
                    {
                        if (comparison == null)
                        {
                            comparison = AddComparisonDiagramModel();
                        }

                        IService service = new TotalMateriaService();
                        PropertyD property = GetPropertyObject(curveType, comparison, propertyUnit, propertyName, groupName, GroupTypeEnum.Thermal);
                        if (IsAddedMaxNumberOfCurves(property) == false)
                        {
                            MaterialD material = GetMaterialObject(item.MaterialId, item.MaterialId, 1, item.SubgroupId, materialContextUow, comparison, property);
                            ConditionD condition = GetConditionObject(material, item.ConditionId.ToString(), cond.Name);
                            InterativeCurve curveObj = GetCurveObject(propertyUnit, propertyName, condition, item.Variable);
                            curveObj.PointsForDiagram = cond.TemperatureList.Where(p => p.type == ChemicalUnitValuesType.Origin).Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                            curveObj.Points = curveObj.PointsForDiagram;
                            SetCurvesIdsForProperty(property);
                            System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;
                            success = success & true;
                        }
                        else
                        {
                            return ComparisonSuccessMessage.AddedMaxNumberOfCurves;
                        }
                    }                    
                }
                else
                {
                    success = success & false;
                }
                
            }
            if (success)
            {
                return ComparisonSuccessMessage.Success;
            }
            else
            {
                return ComparisonSuccessMessage.None;
            }
            
        }
       
        public ComparisonSuccessMessage AddToComparisonFatigueDiagram(int materialId, int sourceMaterialId, int sourceId, int subgroupId, string materialConditionId, string testConditionId, FatigueType fatigueType, int curveType, string selectedCurveName, IMaterialsContextUow materialContextUow)
        {

            
            ComparisonD comparison = GetComparisonDiagramModel();
            if (comparison != null && IsAddedMaxNumberOfMaterilas(materialId, sourceMaterialId, sourceId) == true)
            {
                return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;
            }
            else
            {
                string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
                IService service = new TotalMateriaService();


                if (comparison == null)
                {
                    comparison = AddComparisonDiagramModel();
                }

                PropertyD property = null;

                if (selectedCurveName == "+st.dev" || selectedCurveName == "-st.dev")
                {
                    selectedCurveName = "S-N curve";
                }

                if (!IsAddedProperty(comparison, curveType))
                {
                    string propertyUnit = "";
                    string propertyName = "strain life - " + selectedCurveName;
                    if (fatigueType == FatigueType.StressLife)
                    {
                        propertyUnit = "MPa";
                        propertyName = "stress life - " + selectedCurveName;
                    }
                    property = AddProperty(comparison, curveType, propertyName, propertyUnit, "fatigue", GroupTypeEnum.FatigueData);
                }
                else
                {
                    property = GetProperty(comparison, curveType);
                }


                if (IsAddedMaxNumberOfCurves(property) == false)
                {


                    MaterialD material = null;
                    if (!IsAddedMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId))
                    {
                        material = AddMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    }
                    else
                    {
                        material = GetMaterial(property, materialId, sourceMaterialId, sourceId, subgroupId);
                    }

                    ConditionD condition = null;

                    if (!IsAddedCondition(material, testConditionId))
                    {
                        IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetFatigueMaterialConditionsFromService(sessionId, sourceMaterialId, (int)fatigueType);
                        if (materialConditionId == "" && materialConditions.Count == 1)
                        {
                            materialConditionId = materialConditions[0].ConditionId;
                        }

                         ElsevierMaterials.Models.MaterialCondition  materialCondition = materialConditions.Where(m => m.ConditionId == materialConditionId).FirstOrDefault();
                        IList<Api.Models.TestCondition> tempConds = service.GetFatigueTestConditionsFromService(sessionId, sourceMaterialId, materialConditionId, (int)fatigueType);
                        Api.Models.TestCondition testCondition= tempConds.Where(m => m.ConditionId == testConditionId).FirstOrDefault();

                        string conditionText = "";
                        if (materialCondition != null)
                        {
                            conditionText = conditionText + materialCondition.Description;
                        }
                        if (materialCondition != null && testCondition != null)
                        {
                            conditionText = conditionText + "; " + testCondition.Description;
                        }
                        if (materialCondition == null && testCondition != null)
                        {
                            conditionText = conditionText + testCondition.Description;
                        }


                        condition = AddCondition(material, materialConditionId, testConditionId, conditionText);
                    }
                    else
                    {
                        condition = GetCondition(material, testConditionId);
                    }

                    InterativeCurve temperatureObj = null;
                    if (!IsAddedTemperature(condition, 20, "°C"))
                    {
                        string xName = "Reversals to failure, 2Nf";
                        string yName = "Strain Amplitude";
                        string xUnit = "log scale";
                        string yUnit = "log scale";
                        if (fatigueType == FatigueType.StressLife)
                        {
                            xName = "Cycle to failure, Nf";
                            yName = "Stress Amplitude";

                        }
                        temperatureObj = AddTemperature(condition, 20, xName, yName, xUnit, yUnit);
                    }
                    else
                    {
                        temperatureObj = GetTemperature(condition, 20);
                    }




                    StressStrainConditionDiagram modelObj = new StressStrainConditionDiagram();
                    IList<Api.Models.Fatigue.StrainLifePoint> points = null;
                    Api.Models.Fatigue.FatigueConditionDiagram pointsForDiagram = null;

                    if (fatigueType == FatigueType.StrainLife)
                    {
                        points = service.GetFatigueStrainSNCurveDataFromService(sessionId, sourceMaterialId, testConditionId.ToString());
                        pointsForDiagram = service.GetFatigueStrainSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, testConditionId.ToString());
                    }
                    else
                    {
                        points = service.GetFatigueStressSNCurveDataFromService(sessionId, sourceMaterialId, testConditionId.ToString());
                        pointsForDiagram = service.GetFatigueStressSNCurveDiagramPointsFromService(sessionId, sourceMaterialId, testConditionId.ToString());
                    }


                    temperatureObj.PointsForDiagram = pointsForDiagram.Curves.Where(m => m.CurveName == selectedCurveName).FirstOrDefault().PointsForDiagram.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();

                    temperatureObj.Points = new List<PointD>();

                    if (fatigueType == FatigueType.StressLife)
                    {

                        foreach (var point in points)
                        {
                            PointD p = new PointD();
                            var hasFractionalPart = (point.X - Math.Round((decimal)point.X) != 0);

                            if (hasFractionalPart)
                            {
                                p.X = point.X.ToString().Split('.')[1] + "E" + point.X.ToString().Split('.')[0];
                            }
                            else
                            {
                                p.X = "1E" + point.X.ToString();
                            }

                            p.Y = Math.Round(Math.Pow(10, (double)point.Y)).ToString();
                            temperatureObj.Points.Add(p);
                        }
                    }
                    else
                    {
                        temperatureObj.Points = points.Select(m => new PointD { X = "1E" + m.X.ToString(), Y = m.Y.ToString() }).ToList();
                    }


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

                    System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;
                    return ComparisonSuccessMessage.Success;
                }
                else
                {
                    return ComparisonSuccessMessage.AddedMaxNumberOfCurves;
                }
          
            }
          
        }

        public ComparisonSuccessMessage AddToComparisonTrueStressStrainDiagram(int materialId, int sourceMaterialId, int sourceId, int subgroupId, string materialConditionId, int testConditionId, double temperature, IMaterialsContextUow materialContextUow, bool addForAllTemperatues)
        {
            ComparisonD comparison = GetComparisonDiagramModel();

            if (comparison != null && IsAddedMaxNumberOfMaterilas(materialId, sourceMaterialId, sourceId) == true)
            {
                return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;
            
            }
            else
            {
                IService service = new TotalMateriaService();
                string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();


                if (comparison == null)
                {
                    comparison = AddComparisonDiagramModel();
                }

                PropertyD property = null;
                if (!IsAddedProperty(comparison, -1))
                {
                    property = AddProperty(comparison, -1, "stress strain curve", "MPa", "stress strain", GroupTypeEnum.StressStrain);
                }
                else
                {
                    property = GetProperty(comparison, -1);
                }

                if (IsAddedMaxNumberOfCurves(property) == false)
                {
                    MaterialD material = null;
                    if (!IsAddedMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId))
                    {
                        material = AddMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    }
                    else
                    {
                        material = GetMaterial(property, materialId, sourceMaterialId, sourceId, subgroupId);
                    }

                    ConditionD condition = null;

                    if (!IsAddedCondition(material, testConditionId.ToString()))
                    {
                        IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetStressStrainMaterialConditionsFromService(sessionId, material.SourceMaterialId, 1);
                        ElsevierMaterials.Models.MaterialCondition materialCondition = null;
                        if (materialConditionId == "" && materialConditions.Count == 1)
                        {
                            materialCondition = materialConditions.FirstOrDefault();
                            materialConditionId = materialCondition.ConditionId;
                        }
                        else
                        {
                            materialCondition = service.GetStressStrainMaterialConditionsFromService(sessionId, material.SourceMaterialId, 1).Where(m => m.ConditionId == materialConditionId).FirstOrDefault();
                        }

                        StressStrainModel ssModel = service.GetStressStrainTestConditionsWithDataFromService(sessionId, material.SourceMaterialId, materialConditionId, 1);
                        StressStrainConditionModel testCondition = ssModel.TestConditions.Where(m => m.No == testConditionId).FirstOrDefault();
                        string conditionText = "";
                        if (materialCondition != null)
                        {
                            conditionText = conditionText + materialCondition.Description;
                        }
                        if (materialCondition != null && testCondition != null)
                        {
                            conditionText = conditionText + "; " + testCondition.Condition;
                        }
                        if (materialCondition == null && testCondition != null)
                        {
                            conditionText = conditionText + testCondition.Condition;
                        }

                        condition = AddCondition(material, materialConditionId, testConditionId.ToString(), conditionText);
                    }
                    else
                    {
                        condition = GetCondition(material, testConditionId.ToString());
                    }                  

                    StressStrainDetails model = service.GetStressStrainDetails(sessionId, sourceMaterialId, testConditionId, temperature, 1);

                    int numberOfCurves = 0;
                    foreach (var mat in property.Materials)
                    {
                        foreach (var con in mat.Conditions)
                        {
                            numberOfCurves += con.Temperatures.Count;
                        }
                    }
                    numberOfCurves = 12 - numberOfCurves;

                    if (addForAllTemperatues)
                    {
                        int curveNum = 0;
                        foreach (var temp in model.PointsForDiagram.Curves)
                        {
                            if (curveNum < numberOfCurves)
                            {
                                InterativeCurve temperatureObj = null;
                                if (!IsAddedTemperature(condition, temp.Temperature,"°C"))
                                {
                                    temperatureObj = AddTemperature(condition, temp.Temperature, "Strain", "Stress", "", "MPa");
                                }
                                else
                                {
                                    temperatureObj = GetTemperature(condition, temp.Temperature);
                                }

                                StressStrainConditionDiagram modelObj = new StressStrainConditionDiagram();
                                StressStrainConditionCurve curve = temp;

                                temperatureObj.PointsForDiagram = curve.PointsForDiagram.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                                temperatureObj.Points = curve.PointsForTable.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                            }
                            else
                            {
                                break;
                            }
                            curveNum += 1;                     
                        }
                    }
                    else
                    {
                        InterativeCurve temperatureObj = null;
                        if (!IsAddedTemperature(condition, temperature, "°C"))
                        {
                            temperatureObj = AddTemperature(condition, temperature, "Strain", "Stress", "", "MPa");
                        }
                        else
                        {
                            temperatureObj = GetTemperature(condition, temperature);
                        }

                        StressStrainConditionDiagram modelObj = new StressStrainConditionDiagram();
                        StressStrainConditionCurve curve = model.PointsForDiagram.Curves.Where(m => m.Temperature == temperature).FirstOrDefault();

                        temperatureObj.PointsForDiagram = curve.PointsForDiagram.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                        temperatureObj.Points = model.Points.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                     
                    }

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


                    System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;
                    return ComparisonSuccessMessage.Success;
                }
                else
                {
                    return ComparisonSuccessMessage.AddedMaxNumberOfCurves;


                }
            
          
            }
           
        }

        public ComparisonSuccessMessage AddToComparisonTrueSSDiagram(int materialId, int sourceMaterialId, int sourceId, int subgroupId, string materialConditionId, int testConditionId, IList<double> temperatures, IMaterialsContextUow materialContextUow)
        {
            ComparisonD comparison = GetComparisonDiagramModel();

            if (comparison != null && IsAddedMaxNumberOfMaterilas(materialId, sourceMaterialId, sourceId) == true)
            {
                return ComparisonSuccessMessage.AddedMaxNumberOfMaterials;

            }
            else
            {
                IService service = new TotalMateriaService();
                string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();


                if (comparison == null)
                {
                    comparison = AddComparisonDiagramModel();
                }

                PropertyD property = null;
                if (!IsAddedProperty(comparison, -1))
                {
                    property = AddProperty(comparison, -1, "stress strain curve", "MPa", "stress strain", GroupTypeEnum.StressStrain);
                }
                else
                {
                    property = GetProperty(comparison, -1);
                }

                if (IsAddedMaxNumberOfCurves(property) == false)
                {
                    MaterialD material = null;
                    if (!IsAddedMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId))
                    {
                        material = AddMaterial(comparison, property, materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    }
                    else
                    {
                        material = GetMaterial(property, materialId, sourceMaterialId, sourceId, subgroupId);
                    }

                    ConditionD condition = null;

                    if (!IsAddedCondition(material, testConditionId.ToString()))
                    {
                        IList<ElsevierMaterials.Models.MaterialCondition> materialConditions = service.GetStressStrainMaterialConditionsFromService(sessionId, material.SourceMaterialId, 1);
                        ElsevierMaterials.Models.MaterialCondition materialCondition = null;
                        if (materialConditionId == "" && materialConditions.Count == 1)
                        {
                            materialCondition = materialConditions.FirstOrDefault();
                            materialConditionId = materialCondition.ConditionId;
                        }
                        else
                        {
                            materialCondition = service.GetStressStrainMaterialConditionsFromService(sessionId, material.SourceMaterialId, 1).Where(m => m.ConditionId == materialConditionId).FirstOrDefault();
                        }

                        StressStrainModel ssModel = service.GetStressStrainTestConditionsWithDataFromService(sessionId, material.SourceMaterialId, materialConditionId, 1);
                        StressStrainConditionModel testCondition = ssModel.TestConditions.Where(m => m.No == testConditionId).FirstOrDefault();
                        string conditionText = "";
                        if (materialCondition != null)
                        {
                            conditionText = conditionText + materialCondition.Description;
                        }
                        if (materialCondition != null && testCondition != null)
                        {
                            conditionText = conditionText + "; " + testCondition.Condition;
                        }
                        if (materialCondition == null && testCondition != null)
                        {
                            conditionText = conditionText + testCondition.Condition;
                        }

                        condition = AddCondition(material, materialConditionId, testConditionId.ToString(), conditionText);
                    }
                    else
                    {
                        condition = GetCondition(material, testConditionId.ToString());
                    }

                    StressStrainDetails model = service.GetStressStrainDetails(sessionId, sourceMaterialId, testConditionId, temperatures[0], 1);

                    int numberOfCurves = 0;
                    foreach (var mat in property.Materials)
                    {
                        foreach (var con in mat.Conditions)
                        {
                            numberOfCurves += con.Temperatures.Count;
                        }
                    }
                    numberOfCurves = 12 - numberOfCurves;

                    if (temperatures.Count()>1)
                    {
                        int curveNum = 0;
                        foreach (var temp in model.PointsForDiagram.Curves.Where(r=>temperatures.Contains(r.Temperature)))
                        {
                            if (curveNum < numberOfCurves)
                            {
                                InterativeCurve temperatureObj = null;
                                if (!IsAddedTemperature(condition, temp.Temperature, "°C"))
                                {
                                    temperatureObj = AddTemperature(condition, temp.Temperature, "Strain", "Stress", "", "MPa");
                                }
                                else
                                {
                                    temperatureObj = GetTemperature(condition, temp.Temperature);
                                }

                                StressStrainConditionDiagram modelObj = new StressStrainConditionDiagram();
                                StressStrainConditionCurve curve = temp;

                                temperatureObj.PointsForDiagram = curve.PointsForDiagram.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                                temperatureObj.Points = curve.PointsForTable.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                            }
                            else
                            {
                                break;
                            }
                            curveNum += 1;
                        }
                    }
                    else
                    {
                        InterativeCurve temperatureObj = null;
                        if (!IsAddedTemperature(condition, temperatures[0], "°C"))
                        {
                            temperatureObj = AddTemperature(condition, temperatures[0], "Strain", "Stress", "", "MPa");
                        }
                        else
                        {
                            temperatureObj = GetTemperature(condition, temperatures[0]);
                        }

                        StressStrainConditionDiagram modelObj = new StressStrainConditionDiagram();
                        StressStrainConditionCurve curve = model.PointsForDiagram.Curves.Where(m => m.Temperature == temperatures[0]).FirstOrDefault();

                        temperatureObj.PointsForDiagram = curve.PointsForDiagram.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();
                        temperatureObj.Points = model.Points.Select(m => new PointD { X = m.X.ToString(), Y = m.Y.ToString() }).ToList();

                    }

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


                    System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;
                    return ComparisonSuccessMessage.Success;
                }
                else
                {
                    return ComparisonSuccessMessage.AddedMaxNumberOfCurves;


                }


            }

        }

        private bool IsAddedMaxNumberOfMaterilas(int materialId, int sourceMaterialId, int sourceId)
        {
            ComparisonD comparisonD = System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] as ComparisonD;
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = System.Web.HttpContext.Current.Session["ComparisonContainer"] as ElsevierMaterials.Models.Domain.Comparison.Comparison;

            IList<MaterialD> materialsD = null;
            IList<ElsevierMaterials.Models.Domain.Comparison.Material> materials = null;
            if (comparisonD != null)
            {

                foreach (var property in comparisonD.Properties)
                {
                    if (materialsD == null)
                    {
                        materialsD = new List<MaterialD>();
                    }
                    materialsD = materialsD.Union(property.Materials, new MaterialDComparer()).ToList();
                }
            }

            if (comparison != null)
            {
                foreach (var property1 in comparison.Properties)
                {
                    if (materials == null)
                    {
                        materials = new List<ElsevierMaterials.Models.Domain.Comparison.Material>();
                    }
                    materials = materials.Union(property1.Materials, new MaterialComparer()).ToList();
                }
            }


            if (materials!= null && materialsD != null)
            {
                materials = materials.Union(materialsD.Select(m => new ElsevierMaterials.Models.Domain.Comparison.Material { MaterialId = m.MaterialId, SourceId = m.SourceId, SourceMaterialId = m.SourceMaterialId }), new MaterialComparer()).ToList();
                if (materials.Count >= 5 && !materials.Where(m => m.MaterialId == materialId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).Any())
                {
                    return true;
                }
            }
            else if (materials == null && materialsD != null)
            {
                if (materialsD.Count >= 5 && !materialsD.Where(m => m.MaterialId == materialId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).Any())
                {
                    return true;
                }
            
            } else if (materials != null && materialsD == null)
            {
                if (materials.Count >= 5 && !materials.Where(m => m.MaterialId == materialId && m.SourceId == sourceId && m.SourceMaterialId == sourceMaterialId).Any())
                {
                    return true;
                }
            }            
          
            return  false;
        }

        private bool IsAddedMaxNumberOfCurves(PropertyD  property)
        {
            IList<InterativeCurve> temperatures = new List<InterativeCurve>();         
		            
		    foreach (var material in property.Materials)
	        {
		            foreach (var condition in  material.Conditions)
	                {
		                    foreach (var temperature  in  condition.Temperatures)
	                    {
		                        temperatures.Add(temperature);
	                    }
	                }
	          }
	                
	           


            if (temperatures.Count> 0 && temperatures.Select(m=>m.Id).Max() >= 12)
            {
                return true;
            }

            return false;
        }




        public ComparisonD GetComparisonDiagramModel() {
            ComparisonD comparison = System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] as ComparisonD;
            return comparison;
        }

        public ComparisonD AddComparisonDiagramModel()
        {
            ComparisonD comparison = new ComparisonD();
            System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] = comparison;        
            return comparison;
        }


        public InterativeCurve AddTemperature(ConditionD condition, double temperature, string xName, string yName, string xUnit, string yUnit)
        {
            InterativeCurve temperatureObj = new InterativeCurve();
            temperatureObj.Temperature = temperature;
            temperatureObj.XName = xName;
            temperatureObj.YName = yName;
            temperatureObj.XUnit = xUnit;
            temperatureObj.YUnit = yUnit;
            condition.Temperatures.Add(temperatureObj);
            return temperatureObj;
        }

        public ConditionD AddCondition(MaterialD material, string  materialConditionId, string testConditionId, string conditionName)
        {
            ConditionD condition = new ConditionD();
            condition.ConditionId = testConditionId;
            condition.Condition = conditionName;
            material.Conditions.Add(condition);
            return condition;
        }


        public ComparisonD RemoveMaterial( int materialId, int sourceMaterialId, int sourceId, int subgroupId)
        {
            ComparisonD comparison = System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] as ComparisonD;
            if (comparison != null)
            {
                foreach (var property in comparison.Properties)
                {
                    MaterialD material = property.Materials.Where(m => m.MaterialId == materialId && m.SourceMaterialId == sourceMaterialId && m.SubgroupId == subgroupId && m.SourceId == sourceId).FirstOrDefault();
                    if (material != null)
                    {
                        property.Materials.Remove(material);
                    }                  
                }

                IList<PropertyD> properties = new List<PropertyD>();
                foreach (var property in comparison.Properties)
                {
                    if (property.Materials.Count > 0)
                    {
                        properties.Add(property);
                    }
                }
                comparison.Properties = properties;
            }
        
           
            return comparison;
        }

        public PropertyD AddProperty(ComparisonD comparison, int propertyId, string name, string unit, string groupName, GroupTypeEnum groupId)
        {
            PropertyD property = new PropertyD();
            property.Id = comparison.Properties.Count > 0 ? comparison.Properties.Select(m => m.Id).Max() + 1 : 1;
            property.Name = name;
            property.GroupId = groupId;
            property.GroupName = groupName;
            property.Unit = unit;
            property.SourceTypeId = propertyId;
            comparison.Properties.Add(property);
            return property;
        }

        public MaterialD AddMaterial(ComparisonD comparison, PropertyD property, int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow materialContextUow)
        {
            MaterialD material = new MaterialD();
            material.MaterialId = materialId;
            material.SourceMaterialId = sourceMaterialId;
            material.SourceId = sourceId;
            material.SubgroupId = subgroupId;    
            MaterialBasicInfo  materialObj = _materialBinder.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
            material.Name = materialObj.Name;
            material.Standard = materialObj.Standard;
            material.SubgroupName = materialObj.SubgroupName;
            property.Materials.Add(material);
            return material;
        }


        public InterativeCurve GetTemperature(ConditionD condition, double temperature)
        {
            return condition.Temperatures.Where(m => m.Temperature == temperature).FirstOrDefault();
        }
        public ConditionD GetCondition(MaterialD material, string conditionId)
        {
            return material.Conditions.Where(m => m.ConditionId == conditionId).FirstOrDefault();
        }

        public MaterialD GetMaterial(PropertyD property, int materialId, int sourceMaterialId, int sourceId, int subgroupId)
        {
            return property.Materials.Where(m => m.MaterialId == materialId && m.SourceMaterialId == sourceMaterialId && m.SubgroupId == subgroupId && m.SourceId == sourceId).FirstOrDefault();
        }


        public bool IsAddedCondition(MaterialD material, string conditionId)
        {
            if (material.Conditions.Where(m => m.ConditionId == conditionId).Any())
            {
                return true;
            }
            return false;
        }

        public bool IsAddedTemperature(ConditionD condition, double temperature, string xunit)
        {
            if (condition.Temperatures.Where(m => m.Temperature == temperature && m.XUnit==xunit).Any())
            {
                return true;
            }
            return false;
        }

        public PropertyD GetProperty(ComparisonD comparison, int propertyId)
        {
            return comparison.Properties.Where(m => m.SourceTypeId == propertyId).FirstOrDefault();
        }

        public bool IsAddedProperty(ComparisonD comparison,  int propertyId)
        {
            if (comparison.Properties.Where(m => m.SourceTypeId == propertyId).Any())
            {
                return true;
            }
            return false;
        }


        public bool IsAddedMaterial(ComparisonD comparison, PropertyD property, int materialId, int sourceMaterialId, int sourceId, int subgroupId)
        {
            if (property.Materials.Where(m => m.MaterialId == materialId && m.SourceMaterialId == sourceMaterialId && m.SubgroupId == subgroupId && m.SourceId == sourceId).Any())
            {
                return true;
            }
            return false;
        }

        public IList<ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo> GetMaterialsAddedToBothComparison()
        {
            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = System.Web.HttpContext.Current.Session["ComparisonContainer"] as ElsevierMaterials.Models.Domain.Comparison.Comparison;            
            IList<ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo> materialsAddedToDiagramComparison = GetMaterialsAddedToDiagramComparison();
                     
            return comparison.MaterialNames.Union(materialsAddedToDiagramComparison, new MaterialInfoComparer()).ToList();
        }


        
        public IList<ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo> GetMaterialsAddedToDiagramComparison()
        {
            IList<MaterialBasicInfo> materialNames = new List<MaterialBasicInfo>();

            ElsevierMaterials.Models.Domain.Comparison.Comparison comparison = System.Web.HttpContext.Current.Session["ComparisonContainer"] as ElsevierMaterials.Models.Domain.Comparison.Comparison;

            ComparisonD comparisonDiagram = System.Web.HttpContext.Current.Session["ComparisonDiagramContainer"] as ComparisonD;
            if (comparisonDiagram != null && comparisonDiagram.Properties != null && comparisonDiagram.Properties.Count > 0)
            {
                foreach (PropertyD prop in comparisonDiagram.Properties)
	            {
                    foreach (MaterialD material in prop.Materials)
	                {
		                if (materialNames.Where(m => m.MaterialId == material.MaterialId).FirstOrDefault() == null)
                        {
                            MaterialBasicInfo mat = new MaterialBasicInfo();
                            mat.MaterialId = material.MaterialId;
                            mat.Name = material.Name;
                            mat.SourceId = material.SourceId;
                            mat.SourceMaterialId = material.SourceMaterialId;
                            mat.SubgroupId = material.SubgroupId;
                            mat.SubgroupName = material.SubgroupName;
                            materialNames.Add(mat);
                        }
	                }                
	            }               
            }
            return materialNames;
        }

    


        class MaterialInfoComparer : IEqualityComparer<MaterialBasicInfo>
        {
            public bool Equals(MaterialBasicInfo x, MaterialBasicInfo y)
            {
                return x.SourceId == y.SourceId && x.SourceMaterialId == y.SourceMaterialId && x.MaterialId == y.MaterialId;
            }

            public int GetHashCode(MaterialBasicInfo myModel)
            {
                return myModel.SourceId.GetHashCode() + myModel.MaterialId.GetHashCode() + myModel.SourceMaterialId.GetHashCode();
            }
        }


        class MaterialDComparer : IEqualityComparer<MaterialD>
        {
            public bool Equals(MaterialD x, MaterialD y)
            {
                return x.SourceId == y.SourceId && x.SourceMaterialId == y.SourceMaterialId && x.MaterialId == y.MaterialId;
            }

            public int GetHashCode(MaterialD myModel)
            {
                return myModel.SourceId.GetHashCode() + myModel.MaterialId.GetHashCode() + myModel.SourceMaterialId.GetHashCode();
            }
        }

        class MaterialComparer : IEqualityComparer<ElsevierMaterials.Models.Domain.Comparison.Material>
        {
            public bool Equals(ElsevierMaterials.Models.Domain.Comparison.Material x, ElsevierMaterials.Models.Domain.Comparison.Material y)
            {
                return x.SourceId == y.SourceId && x.SourceMaterialId == y.SourceMaterialId && x.MaterialId == y.MaterialId;
            }

            public int GetHashCode(ElsevierMaterials.Models.Domain.Comparison.Material myModel)
            {
                return myModel.SourceId.GetHashCode() + myModel.MaterialId.GetHashCode() + myModel.SourceMaterialId.GetHashCode();
            }
        }





        public double GetMaxYValueForAllSeries(PropertyD property)
        {
            double value = -9999;
            foreach (var material in property.Materials)
            {
                foreach (var condition in material.Conditions)
                {
                    foreach (var temperature in condition.Temperatures)
                    {
                       double valueTemp = temperature.PointsForDiagram.Select(m => double.Parse(m.Y)).Max();
                        if (valueTemp > value)
	                    {
		                value = valueTemp;
	                    }
                    }
                }
            }
            return value;
        }


         public double GetMaxXValueForAllSeries(PropertyD property)
        {
            double value = -9999;
            foreach (var material in property.Materials)
            {
                foreach (var condition in material.Conditions)
                {
                    foreach (var temperature in condition.Temperatures)
                    {
                       double valueTemp = temperature.PointsForDiagram.Select(m => double.Parse(m.X)).Max();
                        if (valueTemp > value)
	                    {
		                value = valueTemp;
	                    }
                    }
                }
            }
            return value;
        }


         public double GetMinYValueForAllSeries(PropertyD property)
         {
             double value = -9999;
             foreach (var material in property.Materials)
             {
                 foreach (var condition in material.Conditions)
                 {
                     foreach (var temperature in condition.Temperatures)
                     {
                         double valueTemp = temperature.PointsForDiagram.Select(m => double.Parse(m.Y)).Min();
                         if (valueTemp > value)
                         {
                             value = valueTemp;
                         }
                     }
                 }
             }
             return value;
         }


         public double GetMinXValueForAllSeries(PropertyD property)
         {
             double value = -9999;
             foreach (var material in property.Materials)
             {
                 foreach (var condition in material.Conditions)
                 {
                     foreach (var temperature in condition.Temperatures)
                     {
                         double valueTemp = temperature.PointsForDiagram.Select(m => double.Parse(m.X)).Min();
                         if (valueTemp > value)
                         {
                             value = valueTemp;
                         }
                     }
                 }
             }
             return value;
         }



      
    }
}