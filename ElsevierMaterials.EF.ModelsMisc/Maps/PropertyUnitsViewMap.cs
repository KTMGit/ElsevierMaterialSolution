using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class PropertyUnitsViewMap : EntityTypeConfiguration<PropertyUnit>
    {
        // UnitKey, UnitLabel, Factor, Offset, Metric, UnitId_new, PropertyID

        public PropertyUnitsViewMap()
        {
            ToTable("View_EMS_GetUnits");
            //HasKey(t => new { t.UnitKey, t.PropertyID });
            HasKey(t => new { t.PropertyID });

            Property(m => m.UnitKey)
               .HasColumnName("unit_key");
            Property(m => m.UnitLabel)
              .HasColumnName("unit_lbl");
            Property(m => m.Factor)
               .HasColumnName("factor");
            Property(m => m.Offset)
              .HasColumnName("offset");
            Property(m => m.Metric)
                .HasColumnName("metric");
            Property(m => m.PropertyName)
                .HasColumnName("PropertyName");
            Property(m => m.PropertyID)
               .HasColumnName("PropertyID");
            //Ignore(m => m.ClassificationId);
            //Ignore(m => m.Classification);
        }
    }
}
