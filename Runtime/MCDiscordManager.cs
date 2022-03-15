using System.Collections;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        public static MCDiscordManager Instance { get; private set; }

        private static Discord.Discord discordInstance;

        [SerializeField] private RawImage imgDiscordUser;

        private User userData;

        private Texture2D usersAvatar;

        private long startTime;

        public Action onUserUpdate;

        //
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            startTime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

            discordInstance = new Discord.Discord(DiscordManagerData.Settings.discordAppId, (UInt64)CreateFlags.Default);

            if (DiscordManagerData.Settings.hasSteamID)
            {
                discordInstance.GetActivityManager().RegisterSteam(DiscordManagerData.Settings.steamAppID);
            }

            discordInstance.GetUserManager().OnCurrentUserUpdate += OnUserUpdate;

            if (DiscordManagerData.Settings.useDebugLogging)
            {
                discordInstance.SetLogHook(DiscordManagerData.Settings.minLoggingLevel, DiscordInternalLog);
            }
        }

        private void Update()
        {
            discordInstance.RunCallbacks();
        }

#if UNITY_EDITOR
        //Removes the activity instance on the discord user.
        private void OnDisable()
        {
            if (DiscordManagerData.Settings.useDebugLogging)
            {
                Debug.Log("Deleting Discord Obj");
            }
            discordInstance.Dispose();
        }
#else
    private void OnApplicationQuit()
    {
        if (DiscordManagerData.Settings.useDebugLogging)
        {
            Debug.Log("Deleting Discord Obj");
        }
        discordInstance.Dispose();
    }
#endif

        #region Application
        public string GetCurrentLangaugae()
        {
            return discordInstance.GetApplicationManager().GetCurrentLocale();
        }

        public void GetOAuth2Token(ApplicationManager.GetOAuth2TokenHandler _onComplete)
        {
            discordInstance.GetApplicationManager().GetOAuth2Token(
                delegate (Result _result, ref OAuth2Token _token)
                {
                    _onComplete?.Invoke(_result, ref _token);
                });
        }
        #endregion

        #region Utility
        private void DiscordInternalLog(LogLevel _level, string _message)
        {
            if (_level == LogLevel.Debug || _level == LogLevel.Info)
            {
                Debug.Log($"Discord - {_message}");
            }
            else if (_level == LogLevel.Warn)
            {
                Debug.LogWarning($"Discord - {_message}");
            }
            else if (_level == LogLevel.Error)
            {
                Debug.LogError($"Discord - {_message}");
            }
        }
        #endregion
    }
}
