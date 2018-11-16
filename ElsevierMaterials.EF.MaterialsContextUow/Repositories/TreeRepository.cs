using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElsevierMaterials.Common.Interfaces;
using ElsevierMaterials.Models;
using ElsevierMaterials.EF.Common.Models;
using System.Data.Entity;

namespace ElsevierMaterials.EF.MaterialsContextUow.Repositories
{
    public class TreeRepository : BaseRepository<Tree>, ITreeRepository
    {

        public TreeRepository(DbContext dbContext) : base(dbContext) { }


        public IList<Tree> GetFullTreeForMaterial(int materialId)
        {
            List<Tree> treeM = new List<Tree>();
            Tree prevLastLevel = null;
            Tree afterFirst = null;
            Tree first = null;

            Tree materialNode = DataSet.Where(b => b.PN_ID == materialId).FirstOrDefault();
            if (materialNode != null)
            {
                Tree lastLevel = DataSet.Where(b => b.PN_ID == materialNode.BT_ID).FirstOrDefault();
                if (lastLevel != null)
                {
                    treeM.Add(lastLevel);
                    prevLastLevel = DataSet.Where(b => b.PN_ID == lastLevel.BT_ID).FirstOrDefault();
                    if (prevLastLevel != null)
                    {
                        treeM.Add(prevLastLevel);
                        afterFirst = DataSet.Where(b => b.PN_ID == prevLastLevel.BT_ID).FirstOrDefault();
                        if (afterFirst != null)
                        {
                            treeM.Add(afterFirst);
                            first = DataSet.Where(b => b.PN_ID == afterFirst.BT_ID).FirstOrDefault();
                            if (first != null)
                            {
                                treeM.Add(first);
                            }
                        }
                    }
                }
            }
            List<Tree> realTree = new List<Tree>();
            if (treeM.Count > 0)
            {
                for (int i = treeM.Count - 1; i >= 0; i--)
                {
                    realTree.Add(treeM[i]);
                }
            }
            return realTree;
        }

        public IList<TypeClass> GetFullTreeFor(IDictionary<int, int> records = null)
        {
            IList<TypeClass> classificationList = new List<TypeClass>();

            IList<Tree> types = DataSet.Where(m => m.taxonomy_id == 1 && m.BT_ID == null).ToList();
            foreach (var item in types)
            {
                TypeClass tc = new TypeClass
                {
                    TypeClassId = item.PN_ID,
                    Classes = new List<ClassModel>(),
                    TypeClassName = item.Name,
                    TypeClassCount = (records != null && records.ContainsKey(item.PN_ID) ? records[item.PN_ID] : 0)
                };

                IList<Tree> classes = DataSet.Where(n => n.BT_ID == item.PN_ID).ToList();
                foreach (var itemC in classes)
                {
                    ClassModel cm = new ClassModel
                    {
                        ClassModelId = itemC.PN_ID,
                        Subclasses = new List<SubclassModel>(),
                        ClassModelName = itemC.Name,
                        ClassCount = (records != null && records.ContainsKey(itemC.PN_ID) ? records[itemC.PN_ID] : 0)
                    };

                    IList<Tree> subclasses = DataSet.Where(m => m.BT_ID == itemC.PN_ID).ToList();
                    foreach (var itemS in subclasses)
                    {
                        SubclassModel sm = new SubclassModel
                        {
                            SubclassModelId = itemS.PN_ID,
                            Groups = new List<GroupModel>(),
                            SubclassName = itemS.Name,
                            SubclassCount = (records != null && records.ContainsKey(itemS.PN_ID) ? records[itemS.PN_ID] : 0)
                        };

                        IList<Tree> groups = DataSet.Where(l => l.BT_ID == itemS.PN_ID).ToList();
                        foreach (var itemG in groups)
                        {
                            sm.Groups.Add(new GroupModel
                            {
                                GroupModelId = itemG.PN_ID,
                                GroupModelName = itemG.Name,
                                GroupCount = (records != null && records.ContainsKey(itemG.PN_ID) ? records[itemG.PN_ID] : 0)
                            });
                        }
                        cm.Subclasses.Add(sm);
                    }
                    tc.Classes.Add(cm);
                }
                classificationList.Add(tc);
            }
            return classificationList;
        }


        public IDictionary<int, string> GetTreeNodesNames(IList<int> ids)
        {
            return (
                from c in (DataSet.Where(m => m.taxonomy_id == 1 && ids.Contains(m.PN_ID)).ToList())
                group c by new
                {
                    c.PN_ID,
                    c.Name
                } into grp
                select grp.First()
                ).ToDictionary(kvp => kvp.PN_ID, kvp => kvp.Name);
            //return DataSet.Where(m => m.taxonomy_id == 1 && ids.Contains(m.PN_ID)).Select(n => new { n.PN_ID, n.Name }).ToDictionary(kvp => kvp.PN_ID, kvp => kvp.Name);
        }



        public IList<PropertyGroupModel> GetFullPropertyGroups(int groupId = 0)
        {
            IList<PropertyGroupModel> groupList = new List<PropertyGroupModel>();
            IList<Tree> groups = DataSet.Where(m => m.taxonomy_id == 2 && m.BT_ID == null).ToList();
            PropertyGroupModel tc = new PropertyGroupModel();
            IList<Tree> properties = new List<Tree>();

            if (groupId == 0)
            {
                foreach (var item in groups)
                {
                    tc = new PropertyGroupModel { PropertyGroupModelId = item.PN_ID, Properties = new List<PropertyModel>(), PropertyGroupModelName = item.Name };
                    properties = DataSet.Where(n => n.BT_ID == item.PN_ID).ToList();
                    foreach (var itemP in properties)
                    {
                        tc.Properties.Add(new PropertyModel { PropertyModelId = itemP.PN_ID, PropertyModelName = itemP.Name });
                    }
                    groupList.Add(tc);
                }
            }
            else
            {
                switch (groupId)
                {
                    case 710:
                        foreach (var item in groups.Where(t => t.metals == true))
                        {
                            tc = new PropertyGroupModel { PropertyGroupModelId = item.PN_ID, Properties = new List<PropertyModel>(), PropertyGroupModelName = item.Name };
                            properties = DataSet.Where(n => n.BT_ID == item.PN_ID).ToList();
                            foreach (var itemP in properties.Where(p => p.metals == true))
                            {
                                tc.Properties.Add(new PropertyModel { PropertyModelId = itemP.PN_ID, PropertyModelName = itemP.Name });
                            }
                            groupList.Add(tc);
                        }
                        break;
                    case 711:
                        foreach (var item in groups.Where(t => t.plastics == true))
                        {
                            tc = new PropertyGroupModel { PropertyGroupModelId = item.PN_ID, Properties = new List<PropertyModel>(), PropertyGroupModelName = item.Name };
                            properties = DataSet.Where(n => n.BT_ID == item.PN_ID).ToList();
                            foreach (var itemP in properties.Where(p => p.plastics == true))
                            {
                                tc.Properties.Add(new PropertyModel { PropertyModelId = itemP.PN_ID, PropertyModelName = itemP.Name });
                            }
                            groupList.Add(tc);
                        }
                        break;
                    case 1:
                        foreach (var item in groups.Where(t => t.chemicals == true))
                        {
                            tc = new PropertyGroupModel { PropertyGroupModelId = item.PN_ID, Properties = new List<PropertyModel>(), PropertyGroupModelName = item.Name };
                            properties = DataSet.Where(n => n.BT_ID == item.PN_ID).ToList();
                            foreach (var itemP in properties.Where(p => p.chemicals == true))
                            {
                                tc.Properties.Add(new PropertyModel { PropertyModelId = itemP.PN_ID, PropertyModelName = itemP.Name });
                            }
                            groupList.Add(tc);
                        }
                        break;

                    default:
                        break;
                }

            }

            return groupList;
        }
    }
}
