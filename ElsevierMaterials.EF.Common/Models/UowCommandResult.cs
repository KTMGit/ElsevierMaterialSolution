using ElsevierMaterials.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.Common.Models
{
    public class UowCommandResult : IUowCommandResult
    {
        public UowCommandResult()
        {
            HasError = true;
            Description = "Just constructed without exception!";
        }

        public bool HasError { get; set; }
        public string Description { get; set; }
        public Exception Exception { get; set; }
        public int RecordsAffected { get; set; }
        public IDictionary<string, string> ValidationErrors
        {
            get
            {
                if (_validationErrors == null)
                { _validationErrors = new Dictionary<string, string>(); }

                return _validationErrors;
            }
        }

        private IDictionary<string, string> _validationErrors;
    }
}
