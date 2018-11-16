using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Search;

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class MaterialDetailsModel
    {
        public PropertiesModel Properties { get; set; }
        public Material Material { get; set; }
        public SearchCondition Filter { get; set; }
        public EquivalencyModel Equivalency { get; set; }
        public ProcessingModel Processing { get; set; }
        public string  TypeName {get;set;}
        public string  GroupName {get; set;}
        public string ClassName{get;set;}
        public string SubClassName { get; set; }
        public string UNSNo { get; set; }
        public string CASRN { get; set; }
        public string ActiveTab { get; set; }
        public bool IsChemical { get; set; }
        public IList<ElsevierMaterialsMVC.Models.Shared.PropertyDescription> ChemicalElsProperties { get; set; }
        public IList<MaterialCountersModel> MaterialCounters { get; set; }       
        
        
        public MaterialDetailsModel() {
            Properties = new  PropertiesModel();
            MaterialCounters = new List<MaterialCountersModel>();
            ChemicalElsProperties = new List<ElsevierMaterialsMVC.Models.Shared.PropertyDescription>();            
          
        }   
      
    }
}