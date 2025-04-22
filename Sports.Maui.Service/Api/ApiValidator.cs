namespace Sports.Maui.Service.Api;

using Sports.Maui.Model.Interface.Api;

public static class ApiValidator
{
    public static bool IdIsValid(IApiId apiObject)
    {
        if (int.TryParse(apiObject.Id, out var id))
        {
            return id > 0;
        }

        return false;
    }
}
