using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.EF.Common.Models;
using ElsevierMaterials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories {
    public class ClassificationRepository : BaseRepository<PrefferedNames>, IClassificationRepository {
        public ClassificationRepository(DbContext dbContext) : base(dbContext) { 
            
        }
        public Classification GetFullClassificationForMaterial(List<Tree> fullHierarchy) {
            Classification classification = new Classification();

            /*Full hierarchy list  has as much member as depth of the classification*/
            if (fullHierarchy.Count == 0) return classification;
            for (int i = 1; i <= fullHierarchy.Count; i++)
			{
                classification.ClassificationNames.Add((ClassificationType)i, DataSet.Find(fullHierarchy[i - 1].PN_ID).PN);
			}
            classification.Id = fullHierarchy[fullHierarchy.Count - 1].PN_ID;
            classification.Level = fullHierarchy.Count;
            classification.Parent = fullHierarchy.Count > 1 ? fullHierarchy[fullHierarchy.Count - 2].PN_ID : 0;
            classification.Name = DataSet.Find(fullHierarchy[fullHierarchy.Count - 1].PN_ID).PN;
            return classification;
        }

        //public IList<TypeClass> FillFullClassificationWithNames(IList<TypeClass> classificationList){
        //    foreach (var item in classificationList) {
        //        item.TypeClassName = DataSet.Find(item.TypeClassId).PN;
        //        foreach (var itemG in item.Groups) {
        //            itemG.GroupModelName = DataSet.Find(itemG.GroupModelId).PN;
        //            foreach (var itemC in itemG.Classes) {
        //                itemC.ClassModelName = DataSet.Find(itemC.ClassModelId).PN;
        //                foreach (var itemSC in itemC.Subclasses) {
        //                    itemSC.SubclassName = DataSet.Find(itemSC.SubclassModelId).PN;
        //                }
        //            }
        //        }
        //    }
        //        return classificationList;
        //}

        //public IList<PropertyGroupModel> FillAllPropertiesAndGroupsWithNames(IList<PropertyGroupModel> groupList) {
        //    foreach (var item in groupList) {
        //        item.PropertyGroupModelName = DataSet.Find(item.PropertyGroupModelId).PN;
        //        foreach (var itemP in item.Properties) {
        //            itemP.PropertyModelName = DataSet.Find(itemP.PropertyModelId).PN;
        //        }
        //    }
        //    return groupList;
        //}
    }
}