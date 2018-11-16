using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain;
using ElsevierMaterialsMVC.Models.MaterialDetails;
using ElsevierMaterialsMVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace ElsevierMaterialsMVC.BL.Binders
{
    public class TableFiltersBinder
    {
        public TableFilters GetSearchTableColumns()
        {
            TableFilters TableFiltersModel = new TableFilters();
            TableFiltersModel.Page = ElsevierMaterialsMVC.Models.Shared.PageEnum.SearchResults;
            TableFiltersModel.HasColumnsHidePosibility = true;
            TableFiltersModel.HasOrderPosibility = true;
            TableFiltersModel.HasInputSearch = true;
            TableFiltersModel.ContainerId = "resultsResizable";
            TableFiltersModel.TableName = "materialList";

            ElsevierMaterials.Models.Domain.SearchFilterColumnsAll allFilters = (ElsevierMaterials.Models.Domain.SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
            if (allFilters != null)
            {
                foreach (var colItem in allFilters.AllFilters)
                {
                    TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = colItem.Id, Class = "cbSelectColumn1", IsChecked = colItem.isVisible, IsDisabled = (colItem.Id == 0 ? true : false), Name = colItem.Name });
                }
            }
            else
            {

                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 0, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = true, Name = "Material Name" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 1, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Type" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 2, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Class" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 3, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Subclass" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 4, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Group" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 5, Class = "cbSelectColumn1", IsChecked = false, IsDisabled = false, Name = "UNS No." });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 6, Class = "cbSelectColumn1", IsChecked = false, IsDisabled = false, Name = "CAS RN" });
            }
            return TableFiltersModel;
        }


        public TableFilters GetMaterialDetailsMaterialInfoColumns()
        {

            ElsevierMaterialsMVC.Models.Shared.TableFilters TableFiltersModel = new ElsevierMaterialsMVC.Models.Shared.TableFilters();
            TableFiltersModel.Page = ElsevierMaterialsMVC.Models.Shared.PageEnum.MaterialDetails;
            TableFiltersModel.HasColumnsHidePosibility = true;
            TableFiltersModel.TableName = "materialDetailsInfoTable";

            ElsevierMaterials.Models.Domain.SearchFilterColumnsAll allFilters = (ElsevierMaterials.Models.Domain.SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"];
            if (allFilters != null)
            {
                foreach (var colItem in allFilters.AllFilters)
                {
                    TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = colItem.Id, Class = "cbSelectColumn1", IsChecked = colItem.isVisible, IsDisabled = (colItem.Id == 0 ? true : false), Name = colItem.Name });
                }
            }
            else
            {
                MaterialDetailsModel material = (MaterialDetailsModel)System.Web.HttpContext.Current.Session["materialInfoData"];

                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 0, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = true, Name = "Material Name" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 1, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Type" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 2, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Class" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 3, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Subclass" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 4, Class = "cbSelectColumn1", IsChecked = ((!string.IsNullOrEmpty(material.SubClassName) && material.SubClassName != "-" && material.SubClassName != "")), IsDisabled = false, Name = "Group" });

                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 5, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Reference" });

                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 6, Class = "cbSelectColumn1", IsChecked = (!string.IsNullOrEmpty(material.Material.Manufacturer) && material.Material.Manufacturer != "-") ? true : false, IsDisabled = false, Name = "Supplier" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 7, Class = "cbSelectColumn1", IsChecked = ((!string.IsNullOrEmpty(material.Material.Standard) && material.Material.Standard != "-") || (!string.IsNullOrEmpty(material.Material.Specification)) && material.Material.Specification != "-") ? true : false, IsDisabled = false, Name = "Std. Org.<span> / </span>Specification" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 8, Class = "cbSelectColumn1", IsChecked = (!string.IsNullOrEmpty(material.Material.Filler) && material.Material.Filler != "-") ? true : false, IsDisabled = false, Name = "Filler" });

                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 9, Class = "cbSelectColumn1", IsChecked = (!string.IsNullOrEmpty(material.Material.UNSNo) && material.Material.UNSNo != "-") ? true : false, IsDisabled = false, Name = "UNS No." });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 10, Class = "cbSelectColumn1", IsChecked = (!string.IsNullOrEmpty(material.Material.CASRN) && material.Material.CASRN != "-") ? true : false, IsDisabled = false, Name = "CAS RN" });
                        
            }

            return TableFiltersModel;

        }

        public TableFilters GetSubgroupListMaterialInfoColumns()
        {
            ElsevierMaterialsMVC.Models.Shared.TableFilters TableFiltersModel = new ElsevierMaterialsMVC.Models.Shared.TableFilters();
            TableFiltersModel.HasColumnsHidePosibility = true;
            TableFiltersModel.FiltersGroup = ElsevierMaterialsMVC.Models.Shared.FiltersGroup.MaterialInfo;
            TableFiltersModel.Page = ElsevierMaterialsMVC.Models.Shared.PageEnum.SubgroupList;
            TableFiltersModel.TableName = "materialInfoTable";

            ElsevierMaterials.Models.Domain.SearchFilterColumnsAll allFilters = (ElsevierMaterials.Models.Domain.SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListMaterialInfo"];
            if (allFilters != null)
            {
                foreach (var colItem in allFilters.AllFilters)
                {
                    TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = colItem.Id, Class = "cbSelectColumn1", IsChecked = colItem.isVisible, IsDisabled = (colItem.Id == 0 ? true : false), Name = colItem.Name });
                }
            }
            else
            {
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 0, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = true, Name = "Material Name" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 1, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Type" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 2, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Class" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 3, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Subclass" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 4, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Group" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 5, Class = "cbSelectColumn1", IsChecked = false, IsDisabled = false, Name = "UNS No." });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 6, Class = "cbSelectColumn1", IsChecked = false, IsDisabled = false, Name = "CAS RN" });
            }
            return TableFiltersModel;
        }


        public TableFilters GetSubgroupListResultsColumns()
        {

            ElsevierMaterialsMVC.Models.Shared.TableFilters TableFiltersModel = new ElsevierMaterialsMVC.Models.Shared.TableFilters();
            TableFiltersModel.Page = ElsevierMaterialsMVC.Models.Shared.PageEnum.SubgroupList;
            TableFiltersModel.FiltersGroup = ElsevierMaterialsMVC.Models.Shared.FiltersGroup.SubgroupList;
            TableFiltersModel.HasColumnsHidePosibility = true;
            TableFiltersModel.HasOrderPosibility = true;
            TableFiltersModel.HasInputSearch = true;
            TableFiltersModel.ContainerId = "subgroupListTableContainer";
            TableFiltersModel.TableName = "materialsSubgroupList";

            ElsevierMaterials.Models.Domain.SearchFilterColumnsAll allFilters = (ElsevierMaterials.Models.Domain.SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListResults"];
            if (allFilters != null)
            {
                foreach (var colItem in allFilters.AllFilters)
                {
                    TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = colItem.Id, Class = "cbSelectColumn1", IsChecked = colItem.isVisible, IsDisabled = (colItem.Id == 0 ? true : false), Name = colItem.Name });
                }
            }
            else
            {
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 0, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = true, Name = "Reference" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 1, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Supplier" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 2, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Std. Org. / Country" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 3, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Specification" });
                TableFiltersModel.Columns.Add(new ElsevierMaterialsMVC.Models.Shared.Column { Id = 4, Class = "cbSelectColumn1", IsChecked = true, IsDisabled = false, Name = "Filler" });
            }
            return TableFiltersModel;
        }

        public void resetAllTableFilters()
        {
            System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"] = null;
            System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"] = null;
            System.Web.HttpContext.Current.Session["SubgroupListResults"] = null;
            System.Web.HttpContext.Current.Session["SubgroupListMaterialInfo"] = null;
        }


        public string GetFiltersModel(PageEnum page, FiltersGroup filtersGroup)
        {
            SearchFilterColumnsAll allFilters = new SearchFilterColumnsAll();
            switch (page)
            {

                case PageEnum.SearchResults:
                    allFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SearchFilterColumnsAll"];
                    break;
                case PageEnum.SubgroupList:
                    if (filtersGroup == FiltersGroup.MaterialInfo)
                    {
                        allFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListMaterialInfo"];
                    }
                    else
                    {
                        allFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["SubgroupListResults"];
                    }

                    break;
                case PageEnum.MaterialDetails:
                    allFilters = (SearchFilterColumnsAll)System.Web.HttpContext.Current.Session["MaterialDetailsMaterialInfo"];
                    break;
                default:
                    break;
            }
            string allFiltersList = "";
            if (allFilters != null)
            {
                allFiltersList = Json.Encode(allFilters.AllFilters);
            }
            return allFiltersList;
        }
    }
}