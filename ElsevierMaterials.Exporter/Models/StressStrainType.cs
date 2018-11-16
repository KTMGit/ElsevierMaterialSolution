using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Exporter.Models
{
    /// <summary>
    /// Stress strain types
    /// </summary>
    public enum StressStrainType
    {
        None = 0,

        /// <summary>
        /// Altair: E
        /// </summary>
        StressStrainTension = 1,
        /// <summary>
        /// Altair: NU
        /// </summary>
        StressStrainCompression = 2

    }
}
