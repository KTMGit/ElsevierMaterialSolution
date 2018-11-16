using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Exporter.Models
{
    public enum TMPropertyTypeEnum
    {
        None = 0,
         
        PhysicalModulusOfElasticity = 1,
  
        PhysicalPoissonCoefficient = 2,
     
        PhysicalDensity = 3,
 
        PhysicalMeanCoeffThermalExpansion = 4,
    
        PhysicalThermalConductivity = 5,
      
        MechanicalYield = 6,
    
        MechanicalTensile = 7,
      
        FatigueA = 8,
     
        FatigueB = 9,
     
        FatigueNf_maxSNDiagram = 10,
     
        FatigueLimit = 11,
     
        FatigueStrengthCoefficient = 12,
  
        FatigueStrengthExponent = 13,
       
        FatigueDuctilityExponent = 14,

        FatigueDuctilityCoefficient = 15,
     
        FatigueCyclicStrengthExponent = 16,
     
        FatigueCyclicStrengthCoefficient = 17,
    
        FatigueNf_maxENDiagram = 18,
   
        StressStrain002 = 19,
    
        StressStrainK = 20,
       
        StressStrainn = 21,

        PhysicalSpecificThermalCapacity = 22,

        MechanicalElongation = 23,

        StressStrain = 24,
    
        ChemicalCompositions = 28,
    
        FatigueStressPoints = 29,
   
        PlasticStrainStress = 30

    }

}
