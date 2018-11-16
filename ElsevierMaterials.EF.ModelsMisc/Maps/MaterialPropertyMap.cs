using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    public class MaterialPropertyMap : EntityTypeConfiguration<MaterialProperty> {
        public MaterialPropertyMap() {
            ToTable("material_property");
            HasKey(n => new { n.MaterialId,n.SubgroupId, n.RowId, n.PropertyId, n.ValueId });

            //Property(b => b.OrigValue)
            //    .IsOptional()
            //    .HasColumnType("float");
        //     public decimal? Temperature { get; set; }
        //public decimal? OrigValue { get; set; }
        //public decimal? OrigValueMin { get; set; }
        //public decimal? OrigValueMax { get; set; }
        //public string OrigValueText { get; set; }
        //public int? OrigUnit { get; set; }
        //public decimal? ConvValue { get; set; }
        //public decimal? ConvValueMin { get; set; }
        //public decimal? ConvValueMax { get; set; }
        }
    }
}
