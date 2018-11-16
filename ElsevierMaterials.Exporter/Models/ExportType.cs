using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterials.Exporter.Models
{
    public class ExportType
    {
        public ExportType() {
            Properties = new List<PropertyTypeEnum>();
        }

        public string IdKey { get; set; }
        public string IdString { get; set; }
        public ExportTypeEnum ExportTypeId { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public IList<PropertyTypeEnum> Properties { get; set; }

    }
}