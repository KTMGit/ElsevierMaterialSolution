using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using System.Data.Entity;
using System.Linq;
using ElsevierMaterials.Models.Domain.Property;
using System.Collections.Generic;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{

    public class PropertyConversionFactorAndUnitRepository : BaseRepository<PropertyConversionFactorAndUnit>, IPropertyWithConversionFactorsAndUnitsRepository
    {
        public PropertyConversionFactorAndUnitRepository(DbContext dbContext)
            : base(dbContext)
        {
        }
       public IList<PropertyConversionFactorAndUnit> GetPropertiesWithConversionFactorsAndUnits()
     //   public PropertyConversionFactorAndUnit GetPropertiesWithConversionFactorsAndUnits()
        {
            IList<PropertyConversionFactorAndUnit> properties = DataSet.ToList();
            //  PropertyConversionFactorAndUnit properties = DataSet.FirstOrDefault();
            return properties;
        }
    }
}

