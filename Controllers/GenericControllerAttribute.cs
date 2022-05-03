using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CharacterDatabaseAPI.Controllers;

public class GenericControllerAttribute : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        Type entityType = controller.ControllerType.GetGenericArguments()[0];

        controller.ControllerName = entityType.Name;
    }
}
