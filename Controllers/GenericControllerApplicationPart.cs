using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace CharacterDatabaseAPI.Controllers;

public class GenericControllerApplicationPart : ApplicationPart, IApplicationPartTypeProvider
{
    public GenericControllerApplicationPart(IEnumerable<TypeInfo> typeInfos)
    {
        Types = typeInfos;
    }

    public override string Name => "GenericController";
    public IEnumerable<TypeInfo> Types { get; }
}
