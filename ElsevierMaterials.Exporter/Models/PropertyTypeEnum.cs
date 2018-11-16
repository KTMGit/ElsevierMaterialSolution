using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Exporter.Models
{
    public enum PropertyTypeEnum
    {
        None = 0,

        /// <summary>
        /// Altair: E
        /// </summary>
        PhysicalModulusOfElasticity = 1,
        /// <summary>
        /// Altair: NU
        /// </summary>
        PhysicalPoissonCoefficient = 2,
        /// <summary>
        /// Altair: RHO
        /// </summary>
        PhysicalDensity = 3,
        /// <summary>
        /// Altair: A
        /// </summary>
        PhysicalMeanCoeffThermalExpansion = 4,
        /// <summary>
        /// Altair: K
        /// </summary>
        PhysicalThermalConductivity = 5,
        /// <summary>
        /// Altair: YS
        /// </summary>
        MechanicalYield = 6,
        /// <summary>
        /// Altair: UTS
        /// </summary>
        MechanicalTensile = 7,
        /// <summary>
        /// Altair: SRI1
        /// </summary>
        FatigueA = 8,
        /// <summary>
        /// Altair: b1
        /// </summary>
        FatigueB = 9,
        /// <summary>
        /// Altair: Nc1
        /// </summary>
        FatigueNf_maxSNDiagram = 10,
        /// <summary>
        /// Altair: FL
        /// </summary>
        FatigueLimit = 11,
        /// <summary>
        /// Altair: Sf
        /// </summary>
        FatigueStrengthCoefficient = 12,
        /// <summary>
        /// Altair: b
        /// </summary>
        FatigueStrengthExponent = 13,
        /// <summary>
        /// Altair: c
        /// </summary>
        FatigueDuctilityExponent = 14,
        /// <summary>
        /// Altair: ef
        /// </summary>
        FatigueDuctilityCoefficient = 15,
        /// <summary>
        /// Altair: np
        /// </summary>
        FatigueCyclicStrengthExponent = 16,
        /// <summary>
        /// Altair: Kp
        /// </summary>
        FatigueCyclicStrengthCoefficient = 17,
        /// <summary>
        /// Altair: Nc
        /// </summary>
        FatigueNf_maxENDiagram = 18,
        /// <summary>
        /// Altair: Y
        /// </summary>
        StressStrain002 = 19,
        /// <summary>
        /// Altair: K
        /// </summary>
        StressStrainK = 20,
        /// <summary>
        /// Altair: n
        /// </summary>
        StressStrainn = 21,

        /// <summary>
        /// Specific heat
        /// </summary>
        PhysicalSpecificThermalCapacity = 22,

        MechanicalElongation = 23,
        /// <summary>
        /// Altair: Y
        /// </summary>
        StressStrain = 24,
        /// <summary>
        /// ESI: The chemical composition
        /// </summary>
        ChemicalCompositions = 28,
        /// <summary>
        /// ANSYS: Fatigue second tab (Stress)
        /// </summary>
        FatigueStressPoints = 29,
        /// <summary>
        /// ANSYS: Stress Strain
        /// </summary>
        PlasticStrainStress = 30

    }

}
