using ElsevierMaterials.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps {

    public class PrefferedNamesMap : EntityTypeConfiguration<PrefferedNames>
    {
        public PrefferedNamesMap()
        {
            Map(e =>
            {
                e.Properties(p =>
                    new { p.PN_ID, p.PN, p.taxonomy_id });
                e.ToTable("preferred names");
                Property(b => b.PN_ID).HasColumnName("PN ID");
                Property(b => b.PN).HasColumnName("PN");
                Property(b => b.taxonomy_id).HasColumnName("taxonomy_id");
            });

            HasKey(m => m.PN_ID);
          

        }
    }
}
