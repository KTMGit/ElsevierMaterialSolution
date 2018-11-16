using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Interfaces
{
    public interface ITypeValidator
    {
        bool IsMatch(DbEntityValidationResult result);
        void ValidateEntity(DbEntityValidationResult result);
    }
}
