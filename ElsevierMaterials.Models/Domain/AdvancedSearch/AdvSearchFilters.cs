using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
    public class AdvSearchFilters
    {
        public PropertyType propertyType { get; set; }
        public LogicalOperators logicalOperators { get; set; }
        public BinaryOperators binaryOperators { get; set; }
        public int propertyId { get; set; }
        public string propertyName { get; set; }
        public decimal valueFrom { get; set; }
        public decimal valueTo { get; set; }
        public string valueFrom_orig { get; set; }
        public string valueTo_orig { get; set; }
        public int unitId { get; set; }
        public string unitName { get; set; }

        public IList<PropertyConditionModel> PropertyConditions { get; set; }
        public bool isPropertyConditionsActive { get; set; }
    }

}
