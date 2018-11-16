using System.Collections.Generic;
using ElsevierMaterials.Models;


namespace ElsevierMaterials.Common.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        ICollection<Material> GetMaterialByEquivalence(int equivalentId, FullTextSearch cl, string sessionId);
       
    }
}
