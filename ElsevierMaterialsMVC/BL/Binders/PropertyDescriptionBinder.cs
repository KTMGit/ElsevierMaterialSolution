using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterialsMVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders
{
    public class PropertyDescriptionBinder
    {
        public IList<PropertyDescription> GetProperties(int materialId, IMaterialsContextUow context) {
            IList<PropertyDescription> properties = new List<PropertyDescription>();
   
            IList<ElsevierMaterials.Models.PropertyDescription> propertiesDb = context.PropertiesDescriptions.AllAsNoTracking.Where(s => s.MaterialId == materialId).ToList();
            foreach (var prop in propertiesDb)
            {

                if (prop.CASName != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.CAS_name, Text = prop.CASName, Name = "CAS name" });
                }
                   if (prop.IUPACName != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.IUPAC_name, Text = prop.IUPACName, Name = "IUPAC name" });
                }

                if (prop.Family != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.family, Text = prop.Family, Name = "family" });
                }
                   if (prop.Subfamily != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.subfamily, Text = prop.Subfamily, Name = "subfamily" });
                }
                  if (prop.StandardState != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.standard_state, Text = prop.StandardState, Name = "standard state" });
                }
                if (prop.MolecularFormula != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.molecular_formula, Text = prop.MolecularFormula, Name = "molecular formula" });
                }
                if (prop.molecular_weight != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.molecular_weight, Text = prop.molecular_weight.ToString(), Name = "molecular weight" });
                }           
                if (prop.Structure != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.structure, Text = prop.Structure, Name = "structure" });
                }
                if (prop.SMILES != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.SMILES, Text = prop.SMILES, Name = "SMILES" });
                }
                if (prop.InChI != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.InChI, Text = prop.InChI, Name = "InChI" });
                }

                if (prop.InChIKey != null)
                {
                    properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.InChIKey, Text = prop.InChIKey, Name = "InChI Key" });
                }

              
              
             
        
            }
            //properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.CAS_name, Name = "CAS name", Text = "dasdasdasd" });
            //properties.Add(new PropertyDescription { Type = PropertyDescriptionEnum.family, Name = "family", Text = "111111" });
            return properties;
        }
    }
}