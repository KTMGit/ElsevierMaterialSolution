using ElsevierMaterials.Models.Domain.Property;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class PropertiesConversionFactorsMap : EntityTypeConfiguration<PropertyConversionFactorAndUnit>
    {
        public PropertiesConversionFactorsMap()
        {
            ToTable("View_PropertiesConversionFactors");
            HasKey(t => new { t.PropertyId});
            Property(m => m.PropertyId)
                .HasColumnName("PropertyID");
            Property(m => m.Unit)
                .HasColumnName("unit_lbl");
            Property(m => m.Factor)
                .HasColumnName("factor");
            Property(m => m.Offset)
                .HasColumnName("offset");
            Property(m => m.DecimalsUS)
               .HasColumnName("DecimalsUS");
          }
    }
}
