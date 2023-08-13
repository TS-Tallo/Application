using System.Xml;

namespace CadreApp.Context;

using System;
using System.IO;
using Newtonsoft.Json;


public class ConfigurationManager
{
    
    private static readonly string AppDataFolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CadreApp"
    );

    private static readonly string ConfigFilePath = Path.Combine(
        AppDataFolderPath,
        "config.json"
    );

    public static Config LoadConfig()
    {
        if (File.Exists(ConfigFilePath))
        {
            string configJson = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<Config>(configJson);
        }

        return null; // Return a default configuration or handle missing config
    }
}