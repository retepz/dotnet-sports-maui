namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiSeason : ApiCacheItem, IApiId
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public ApiSeasonType Type { get; set; }
    public int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [JsonIgnore]
    public ApiUrl? CurrentWeekUrl => Type.CurrentWeek;

    [JsonIgnore]
    public ApiUrl? CurrentWeeksUrl => Type.CurrentWeeks;

    [JsonIgnore]
    public bool IsOffSeason => !DatesAreInSeason || TypeIsOffSeason;

    [JsonIgnore]
    public bool DatesAreInSeason => DateTime.UtcNow.Ticks > StartDate.Ticks && DateTime.UtcNow.Ticks < EndDate.Ticks;

    [JsonIgnore]
    public bool TypeIsOffSeason => Type != null && Type.TypeId == ApiSeasonTypeId.Off;
}
