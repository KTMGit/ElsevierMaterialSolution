using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Models;

namespace ElsevierMaterials.Common.Interfaces {
    public interface ITreeRepository : IRepository<Tree>{
        IList<Tree> GetFullTreeForMaterial(int materialId);
        IList<TypeClass> GetFullTreeFor(IDictionary<int, int> records = null);
        IList<PropertyGroupModel> GetFullPropertyGroups(int groupId);
        IDictionary<int, string> GetTreeNodesNames(IList<int> ids);
    }
}
