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
    public class GroupChemicalPropertyAllMap: EntityTypeConfiguration<GroupChemicalPropertyAll>
    {
        public GroupChemicalPropertyAllMap()
        {
            ToTable("GroupChemicalPropertiesAll");
            HasKey(n => new { n.MaterialID, n.SubgroupID, n.GroupId, n.PropertyID});
            Ignore(n => n.ConvUnit);
            Ignore(n => n.UsValue);
            Ignore(n => n.UsValueMin);
            Ignore(n => n.UsValueMax);
            Ignore(n => n.UsUnit);
        }
    }
}
