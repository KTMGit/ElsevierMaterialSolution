using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models
{
    public class Citation
    {
        public int record_id { get; set; }
        public string MaterialName { get; set; }
        public string InChIKey { get; set; }
        public string Property { get; set; }
        public string MeasuredData { get; set; }
        public string PredictedData { get; set; }
        public string EquationTxt { get; set; }
        public string EquationLink { get; set; }
    }
}
