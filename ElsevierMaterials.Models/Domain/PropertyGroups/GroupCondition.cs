using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.PropertyGroups
{
    public class GroupCondition
    {

        public int MaterialID { get; set; }
        public int SubgroupID { get; set; }
        public int RowID { get; set; }
        public int GroupId { get; set; }
        public string ProductForm { get; set; }
        public string Condition { get; set; }
        public string Temperature { get; set; }
        public string Thickness { get; set; }
        public string MaterialDesc   { get; set; }
        public string Basis { get; set; }
    }
}
