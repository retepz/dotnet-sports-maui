namespace Sports.Maui.Model.Interface;

public interface IWebview
{
    Task<string> EvaluateJavaScriptAsync(string script);
}
