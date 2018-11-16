using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps 
{
    public class EquivalentPropertyViewMap : EntityTypeConfiguration<EquivalentProperty>
    {
        // MaterialId, SourceMaterialId, SourceId, SubgroupID, SubgroupName, Standard, materialName, SourceText

        public EquivalentPropertyViewMap()
        {
            ToTable("View_EquivalentProperty");

            HasKey(t => new { t.SourcePropertyId });

            Property(m => m.PropertyId)
                .HasColumnName("PropertyId");
            Property(m => m.Name)
              .HasColumnName("PN");
            Property(m => m.SourcePropertyId)
               .HasColumnName("SourcePropertyId");
            Property(m => m.SourceId)
                 .HasColumnName("SourceId");
            Property(m => m.DefaultUnitId)
                 .HasColumnName("DefaultUnitId");
            Property(m => m.DefaultUnitName)
                 .HasColumnName("DefaultUnitName");


            
        }          
            
    }
}
