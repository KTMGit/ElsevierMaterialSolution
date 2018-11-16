using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class AdvSearchResultViewMap : EntityTypeConfiguration<AdvSearchResults>
    {
        // MaterialID, PropertyID, Value, ValueMin, ValueMax

        public AdvSearchResultViewMap()
        {
            ToTable("View_EMS_AdvSearch");
            HasKey(t => new { t.MaterialID });

            Property(m => m.MaterialID)
                .HasColumnName("MaterialID");
            Property(m => m.PropertyID)
               .HasColumnName("PropertyID");
            Property(m => m.Value)
               .HasColumnName("Value");
            Property(m => m.ValueMin)
               .HasColumnName("ValueMin");
            Property(m => m.ValueMax)
               .HasColumnName("ValueMax");

            //Ignore(m => m.ClassificationId);
            //Ignore(m => m.Classification);
        }

    }
}