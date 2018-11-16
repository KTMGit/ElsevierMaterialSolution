using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;

namespace ElsevierMaterialsMVC.Models.Search
{
    public class SampleMaterialModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public Classification Classification { get; set; }

        public string TypeName { get; set; }
        public string GroupName { get; set; }
        public string ClassName { get; set; }
        public string SubClassName { get; set; }

        public int? TypeId { get; set; }
        public int? GroupId { get; set; }
        public int? ClassId { get; set; }
        public int? SubClassId { get; set; }
        //public int EquivalenceId { get; set; }
        public string UNS { get; set; }
        public string CAS_RN { get; set; }
        public string StructureImage { get; set; }
    }
}