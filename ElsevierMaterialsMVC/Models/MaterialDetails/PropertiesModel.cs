using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.Models.ChemicalComposition;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails {
    public class PropertiesModel {
        public IDictionary<ProductGroup.ProductGroupType, ProductGroup> ProductGroups { get; set; }
        //public  ChemicalComposition Composition { get; set; }
        public StressStrainModel StressStrain { get; set; }
        public FatigueModel FatigueStrain { get; set; }
        public FatigueModel FatigueStress { get; set; }
        public FatiguePlusModel FatiguePlus { get; set; }
        public MultipointDataModel MultipointData { get; set; }
        public CreepDataModel CreepData { get; set; }
        public CreepPlusModel CreepPlusData { get; set; }
        public PropertiesModel()
        {
            FatigueStrain = new FatigueModel();
            FatigueStress = new FatigueModel();
            CreepData = new CreepDataModel();
            CreepPlusData = new CreepPlusModel();
      

            ProductGroups = new Dictionary<ProductGroup.ProductGroupType, ProductGroup>();
        }
    }
}