using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class PropertyConditionModel
    {
        public string UniqueID { get; set; }
        public AdvSearchPropertyConditions Condition { get; set; }
        public IList<UnitModel> Units { get; set; }
        public int SelectedUnit { get; set; }
        public string ValueFrom { get; set; }
        public string ValueTo { get; set; }
        public IList<LogicalOperator> LogicalOperators { get; set; }
        public LogicalOperators SelectedLogical { get; set; }

        public PropertyConditionModel()
        {
            UniqueID = Guid.NewGuid().ToString("N");
            Units = new List<UnitModel>();

            LogicalOperators = new List<LogicalOperator>();
            LogicalOperators.Add(new LogicalOperator() { Name = "<=", Id = ElsevierMaterials.Models.LogicalOperators.Lte });
            LogicalOperators.Add(new LogicalOperator() { Name = ">=", Id = ElsevierMaterials.Models.LogicalOperators.Gte });
            LogicalOperators.Add(new LogicalOperator() { Name = "is between", Id = ElsevierMaterials.Models.LogicalOperators.Between });
            SelectedLogical = ElsevierMaterials.Models.LogicalOperators.Between;
        }

        public void FillUnits()
        {
            // TODO: FillUnits          ( f(this.Condition) )
            // - Setuj Default SelectedUnit

            if (this.Condition.X_label.Trim().ToLower() == "pressure")
            {
                this.Units.Add(new UnitModel()
                {
                    Factor = 1,
                    Offset = 0,
                    UnitLabel = "kPa",
                    Metric = true,
                    UnitKey = -1
                });
            }
            else if (this.Condition.X_label.Trim().ToLower() == "temperature")
            {
                this.Units.Add(new UnitModel()
                {
                    Factor = 1,
                    Offset = 0,
                    UnitLabel = "K",
                    Metric = true,
                    UnitKey = -1
                });
            }
            else if (this.Condition.X_label.Trim().ToLower() == "wavelength")
            {
                this.Units.Add(new UnitModel()
                {
                    Factor = 1,
                    Offset = 0,
                    UnitLabel = "nm",
                    Metric = true,
                    UnitKey = -1
                });
            }

        }
    }
}
