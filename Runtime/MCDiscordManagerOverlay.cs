using System;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        #region Overlay Manager
        /// <summary>
        /// Join the discord server
        /// </summary>
        /// <param name="onComplete">Returns true if was able to successfuly join the server. False if there was an issue.</param>
        public void RequestInviteToDiscordServer(Action<bool> _onComplete = null)
        {
            if(!IsInitialized)
            {
                _onComplete?.Invoke(false);
                return;
            }

            discordInstance.GetOverlayManager().OpenGuildInvite(DiscordManagerData.Settings.serverInviteCode,
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        if (_result == Result.Ok)
                        {
                            Debug.Log("All Good");
                        }
                    }

                    _onComplete?.Invoke(_result == Result.Ok);
                });
        }

        public void InviteToActivity(Action<Result> _onInvite)
        {
            if(!IsInitialized)
            {
                _onInvite?.Invoke(Result.NotRunning);
                return;
            }

            discordInstance.GetOverlayManager().OpenActivityInvite(ActivityActionType.Spectate,
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        Debug.Log($"Invite returned result: {_result}");
                    }

                    _onInvite?.Invoke(_result);
                });
        }

        public void OpenVoiceSettings()
        {
            if (!IsInitialized)
                return;

            discordInstance.GetOverlayManager().OpenVoiceSettings(
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        Debug.Log($"Open Voice returned result: {_result}");
                    }
                });
        }

        public void SetOverlayLockState(bool _isLocked)
        {
            if (!IsInitialized)
                return;

            discordInstance.GetOverlayManager().SetLocked(_isLocked,
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        Debug.Log($"Setting overlay returned result: {_result}");
                    }
                });
        }

        public bool GetOverlayLockedState()
        {
            if (!IsInitialized)
                return false;

            return discordInstance.GetOverlayManager().IsLocked();
        }
        #endregion
    }
}
