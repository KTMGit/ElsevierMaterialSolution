using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class MaterialMap : EntityTypeConfiguration<Material>
    {
        public MaterialMap()
        {
            //Map(e =>
            //{
            //    e.Properties(p =>
            //      new { p.Id });
            //    e.ToTable("material");
            //    Property(b => b.Id).HasColumnName("MaterialID");
            //});


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
                    new {p.Id, p.MaterialId, p.SourceId });
                e.ToTable("EquivalentMaterial");
                Property(b => b.Id).HasColumnName("MaterialId");
                Property(b => b.MaterialId).HasColumnName("SourceMaterialId");
                Property(b => b.SourceId).HasColumnName("SourceId");
            });

            Map(e =>
            {
                e.Properties(p =>
                    new { p.Id, p.SubgroupId, p.Specification, p.Standard });
                e.ToTable("material_subgroup");
                Property(b => b.Id).HasColumnName("MaterialID");
                Property(b => b.SubgroupId).HasColumnName("SubgroupID");
                Property(b => b.Specification).HasColumnName("SubgroupName");
                Property(b => b.Standard).HasColumnName("Standard");
            });

            HasKey(m => m.Id);
          



            
            //Property(m => m.SourceText)
            //      .HasMaxLength(100)
            //   .HasColumnName("SourceText");
        
          

            HasRequired(p => p.Source)
                .WithMany(g => g.Materials)
                .HasForeignKey(p => p.SourceId);

            Property(m => m.SourceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
             .HasColumnName("SourceId");

            HasRequired(p => p.Classification)
                .WithMany(g => g.Materials)
                .HasForeignKey(p => p.ClassificationId);

            Property(m => m.ClassificationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnName("ClassificationId");

        
        }
    }
}
