using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class AdvancedSearchResults : BaseNavigablePage {
        public AdvancedSearchResults() {
            this.NavigableID = "AdvancedSearchResults";
            this.NavigableName = "Search results";
            this.PageId = "AdvancedSearchResults";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "AdvSearch";
            this.Action = "AdvSearchResults";
        }
    }
}