using ElsevierMaterials.Models.Domain.Property;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class PropertyWithConvertedValuesMap : EntityTypeConfiguration<PropertyWithConvertedValues>
    {

        public PropertyWithConvertedValuesMap()
        {
            ToTable("View_PropertiesConversions");
                 HasKey(t => new { t.MaterialId, t.SubgroupId, t.PropertyId, t.RowId, t.ValueId});

            Property(m => m.RowId)
                .HasColumnName("RowID");
            Property(m => m.PropertyId)
                .HasColumnName("PropertyID");
            Property(m => m.MaterialId)
                .HasColumnName("MaterialID");
            Property(m => m.SubgroupId)
                .HasColumnName("SubgroupID");
            Property(m => m.ValueId)
                 .HasColumnName("ValueID");
            Property(m => m.GroupId)
                 .HasColumnName("GroupId");
            
            Property(m => m.AdditionalCondition)
                .HasColumnName("AdditionalCondition");
                Property(m => m.SpecimenOrientation)
              .HasColumnName("specimen_orientation");

            Property(m => m.Temperature)
               .HasColumnName("Temperature");
            Property(m => m.OrigValue)
               .HasColumnName("OrigValue");
            Property(m => m.OrigValueMin)
               .HasColumnName("OrigValueMin");
            Property(m => m.OrigValueMax)
              .HasColumnName("OrigValueMax");

                Property(m => m.OrigValueText)
               .HasColumnName("OrigValueText");

                Property(m => m.OrigUnitId)
               .HasColumnName("OrigUnitId");
            
                Property(m => m.ConvValue)
                 .HasColumnName("ConvValue");
                Property(m => m.ConvValueMin)
                 .HasColumnName("ConvValueMin");
                Property(m => m.ConvValueMax)
                 .HasColumnName("ConvValueMax");


                Property(m => m.UsValueMax)
              .HasColumnName("UsValueMax");
                Property(m => m.UsValueMin)
                 .HasColumnName("UsValueMin");
                Property(m => m.UsValue)
                 .HasColumnName("UsValue");




                Property(m => m.DefaultUnitId)
                 .HasColumnName("DefaultUnitId");
            
                Property(m => m.DefaultUnit)
                  .HasColumnName("DefaultUnit");
                Property(m => m.OriginalUnit)
                 .HasColumnName("OriginalUnit");
                Property(m => m.UsUnit)
                  .HasColumnName("UsUnit");

                Property(m => m.PropertyName)
                .HasColumnName("PropertyName");
    


        }
    }
}
