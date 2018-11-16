
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterials.Models.Domain.Material;
using ElsevierMaterials.Models;
using ElsevierMaterials.Common.Interfaces;

namespace ElsevierMaterialsMVC.BL.Binders.MaterialBasic
{
    public class MaterialBinder
    {
        public MaterialBinder()
        {
            _materialElsBinder = new MaterialElsBinder();
            _materialTMMetalsBinder = new MaterialTMMetalsBinder();
            _materialTMPlusBinder = new MaterialTMPlusBinder();     
        }

        private MaterialElsBinder _materialElsBinder;
        private MaterialTMMetalsBinder _materialTMMetalsBinder;
        private MaterialTMPlusBinder _materialTMPlusBinder;


        public MaterialBasicInfo GetMaterialInfo(int materialId, int sourceMaterialId, int sourceId, int subgroupId, IMaterialsContextUow materialContextUow)
        {
            MaterialBasicInfo material = null;
            switch (sourceId)
            {
                case 1:
                    material = _materialElsBinder.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    break;
                case 2:
                    material = _materialTMMetalsBinder.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    break;
                case 3:
                    material = _materialTMPlusBinder.GetMaterialInfo(materialId, sourceMaterialId, sourceId, subgroupId, materialContextUow);
                    break;
                default:
                    break;
            }
            FullTextSearch fts = materialContextUow.FullTextSearch.GetMaterialById(materialId);
            material.TypeId = fts.type_ID;
            material.TypeName = fts.material_type;
            return material;
        }
        
    }
}