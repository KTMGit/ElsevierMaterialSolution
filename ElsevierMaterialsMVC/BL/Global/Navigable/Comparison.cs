using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class ComparisonNav : BaseNavigablePage {
        public ComparisonNav()
        {
            this.NavigableID = "Comparison";
            this.NavigableName = "Compare Materials";
            this.PageId = "Comparison";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Comparison";
            this.Action = "Materials";
        }
    }
}