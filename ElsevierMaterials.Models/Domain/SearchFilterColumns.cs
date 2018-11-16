using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class SearchFilterColumns
    {
        public int Id { get; set; }
        public bool isVisible { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
    }
}
