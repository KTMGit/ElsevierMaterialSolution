using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Interfaces
{
    public interface ITypeCorrector
    {
        bool IsMatch(object entity);
        void CorrectEntity(object entity, EntityState entityState);
    }
}
