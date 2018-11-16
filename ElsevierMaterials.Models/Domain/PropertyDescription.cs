using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models {
    public class PropertyDescription {
        public int MaterialId { get; set; }
          public string CASName { get; set; }
          public string IUPACName { get; set; }
          public string Family { get; set; }
          public string Subfamily { get; set; }
          public string StandardState { get; set; }
          public string MolecularFormula { get; set; }
          public string Structure { get; set; }
          public string SMILES { get; set; }
          public string InChI { get; set; }
          public string InChIKey { get; set; }
          public double? molecular_weight { get; set; }
    }
}
