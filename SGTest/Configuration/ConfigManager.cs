using Newtonsoft.Json;

public static class ConfigManager
{
    public static DbConfig? ReadDbConfig(string configFile)
    {
        using var file = File.OpenText(configFile);
        return JsonConvert.DeserializeObject<DbConfig>(file.ReadToEnd());
    }
}