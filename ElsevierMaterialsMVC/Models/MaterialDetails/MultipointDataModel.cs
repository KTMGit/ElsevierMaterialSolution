using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class MultipointDataModel
    {
       public IList<Api.Models.PLUS.MultiPointData.DiagramType> DiagramTypes { get; set; }
       public MultipointDataDetailsModel SelectedDiagram { get; set; }
       public int Count { get; set; }
       public MultipointDataModel()
       {
           DiagramTypes = new List<Api.Models.PLUS.MultiPointData.DiagramType>();
       }
    }
}