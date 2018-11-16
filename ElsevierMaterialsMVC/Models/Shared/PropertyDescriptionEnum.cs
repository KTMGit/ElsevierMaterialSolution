using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Shared
{
    public enum PropertyDescriptionEnum
    {
        None = 0,
        CAS_name,
        IUPAC_name, 
        family,
        subfamily,
        standard_state,
        molecular_formula,
        structure,
        SMILES,
        InChI,
        InChIKey,
        molecular_weight
    }
}