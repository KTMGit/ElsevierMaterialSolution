//using ElsevierMaterials.Models;
//using ElsevierMaterials.Models.Domain.AdvancedSearch;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace ElsevierMaterialsMVC.Models.AdvancedSearch
//{
//    public class PropertyConditionModel
//    {
//        public string UniqueID { get; set; }
//        public AdvSearchPropertyConditions Condition { get; set; }
//        public IList<UnitModel> Units { get; set; }
//        public int SelectedUnit { get; set; }
//        public string ValueFrom { get; set; }
//        public string ValueTo { get; set; }
//        public IList<LogicalOperator> LogicalOperators { get; set; }
//        public LogicalOperators SelectedLogical { get; set; }

//        public PropertyConditionModel()
//        {
//            UniqueID = Guid.NewGuid().ToString("N");
//            Units = new List<UnitModel>();

//            LogicalOperators = new List<LogicalOperator>();
//            LogicalOperators.Add(new LogicalOperator() { Name = "<=", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Lte });
//            LogicalOperators.Add(new LogicalOperator() { Name = ">=", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Gte });
//            LogicalOperators.Add(new LogicalOperator() { Name = "is between", Id = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Between });
//            SelectedLogical = ElsevierMaterials.Models.Domain.AdvancedSearch.LogicalOperators.Between;
//        }

//        // TODO:
//        // - Napravi metodu za punjenje Units-a
//        // - Setuj Default SelectedUnit

//        public void FillUnits()
//        {
//            // TODO: FillUnits
//        }
//    }
//}