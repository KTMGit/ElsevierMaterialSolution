using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class GroupTestConditionMap : EntityTypeConfiguration<GroupTestCondition>
    {

        public GroupTestConditionMap()
        {
            ToTable("View_GroupTestConditions");
            HasKey(n => new { n.MaterialID, n.SubgroupID, n.GroupId });
        }

    }
}
