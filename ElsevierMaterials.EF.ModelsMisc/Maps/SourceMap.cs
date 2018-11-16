using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace ElsevierMaterials.EF.ModelsMisc.Maps
{

    public class SourceMap : EntityTypeConfiguration<Source>
    {
        public SourceMap()
        {
            ToTable("View_Source");
            HasKey(m => m.Id);

            Property(m => m.Databook_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Databook_id");
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id");
            Property(m => m.Name)
                .HasMaxLength(100)
                .IsRequired();
            Property(m => m.SourceUrl)
             .HasMaxLength(100)
             .IsRequired();
        }
    }


}
