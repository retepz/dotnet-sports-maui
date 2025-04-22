namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiStatus : ApiCacheItem
{
    private readonly ApiStatusTypeName[] _inProgressStatuses =
    [
        ApiStatusTypeName.STATUS_IN_PROGRESS,
        ApiStatusTypeName.STATUS_END_PERIOD,
        ApiStatusTypeName.STATUS_HALFTIME
    ];

    [JsonProperty("period")]
    public int CurrentPeriod { get; set; }
    public string DisplayClock { get; set; }

    [JsonProperty("type")]
    public ApiStatusType StatusType { get; set; }

    [JsonIgnore]
    public bool IsCompleted => StatusType.CurrentName == ApiStatusTypeName.STATUS_FINAL;

    [JsonIgnore]
    public bool IsInProgress => _inProgressStatuses.Contains(StatusType.CurrentName);

    [JsonIgnore]
    public bool IsInFuture => StatusType.CurrentName == ApiStatusTypeName.STATUS_SCHEDULED;

    [JsonIgnore]
    public override bool IgnoreCache => IsInFuture || IsInProgress;
}
