using Api.Models.PLUS.MaterialDetails;
using Api.Models.CrossReference;
using Api.Models.Plus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface IPlusService
    {
        Api.Models.PLUS.MaterialDetails.Condition MechanicalConditionProperties(int materialId, int subgroupId, int conditionId);
        CrossReference GetCrossReference(int materialId);
        PropertiesContainer GetManufactoringProcesses(int materialId, int subgroupId);
        StressStrainModel GetStressStrainFromPLUSService(int materialId, int subgroupId);
        StressStrainDetails GetStressStrainPLUSDetails(string sessionId, int materialId, int subgroupId, int conditionId, double temperature, int ssType);
        IList<Api.Models.FatiguePLUS.Condition> GetFatigueConditionsFromPLUSService(string sessionId, int materialId);
        Api.Models.FatiguePLUS.ConditionDetails GetFatigueConditionDetailsFromPLUSService(string sessionId, int materialId, string condition);
        ImageSource GetFatigueDiagramFromPLUSService(string sessionId, int materialId, string condition);
        IList<Api.Models.PLUS.MultiPointData.DiagramType> GetMPDiagramTypesFromPLUSService(string sessionId, int materialId, int subgroupId);
        IList<Api.Models.PLUS.MultiPointData.Condition> GetMPConditionsForDiagramTypeFromPLUSService(string sessionId, int materialId, int subgroupId, int typeDiagram);
        IList<Api.Models.PLUS.MultiPointData.DiagramLegend> GetMPLegendsForConditionFromPLUSService(string sessionId, int materialId, int subgroupId, int condition,int typeDiagram);
        Api.Models.PLUS.MultiPointData.TablePoints GetMPTablePointsFromPLUSService(string sessionId, int materialId, int subgroupId, int condition, int typeDiagram, int legendId);
        ImageSource GetMultipointDataDiagramFromPlusService(string sessionId, int materialId, int subgroupId, int conditionId, int typeDiagram);
        IList<StressStrainTemperature> GetStressStrainTemperaturesPLUS(string sessionId, int materialId, int subgroupId, int conditionId, int ssType);
        Api.Models.CreepDataPLUS.Data GetCreepDataFromService(string sessionId, int materialId);
        IList<Api.Models.CreepDataPLUS.CreepCondition> GetCreepDiagramConditions(string sessionId, int materialId);
        IList<TemperatureItem> GetCreepConditionTemperatures(string sessionId, int materialId, int conditionId);
        IList<TemperatureItem> GetCreepConditionTemperaturesIso(string sessionId, int materialId, int conditionId);
        IList<Api.Models.CreepDataPLUS.Time> GetCreepTimesIso(string sessionId, int materialId, int conditionId, short temperature);
        IList<Api.Models.CreepDataPLUS.StressPoint> GetCreepStresses(string sessionId, int materialId, int conditionId, short temperature);
        IList<Api.Models.CreepDataPLUS.StressPointIso> GetCreepStressPoints(string sessionId, int materialId, int conditionId, short temperature, double value);
        IList<Api.Models.CreepDataPLUS.StressPointIso> GetCreepStressPointsIso(string sessionId, int materialId, int conditionId, short temperature, string unit, double value);
        ImageSource GetCreepDiagramIso(string sessionId, int materialId, int conditionId, short temperature);
        ImageSource GetCreepDiagram(string sessionId, int materialId, int conditionId, short temperature);
        IList<string> GetAllReferencesForSS(string sessionId, int materialId);
        IList<string> GetReferencesForSelectedConditionForSS(string sessionId, int materialId, int conditionId);

        ICollection<Material> GetPLUSMaterialsSubgroupListFromService(string sessionId, IList<int> materials);
        PropertiesContainer GetPhysicalPropertiesPLUSFromService(string sessionId, int materialId);
        PropertiesContainer GetMechanicalPLUSPropertiesFromService(string sessionId, int materialId, int subgroupId);
        ICollection<Material> GetMaterialSubgroupPLUSListFromService(string sessionId, int materialId, int sourceMaterialId);
        IList<int> GetMaterialIdsForAdvSearchPropertiesFromServicePLUS(string sessionId, AdvSearchFiltersAll filters);
    }
}
