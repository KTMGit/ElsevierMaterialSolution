using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;
using Api.Models;
using Api.Models.Plus;
using Api.Models.ChemicalComposition;
using Api.Models.Machinability;
using Api.Models.HeatTreatment;
using Api.Models.Mechanical;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using Api.Models.SubgroupList;
using Api.Models.StressStrain;
namespace ElsevierMaterials.Common.Interfaces
{
    public interface IService
    {
        string GetSessionFromService();

        ICollection<Material> GetMaterialSubgroupListFromService(string sessionId, int materialId, int sourceMaterialId);
        ICollection<Material> GetMetalsMaterialsSubgroupListFromService(string sessionId, IList<int> materials);                
        
        ICollection<Api.Models.MaterialCounters> GetMetalsMaterialsCountersFromService(string sessionId, int materialId);

        ICollection<Condition> GetPhysicalPropertiesFromService(string sessionId, int materialId);
        MachinabilityModel GetMachinabilityPropertiesFromService(string sessionId, int materialId);
        ICollection<Condition> GetMechanicalRoomPropertiesFromService(string sessionId, int materialId, int subgroup, int type = 1);
        ICollection<Condition> GetMechanicalHighLowPropertiesFromService(string sessionId, int materialId, int subgroupId, MechanicalGroupEnum group, int type = 1);
        EquivalencyModel GetCrossReferenceFromService(string sessionId, int materialId, int subgroupId);
        MetallographyModel GetMetallographyPropertiesFromService(string sessionId, int materialId, int subgroupId);
        HeatTreatment GetHeatTreatmentFromService(string sessionId, int materialId, int subgroupId);
  
        ICollection<Condition> GetChemicalCompositionFromService(string sessionId, int materialId, int subgroupId);
        IList<StressStrainTemperature> GetStressStrainTemperatures(string sessionId, int materialId, int conditionId, int type = 1);
        StressStrainDetails GetStressStrainDetails(string sessionId, int materialId, int conditionId, double temperature, int ssType, int type = 1);
        
        IList<string> GetAllReferencesFromService(string sessionId, int materialId, MaterialDetailType productGroupType);
        IList<string> GetReferencesForSelectedConditionFromService(string sessionId, int materialId, int conditionId, MaterialDetailType productGroupType);
        IList<string> GetReferencesForSelectedConditionFatigueFromService(string sessionId, int materialId, string conditionId, MaterialDetailType productGroupType);

        IList<int> GetMaterialIdsForAdvSearchPropertiesFromService(string sessionId, AdvSearchFiltersAll filters);


        IList<ElsevierMaterials.Models.MaterialCondition> GetStressStrainMaterialConditionsFromService(string sessionId, int materialId, int type = 1);
        StressStrainModel GetStressStrainTestConditionsWithDataFromService(string sessionId, int materialId, string conditionId, int type = 1);
        StressStrainModel GetStressStrainOnlyTestConditionsFromService(string sessionId, int materialId, string conditionId, int type = 1);


        IList<ElsevierMaterials.Models.MaterialCondition> GetFatigueMaterialConditionsFromService(string sessionId, int materialId, int fatigueCategory, int type = 1);
        IList<TestCondition> GetFatigueTestConditionsFromService(string sessionId, int materialId, string conditionId, int fatigueCategory, int type = 1);
        

        IList<Api.Models.Fatigue.FatigueCondition> GetFatigueConditionsFromService(string sessionId, int materialId, int fatigueCategory, int type = 1);

        Api.Models.Fatigue.FatigueConditionDetails GetFatigueStrainLifeConditionDetailsFromService(string sessionId, int materialId, string condition, int type = 1);
        Api.Models.Fatigue.FatigueConditionDetails GetFatigueStressLifeConditionDetailsFromService(string sessionId, int materialId, string condition, int type = 1);

        IList<Api.Models.Fatigue.StrainLifePoint> GetFatigueStrainSNCurveDataFromService(string sessionId, int materialId, string condition, int type = 1);
        IList<Api.Models.Fatigue.StrainLifePoint> GetFatigueStressSNCurveDataFromService(string sessionId, int materialId, string condition, int type = 1);

        ImageSource GetFatigueStrainSNCurveDiagramFromService(string sessionId, int materialId, string condition, int type = 1);
        Api.Models.Fatigue.FatigueConditionDiagram GetFatigueStrainSNCurveDiagramPointsFromService(string sessionId, int materialId, string condition, int type = 1);
        
        ImageSource GetFatigueStressSNCurveDiagramFromService(string sessionId, int materialId, string condition, int type = 1);
        Api.Models.Fatigue.FatigueConditionDiagram GetFatigueStressSNCurveDiagramPointsFromService(string sessionId, int materialId, string condition, int type = 1);

        Api.Models.CreepData.CreepData GetCreepDataFromService(string sessionId, int materialId, int conditionId, int type = 1);
        IList<CreepConditionModel> GetCreepDataConditionsFromService(string sessionId, int materialId);


        IList<ElsevierMaterials.Models.MaterialCondition> GetCreepMaterialConditionsFromService(string sessionId, int materialId);
        IList<TestCondition> GetCreepTestConditionsFromService(string sessionId, int materialId, string conditionId);

        StressStrainConditionDiagram GetStressStrainConditionPointsForDiagram(string sessionId, int materialId, int conditionId, double temperature, int ssType, int type);
        

    }
}
