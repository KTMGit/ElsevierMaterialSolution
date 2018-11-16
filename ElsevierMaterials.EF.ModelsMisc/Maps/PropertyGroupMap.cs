using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
   public class PropertyGroupMap : EntityTypeConfiguration<PropertyGroup>
    {

        public PropertyGroupMap()
        {
            ToTable("View_PropertyGroupCount");
            HasKey(n => new { n.MaterialId, n.SubgroupId,n.PGId });
        }

    }
}
