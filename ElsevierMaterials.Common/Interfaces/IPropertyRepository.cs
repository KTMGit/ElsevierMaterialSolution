using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;

namespace ElsevierMaterials.Common.Interfaces {
   public interface IPropertyRepository : IRepository<MaterialProperty>{
       Property GetPropertyInfoForMaterial( string name, int materialId, int subgroupId, int rowId, int propertyId, int valueId, string unitName);
    }
}
