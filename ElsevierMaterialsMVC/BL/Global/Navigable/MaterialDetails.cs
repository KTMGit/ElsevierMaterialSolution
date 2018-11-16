using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class MaterialDetails1 : BaseNavigablePage {
        public MaterialDetails1() {
            this.NavigableID = "MaterialDetails1";
            this.NavigableName = "Properties";
            this.PageId = "MaterialDetails";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "MaterialDetails";            
            this.Action = "GetMaterialDetails";
        }
    }
}