using System.Collections.Generic;
using ElsevierMaterials.Models;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface ISampleMaterialRepository : IRepository<PrefferedNames>
    {
        IEnumerable<SampleMaterial> GetSampleMaterialSet();
        IEnumerable<SampleMaterial> GetSampleMaterialSetByText(string filter);
    }
}
