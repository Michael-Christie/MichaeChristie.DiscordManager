using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MC.DiscordManager
{
    [ExecuteInEditMode]
    public class DiscordManagerData : MonoBehaviour
    {
        private static DiscordSettings _discordSettings;


        public static DiscordSettings Settings
        {
            get
            {
                if (!_discordSettings)
                {
#if UNITY_EDITOR
                    if (!Directory.Exists($"{Application.dataPath}/Resources"))
                    {
                        Directory.CreateDirectory($"{Application.dataPath}/Resources");
                    }

                    if (!Directory.Exists($"{Application.dataPath}/Resources/DiscordManager"))
                    {
                        Directory.CreateDirectory($"{Application.dataPath}/Resources/DiscordManager");
                    }

                    if (!File.Exists($"{Application.dataPath}/Resources/{GameUtilities.path}.asset"))
                    {
                        _discordSettings = ScriptableObject.CreateInstance<DiscordSettings>();
                        AssetDatabase.CreateAsset(_discordSettings, $"Assets/Resources/{GameUtilities.path}.asset");
                    }

                    AssetDatabase.SaveAssets();
#endif
                    _discordSettings = Resources.Load<DiscordSettings>(GameUtilities.path);
                }
                return _discordSettings;
            }
        }


#if UNITY_EDITOR
        [MenuItem("DiscordManager/CreateAssetMenu", priority = 0)]
        public static void CreateSettingsAsset()
        {
            if (!Directory.Exists($"{Application.dataPath}/Resources"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/Resources");
            }

            if (!Directory.Exists($"{Application.dataPath}/Resources/DiscordManager"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/Resources/DiscordManager");
            }

            if (!File.Exists($"{Application.dataPath}/Resources/{GameUtilities.path}.asset"))
            {
                _discordSettings = ScriptableObject.CreateInstance<DiscordSettings>();
                AssetDatabase.CreateAsset(_discordSettings, $"Assets/Resources/{GameUtilities.path}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("A Discord Managers Settings asset is already created");
            }
        }
#endif
    }
}
