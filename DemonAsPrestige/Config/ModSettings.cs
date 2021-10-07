﻿using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using DemonAsPrestige;
using static UnityModManagerNet.UnityModManager;

namespace DemonAsPrestige.Config {
    static class ModSettings {
        public static ModEntry ModEntry;
       // public static Fixes Fixes;
        //public static AddedContent AddedContent;
        public static Blueprints Blueprints;
       // public static Scaling Scaling;
        public static void LoadAllSettings() {
          //  LoadSettings("Fixes.json", ref Fixes);
          //  LoadSettings("AddedContent.json", ref AddedContent);
            LoadSettings("Blueprints.json", ref Blueprints);
            //LoadSettings("Scaling.json", ref Scaling);
        }
        private static void LoadSettings<T>(string fileName, ref T setting) where T : IUpdatableSettings {
            var assembly = Assembly.GetExecutingAssembly();
            string userConfigFolder = ModEntry.Path + "UserSettings";
            Directory.CreateDirectory(userConfigFolder);
            var resourcePath = $"DemonAsPrestige.Config.{fileName}";
            var userPath = $"{userConfigFolder}{Path.DirectorySeparatorChar}{fileName}";


    
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream)) {
                setting = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            if (File.Exists(userPath)) {
                using (StreamReader reader = File.OpenText(userPath)) {
                    try {
                        T userSettings = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                        setting.OverrideSettings(userSettings);
                    } catch {
                        DemonAsPrestige.Main.Error("Failed to load user settings. Settings will be rebuilt.");
                        try { File.Copy(userPath, userConfigFolder + $"{Path.DirectorySeparatorChar}BROKEN_{fileName}", true); } catch { DemonAsPrestige.Main.Error("Failed to archive broken settings."); }
                    }
                }
            }
            File.WriteAllText(userPath, JsonConvert.SerializeObject(setting, Formatting.Indented));
        }
    }
}
