using System;
using System.Collections.Generic;

namespace ElsevierMaterials.Common.Interfaces
{
    public interface IUowCommandResult : ICommandResult
    {
        int RecordsAffected { get; set; }
        IDictionary<string, string> ValidationErrors { get; }
    }
}
