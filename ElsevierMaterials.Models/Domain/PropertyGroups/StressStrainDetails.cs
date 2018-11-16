using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models.StressStrain;
using System.IO;

namespace ElsevierMaterials.Models {
    public class StressStrainDetails {
        public string Comment { get; set; }
        public IList<DiagramCoordinate> Points { get; set; }
        public ImageSource Diagram { get; set; }
        public int UnitType { get; set; }
        public StressStrainConditionDiagram PointsForDiagram { get; set; }
    }
}
