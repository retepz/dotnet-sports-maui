namespace Sports.Maui.Service.Interface;

public interface IUserPreferenceService
{
    public string GetString(string key);
    public int? GetInt(string key);
    public bool? GetBool(string key);
    void Set<T>(string key, T value);
    void Delete(string key);
}
