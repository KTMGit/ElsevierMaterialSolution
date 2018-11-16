using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Shared
{
    public class Plugin
    {
        public HasSearchFiltersEnum HasSearchFilters { get; set; }
        public PageEnum ActivePage { get; set; }
    }
}