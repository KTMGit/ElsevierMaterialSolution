using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    public class RecordLinkMap : EntityTypeConfiguration<RecordLink> {
        public RecordLinkMap() {
            ToTable("View_RecordLink_Condition_ProductForm");
            HasKey(t => new { t.MaterialID, t.SubgroupID, t.RowID });

            Property(m => m.RecordID)
                .HasColumnName("Record_id");
            Property(m => m.SetID)
                .HasColumnName("set_id");
            Property(m => m.ConditionID)
                .HasColumnName("condition id");
            Property(m => m.ProductFormID)
                .HasColumnName("product form id");
            Property(m => m.Thickness)
                .HasColumnName("thickness");
            Property(m => m.Temperature)
                .HasColumnName("temperature");
            Property(m => m.Condition)
                .HasColumnName("condition");
            Property(m => m.ProductForm)
                .HasColumnName("product form");
        }
    }
}
