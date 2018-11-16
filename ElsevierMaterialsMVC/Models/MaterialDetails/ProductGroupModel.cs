using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.PropertyGroups;

namespace ElsevierMaterialsMVC.Models.MaterialDetails {
    public class ProductGroup {


        public bool MaterialConditionsVisible() {
            //TODO: N: Da li postoji mogucnost da ime uslova bude ""; Kako da pronadjem primer koji ima samo jedan all?
            if ((this.MaterialConditions.Count == 1 && this.MaterialConditions[0].ConditionName == "All") && this.MaterialConditions[0].ConditionName.Trim() != "")
            {
                return false;
            }
            return true;
        }

        public bool TestConditionsVisible()
        {
            //TODO: N: Da li postoji mogucnost da ime uslova bude ""; Da li moram da dodajem all u listu, samo da ispitam da li je lista prazn?Kako da pronadjem primer koji ima samo jedan all?
            if ((this.TestConditions.Count == 1 && this.TestConditions[0].ConditionName == "All") && this.TestConditions[0].ConditionName.Trim() != "")
            {
                return false;
            }
            return true;
        }

        public ProductGroupType ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public int ConditionId { get; set; }
        public string ConditionIdProductFormId { get; set; }
        public int RowId { get; set; }
        public IList<ConditionModel> Conditions { get; set; }
        public IList<ConditionModel> MaterialConditions { get; set; }
        public IList<ConditionModel> TestConditions { get; set; }

        /* Properties list for Chemicals, they are grouped by Properties, not by Conditions*/

        public IList<ChemicalPropertyModel> ChemicalProperties { get; set; }

        public int PropertyCount { get; set; }
        public IList<string> AllReferences { get; set; }
        public IList<ProductGroupType> GetOrder()
        {
            IList<ProductGroupType> list = new List<ProductGroupType>();
            list.Add(ProductGroupType.Mechanical);
            list.Add(ProductGroupType.Physical);
            list.Add(ProductGroupType.Electrical);
            list.Add(ProductGroupType.Machinability);
            list.Add(ProductGroupType.Applications);
            list.Add(ProductGroupType.FatigueData);
            list.Add(ProductGroupType.FractureData);
            list.Add(ProductGroupType.Thermal);
            list.Add(ProductGroupType.CreepData);
            list.Add(ProductGroupType.Chemical);

            list.Add(ProductGroupType.Rheology);
            list.Add(ProductGroupType.SurfaceProperties);
            list.Add(ProductGroupType.EnvironmentalCharacteristics);
            list.Add(ProductGroupType.SolutionProperties);
            list.Add(ProductGroupType.OpticalProperties);
            list.Add(ProductGroupType.MagneticProperties);
            list.Add(ProductGroupType.HazardRelatedProperties);
            list.Add(ProductGroupType.ChemicalProperties);
            list.Add(ProductGroupType.EquationPlotter);
            list.Add(ProductGroupType.ThermodynamicProperties);
            list.Add(ProductGroupType.ProcessingProperties);
            list.Add(ProductGroupType.CorrosionResistance);
            list.Add(ProductGroupType.ApplicationCharacterisitcs);
            return list;
        
        }

        public ProductGroup()
        {
            MaterialConditions = new List<ConditionModel>();
            TestConditions = new List<ConditionModel>();
        }

        public enum ProductGroupType
        {
            Mechanical = 806,
            Physical = 807,
            Electrical = 808,
            Machinability = 809,
            Chemical = 139941,
            Applications = 139946,
            FatigueData = 139968,
            FractureData = 139992,
            CreepData = 139993,
            Thermal = 139994,
            Rheology = 551876,
            SurfaceProperties = 551875,
            EnvironmentalCharacteristics = 551874,
            SolutionProperties = 377375,
            OpticalProperties = 377374,
            MagneticProperties = 377373,
            HazardRelatedProperties = 377372,
            ChemicalProperties = 377371,
            EquationPlotter = 585218,
            ThermodynamicProperties = 609579,
            ProcessingProperties = 609580,
            CorrosionResistance = 609581,
            ApplicationCharacterisitcs = 609582,
            StressStrain = -1
        }
    }

   
}