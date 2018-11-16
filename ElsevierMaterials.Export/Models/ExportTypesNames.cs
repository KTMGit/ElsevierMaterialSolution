using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Exporter.Models
{
    /// <summary>
    /// Enum ExportTypesNames
    /// </summary>
    public enum ExportTypeEnum
    {

        All = 0,
        Radioss = 1,
        Abaqus = 2,
        SolidWorks = 3,
        SolidEdge = 4,
        Esi = 5,
        ANSYS = 6,
        Siemens = 7,
        LsDyna = 8,
        EsiVPS = 9,
        Excel = 10,
        KTMXml = 11,
        FEMAP = 12,
        NASTRAN = 13,
        PTCCreo = 14,
        ESIPamCrash = 15, 
        AutodeskNastran = 16

    }
}
