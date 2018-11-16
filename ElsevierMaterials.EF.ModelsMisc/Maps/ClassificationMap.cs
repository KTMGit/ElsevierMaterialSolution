using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {
    public class ClassificationMap : EntityTypeConfiguration<Classification> {
        public ClassificationMap() {
            //Map(e =>
            //{
            //    e.Properties(p =>
            //        new { p.Id, p.Name });
            //    e.ToTable("[preferred names]");
            //    Property(b => b.Id).HasColumnName("[PN ID]");
            //    Property(b => b.Name).HasColumnName("PN");
            //});
            Map(e =>
            {
                e.Properties(p =>
                    new { p.Id, p.Parent });
                e.ToTable("tree");
                Property(b => b.Id).HasColumnName("PN ID");
                Property(b => b.Parent).HasColumnName("BT ID");
            });

            HasKey(m => m.Id);
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(m => m.Name)
                    .HasMaxLength(100);
                   // .IsRequired();
            //Property(m => m.Level)
            //    .HasColumnName("LevelId")
             //   .IsRequired();
            //Property(m => m.Parent)
            //    .HasColumnName("ParentId")
              //  .IsOptional();

        }
    }
}
