using System;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface ICommandResult
    {
        bool HasError { get; set; }
        string Description { get; set; }
        Exception Exception { get; set; }
    }
}
