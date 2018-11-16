using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.BL.Global
{
    public class Properties : BaseNavigablePage
    {
        public Properties()
        {
            this.NavigableID = "Properties";
            this.NavigableName = "Properties";
            this.PageId = "Properties";
            this.IsActive = true;
            this.ReplaceLast = false;
            this.Controller = "Properties";
            this.Action = "GetPropertiesDetails";
        }
    }
}

