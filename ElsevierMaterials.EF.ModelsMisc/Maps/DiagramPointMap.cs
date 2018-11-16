using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ElsevierMaterials.Models;


namespace ElsevierMaterials.EF.ModelsMisc.Maps
{
    public class DiagramPointMap : EntityTypeConfiguration<DiagramPoint>
    {
        public DiagramPointMap()
        {
            ToTable("View_DiagramsPoints");
            HasKey(n => new { n.MaterialID, n.SubgroupID, n.RowID, n.PropertyID, n.DiagramID, n.CurveId });          
        }
    }
}
