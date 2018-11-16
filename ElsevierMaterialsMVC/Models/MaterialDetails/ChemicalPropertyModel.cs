using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class ChemicalPropertyModel
    {
        public int GroupId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Phase { get; set; }
        public int? PhaseId { get; set; }
        public string PNameAndPhase { get; set; }
        public IList<ChemicalConditionModel> ConditionList { get; set; }
        public int? DiagramID { get; set; }
    }
}