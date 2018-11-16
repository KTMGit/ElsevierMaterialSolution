using ElsevierMaterials.Models;
using ElsevierMaterials.Models.Domain.AdvancedSearch;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class AdvSearchMultipointDataViewMap : EntityTypeConfiguration<AdvSearchMultipointDataView>
    {
        public AdvSearchMultipointDataViewMap()
        {
            HasKey(t => new { t.MaterialID, t.PointID });
            ToTable("View_MultipointData_AdvSearch");
        }
    }
}






