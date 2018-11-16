using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    
    public class BasePropertyMap : EntityTypeConfiguration<BaseProperty> {
        public BasePropertyMap() {
            ToTable("property");
            HasKey(m => m.PropertyId);

            Property(m => m.PropertyId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnName("PropertyID");
            Property(m => m.DefaultUnit)
                .HasColumnName("DefaultUnit");
        }
    }
}
