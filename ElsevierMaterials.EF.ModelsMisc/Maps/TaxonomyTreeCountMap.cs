using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class TaxonomyTreeCountMap : EntityTypeConfiguration<TaxonomyTreeCount>
    {
        public TaxonomyTreeCountMap()
        {
            ToTable("View_TaxonomyTreeCount");
            HasKey(t => new { t.Id, t.TypeId, t.ClassId, t.SubClassId, t.GroupId });

            Property(m => m.Id)
                  .HasColumnName("ID");
            Property(m => m.TypeId)
                 .HasColumnName("type_ID");
            Property(m => m.TypeCount)
                .HasColumnName("NumTYPE_ID");
            Property(m => m.ClassId)
                .HasColumnName("subClass_ID");
            Property(m => m.ClassCount)
                .HasColumnName("NumsubClass_ID");
            Property(m => m.SubClassId)
                .HasColumnName("group_ID");
            Property(m => m.SubClassCount)
                 .HasColumnName("Numgroup_ID");
            Property(m => m.GroupId)
                .HasColumnName("class_ID");
            Property(m => m.GroupCount)
                .HasColumnName("NumClass_ID");
        }
    }
}
