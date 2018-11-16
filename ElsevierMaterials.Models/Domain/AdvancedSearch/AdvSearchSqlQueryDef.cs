using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
    public class AdvSearchSqlQueryDef
    {
        public BinaryOperators Operator { get; set; }
        public AdvSearchFilters Filter { get; set; }
        public string Query { get; set; }
        public IList<object> Args { get; set; }
       

        public AdvSearchSqlQueryDef()
        {
            Operator = BinaryOperators.NotDefined;
            Filter = new AdvSearchFilters();
            Query = "";
            Args = new List<object>();
        }
    }
}
