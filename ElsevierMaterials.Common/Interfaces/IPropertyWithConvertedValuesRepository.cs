using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterials.Common.Interfaces {
    public interface IPropertyWithConvertedValuesRepository : IRepository<PropertyWithConvertedValues>
    {
        Property GetPropertyInfoForMaterialForSelectedMetric(string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId, int type, string unitName);
    }
}
