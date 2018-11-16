using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface IAdvSearchResultsRepository : IRepository<AdvSearchResults>
    {
        IEnumerable<int> GetMaterialsByAdvancedSearch(bool withTracking, AdvSearchFiltersAll filters, string sessionId, IQueryable<EquivalentProperty> propIds, IQueryable<EquivalentMaterial> matIds);
        IEnumerable<int> MaterialStructureSearch(string recordIds);
    }
}
