using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IniCore.Web.Mvc.Extensions;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterials.Common.Interfaces;
using IniCore.Web.Mvc.Html;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using ElsevierMaterials.Models.Domain;
using System.Web.Caching;
using System.Reflection;

namespace ElsevierMaterialsMVC.BL.Binders.Search
{
    public class SearchResultsBinder
    {

        public BaseSearchModel GetResultsFullTextSearch(SearchFilters filters, GridDescriptor request, IMaterialsContextUow context)
        {
            BaseSearchModel model = new BaseSearchModel();
            IQueryable<SampleMaterialModel> resultMat = null;

            int cnt;
            var commandText = "SELECT ID FROM [fn_FullTextSearch] ({0},{1});";
            string cleanSearchString = RemoveCharacters(filters.filter);
            string searchStringWS = RemoveCharactersWS(filters.filter);


            var parameters = new object[] { searchStringWS, cleanSearchString };
            IList<int> results = context.FullTextSearch.SqlQuery<int>(commandText, parameters).ToList();


            resultMat = context.FullTextSearch.GetMaterialsByFullTextSearch(false, filters.filter).Select(m => new SampleMaterialModel() { Id = m.Id, Name = m.material_designation, TypeName = m.material_type, TypeId = m.type_ID, GroupName = m.material_group, GroupId = m.group_ID, ClassName = m.material_class, ClassId = m.class_ID, SubClassName = m.material_subClass, SubClassId = m.subClass_ID, UNS = m.UNS, CAS_RN = m.CAS_RN }).AsQueryable();

            HttpContext.Current.Session["ClassificationRecordsCount"] = results;
            cnt = results.Count;
            model.ListOfMaterials = PagerSearch<SampleMaterialModel>(request.Pager, resultMat, cnt).ToList();


            model.Descriptor = request;
            model.Filter = new SearchResultsCondition();
            model.Filter.FullText = filters.filter;

            return model;





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


        public BaseSearchModel GetResults(SearchFilters filters, GridDescriptor request, IMaterialsContextUow context, string classificationIds, AdvSearchFiltersAll advFilters)
        {
            if (string.IsNullOrWhiteSpace(classificationIds) && HttpContext.Current.Session["ClassificationIds"] != "")
            {
                classificationIds = (string)HttpContext.Current.Session["ClassificationIds"];
            }

            BaseSearchModel model = new BaseSearchModel();
            if (filters.filter == null)
            { filters.filter = ""; }

            IEnumerable<FullTextSearch> results = null;
            IEnumerable<MaterialTaxonomy> taxonomyResults = new List<MaterialTaxonomy>();
            IEnumerable<MaterialTaxonomy> taxonomyResultsL1 = new List<MaterialTaxonomy>();
            IEnumerable<MaterialTaxonomy> taxonomyResultsL2 = new List<MaterialTaxonomy>();
            IEnumerable<MaterialTaxonomy> taxonomyResultsL3 = new List<MaterialTaxonomy>();
            IEnumerable<MaterialTaxonomy> taxonomyResultsL4 = new List<MaterialTaxonomy>();
            IEnumerable<int> taxonomyResultIds = new List<int>();
            IList<int> typeIds = new List<int>();
            IList<int> classIds = new List<int>();
            IList<int> subclassIds = new List<int>();
            IList<int> groupIds = new List<int>();


            IList<int> resultIds = null;
            if (filters.filter != "")
            {
                results = context.FullTextSearch.GetMaterialsByFullTextSearch(false, filters.filter);
                resultIds = context.FullTextSearch.GetMaterialsIdsByFullTextSearch(filters.filter).ToList();
            }
            else
            {

                if ((filters.FromBrowse && filters.ClasificationId != 0))
                {
                    taxonomyResults = context.MaterialTaxonomyAll.AllAsNoTracking;
                    results = context.FullTextSearch.AllAsNoTracking;
                    //resultIds = context.FullTextSearch.AllAsNoTracking.Select(c => c.Id).ToList();
                    resultIds = null;
                }
                else
                {
                    results = new HashSet<FullTextSearch>();
                    resultIds = null;
                }
            }


            IList<string> allIds = new List<string>();
            IList<int> classificationSelection = new List<int>();

            if (classificationIds != null && classificationIds.Length > 0)
            {
                allIds = classificationIds.Split(',').ToList();

                typeIds = new List<int>();
                classIds = new List<int>();
                subclassIds = new List<int>();
                groupIds = new List<int>();
                IList<int> propIds = new List<int>();


                foreach (var item in allIds)
                {
                    if (item.Contains("TYPE"))
                    {
                        typeIds.Add(Int32.Parse(item.Replace("TYPE_", "")));
                    }
                    if (item.Contains("SUBCLASS"))
                    {
                        classIds.Add(Int32.Parse(item.Replace("SUBCLASS_", "")));
                    }
                    if (item.Contains("CLASS") && !item.Contains("SUBCLASS"))
                    {
                        groupIds.Add(Int32.Parse(item.Replace("CLASS_", "")));
                    }
                    if (item.Contains("GROUP"))
                    {
                        subclassIds.Add(Int32.Parse(item.Replace("GROUP_", "")));
                    }
                    if (item.Contains("PROPERTY"))
                    {
                        propIds.Add(Int32.Parse(item.Replace("PROPERTY_", "")));
                    }
                }

                /*
                 * Idem u bazu u MaterialTaxonomy i tamo za materijale koje imam u resultsu radim where po kriterijumima za classificationIds 
                 * i to sto dobijem radim INTERSECT sa results od gore, a ovaj results ispod komentarisem
                 */
                taxonomyResults = context.MaterialTaxonomyAll.AllAsNoTracking.Where(t => resultIds.Contains(t.ID));
                taxonomyResultsL1 = new List<MaterialTaxonomy>();
                taxonomyResultsL2 = new List<MaterialTaxonomy>();
                taxonomyResultsL3 = new List<MaterialTaxonomy>();
                taxonomyResultsL4 = new List<MaterialTaxonomy>();

                taxonomyResultIds = new List<int>();
                if (typeIds.Count > 0)
                {
                    taxonomyResultsL1 = taxonomyResults.Where(l1 => l1.Level1 != null && typeIds.Contains((int)l1.Level1));
                }

                if (groupIds.Count > 0)
                {
                    taxonomyResultsL2 = taxonomyResults.Where(l2 => l2.Level2 != null && groupIds.Contains((int)l2.Level2));
                }

                if (classIds.Count > 0)
                {
                    taxonomyResultsL3 = taxonomyResults.Where(l3 => l3.Level3 != null && classIds.Contains((int)l3.Level3));
                }

                if (subclassIds.Count > 0)
                {
                    taxonomyResultsL4 = taxonomyResults.Where(l4 => l4.Level4 != null && subclassIds.Contains((int)l4.Level4));
                }

                taxonomyResultIds = taxonomyResultsL1.Select(i1 => i1.ID).ToList()
                                        .Union(taxonomyResultsL2.Select(i2 => i2.ID).ToList())
                                        .Union(taxonomyResultsL3.Select(i3 => i3.ID).ToList())
                                        .Union(taxonomyResultsL4.Select(i4 => i4.ID).ToList());

                IList<int> intersectIds = resultIds.Intersect(taxonomyResultIds).Distinct().ToList();
                results = results.Where(r => intersectIds.Contains(r.Id));


                //results = results.Where(m => (typeIds.Count > 0 && m.type_ID != null && typeIds.Contains((int)m.type_ID))
                //    || (classIds.Count > 0 && m.class_ID != null && classIds.Contains((int)m.class_ID))
                //    || (groupIds.Count > 0 && m.group_ID != null && groupIds.Contains((int)m.group_ID))
                //    || (subclassIds.Count > 0 && m.subClass_ID != null && subclassIds.Contains((int)m.subClass_ID))
                //    );



                if (propIds.Count > 0)
                {
                    results = results.Where(r => r.prop_IDs != null && CheckIDs(r.prop_IDs, propIds));
                }

                classificationSelection = typeIds.Concat(classIds).Concat(groupIds).Concat(subclassIds).Concat(propIds).ToList();
                resultIds = null;
            }
            if (filters.FromBrowse)
            {
                if (filters.ClasificationId != 0)
                {
                    switch (filters.ClasificationTypeId)
                    {
                        case 1:
                            results = results.Where(m => m.type_ID == filters.ClasificationId);
                            taxonomyResults = taxonomyResults.Where(m => m.Level1 == filters.ClasificationId);
                            break;
                        case 2:
                            results = results.Where(m => m.group_ID == filters.ClasificationId);
                            taxonomyResults = taxonomyResults.Where(m => m.Level2 == filters.ClasificationId);
                            break;
                        case 3:
                            results = results.Where(m => m.class_ID == filters.ClasificationId);
                            taxonomyResults = taxonomyResults.Where(m => m.Level3 == filters.ClasificationId);
                            break;
                        case 4:
                            results = results.Where(m => m.subClass_ID == filters.ClasificationId);
                            taxonomyResults = taxonomyResults.Where(m => m.Level4 == filters.ClasificationId);
                            break;
                        default:
                            break;
                    }

                    resultIds = null;
                }
            }

            HttpContext.Current.Session["ClassificationSelection"] = classificationSelection;


            //  Inlude source filters
            if (filters.Source != null && filters.Source != "")
            {
                int sourceId;
                int? sourceDatabookId = null;

                if (filters.Source != "0")
                {
                    IList<string> allSourceIds = filters.Source.Split(',').ToList();
                    if (allSourceIds.Count > 1)
                    {
                        sourceId = allSourceIds[0] != "" ? int.Parse(allSourceIds[0]) : 0;
                        sourceDatabookId = allSourceIds[1] != "" ? int.Parse(allSourceIds[1]) : 0;
                    }
                    else sourceId = allSourceIds[0] != "" ? int.Parse(allSourceIds[0]) : 0;
                }
                else
                {
                    sourceId = 0;
                    sourceDatabookId = null;
                }

                if (sourceId != 0)
                {
                    results = results.Where(m => m.source_IDs != null && m.source_IDs.Contains(string.Concat(",", sourceId.ToString(), ",")));
                    resultIds = null;
                }

                if (sourceDatabookId != null && sourceDatabookId != 0)
                {
                    results = results.Where(m => m.databook_IDs != null && m.databook_IDs.Contains(string.Concat(",", sourceDatabookId.ToString(), ",")));
                    resultIds = null;
                }

            }


            //// Inlude column filters
            SearchFilterColumnsAll columnFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
            if (columnFilters != null)
            {
                foreach (SearchFilterColumns f in columnFilters.AllFilters.Where(c => c.Filter != null && c.Filter.Trim() != ""))
                {
                    string oneFilter = f.Filter.Trim().ToUpper();
                    if (f.Name == "Material Name")
                    {
                        results = results.Where(m => m.material_designation != null && m.material_designation.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "Type")
                    {
                        results = results.Where(m => m.material_type != null && m.material_type.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "Class")
                    {
                        results = results.Where(m => m.material_group != null && m.material_group.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "Subclass")
                    {
                        results = results.Where(m => m.material_class != null && m.material_class.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "Group")
                    {
                        results = results.Where(m => m.material_subClass != null && m.material_subClass.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "UNS No.")
                    {
                        results = results.Where(m => m.UNS != null && m.UNS.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                    else if (f.Name == "CAS RN")
                    {
                        results = results.Where(m => m.CAS_RN != null && m.CAS_RN.ToUpper().Contains(oneFilter));
                        resultIds = null;
                    }
                }
            }

            /*adv search*/

            if (advFilters != null && advFilters.AllFilters != null && advFilters.AllFilters.Count > 0)
            {
                string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
                IQueryable<EquivalentProperty> propIds = context.EquivalentProperties.AllAsNoTracking;
                IQueryable<EquivalentMaterial> matIds = context.EquivalentMaterials.AllAsNoTracking;

                IEnumerable<int> materialIds = context.AdvSearchResults.GetMaterialsByAdvancedSearch(false, advFilters, sessionId, propIds, matIds);

                IEnumerable<int> matVisibleIds = results.Select(n => n.Id);
                materialIds = materialIds.Where(n => matVisibleIds.Contains(n));
                IEnumerable<int> resTM = context.FullTextSearch.GetMaterialsByMaterialIds(materialIds).Select(n => n.Id);

                results = results.Where(n => resTM.Contains(n.Id));
                resultIds = null;
            }

            // Eval query and prepare final results
            if (filters.FromBrowse)
            {
                // From taxonomy table
                IList<int> tmpList = null;

                if (resultIds == null)
                {
                    tmpList = taxonomyResults.Select(m => m.ID).Distinct().ToList();
                }
                else
                {
                    tmpList = resultIds.Distinct().ToList();
                }

                HttpContext.Current.Session["ClassificationRecordsCount"] = tmpList;

                if (request.Sort.PropertyName == "Name")
                {
                    IList<SampleMaterialModel> retListTemp = taxonomyResults
                        .OrderBy(l => l.ID)
                        .ThenBy(l1 => l1.Level1Name)
                        .ThenByDescending(l4 => l4.Level4Name)
                        .ThenByDescending(l3 => l3.Level3Name)
                        .ThenByDescending(l2 => l2.Level2Name)
                        .Select(m => new SampleMaterialModel()
                    {
                        Id = m.ID,
                        Name = m.MaterialName,
                        TypeName = m.Level1Name,
                        GroupName = m.Level2Name,
                        ClassName = m.Level3Name,
                        SubClassName = m.Level4Name,
                        UNS = null,
                        CAS_RN = null
                    })
                        .ToList();

                    IList<SampleMaterialModel> retList = new List<SampleMaterialModel>();
                    foreach (int matId in tmpList)
                    {
                        SampleMaterialModel mResult = retListTemp.Where(r => r.Id == matId).FirstOrDefault();

                        if (mResult != null)
                        {
                            retList.Add(new SampleMaterialModel()
                            {
                                Id = mResult.Id,
                                Name = mResult.Name,
                                TypeName = mResult.TypeName,
                                GroupName = mResult.GroupName,
                                ClassName = mResult.ClassName,
                                SubClassName = mResult.SubClassName
                            });
                        }
                    }

                    model.ListOfMaterials = PagerSearch<SampleMaterialModel>(request.Pager, retList.AsQueryable(), tmpList.Count).ToList();
                }
                else
                {
                    model.ListOfMaterials = taxonomyResults.AsQueryable().Slice(request).Select(m => new SampleMaterialModel()
                    {
                        Id = m.ID,
                        Name = m.MaterialName,
                        TypeName = m.Level1Name,
                        GroupName = m.Level2Name,
                        ClassName = m.Level3Name,
                        SubClassName = m.Level4Name,
                        UNS = null,
                        CAS_RN = null
                    })
                        .ToList();
                }
            }
            else
            {
                IList<int> tmpList = null;
                if (resultIds == null)
                {
                    tmpList = results.Select(m => m.Id).ToList();
                }
                else
                {
                    tmpList = resultIds;
                }
                HttpContext.Current.Session["ClassificationRecordsCount"] = tmpList;

                if (request.Sort.PropertyName == "Name")
                {
                    // If search is not "by taxonomy' use FullTextSearch, else use materialTaxonomy table
                    if (typeIds.Count() == 0 && groupIds.Count() == 0 && classIds.Count == 0 && subclassIds.Count() == 0)
                    {
                        IQueryable<SampleMaterialModel> retList = results.Select(m => new SampleMaterialModel()
                        {
                            Id = m.Id,
                            Name = m.material_designation,
                            TypeName = m.material_type,
                            GroupName = m.material_group,
                            ClassName = m.material_class,
                            SubClassName = m.material_subClass,
                            UNS = m.UNS,
                            CAS_RN = m.CAS_RN
                        })
                            .AsQueryable();

                        model.ListOfMaterials = PagerSearch<SampleMaterialModel>(request.Pager, retList, tmpList.Count).ToList();
                    }
                    else
                    {
                        IEnumerable<MaterialTaxonomy> tr = context.MaterialTaxonomyAll.AllAsNoTracking;
                        IEnumerable<MaterialTaxonomy> tr1 = new List<MaterialTaxonomy>();
                        IEnumerable<MaterialTaxonomy> tr2 = new List<MaterialTaxonomy>();
                        IEnumerable<MaterialTaxonomy> tr3 = new List<MaterialTaxonomy>();
                        IEnumerable<MaterialTaxonomy> tr4 = new List<MaterialTaxonomy>();
                        if (typeIds.Count > 0)
                        {
                            tr1 = tr.Where(l1 => l1.Level1 != null && typeIds.Contains((int)l1.Level1));
                        }
                        if (groupIds.Count > 0)
                        {
                            tr2 = tr.Where(l2 => l2.Level2 != null && groupIds.Contains((int)l2.Level2));
                        }
                        if (classIds.Count > 0)
                        {
                            tr3 = tr.Where(l3 => l3.Level3 != null && classIds.Contains((int)l3.Level3));
                        }
                        if (subclassIds.Count > 0)
                        {
                            tr4 = tr.Where(l4 => l4.Level4 != null && subclassIds.Contains((int)l4.Level4));
                        }

                        IList<Tuple<int, string, string, string, string, string>> trs = tr1.Select(i1 => new Tuple<int, string, string, string, string, string>(i1.ID, i1.MaterialName, i1.Level1Name, i1.Level2Name, i1.Level3Name, i1.Level4Name))
                                          .Union(tr2.Select(i2 => new Tuple<int, string, string, string, string, string>(i2.ID, i2.MaterialName, i2.Level1Name, i2.Level2Name, i2.Level3Name, i2.Level4Name)))
                                          .Union(tr3.Select(i3 => new Tuple<int, string, string, string, string, string>(i3.ID, i3.MaterialName, i3.Level1Name, i3.Level2Name, i3.Level3Name, i3.Level4Name)))
                                          .Union(tr4.Select(i4 => new Tuple<int, string, string, string, string, string>(i4.ID, i4.MaterialName, i4.Level1Name, i4.Level2Name, i4.Level3Name, i4.Level4Name)))
                                          .ToList();

                        IList<SampleMaterialModel> retListTemp = trs
                         .OrderBy(l => l.Item1)
                         .ThenBy(l1 => l1.Item3)
                         .ThenByDescending(l4 => l4.Item6)
                         .ThenByDescending(l3 => l3.Item5)
                         .ThenByDescending(l2 => l2.Item4)
                         .Select(m => new SampleMaterialModel()
                         {
                             Id = m.Item1,
                             Name = m.Item2,
                             TypeName = m.Item3,
                             GroupName = m.Item4,
                             ClassName = m.Item5,
                             SubClassName = m.Item6,
                             UNS = null,
                             CAS_RN = null
                         })
                         .ToList();

                        IList<SampleMaterialModel> retList = new List<SampleMaterialModel>();
                        foreach (int matId in tmpList)
                        {
                            SampleMaterialModel mResult = retListTemp.Where(r => r.Id == matId).FirstOrDefault();

                            if (mResult != null)
                            {
                                retList.Add(new SampleMaterialModel()
                                {
                                    Id = mResult.Id,
                                    Name = mResult.Name,
                                    TypeName = mResult.TypeName,
                                    GroupName = mResult.GroupName,
                                    ClassName = mResult.ClassName,
                                    SubClassName = mResult.SubClassName
                                });
                            }
                        }

                        model.ListOfMaterials = PagerSearch<SampleMaterialModel>(request.Pager, retList.AsQueryable(), tmpList.Count).ToList();
                    }

                }
                else
                {
                    model.ListOfMaterials = results.AsQueryable().Slice(request).Select(m => new SampleMaterialModel()
                    {
                        Id = m.Id,
                        Name = m.material_designation,
                        TypeName = m.material_type,
                        GroupName = m.material_group,
                        ClassName = m.material_class,
                        SubClassName = m.material_subClass,
                        UNS = m.UNS,
                        CAS_RN = m.CAS_RN
                    })
                        .ToList();
                }
            }


            model.Descriptor = request;

            model.Filter = new SearchResultsCondition();
            model.Filter.FullText = filters.filter;
            if (classificationSelection.Count > 0)
            {
                HttpContext.Current.Session["NodeNames"] = context.Trees.GetTreeNodesNames(classificationSelection);
            }
            else
            {
                HttpContext.Current.Session["NodeNames"] = new Dictionary<int, string>();
            }
            return model;
        }

        private bool CheckIDs(string cIds, IList<int> iDs)
        {
            bool hasId = false;

            foreach (int propId in iDs)
            {
                hasId = hasId || cIds.Contains(string.Concat(",", propId, ","));
            }

            return hasId;
        }

        public IList<SampleMaterialModel> GetResultsAdvSearch(AdvSearchFiltersAll filters, GridDescriptor request, IMaterialsContextUow context)
        {
            IList<SampleMaterialModel> retList = new List<SampleMaterialModel>();
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            IQueryable<EquivalentProperty> propIds = context.EquivalentProperties.AllAsNoTracking;
            IQueryable<EquivalentMaterial> matIds = context.EquivalentMaterials.AllAsNoTracking;

            IEnumerable<int> materialIds = context.AdvSearchResults.GetMaterialsByAdvancedSearch(false, filters, sessionId, propIds, matIds);

            // Check for Taxonomy and find filter levels
            bool isTaxonomy = false;
            int level = 0;
            int levelId = 0;
            int taxonomyCounter = 0;

            IList<Tuple<int, int, BinaryOperators>> levels = new List<Tuple<int, int, BinaryOperators>>();
            foreach (AdvSearchFilters f in filters.AllFilters)
            {
                if (f.propertyName.ToUpper().StartsWith("TYPE:"))
                {
                    level = 1;
                    taxonomyCounter++;
                    levelId = f.propertyId;
                    levels.Add(new Tuple<int, int, BinaryOperators>(level, levelId, f.binaryOperators));
                }
                if (f.propertyName.ToUpper().StartsWith("CLASS:"))
                {
                    level = 2;
                    taxonomyCounter++;
                    levelId = f.propertyId;
                    levels.Add(new Tuple<int, int, BinaryOperators>(level, levelId, f.binaryOperators));
                }
                if (f.propertyName.ToUpper().StartsWith("SUBCLASS:"))
                {
                    level = 3;
                    taxonomyCounter++;
                    levelId = f.propertyId;
                    levels.Add(new Tuple<int, int, BinaryOperators>(level, levelId, f.binaryOperators));
                }
                if (f.propertyName.ToUpper().StartsWith("GROUP:"))
                {
                    level = 4;
                    taxonomyCounter++;
                    levelId = f.propertyId;
                    levels.Add(new Tuple<int, int, BinaryOperators>(level, levelId, f.binaryOperators));
                }
            }
            isTaxonomy = (level > 0 && taxonomyCounter == 1);



            IEnumerable<int> matVisibleIds = materialIds.Page(request.Pager);

            if (isTaxonomy == false)
            {
                var results = context.FullTextSearch.GetMaterialsByMaterialIds(matVisibleIds);

                retList = results.Select(m => new SampleMaterialModel()
                {
                    Id = m.Id,
                    Name = m.material_designation,
                    TypeName = m.material_type,     // level1
                    GroupName = m.material_group,   // level2
                    ClassName = m.material_class,   // level3
                    SubClassName = m.material_subClass  // level4
                    //,
                    //UNS = m.UNS,
                    //CAS_RN = m.CAS_RN 
                }).ToList();

            }
            else
            {
                IEnumerable<MaterialTaxonomy> taxonomy = context.MaterialTaxonomyAll.AllAsNoTracking;

                //Only if levels contains one filter definition!
                foreach (Tuple<int, int, BinaryOperators> l in levels)
                {
                    if (l.Item1 == 1)
                    {
                        taxonomy = taxonomy.Where(t => t.Level1 == l.Item2);
                    }
                    else if (l.Item1 == 2)
                    {
                        taxonomy = taxonomy.Where(t => t.Level2 == l.Item2);
                    } if (l.Item1 == 3)
                    {
                        taxonomy = taxonomy.Where(t => t.Level3 == l.Item2);
                    }
                    else if (l.Item1 == 4)
                    {
                        taxonomy = taxonomy.Where(t => t.Level4 == l.Item2);
                    }
                }

                // Eval taxonomy page data and store it into memory
                IList<MaterialTaxonomy> lTaxonomy = (from t in taxonomy join m in matVisibleIds on t.ID equals m select t)
                        .OrderBy(l => l.ID)
                        .ThenBy(l1 => l1.Level1Name)
                        .ThenByDescending(l4 => l4.Level4Name)
                        .ThenByDescending(l3 => l3.Level3Name)
                        .ThenByDescending(l2 => l2.Level2Name)
                        .ToList();


                // Fill final data for one page (take only first (because it is it is non-null for sure) element for each id in matVisibleIds)
                retList = new List<SampleMaterialModel>();
                foreach (int mat in matVisibleIds)
                {
                    MaterialTaxonomy mResult = lTaxonomy.Where(r => r.ID == mat).FirstOrDefault();

                    if (mResult != null)
                    {
                        retList.Add(new SampleMaterialModel()
                       {
                           Id = mResult.ID,
                           Name = mResult.MaterialName,
                           TypeName = mResult.Level1Name,
                           GroupName = mResult.Level2Name,
                           ClassName = mResult.Level3Name,
                           SubClassName = mResult.Level4Name
                       });
                    }
                }
            }

            return retList;
        }

        public IDictionary<int, int> TreeCount(IList<int> results, IMaterialsContextUow context)
        {
            IDictionary<int, int> records = new Dictionary<int, int>();
            var treeCounts = context.TreeCounts.AllAsNoTracking.Where(t => results.Contains(t.Id));
            var types = treeCounts.Where(t => t.TypeId != null).GroupBy(t => t.TypeId).Select(t => new { Id = t.Key, Num = t.Sum(s => s.TypeCount) });
            var classes = treeCounts.Where(t => t.ClassId != null).GroupBy(t => t.ClassId).Select(t => new { Id = t.Key, Num = t.Sum(s => s.ClassCount) });
            var subClasses = treeCounts.Where(t => t.SubClassId != null).GroupBy(t => t.SubClassId).Select(t => new { Id = t.Key, Num = t.Sum(s => s.SubClassCount) });
            var groups = treeCounts.Where(t => t.GroupId != null).GroupBy(t => t.GroupId).Select(t => new { Id = t.Key, Num = t.Sum(s => s.GroupCount) });
            foreach (var item in types)
            {
                records.Add((int)item.Id, item.Num);
            }
            foreach (var item in classes)
            {
                records.Add((int)item.Id, item.Num);
            }
            foreach (var item in subClasses)
            {
                records.Add((int)item.Id, item.Num);
            }
            foreach (var item in groups)
            {
                records.Add((int)item.Id, item.Num);
            }


            return records;
        }



        public IDictionary<int, int> TaxonomyTreeCount(IList<int> results, IMaterialsContextUow context)
        {
            IDictionary<int, int> records = new Dictionary<int, int>();
            var treeCounts = context.TaxonomyTreeCounts.AllAsNoTracking.Where(t => results.Contains(t.Id));

            var types = treeCounts
                .Where(t => t.TypeId != null)
                .GroupBy(t => t.TypeId)
                .Select(t => new
                {
                    Id = t.Key,
                    Num = t.Sum(s => s.TypeCount)
                    //Num = 0
                })
                .ToList();

            var groups = treeCounts
               .Where(t => t.GroupId != null)
               .GroupBy(t => t.GroupId)
               .Select(t => new
               {
                   Id = t.Key,
                   Num = t.Sum(s => s.GroupCount)
                   //Num = 0
               })
               .ToList();

            var classes = treeCounts
                .Where(t => t.ClassId != null)
                .GroupBy(t => t.ClassId)
                .Select(t => new
                {
                    Id = t.Key,
                    Num = t.Sum(s => s.ClassCount)
                    //Num = 0
                })
                .ToList();

            var subClasses = treeCounts
               .Where(t => t.SubClassId != null)
               .GroupBy(t => t.SubClassId)
               .Select(t => new
               {
                   Id = t.Key,
                   Num = t.Sum(s => s.SubClassCount)
               })
               .ToList();

            IList<TaxonomyTreeCount> tc = treeCounts.ToList();

            foreach (var item in types.Where(i => i != null))
            {
                //records.Add((int)item.Id, item.Num);

                IList<TaxonomyTreeCount> typeCounters = tc.Where(c => c.TypeId == item.Id).ToList();

                //HashSet<int> finished = new HashSet<int>();
                //int counter = 0;
                //foreach (TaxonomyTreeCount typeCounter in typeCounters)
                //{
                //    // Get only unique material data using HashSet functionality
                //    if (finished.Add(typeCounter.Id))
                //    {
                //        counter++;
                //    }
                //}

                int counter = typeCounters.Select(m => m.Id).Distinct().Count();
                records.Add((int)item.Id, counter);
            }

            foreach (var item in groups.Where(i => i != null))
            {
                //records.Add((int)item.Id, item.Num);

                IList<TaxonomyTreeCount> groupCounters = tc.Where(c => c.GroupId == item.Id).ToList();

                //HashSet<int> finished = new HashSet<int>();
                //int counter = 0;
                //foreach (TaxonomyTreeCount groupCounter in groupCounters)
                //{
                //    // Get only unique material data using HashSet functionality
                //    if (finished.Add(groupCounter.Id))
                //    {
                //        counter += 1;
                //    }
                //}

                int counter = groupCounters.Select(m => m.Id).Distinct().Count();
                records.Add((int)item.Id, counter);
            }

            foreach (var item in classes.Where(i => i != null))
            {
                //records.Add((int)item.Id, item.Num);

                IList<TaxonomyTreeCount> classCounters = tc.Where(c => c.ClassId == item.Id).ToList();

                //HashSet<int> finished = new HashSet<int>();
                //int counter = 0;
                //foreach (TaxonomyTreeCount classCounter in classCounters)
                //{
                //    // Get only unique material data using HashSet functionality
                //    if (finished.Add(classCounter.Id))
                //    {
                //        counter++;
                //    }
                //}

                int counter = classCounters.Select(m => m.Id).Distinct().Count();
                records.Add((int)item.Id, counter);
            }

            foreach (var item in subClasses.Where(i => i != null))
            {
                //records.Add((int)item.Id, item.Num);

                IList<TaxonomyTreeCount> subClassCounters = tc.Where(c => c.SubClassId == item.Id).ToList();

                //HashSet<int> finished = new HashSet<int>();
                //int counter = 0;
                //foreach (TaxonomyTreeCount subClassCounter in subClassCounters)
                //{
                //    // Get only unique material data using HashSet functionality
                //    if (finished.Add(subClassCounter.Id))
                //    {
                //        counter++;
                //    }
                //}

                int counter = subClassCounters.Select(m => m.Id).Distinct().Count();
                records.Add((int)item.Id, counter);
            }

            return records;
        }


        public IList<SampleMaterialModel> GetResultsStructureAdvSearch(string recordIds, GridDescriptor request, IMaterialsContextUow context)
        {
            IList<SampleMaterialModel> retList = new List<SampleMaterialModel>();
            IEnumerable<int> materialIds = context.AdvSearchResults.MaterialStructureSearch(recordIds).Distinct();

            var results = context.FullTextSearch.GetMaterialsByMaterialIds(materialIds);
            results = results.Page(request.Pager);
            retList = results.Select(m => new SampleMaterialModel()
            {
                Id = m.Id,
                Name = m.material_designation,
                TypeName = m.material_type,
                GroupName = m.material_group,
                ClassName = m.material_class,
                SubClassName = m.material_subClass,
                StructureImage = m.structure_image
                //,
                //UNS = m.UNS,
                //CAS_RN = m.CAS_RN 
            }).ToList();

            return retList;
        }

        private IQueryable<T> PagerSearch<T>(PagerDescriptor pagerDesc, IQueryable<T> results, int recordCount)
        {

            int page1 = (int)Math.Ceiling((recordCount + 0.0) / pagerDesc.SelectedPageSize);
            if (pagerDesc.SelectedPage > page1)
            {
                pagerDesc.SelectedPage = page1;
            }
            pagerDesc.TotalRecordCount = recordCount;
            pagerDesc.TotalPageCount = page1;
            if (pagerDesc.SelectedPage > pagerDesc.TotalPageCount)
            {
                pagerDesc.SelectedPage = pagerDesc.TotalPageCount;
            }

            return results.Skip((pagerDesc.SelectedPage - 1) * pagerDesc.SelectedPageSize).Take(pagerDesc.SelectedPageSize);
        }

    }
    //TODO: Ove klase treba izmestiti u projekat sa modelim ili folder Models
    public class SearchFullTextFilters
    {
        public string filter { get; set; }
    }

    public class SearchFilters : SearchFullTextFilters
    {
        public int ClasificationId { get; set; }
        public int ClasificationTypeId { get; set; }
        public int PropertyClasificationId { get; set; }
        public int PropertyClasificationTypeId { get; set; }
        public string Source { get; set; }
        public bool FromBrowse { get; set; }
    }

    public class SearchSubgroupsFilters : SearchFullTextFilters
    {
        public int SourceId { get; set; }
        public string StandardId { get; set; }
        public string Specification { get; set; }
        public int MaterialId { get; set; }
        public bool FromBrowse { get; set; }
        public bool FromAdvanced { get; set; }

        public string Source { get; set; }
    }

}

