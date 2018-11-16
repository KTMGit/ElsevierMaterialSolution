using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class MaterialTaxonomyMap : EntityTypeConfiguration<MaterialTaxonomy>
    {
        public MaterialTaxonomyMap()
        {
            ToTable("MaterialTaxonomy");
            HasKey(n => new { n.ID, n.Level1, n.Level2, n.Level3, n.Level4 });
        }
    }
}
