using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using SingerUtils.Core;
using Serilog;

namespace SingerUtils.Core.Util
{

    public static class Preferences
    {
        public static SerializablePreferences Default;

        static Preferences()
        {
            Load();
        }

        /*public static void Save()
        {
            try
            {
                File.WriteAllText(PathManager.Inst.PrefsFilePath,
                    JsonConvert.SerializeObject(Default, Newtonsoft.Json.Formatting.Indented),
                    Encoding.UTF8);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to save prefs.");
            }
        }*/

        public static void Reset()
        {
            Default = new SerializablePreferences();
        }

        private static void Load()
        {
            try
            {
                if (File.Exists(PathManager.Inst.PrefsFilePath))
                {
                    Default = JsonConvert.DeserializeObject<SerializablePreferences>(
                        File.ReadAllText(PathManager.Inst.PrefsFilePath, Encoding.UTF8));
                }
                else
                {
                    Reset();
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to load prefs.");
                Default = new SerializablePreferences();
            }
        }

        [Serializable]
        public class SerializablePreferences
        {
            public int Theme;
            public string Language = string.Empty;
        }
    }
}