using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{

    public class SampleMaterialMap
        {
            public SampleMaterialMap()
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


                //HasKey(m => m.Id);

                //Property(m => m.Id)
                //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                //Property(m => m.Name)
                //    .HasMaxLength(100)
                //    .IsRequired();

                //HasRequired(p => p.Classification)
                //.WithMany(g => g.SampleMaterials)
                //.HasForeignKey(p => p.ClassificationId);

                //Property(m => m.ClassificationId)
                //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                //    .HasColumnName("ClassificationId");
                //Property(m => m.EquivalenceId)
                //  .HasColumnType("int")
                //    .HasColumnName("EquivalenceId");
               
           
            }
        }
    
}
