using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Export.Models
{
    public enum TMPlusPropertyTypeEnum
    {
        None = 0,

        /// <summary>
        /// Altair: E
        /// </summary>
        PhysicalModulusOfElasticity = 4,
        /// <summary>
        /// Altair: NU
        /// </summary>
        PhysicalPoissonCoefficient = 10,
        /// <summary>
        /// Altair: RHO
        /// </summary>
        PhysicalDensity = 11,
        /// <summary>
        /// Altair: A
        /// </summary>
        ThermalExpansion = 7,
        /// <summary>
        /// Altair: K
        /// </summary>
        ThermalConductivity = 8,
        /// <summary>
        /// Altair: YS
        /// </summary>
        MechanicalYield = 253,
        /// <summary>
        /// Altair: UTS
        /// </summary>
        MechanicalTensile = 2,
        StressStrain = 9
    }
}