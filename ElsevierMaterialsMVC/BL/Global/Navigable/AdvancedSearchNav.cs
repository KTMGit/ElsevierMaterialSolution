using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class AdvancedSearchNav  : BaseNavigablePage {
        public AdvancedSearchNav() {
            this.NavigableID = "AdvancedSearch";
            this.NavigableName = "Advanced search";
            this.PageId = "AdvancedSearch";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "AdvSearch";
            this.Action = "AdvSearch";
        }
    }
}