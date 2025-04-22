namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiStatusType
{
    public bool Completed { get; set; }
    public string Name { get; set; }

    public string ShortDetail { get; set; }

    [JsonIgnore]
    public ApiStatusTypeName CurrentName
    {
        get
        {
            if (Enum.TryParse<ApiStatusTypeName>(Name, out var typeName))
            {
                return typeName;
            }

            return ApiStatusTypeName.NONE;
        }
    }
}
