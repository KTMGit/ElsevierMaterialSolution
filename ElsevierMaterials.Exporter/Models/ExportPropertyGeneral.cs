using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Exporter.Models
{
    public class ExportPropertyGeneral
    {
        //because we can edit one chemical composition element
        public int Id { get; set; }
        public PropertyTypeEnum Type { get; set; }
        public string Value { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public string Mark { get; set; }

        public IDictionary<ExportTypeEnum, bool> SelectedForExports { get; set; }
        public List<Value> Values { get; set; }

        public ExportPropertyGeneral()
        {

            SelectedForExports = new Dictionary<ExportTypeEnum, bool>();
            SelectedForExports.Add(ExportTypeEnum.Radioss, false);
            SelectedForExports.Add(ExportTypeEnum.Abaqus, false);
            SelectedForExports.Add(ExportTypeEnum.SolidEdge, false);
            SelectedForExports.Add(ExportTypeEnum.SolidWorks, false);
            SelectedForExports.Add(ExportTypeEnum.Esi, false);
            SelectedForExports.Add(ExportTypeEnum.ESIPamCrash, false);
            SelectedForExports.Add(ExportTypeEnum.ANSYS, false);
            SelectedForExports.Add(ExportTypeEnum.KTMXls, false);
            SelectedForExports.Add(ExportTypeEnum.KTMXml, false);
            SelectedForExports.Add(ExportTypeEnum.Siemens, false);
            SelectedForExports.Add(ExportTypeEnum.NASTRAN, false);
            SelectedForExports.Add(ExportTypeEnum.LsDyna, false);
            SelectedForExports.Add(ExportTypeEnum.FEMAP, false);
            SelectedForExports.Add(ExportTypeEnum.PTCCreo, false);
        }
    }

}
