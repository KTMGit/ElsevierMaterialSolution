using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.CreepData
{
    public class CreepDataContainer
    {
        public int UnitType { get; set; }
        public Api.Models.CreepData.CreepData  Data { get; set; }
        public IList<string> SelectedReferences { get; set; }
    }
}
