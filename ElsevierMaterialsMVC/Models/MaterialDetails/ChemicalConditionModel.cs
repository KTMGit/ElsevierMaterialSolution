using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class ChemicalConditionModel
    {
      public bool HasMultipleTemperatures { get; set; }
      public IList<ChemicalConditionPointModel> TemperatureList { get; set; }
      public int RowId { get; set; }
      public int ValueId { get; set; }
      public string Name{ get; set; }
      public string PropertyRange{ get; set; }
      public string PropertyUnits{ get; set; }
      public string VariableRange{ get; set; }
      public string VariableUnits{ get; set; }
      public string DefaultPropertyRange { get; set; }
      public string DefaultPropertyUnits { get; set; }
      public string DefaultVariableRange { get; set; }
      public string DefaultVariableUnits { get; set; }
      public string USPropertyRange { get; set; }
      public string USPropertyUnits { get; set; }
      public string USVariableRange { get; set; }
      public string USVariableUnits { get; set; }
      public string Notes{ get; set; }
      public int? cit_record_id { get; set; }
    }
  
}