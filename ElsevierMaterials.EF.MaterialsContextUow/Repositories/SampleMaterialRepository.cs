using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Common.Interfaces;
using System.Data.Entity;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{
    public class SampleMaterialRepository : BaseRepository<PrefferedNames>, ISampleMaterialRepository
    {
        public SampleMaterialRepository(DbContext dbContext) : base(dbContext) { }

        public IEnumerable<SampleMaterial> GetSampleMaterialSet() {
            IEnumerable<PrefferedNames> mat = DataSet.Where(b => b.taxonomy_id == null);
            return mat.Select(m => new SampleMaterial() { Id = m.PN_ID, Name = m.PN });
        }
        public IEnumerable<SampleMaterial> GetSampleMaterialSetByText(string filter)
        {
            IEnumerable<PrefferedNames> mat = DataSet.Where(b => b.taxonomy_id == null && b.PN.Contains(filter));
            return mat.Select(m => new SampleMaterial() { Id = m.PN_ID, Name = m.PN });
        }
    }
}
