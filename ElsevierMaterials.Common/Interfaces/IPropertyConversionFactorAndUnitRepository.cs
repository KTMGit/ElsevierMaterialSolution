using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using ElsevierMaterials.Models.Domain.Property;

namespace ElsevierMaterials.Common.Interfaces {

    public interface IPropertyWithConversionFactorsAndUnitsRepository : IRepository<PropertyConversionFactorAndUnit>
    {
        IList<PropertyConversionFactorAndUnit> GetPropertiesWithConversionFactorsAndUnits();
        //PropertyConversionFactorAndUnit GetPropertiesWithConversionFactorsAndUnits();
    }
}
