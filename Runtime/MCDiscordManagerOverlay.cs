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
            if (IsInitialized)
            {
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
            else
            {
                Application.OpenURL($"https://discord.gg/{DiscordManagerData.Settings.serverInviteCode}");
                _onComplete?.Invoke(true);
            }
        }

        public void InviteToActivity(Action<bool> _onInvite)
        {
            discordInstance?.GetOverlayManager().OpenActivityInvite(ActivityActionType.Spectate,
                delegate (Result _result)
                {
                    if (DiscordManagerData.Settings.useDebugLogging)
                    {
                        Debug.Log($"Invite returned result: {_result}");
                    }

                    _onInvite?.Invoke(_result == Result.Ok);
                });
        }

        public void OpenVoiceSettings()
        {
            discordInstance?.GetOverlayManager().OpenVoiceSettings(
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
            discordInstance?.GetOverlayManager().SetLocked(_isLocked,
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
            return discordInstance?.GetOverlayManager().IsLocked() ?? false;
        }
        #endregion
    }
}
