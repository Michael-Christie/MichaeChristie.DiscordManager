using UnityEditor;
using UnityEngine;

namespace MC.DiscordManager
{
    [CustomEditor(typeof(DiscordSettings))]
    public class DiscordSettingsEditor : Editor
    {
        private string password = "";

        private int steamApp = 0;

        //
        public override void OnInspectorGUI()
        {
            GUIStyle _redText = new GUIStyle(EditorStyles.label);
            _redText.normal.textColor = Color.red;

            //Custom draw the inspector window
            DiscordSettings _settingTarget = (DiscordSettings)target;

            EditorGUILayout.LabelField("Discord Settings", EditorStyles.boldLabel);

            password = EditorGUILayout.PasswordField("Discord App ID", $"{password}");

            if (!string.IsNullOrEmpty(password))
            {
                long _appID;
                if (long.TryParse(password, out _appID))
                {
                    _settingTarget.discordAppId = _appID;
                }
                else
                {
                    _settingTarget.discordAppId = 0;
                    GUILayout.Label("THIS IS NOT A VALID LONG TYPE", _redText);
                }
            }

            _settingTarget.serverInviteCode = EditorGUILayout.TextField("Discord Server Invite Code", _settingTarget.serverInviteCode);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Activity Images", EditorStyles.boldLabel);
            _settingTarget.largeImageKey = EditorGUILayout.TextField("Large Image Key", _settingTarget.largeImageKey);
            _settingTarget.largeImageText = EditorGUILayout.TextField("Large Image Text", _settingTarget.largeImageText);
            _settingTarget.smallImageKey = EditorGUILayout.TextField("Small Image Key", _settingTarget.smallImageKey);
            _settingTarget.smallImageText = EditorGUILayout.TextField("Small Image Text", _settingTarget.smallImageText);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Webhooks", EditorStyles.boldLabel);
            _settingTarget.useWebhooks = EditorGUILayout.Toggle("Use Webhooks?", _settingTarget.useWebhooks);

            if (_settingTarget.useWebhooks)
            {
                _settingTarget.defaultWebhookURL = EditorGUILayout.TextField("Default Webhook URL", _settingTarget.defaultWebhookURL);
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Steam Intergration", EditorStyles.boldLabel);
            _settingTarget.hasSteamID = EditorGUILayout.Toggle("Have Steam Page?", _settingTarget.hasSteamID);

            if (_settingTarget.hasSteamID)
            {
                steamApp = EditorGUILayout.IntField("Steam App ID", steamApp);

                uint _steamID;
                if (uint.TryParse(steamApp.ToString(), out _steamID))
                {
                    _settingTarget.steamAppID = _steamID;
                }
                else
                {
                    _settingTarget.steamAppID = 0;
                    GUILayout.Label("THIS IS NOT A VALID UINT TYPE", _redText);
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);

            _settingTarget.useDebugLogging = EditorGUILayout.Toggle("Debug Logging", _settingTarget.useDebugLogging);

            if (_settingTarget.useDebugLogging)
            {
                _settingTarget.minLoggingLevel = (Discord.LogLevel)EditorGUILayout.EnumPopup("Min Debug Level", _settingTarget.minLoggingLevel);

                EditorGUILayout.LabelField("Do Not Leave This On In A Full Build", _redText);
            }
        }
    }
}
