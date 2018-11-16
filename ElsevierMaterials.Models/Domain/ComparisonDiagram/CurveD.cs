using Api.Models.StressStrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public class CurveD
    {
        public IList<DiagramCoordinate> PointsForTable { get; set; }
        public StressStrainConditionDiagram PointsForDiagram { get; set; }

        public CurveD() {
            PointsForTable = new List<DiagramCoordinate>();
            PointsForDiagram = new StressStrainConditionDiagram();            
        }
 
    }
}
