using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class DiagramPoint
    {
        public int MaterialID {get; set;}
        public int SubgroupID { get; set; }
        public int PropertyID { get; set; }
        public int RowID { get; set; }  
        public int DiagramID { get; set; }
        public int CurveId { get; set; }
        public string Legend { get; set; }
        public double X_value { get; set; }
        public double Y_value { get; set; }
        public int PointOrder { get; set; }
    }
}


