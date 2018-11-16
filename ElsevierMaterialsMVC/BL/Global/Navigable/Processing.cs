
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global
{
    public class Processing : BaseNavigablePage
    {
        public Processing()
        {
            this.NavigableID = "Processing";
            this.NavigableName = "Processing";
            this.PageId = "Processing";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Processing";
            this.Action = "GetProcessingDetails";
        }
    }
}

