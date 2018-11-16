using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Binders.MaterialBasic
{
    public class MaterialTMPlusBinder
    {
        
        public ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo GetMaterialInfo(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow context)
        {
            FullTextSearch fts = context.FullTextSearch.GetMaterialById(materialId);

            ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo material = null;
            IPlusService servicePLUS = new TMPlusService();
            material = servicePLUS.GetMaterialSubgroupPLUSListFromService(System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString(), materialId, sourceMaterialId).Where(m => m.MaterialId == materialId && m.SubgroupId == subgroupId).Select(m => new ElsevierMaterials.Models.Domain.Material.MaterialBasicInfo { MaterialId = m.MaterialId, Name = m.Name, SourceId = m.SourceId, SourceMaterialId = m.SourceMaterialId, Standard = m.Standard, SubgroupId = m.SubgroupId, SubgroupName = (m.SourceText == "-" ? "" : m.SourceText.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Standard == "-" ? "" : m.Standard.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ") + "; ") + (m.Specification == "-" ? "" : m.Specification.Replace("<br>", " ").Replace("<br >", " ").Replace("<br />", " ").Replace("<br/>", " ")) }).FirstOrDefault();

            if (material != null)
            {
                material.Name = fts.material_designation;
            }
            
            return material;
        }

   
    }
}