using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElsevierMaterialsMVC.Models.Search
{
    public class MaterialModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SourceId { get; set; }
    }
}