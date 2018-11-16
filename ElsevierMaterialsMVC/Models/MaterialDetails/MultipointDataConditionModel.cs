using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.Models.PLUS.MultiPointData;
using ElsevierMaterials.Models;
namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class MultipointDataConditionModel
    {
        public IList<Api.Models.PLUS.MultiPointData.DiagramLegend> DiagramLegends { get; set; }
        public MultipointDataDiagramLegendsModel SelectedDiagramLegend { get; set; }
        public ImageSource Diagram { get; set; }
        public MultipointDataConditionModel()
        {
            DiagramLegends = new List<Api.Models.PLUS.MultiPointData.DiagramLegend>();
        }
    }
}