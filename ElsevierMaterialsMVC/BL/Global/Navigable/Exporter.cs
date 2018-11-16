using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global {
    public class ExporterNav : BaseNavigablePage {
        public ExporterNav()
        {
            this.NavigableID = "Exporter";
            this.NavigableName = "Export Data";
            this.PageId = "Exporter";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Exporter";
            this.Action = "Materials";
        }
    }
}