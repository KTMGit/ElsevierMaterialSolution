using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class HomePage : BaseNavigablePage {
        public HomePage() {
            this.NavigableID = "HomePage";
            this.NavigableName = "Home";
            this.PageId = "HomePage";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Home";
            this.Action = "Index";
        }
    }
}