using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    public class UnitMap : EntityTypeConfiguration<Unit> {
        public UnitMap() {
            ToTable("units");
            HasKey(m => m.UnitId);

            Property(m => m.UnitId)
                .HasColumnName("unit_key");
            Property(m => m.UnitText)
                .HasColumnName("unit_lbl");
        }
    }
}
