using Api.Common.Interfaces.Fatigue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.PropertyGroups.Fatigue
{
    public class FatigueConditionDiagram : IFatigueConditionDiagram
    {
        public IList<IFatigueConditionCurve> Curves { get; set; }
    }
}
