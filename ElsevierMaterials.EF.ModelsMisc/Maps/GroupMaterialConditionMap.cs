using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class GroupMaterialConditionMap : EntityTypeConfiguration<GroupMaterialCondition>
    {

        public GroupMaterialConditionMap()
        {
            ToTable("View_GroupMaterialConditions");
            HasKey(n => new { n.MaterialID, n.SubgroupID, n.GroupId });
        }

    }
}
