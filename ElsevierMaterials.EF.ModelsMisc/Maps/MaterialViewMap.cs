using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class MaterialViewMap : EntityTypeConfiguration<Material>
    {
        // MaterialId, SourceMaterialId, SourceId, SubgroupID, SubgroupName, Standard, materialName, SourceText

        public MaterialViewMap()
        {
            ToTable("View_material");
            HasKey(t => new { t.MaterialId, t.SubgroupId, t.SourceMaterialId });

            Property(m => m.MaterialId)
                  .HasColumnName("MaterialID");
            Property(m => m.SourceMaterialId)
                 .HasColumnName("SourceMaterialId");
            Property(m => m.SourceId)
                .HasColumnName("SourceId");
            Property(m => m.SubgroupId)
                .HasColumnName("SubgroupID");
            Property(m => m.Specification)
                .HasColumnName("SubgroupName");
            Property(m => m.Name)
                .HasColumnName("materialName");
            Property(m => m.SourceText)
                 .HasColumnName("SourceText");
            Property(m => m.NumProperties)
                .HasColumnName("NumProperties");
            Property(m => m.Manufacturer)
                .HasColumnName("Manufacturer");
            Property(m => m.DatabookId)
               .HasColumnName("databook_id");
            Property(m => m.Filler)
               .HasColumnName("Filler");
                Property(m => m.CASRN)
               .HasColumnName("CAS RN");
                Property(m => m.UNSNo)
               .HasColumnName("UNS");
            Ignore(m => m.ClassificationId);
            Ignore(m => m.Classification);
            Ignore(m => m.NumEquivalency);
            Ignore(m => m.NumProcessing);

        }

    }
}
