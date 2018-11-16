using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class AdvSearchPropertyConditionsMap : EntityTypeConfiguration<AdvSearchPropertyConditions>
    {

        public AdvSearchPropertyConditionsMap()
        {
            ToTable("AdvSearchPropertyConditions");
            HasKey(t => new { t.PropertyID, t.X_label });
        }

    }
}