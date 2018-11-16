using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Common.Interfaces {
    public interface IFullTextSearchRepository : IRepository<FullTextSearch> {
         IEnumerable<FullTextSearch> GetMaterialsByFullTextSearch(bool withTracking, string sqlString);
         IEnumerable<FullTextSearch> GetMaterialsByMaterialIds(IEnumerable<int> ids);
         FullTextSearch GetMaterialById(int materialId);
         IEnumerable<int> GetMaterialsIdsByFullTextSearch(string searchString);
         int GetMaterialsCount(string searchString);
    }
}
