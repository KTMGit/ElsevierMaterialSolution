using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class TreeCountMap : EntityTypeConfiguration<TreeCount>
    {
        public TreeCountMap()
        {
           ToTable("View_TreeCount");
           HasKey(t => new { t.Id });

            Property(m => m.Id)
                  .HasColumnName("ID");
            Property(m => m.TypeId)
                 .HasColumnName("type_ID");
            Property(m => m.TypeCount)
                .HasColumnName("NumTYPE_ID");
            Property(m => m.ClassId)
                .HasColumnName("group_ID");
            Property(m => m.ClassCount)
                .HasColumnName("Numgroup_ID");
            Property(m => m.SubClassId)
                .HasColumnName("class_ID");
            Property(m => m.SubClassCount)
                 .HasColumnName("NumClass_ID");
            Property(m => m.GroupId)
                .HasColumnName("subClass_ID");
            Property(m => m.GroupCount)
                .HasColumnName("NumsubClass_ID");
        }
    }
}
