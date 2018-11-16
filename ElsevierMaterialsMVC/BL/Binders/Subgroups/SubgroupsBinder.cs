using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IniCore.Web.Mvc.Extensions;
using ElsevierMaterials.Models;
using ElsevierMaterialsMVC.Models.Search;
using ElsevierMaterialsMVC.Models.Subgroups;
using ElsevierMaterials.EF.MaterialsContextUow;
using ElsevierMaterials.Common.Interfaces;
using IniCore.Web.Mvc.Html;
using ElsevierMaterialsMVC.BL.Binders.Search;
using ElsevierMaterialsMVC.BL.Global;
using ElsevierMaterials.Models.Domain;

namespace ElsevierMaterialsMVC.BL.Binders.Subgroups
{
    public class SubgroupsBinder
    {


        public SearchSubgroupsFilters BreadcrumbNavigationGetSet(SearchSubgroupsFilters filters)
        {
            BreadcrumbNavigation nav = (BreadcrumbNavigation)System.Web.HttpContext.Current.Session["Navigation"];
             if (nav == null)
            {
                nav = new BreadcrumbNavigation();
            }
         
            if (nav.Contains("Exporter"))
            {
                nav.Pop();
            }
            if (nav.Contains("Comparison"))
            {
                nav.Pop();
            }
            if (nav.Contains("MaterialDetails1"))
            {
                nav.Pop();
            }
        
            ElsevierMaterialsMVC.BL.Global.Subgroups hp = nav.GetOrderedItems().Where(n => n.NavigableID == "Subgroups").FirstOrDefault() as ElsevierMaterialsMVC.BL.Global.Subgroups;
            if (hp == null)
            {
                hp = new ElsevierMaterialsMVC.BL.Global.Subgroups();
                hp.PageData = filters;
            }
            else
            {
                if (filters.MaterialId == 0)
                {
                    filters = hp.PageData as SearchSubgroupsFilters;
                }
                else
                {
                    hp.PageData = filters;
                }
            }

            nav.LastNavigable = "Subgroups";
            hp.IsVisible = true;
            nav.Push(hp);

            System.Web.HttpContext.Current.Session["Navigation"] = nav;
            return filters;
        }

        public IList<Material> GetEquivalentMaterials(int id, GridDescriptor request, IMaterialsContextUow context)
        {
            IList<Material> tmp = GetAllEquivalentMaterials(id, context);

            return tmp.Page(request.Pager).ToList();
        }

        public IList<Material> GetAllEquivalentMaterials(int id, IMaterialsContextUow context)
        {
            FullTextSearch cl = context.FullTextSearch.GetMaterialById(id);
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            IList<Material> tmp = context.Materials.GetMaterialByEquivalence(id, cl, sessionId).ToList();
            return tmp;
        }

        public ICollection<Material> GetAllEquivalentMaterialsEn(int id, IMaterialsContextUow context)
        {
            FullTextSearch cl = context.FullTextSearch.GetMaterialById(id);
            string sessionId = System.Web.HttpContext.Current.Session["TotalMateriaSession"].ToString();
            ICollection<Material> tmp = context.Materials.GetMaterialByEquivalence(id, cl, sessionId);

            return tmp;
        }

        public SubgroupsModel GetSubgroupsModel(GridDescriptor request, SearchSubgroupsFilters filters, IMaterialsContextUow context)
        {

            SubgroupsModel model = new SubgroupsModel();
            model.Descriptor = request;
            ICollection<Material> materials = GetAllEquivalentMaterialsEn(filters.MaterialId, context);
            model.MaterialInfo = materials.FirstOrDefault();
            var standardList = materials.Select(m => new PropertyGroupModel { PropertyGroupModelName = m.Standard }).Distinct(new PropertyGroupComparer()).OrderBy(m => m.PropertyGroupModelName).ToList();

            var sourcesInitial = materials.Select(m => string.Concat(m.SourceId, "#", m.DatabookId)).Distinct();




            int databookId = 0;
            if (filters.SourceId > 0)
            {
                bool isInt = int.TryParse(filters.Source.Split('#')[1], out databookId);
                databookId = isInt ? databookId : 0;

                materials = materials.Where(m => m.SourceId == filters.SourceId && m.DatabookId == databookId).ToList();
            }

            if (filters.StandardId != null && filters.StandardId != "" && filters.StandardId != "0")
            {
                materials = materials.Where(m => m.Standard == filters.StandardId).ToList();
            }

            if (filters.Specification != "" && filters.Specification != null)
            {
                materials = materials.Where(m => m.Specification.ToLower().Contains(filters.Specification.ToLower())).ToList();
            }

            IEnumerable<Material> materialsEnum = materials.AsEnumerable();

            SearchFilterColumnsAll columnFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListResults"];
            if (columnFilters != null)
            {
                foreach (SearchFilterColumns f in columnFilters.AllFilters.Where(c => c.Filter != null && c.Filter.Trim() != ""))
                {
                    string oneFilter = f.Filter.Trim().ToUpper();
                    if (f.Name == "Reference")
                    {
                        materialsEnum = materialsEnum.Where(m => m.SourceText != null && m.SourceText.ToUpper().Contains(oneFilter));
                    }
                    else if (f.Name == "Supplier")
                    {
                        materialsEnum = materialsEnum.Where(m => m.Manufacturer != null && m.Manufacturer.ToUpper().Contains(oneFilter));
                    }
                    else if (f.Name == "Std. Org. / Country")
                    {
                        materialsEnum = materialsEnum.Where(m => m.Standard != null && m.Standard.ToUpper().Contains(oneFilter));
                    }
                    else if (f.Name == "Specification")
                    {
                        materialsEnum = materialsEnum.Where(m => m.Specification != null && m.Specification.ToUpper().Contains(oneFilter));
                    }
                    else if (f.Name == "Filler")
                    {
                        materialsEnum = materialsEnum.Where(m => m.Filler != null && m.Filler.ToUpper().Contains(oneFilter));
                    }
                 
                }
            }

          model.Materials = materialsEnum.ToList();
            model.Descriptor.Pager.TotalRecordCount = model.Materials.Count;
            model.Descriptor = request;
            if (model.Materials.Count > 0)
            {
                model.Name = model.Materials[0].Name;
            }
            else
            {
                model.Name = "";
            }


            SearchSubgroupCondition filterModel = new SearchSubgroupCondition();
            filterModel.SourceId = filters.SourceId;
            filterModel.StandardId = filters.StandardId;
            filterModel.Specification = filters.Specification;
            filterModel.FullText = filters.filter;
            filterModel.ShowPropertiesFilters = false;
            //IEnumerable<string> subSources = sourcesInitial.Select(m => string.Concat(m.SourceId, "#", m.DatabookId)).Distinct();
            filterModel.Sources = context.Sources.AllAsNoTracking.Where(s =>
                s.Id == 0 /* for All */
                || sourcesInitial.Contains(string.Concat(s.Id, "#", s.Databook_id))
                ).ToList();
            filterModel.PropertyGroups = standardList;
            filterModel.MaterialId = filters.MaterialId;
            filterModel.ShowPropertiesFilters = false;
            filterModel.FromBrowse = filters.FromBrowse;
            filterModel.FromAdvanced = filters.FromAdvanced;
            filterModel.SelectedSource = filters.Source;

            model.Filters = filterModel;
            return model;
        }

    }

    public class SourceComparer : IEqualityComparer<Source>
    {
        public bool Equals(Source x, Source y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Name == y.Name;
        }

        public int GetHashCode(Source obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;
            return obj.Id.GetHashCode();
        }
    }

    public class PropertyGroupComparer : IEqualityComparer<PropertyGroupModel>
    {
        public bool Equals(PropertyGroupModel x, PropertyGroupModel y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.PropertyGroupModelName == y.PropertyGroupModelName;
        }

        public int GetHashCode(PropertyGroupModel obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null)) return 0;
            return obj.PropertyGroupModelId.GetHashCode();
        }
    }
}