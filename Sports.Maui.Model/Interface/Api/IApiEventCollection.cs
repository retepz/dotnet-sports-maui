namespace Sports.Maui.Model.Interface.Api;

using Sports.Maui.Model.Api;
using System.Collections.Concurrent;

public interface IApiEventCollection
{
    ConcurrentQueue<ApiUrl> EventUrls { get; set; }
    int EventCount { get; set; }
    int PageCount { get; set; }
    int PageSize { get; set; }
    int PageIndex { get; set; }
}
