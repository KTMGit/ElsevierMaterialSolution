using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories {
    public class FullTextSearchRepository : BaseRepository<FullTextSearch>, IFullTextSearchRepository {
        public FullTextSearchRepository(DbContext dbContext)
            : base(dbContext) { 
            
        }
        public IEnumerable<FullTextSearch> GetMaterialsByFullTextSearch(bool withTracking, string searchString) {
          string cleanSearchString =  RemoveCharacters(searchString);
          string   searchStringWS = RemoveCharactersWS(searchString);

            var commandText = "SELECT * FROM [fn_FullTextSearch] ({0},{1});";       
            var parameters = new object[] { searchStringWS, cleanSearchString };
            var datasetSqlQuery = DataSet.SqlQuery(commandText, parameters);
            if (!withTracking) { datasetSqlQuery = datasetSqlQuery.AsNoTracking(); }

            return datasetSqlQuery;

        }
        public IEnumerable<int> GetMaterialsIdsByFullTextSearch(string searchString)
        {
            string cleanSearchString = RemoveCharacters(searchString);
            string searchStringWS = RemoveCharactersWS(searchString);

            var commandText = "SELECT [ID] FROM [fn_FullTextSearch] ({0},{1});";
            var parameters = new object[] { searchStringWS, cleanSearchString };
            var datasetSqlQuery =SqlQuery<int>(commandText, parameters);
            //if (!withTracking) { datasetSqlQuery = datasetSqlQuery.AsNoTracking(); }

            return datasetSqlQuery;

        }
        public int GetMaterialsCount(string searchString)
        {
            var commandText = "SELECT count(*) FROM [fn_FullTextSearch] ({0},{1});";
            string cleanSearchString = RemoveCharacters(searchString);
            string searchStringWS = RemoveCharactersWS(searchString);


            var parameters = new object[] { searchStringWS, cleanSearchString };
            //IList<int> results = context.FullTextSearch.SqlQuery<int>(commandText, parameters).ToList();
            return SqlQuery<int>(commandText, parameters).Single();
        }

        public FullTextSearch GetMaterialById(int materialId) {
            return DataSet.Find(materialId);
        }

        public IEnumerable<FullTextSearch> GetMaterialsByMaterialIds(IEnumerable<int> ids)
        {
            return DataSet.Where(m => ids.Contains(m.Id));
            //return DataSet.Where(m => ids.Any(id => m.Id == id));
        }

        public string RemoveCharacters(string m)
        {
            string retval = null;
            retval = m.Replace("-", "");
            retval = retval.Replace("-", "");
            retval = retval.Replace(" ", "");
            retval = retval.Replace(".", "");
            retval = retval.Replace(",", "");
            retval = retval.Replace(":", "");
            retval = retval.Replace("/", "");
            retval = retval.Replace("\\", "");
            retval = retval.Replace("{", "");
            retval = retval.Replace("}", "");
            retval = retval.Replace("[", "");
            retval = retval.Replace("]", "");
            retval = retval.Replace("(", "");
            retval = retval.Replace(")", "");
            retval = retval.Replace(";", "");
            retval = retval.Replace("+", "");
            retval = retval.Replace("'", "");
           
            return retval;
        }
        public string RemoveCharactersWS(string m)
        {
            string retval = null;
            retval = m.Replace("-", "");
            retval = retval.Replace("-", "");
         
            retval = retval.Replace(".", "");
            retval = retval.Replace(",", "");
            retval = retval.Replace(":", "");
            retval = retval.Replace("/", "");
            retval = retval.Replace("\\", "");
            retval = retval.Replace("{", "");
            retval = retval.Replace("}", "");
            retval = retval.Replace("[", "");
            retval = retval.Replace("]", "");
            retval = retval.Replace("(", "");
            retval = retval.Replace(")", "");
            retval = retval.Replace(";", "");
            retval = retval.Replace("+", "");
            retval = retval.Replace("'", "");

            return retval;
        }
    }
}
