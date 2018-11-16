using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;



namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
   
        public class TreeMap : EntityTypeConfiguration<Tree>
        {
            public TreeMap()
            {
                ToTable("View_Tree");
                HasKey(m => m.PN_ID);
                //HasKey(n => new { n.PN_ID, n.BT_ID });  

                //Property(m => m.Id)
                //    .HasColumnName("Id");
                Property(m => m.PN_ID)
                      .HasColumnName("PN ID")
                    .IsRequired();
                 Property(m => m.BT_ID)
                      .HasColumnName("BT ID");
                 Property(m => m.Name)
                     .HasColumnName("PN");
                 Property(m => m.Root)
                     .HasColumnName("Root");
                 Property(m => m.taxonomy_id)
                     .HasColumnName("taxonomy_id");
                 Property(m => m.metals)
                     .HasColumnName("metals");
                 Property(m => m.plastics)
                     .HasColumnName("plastics");
                 Property(m => m.chemicals)
                      .HasColumnName("chemicals");

            }
        }

   
}

