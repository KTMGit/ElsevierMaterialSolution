using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElsevierMaterials.Models.Domain.AdvancedSearch
{
    public class AdvSearchFiltersAll
    {
        public IList<AdvSearchFilters> AllFilters { get; set; }
        public AdvStructureSearch StructureSearch { get; set; }

        public string SelectedSource { get; set; }
        public bool IsChemical { get; set; }
    }
}
