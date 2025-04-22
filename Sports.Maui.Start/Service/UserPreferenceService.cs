namespace Sports.Maui.Start.Service;

using Sports.Maui.Service.Interface;

public class UserPreferenceService : IUserPreferenceService
{
    public string GetString(string key)
    {
        return Preferences.Default.Get<string>(key, null);
    }

    public int? GetInt(string key)
    {
        try
        {
            var result = GetString(key);
            return int.Parse(result);
        }
        catch
        {
            return null;
        }
    }

    public bool? GetBool(string key) 
    {
        try
        {
            var result = GetString(key);
            return bool.Parse(result);
        }
        catch
        {
            return null;
        }
    }

    public void Set<T>(string key, T value)
    {
        var tType = typeof(T);
        if (tType == typeof(bool) || tType == typeof(int))
        {
            Preferences.Default.Set(key, value!.ToString());
            return;
        }

        Preferences.Default.Set(key, value);
    }

    public void Delete(string key) 
    {
        Preferences.Default.Remove(key);
    }
}
