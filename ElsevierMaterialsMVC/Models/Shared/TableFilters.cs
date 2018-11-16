using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElsevierMaterialsMVC.Models.Shared;
namespace ElsevierMaterialsMVC.Models.Shared
{
    public class TableFilters
    {
        public FiltersGroup FiltersGroup { get; set; }
        public PageEnum Page { get; set; }
        public string TableName { get; set; }
        public bool HasOrderPosibility { get; set; }
        public bool HasColumnsHidePosibility { get; set; }
        public bool HasInputSearch { get; set; }
        public string ContainerId { get; set; }
        public IList<Column> Columns { get; set; }
        
        public TableFilters() {

            Columns = new List<Column>(); 
        }
    }

    public class Column
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public bool IsDisabled { get; set; }
       
        public bool IsChecked { get; set; }
        public string Class { get; set; }
    }
}