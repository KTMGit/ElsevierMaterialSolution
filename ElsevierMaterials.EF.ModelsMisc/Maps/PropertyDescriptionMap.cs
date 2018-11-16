using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class PropertyDescriptionMap : EntityTypeConfiguration<PropertyDescription> 
    {
        public PropertyDescriptionMap()
        {
            ToTable("View_PropertyDescription");
            HasKey(m => m.MaterialId);


            Property(m => m.MaterialId)
                .HasColumnName("PN ID");
            Property(m => m.CASName)
                .HasColumnName("CAS_name");
            Property(m => m.IUPACName)
               .HasColumnName("IUPAC_name");
            Property(m => m.Family)
               .HasColumnName("family");
            Property(m => m.Subfamily)
               .HasColumnName("subfamily");
            Property(m => m.StandardState)
               .HasColumnName("standard_state");
          Property(m => m.MolecularFormula)
               .HasColumnName("molecular_formula");
          Property(m => m.Structure)
               .HasColumnName("structure");
          Property(m => m.SMILES)
               .HasColumnName("SMILES");
          Property(m => m.InChI)
           .HasColumnName("InChI");
                  Property(m => m.InChIKey)
           .HasColumnName("InChIKey");
                  Property(m => m.molecular_weight)
           .HasColumnName("molecular_weight");

        }
    }
}
