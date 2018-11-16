using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.PropertyGroups
{
    public class GroupMaterialCondition
    {
        public int MaterialID { get; set; }
        public int SubgroupID { get; set; }
        public int GroupId { get; set; }
        public int ConditionId { get; set; }
        public string Condition { get; set; }
        public int ProductFormId { get; set; }
        public string ProductForm { get; set; }
        public string  Thickness { get; set; }
        //public string TSCF { get; set; }
        public string MaterialDescription { get; set; }
        public string Phase { get; set; }
    }
}
