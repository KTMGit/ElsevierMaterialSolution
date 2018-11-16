using System;
namespace ElsevierMaterials.Models.Domain.Property
{
    interface IPropertyBasicInfo
    {
        int Id { get; set; }
        string Name { get; set; }
        string Unit { get; set; }
    }
}
