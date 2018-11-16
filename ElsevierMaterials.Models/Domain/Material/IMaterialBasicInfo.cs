using System;
namespace ElsevierMaterials.Models.Domain.Material
{
    interface IMaterialBasicInfo
    {
        int SourceId { get; set; }
        int MaterialId { get; set; }
        int SourceMaterialId { get; set; }  
        int SubgroupId { get; set; }
        string Name { get; set; }
        string Standard { get; set; }
        string SubgroupName { get; set; }
    }
}
