using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Api.Models;
using ElsevierMaterials.Models;
using Api.Models.HeatTreatment;

namespace ElsevierMaterialsMVC.Models.MaterialDetails
{
    public class ProcessingModel
    {
        public HeatTreatment HeatTreatment { get; set; }
        public MetallographyModel Metallography { get; set; }
     public MachinabilityModel Machinability { get; set; }
     public ManufacturingModel Manufacturing { get; set; }
    }
}