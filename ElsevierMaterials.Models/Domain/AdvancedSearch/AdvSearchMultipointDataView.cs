using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class AdvSearchMultipointDataView
    {
        public int MaterialID { get; set; }
        public int DiagramID { get; set; }
        public int CurveID { get; set; }
        public int Expr1 { get; set; }
        public int PointID { get; set; }
        public double? X_value { get; set; }
        public double? Y_value { get; set; }
        public int PropertyID { get; set; }
        public int X_unit_id { get; set; }
        public int Y_unit_id { get; set; }
        public string X_label { get; set; }
    }
}