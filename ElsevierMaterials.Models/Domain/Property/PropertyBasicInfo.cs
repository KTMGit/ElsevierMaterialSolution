using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterials.Models.Domain.Property
{
    public class PropertyBasicInfo : IPropertyBasicInfo
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int TypeId { get; set; }
        public int SourceTypeId { get; set; }
        public int RowId { get; set; }
        public int ConditionId { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public double Temperature { get; set; }

    }
    
}
