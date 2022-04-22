using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        #region Activity's
        /// <summary>
        /// Clear the users activity
        /// </summary>
        public void ClearActivity()
        {
            discordInstance?.GetActivityManager().ClearActivity(
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        if (_result == Result.Ok)
                        {
                            Debug.Log("Succsessfuly cleared Activity");
                        }
                        else
                        {
                            Debug.Log("Couldn't clear Activity");
                        }
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

            SetActivity(_activity);
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

            _activityManager?.UpdateActivity(_activity,
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        if (_result == Result.Ok)
                        {
                            Debug.Log("Set Up");
                        }
                        else
                        {
                            Debug.Log("Error");
                        }
                    }
                });
        }

        ///TODO add in joining / spectating / inviting players here....
        #endregion
    }
}
