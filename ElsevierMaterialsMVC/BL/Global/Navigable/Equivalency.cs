using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class Equivalency : BaseNavigablePage {
        public Equivalency()
        {
            this.NavigableID = "Equivalency";
            this.NavigableName = "Equivalency";
            this.PageId = "Equivalency";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Equivalency";
            this.Action = "GetEquivalencyDetails";
        }
    }
}