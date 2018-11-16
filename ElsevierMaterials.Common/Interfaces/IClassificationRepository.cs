using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;

namespace ElsevierMaterials.Common.Interfaces {
    public interface IClassificationRepository : IRepository<PrefferedNames> {
        Classification GetFullClassificationForMaterial(List<Tree> fullHierarchy);
        //IList<TypeClass> FillFullClassificationWithNames(IList<TypeClass> classificationList);
        //IList<PropertyGroupModel> FillAllPropertiesAndGroupsWithNames(IList<PropertyGroupModel> groupList);
    }
}
