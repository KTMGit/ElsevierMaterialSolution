using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class ChemicalConditionPointModel
    {
        public ChemicalUnitValuesType type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    
    }
    public enum ChemicalUnitValuesType
    {
        Origin,
        Default,
        Us
    }
}