using Newtonsoft.Json;

public static class ConfigManager
{
    public static DatabaseSettings? ReadDbConfig(string configFile)
    {
        using var file = File.OpenText(configFile);
        return JsonConvert.DeserializeObject<DatabaseSettings>(file.ReadToEnd());
    }
}