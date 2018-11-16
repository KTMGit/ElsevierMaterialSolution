
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class BrowseFacets : BaseNavigablePage {
        public BrowseFacets()
        {
            this.NavigableID = "BrowseFacets";
            this.NavigableName = "Browse results";
            this.PageId = "BrowseFacets";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Search";
            this.Action = "BrowseSearch";
        }
    }
}