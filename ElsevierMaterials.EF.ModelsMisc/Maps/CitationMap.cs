using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ElsevierMaterials.Models;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class CitationMap : EntityTypeConfiguration<Citation>
    {
        public CitationMap()
        {
            ToTable("View_Citations");
            HasKey(n => new { n.record_id}); 
        }
    }
}
