using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.PropertyGroups
{
    public class GroupTestCondition
    {

        //TODO: Ne treba mi lista sledecih osobina
        public int MaterialID { get; set; }
        public int SubgroupID { get; set; }
        public int RowID { get; set; }
        public int GroupId { get; set; }
        public int ProductFormId { get; set; }

        public string ProductForm { get; set; }
        public int ConditionId { get; set; }
        public string Condition { get; set; }
        //TODO: Ne treba mi lista sledecih osobina


        public string Temperature { get; set; }

        public string MaterialDesc { get; set; }

        public string Basis { get; set; }

        public string TestType { get; set; }

        public string StressRatio { get; set; }

        public string SpecimenOrientation { get; set; }

        public string HoldingTemperature { get; set; }

        public string HoldingTime { get; set; }

        public string SpecimenType { get; set; }

        public string Comment { get; set; }

        public string TSCF { get; set; }
        public string Thickness { get; set; }

        public string Pressure { get; set; }
    }
}
