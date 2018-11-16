using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class FullSearch : BaseNavigablePage {
        public FullSearch() {
            this.NavigableID = "FullSearch";
            this.NavigableName = "Search results";
            this.PageId = "FullSearch";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Search";
            this.Action = "Search";
        }
    }
}