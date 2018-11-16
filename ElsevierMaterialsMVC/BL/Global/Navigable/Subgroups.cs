using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class Subgroups : BaseNavigablePage {
        public Subgroups()
        {
            this.NavigableID = "Subgroups";
            this.NavigableName = "Subgroups";
            this.PageId = "Subgroups";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Subgroups";
            this.Action = "Subgroup";
        }
    }
}