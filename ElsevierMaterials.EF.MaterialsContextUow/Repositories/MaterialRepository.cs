using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using ElsevierMaterials.Services;
namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(DbContext dbContext) : base(dbContext) { }



        public ICollection<Material> GetMaterialByEquivalence(int equivalentId, FullTextSearch fts,string sessionId)
        {
            Classification cl = new Classification();
            if (fts.material_type != null) cl.ClassificationNames.Add(ClassificationType.Type, fts.material_type);
            if (fts.material_group != null) cl.ClassificationNames.Add(ClassificationType.Group, fts.material_group);
            if (fts.material_class != null) cl.ClassificationNames.Add(ClassificationType.Class, fts.material_class);
            if (fts.material_subClass != null) cl.ClassificationNames.Add(ClassificationType.Subclass, fts.material_subClass);
          
            ICollection<Material> materialsELS= DataSet.Where(m => m.MaterialId == equivalentId).Where(m => m.SourceId == 1).ToList();
            ICollection<Material> materialsTMMetals = DataSet.Where(m => m.MaterialId == equivalentId).Where(m=>m.SourceId == 2).ToList();
            ICollection<Material> materialsTMPLUS = DataSet.Where(m => m.MaterialId == equivalentId).Where(m => m.SourceId == 3).ToList();

            ICollection<Material> results = new List<Material>();
            int eqNumber = 0;

            foreach (var item in materialsELS)        
            {  
                item.Name = fts.material_designation;
                item.Classification = cl;
                results.Add(item);
            }

            IService service = new TotalMateriaService();
            IPlusService servicePLUS = new TMPlusService();
            ICollection<Material> materialList = new HashSet<Material>();



            if (materialsTMMetals.Count > 0)
            {
                materialList = service.GetMetalsMaterialsSubgroupListFromService(sessionId, materialsTMMetals.Select(m => m.SourceMaterialId).ToList());
                foreach (Material itemTM in materialList)
                {
                    itemTM.MaterialId = materialsTMMetals.Where(m => m.SourceMaterialId == itemTM.SourceMaterialId).Select(m => m.MaterialId).FirstOrDefault();
                    itemTM.Classification = cl;
                    itemTM.Name = fts.material_designation;
                    if (eqNumber == 0)
                    {
                        eqNumber = itemTM.NumEquivalency;
                    }
                    results.Add(itemTM);
                }
            }
        

            if (materialsTMPLUS.Count > 0)
            {
                materialList = servicePLUS.GetPLUSMaterialsSubgroupListFromService(sessionId, materialsTMPLUS.Select(m => m.SourceMaterialId).ToList());
                foreach (Material itemTM in materialList)
                {
                    itemTM.MaterialId = materialsTMPLUS.Where(m => m.SourceMaterialId == itemTM.SourceMaterialId).Select(m => m.MaterialId).FirstOrDefault();
                    itemTM.Classification = cl;
                    itemTM.Name = fts.material_designation;
                    if (eqNumber == 0)
                    {
                        eqNumber = itemTM.NumEquivalency;
                    }
                    results.Add(itemTM);
                }
            }
          
                      

            foreach (var it in results.Where(s => s.SourceId == 1))
            {
                it.NumEquivalency = eqNumber;
            }

            return results;
        }
    }
}
       
               
