using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    public class FullTextSearchMap : EntityTypeConfiguration<FullTextSearch> {
        public FullTextSearchMap() {
            ToTable("QuickSearchFullText");
            HasKey(m => m.Id);

            Property(m => m.Id)
               .HasColumnName("ID");
            Property(m => m.material_designation)
                 .HasColumnName("material_designation");
            Property(m => m.properties)
                 .HasColumnName("properties");
        }
    }
}
