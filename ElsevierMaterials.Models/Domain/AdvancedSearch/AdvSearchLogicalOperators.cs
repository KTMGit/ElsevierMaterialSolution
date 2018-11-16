using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public enum LogicalOperators
    {
        NotDefined = -1,
        Exists = 0,
        Eq = 1,
        Lte = 2,
        Gte = 3,
        Between = 4
    }

}
