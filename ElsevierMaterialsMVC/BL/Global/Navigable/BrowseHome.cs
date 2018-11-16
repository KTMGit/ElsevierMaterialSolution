using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global
{
    public class BrowseHome : BaseNavigablePage
    {
        public BrowseHome()
        {
            this.NavigableID = "BrowseHome";
            this.NavigableName = "Browse";
            this.PageId = "BrowseFacets";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Home";
            this.Action = "Browse";
         }
    }
}

