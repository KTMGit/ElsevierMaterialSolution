using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class EquivalentMaterialMap : EntityTypeConfiguration<EquivalentMaterial>
    {
        public EquivalentMaterialMap() {
            ToTable("EquivalentMaterial");
            HasKey(t => new { t.MaterialId, t.SourceId, t.SourceMaterialId });
        }
    }
}