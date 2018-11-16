//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using ElsevierMaterials.Models.Domain.AdvancedSearch;
//using ElsevierMaterials.Models;
//using ElsevierMaterials.EF.MaterialsContextUow;

//namespace ElsevierMaterialsMVC.Models.AdvancedSearch
//{
//    public class PropertyUnitModel
//    {
//        private MaterialsContextUow MaterialContextUow = new MaterialsContextUow();

//        public PropertyType PropertyType { get; set; }
//        public string UniqueID { get; set; }
//        public int PropertyID { get; set; }
//        public string PropertyName { get; set; }
//        public IList<UnitModel> Units { get; set; }
//        public int SelectedUnit { get; set; }
//        public string ValueFrom { get; set; }
//        public string ValueTo { get; set; }

//        public IList<PropertyConditionModel> PropertyConditions { get; set; }
//        public bool IsPropertyConditionsActive { get; set; }

//        public IList<BinaryOperator> BinaryOperators { get; set; }
//        public BinaryOperators SelectedBinary { get; set; }
//        public IList<LogicalOperator> LogicalOperators { get; set; }
//        public LogicalOperators SelectedLogical { get; set; }

//        public AdvStructureSearch StructureSearch { get; set; }

//        public PropertyUnitModel()
//        {
//            UniqueID = Guid.NewGuid().ToString("N");
//            Units = new List<UnitModel>();
//            BinaryOperators = new List<BinaryOperator>();
//            BinaryOperators.Add(new BinaryOperator() { Name = "And", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.BinaryOperators.And });
//            BinaryOperators.Add(new BinaryOperator() { Name = "Or", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.BinaryOperators.Or });
//            BinaryOperators.Add(new BinaryOperator() { Name = "Not", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.BinaryOperators.Not });
//            SelectedBinary = ElsevierMaterials.Models.Domain.AdvancedSearch.BinaryOperators.And;
//            LogicalOperators = new List<LogicalOperator>();
//            LogicalOperators.Add(new LogicalOperator() { Name = "exists", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Exists });
//            LogicalOperators.Add(new LogicalOperator() { Name = "=", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Eq });
//            LogicalOperators.Add(new LogicalOperator() { Name = "<=", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Lte });
//            LogicalOperators.Add(new LogicalOperator() { Name = ">=", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Gte });
//            LogicalOperators.Add(new LogicalOperator() { Name = "is between", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Between });
//            SelectedLogical = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Exists;
//            PropertyConditions = new List<PropertyConditionModel>();
//            IsPropertyConditionsActive = false;
//        }

//        // Fill PropertyConditions for selected property
//        public void FillPropertyConditions()
//        {
//            IList<AdvSearchPropertyConditions> pcs = MaterialContextUow.AdvSearchPropertyConditionsAll.AllAsNoTracking.Where(p => p.PropertyID == this.PropertyID).ToList();

//            foreach (AdvSearchPropertyConditions pc in pcs)
//            {
//                PropertyConditionModel pcm = new PropertyConditionModel();
//                pcm.Condition = new AdvSearchPropertyConditions()
//                {
//                    PropertyID = pc.PropertyID,
//                    UnitGroup = pc.UnitGroup,
//                    X_label = pc.X_label.Trim()
//                };
//                pcm.FillUnits();

//                PropertyConditions.Add(pcm);
//            }
//        }
//    }


//}