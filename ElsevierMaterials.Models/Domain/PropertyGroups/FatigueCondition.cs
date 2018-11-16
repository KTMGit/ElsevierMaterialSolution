using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
   public class FatigueCondition
    {
     
       public int UnitType { get; set; }
       public  TestCondition Condition { get; set; }
       public Api.Models.Fatigue.FatigueConditionDetails Details { get; set; }
       public ImageSource Diagram { get; set; }
       public IList<Api.Models.Fatigue.StrainLifePoint> Points { get; set; }
       public Api.Models.Fatigue.FatigueConditionDiagram PointsForDiagram { get; set; }
       public IList<string> SelectedReferences { get; set; }
       public FatigueType Type { get; set; }
    }

   public enum FatigueType
   {
       StrainLife = 1,
       StressLife = 2
   }
}
