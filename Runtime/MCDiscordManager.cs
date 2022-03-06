using System.Collections;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Discord;

namespace MC.DiscordManager
{
    public class MCDiscordManager : MonoBehaviour
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
        }

        private void Update()
        {
            discordInstance.RunCallbacks();
        }

#if UNITY_EDITOR
        //Removes the activity instance on the discord user.
        private void OnDisable()
        {
            Debug.Log("Deleting Discord Obj");
            discordInstance.Dispose();
        }
#else
    private void OnApplicationQuit()
    {
        Debug.Log("Deleting Discord Obj");
        discordInstance.Dispose();
    }
#endif

#region Activity's
        /// <summary>
        /// Clear the users activity
        /// </summary>
        public void ClearActivity()
        {
            discordInstance.GetActivityManager().ClearActivity(delegate (Result _result)
                {
                    if (_result == Result.Ok)
                    {
                        Debug.Log("Succsessfuly cleared Activity");
                    }
                    else
                    {
                        Debug.Log("Couldn't clear Activity");
                    }
                });
        }

        /// <summary>
        /// Sets an activity with a start and end time for time remaining.
        /// </summary>
        /// <param name="_state">What are you doing</param>
        /// <param name="_details">Details</param>
        /// <param name="_startTime">Start time should be Unix Seconds of time.Now</param>
        /// <param name="_endTime">End time should be Unix Seconds of end time</param>
        public void SetActivity(string _state, string _details, long _startTime, long _endTime)
        {
            Activity _activity = new Activity
            {
                State = _state,
                Details = _details,
                Timestamps = new ActivityTimestamps
                {
                    Start = _startTime,
                    End = _endTime
                },
                Assets = new ActivityAssets
                {
                    LargeImage = DiscordManagerData.Settings.largeImageKey,
                    LargeText = DiscordManagerData.Settings.largeImageText,
                    SmallImage = DiscordManagerData.Settings.smallImageKey,
                    SmallText = DiscordManagerData.Settings.smallImageText
                },
            };
        }

        /// <summary>
        /// Sets the Activity with a time elapsed time
        /// </summary>
        /// <param name="_state">What are you doing</param>
        /// <param name="_details">Details</param>
        /// <param name="_startTime">Start time should be Unix Seconds</param>
        public void SetActivity(string _state, string _details, long _startTime)
        {
            Activity _activity = new Activity
            {
                State = _state,
                Details = _details,
                Timestamps = new ActivityTimestamps
                {
                    Start = _startTime
                },
                Assets = new ActivityAssets
                {
                    LargeImage = DiscordManagerData.Settings.largeImageKey,
                    LargeText = DiscordManagerData.Settings.largeImageText,
                    SmallImage = DiscordManagerData.Settings.smallImageKey,
                    SmallText = DiscordManagerData.Settings.smallImageText
                },
            };

            SetActivity(_activity);
        }

        /// <summary>
        /// Sets an activity with a state and details
        /// </summary>
        /// <param name="_state"></param>
        /// <param name="_details"></param>
        public void SetActivity(string _state, string _details)
        {
            SetActivity(_state, _details, startTime);
        }

        /// <summary>
        /// Sets the Activity
        /// </summary>
        /// <param name="_activity">Discords Activity Struct</param>
        public void SetActivity(Activity _activity)
        {
            ActivityManager _activityManager = discordInstance.GetActivityManager();

            _activityManager.UpdateActivity(_activity, delegate (Result _result)
            {
                if (_result == Result.Ok)
                {
                    Debug.Log("Set Up");
                }
                else
                {
                    Debug.Log("Error");
                }
            });
        }
#endregion

#region Overlay Manager
        /// <summary>
        /// Join the discord server
        /// </summary>
        /// <param name="onComplete">Returns true if was able to successfuly join the server. False if there was an issue.</param>
        public void RequestInviteToDiscordServer(Action<bool> onComplete = null)
        {
            discordInstance.GetOverlayManager().OpenGuildInvite(DiscordManagerData.Settings.serverInviteCode,
                delegate (Result _result)
                {
                    if (_result == Result.Ok)
                    {
                        Debug.Log("All Good");
                    }

                    onComplete?.Invoke(_result == Result.Ok);
                });
        }
#endregion

#region UserData
        private void OnUserUpdate()
        {
            userData = discordInstance.GetUserManager().GetCurrentUser();

            FetchUsersAvatar();

            onUserUpdate?.Invoke();
        }

        /// <summary>
        /// Returns the Username of the discord user.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if (userData.Bot)
                return "Bot";

            return userData.Username;
        }

        /// <summary>
        /// Currently this is broken on discords end 
        /// </summary>
        /// <returns></returns>
        public Texture2D GetUsersAvatar()
        {
            if (usersAvatar == null)
            {
                FetchUsersAvatar();
            }
            return usersAvatar;
        }

        /// <summary>
        /// Get the raw Data of the discord user.
        /// </summary>
        /// <returns></returns>
        public User GetRawUserData()
        {
            return userData;
        }

        /// <summary>
        /// Currently this is broken on discords side
        /// </summary>
        private void FetchUsersAvatar()
        {
            discordInstance.GetImageManager().Fetch(ImageHandle.User(userData.Id, 256),
                delegate (Result _result, ImageHandle _handle)
                {
                    if (_result == Result.Ok)
                    {
                        usersAvatar = discordInstance.GetImageManager().GetTexture(_handle);
                        imgDiscordUser.texture = usersAvatar;
                    }
                    else
                    {
                        Debug.Log(_result);
                    }
                });
        }
#endregion

#region Utility
        public static long TimeToUnixSeconds(DateTime _dateTime)
        {
            return new DateTimeOffset(_dateTime).ToUnixTimeSeconds();
        }
#endregion
    }
}
