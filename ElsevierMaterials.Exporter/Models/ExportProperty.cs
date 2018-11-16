using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ElsevierMaterials.Exporter.Models
{   
    public class ExportProperty : ExportPropertyGeneral
    {
       public StressStrainType SubType { get; set; }     
    }

}