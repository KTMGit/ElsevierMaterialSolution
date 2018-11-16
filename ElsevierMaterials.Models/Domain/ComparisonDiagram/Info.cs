using ElsevierMaterials.Models.Domain.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.ComparisonDiagram
{
    public class Info : IPropertyBasicInfo
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public int SourceTypeId { get; set; }
      public int TypeId { get; set; }
      public string Unit { get; set; }
    }
}
