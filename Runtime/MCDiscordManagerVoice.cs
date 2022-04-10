using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        #region Voice
        /// <summary>
        /// Gets / Sets the users mute state
        /// </summary>
        public bool SelfMute
        {
            get
            {
                return discordInstance?.GetVoiceManager().IsSelfMute() ?? false;
            }
            set
            {
                discordInstance?.GetVoiceManager().SetSelfMute(value);
            }
        }

        /// <summary>
        /// Gets / Sets the users deaf state
        /// </summary>
        public bool SelfDeaf
        {
            get
            {
                return discordInstance?.GetVoiceManager().IsSelfDeaf() ?? false;
            }
            set
            {
                discordInstance?.GetVoiceManager().SetSelfDeaf(value);
            }
        }

        //
        /// <summary>
        /// Gets the current users voice mode (Type & input key).
        /// </summary>
        /// <returns>The users current voice mode.</returns>
        public InputMode GetVoiceMode()
        {
            return discordInstance?.GetVoiceManager().GetInputMode() ?? new InputMode();
        }

        /// <summary>
        /// Sets the current users voice mode
        /// </summary>
        /// <param name="_inputType">Push to talk or Voice Activity</param>
        /// <param name="_shortcut">With keycode should activate it. If left empty will default to current input</param>
        /// <param name="_onComplete">Callback on Completed</param>
        public void SetInputMode(InputModeType _inputType, string _shortcut, Action<bool> _onComplete)
        {
            if (string.IsNullOrEmpty(_shortcut))
            {
                _shortcut = GetVoiceMode().Shortcut;
            }

            InputMode _newInput = new InputMode()
            {
                Type = _inputType,
                Shortcut = _shortcut
            };

            SetInputMode(_newInput, _onComplete);
        }

        /// <summary>
        /// Sets the current users voice mode, using current shortcut.
        /// </summary>
        /// <param name="_inputType">Push to talk or Voice Activity</param>
        /// <param name="_onComplete">Callback on Completed</param>
        public void SetInputMode(InputModeType _inputType, Action<bool> _onComplete)
        {
            InputMode _newInput = new InputMode()
            {
                Type = _inputType,
                Shortcut = GetVoiceMode().Shortcut
            };

            SetInputMode(_newInput, _onComplete);
        }

        /// <summary>
        /// Sets the current users voice mode
        /// </summary>
        /// <param name="_mode">The input mode</param>
        /// <param name="_onComplete">Callback on Completed</param>
        public void SetInputMode(InputMode _mode, Action<bool> _onComplete)
        {
            discordInstance?.GetVoiceManager().SetInputMode(_mode,
                delegate (Result _result)
                {
                    _onComplete?.Invoke(_result == Result.Ok);
                });
        }

        /// <summary>
        /// Gets the volume of a user locally.
        /// </summary>
        /// <param name="_user">The user's volume you want to get</param>
        /// <returns>The volume as an int betwen 0-200. With 100 being default</returns>
        public int GetLocalVolume(User _user)
        {
            return discordInstance?.GetVoiceManager().GetLocalVolume(_user.Id) ?? 100;
        }

        /// <summary>
        /// Sets the volume of a user locally
        /// </summary>
        /// <param name="_user">The User you wish to change the volume of</param>
        /// <param name="_volume">The volume to set to. Is between 0-200 with 100 being default</param>
        public void SetLocalVolume(User _user, int _volume)
        {
            _volume = Mathf.Clamp(_volume, 0, 200);

            discordInstance?.GetVoiceManager().SetLocalVolume(_user.Id, Convert.ToByte(_volume));
        }

        /// <summary>
        /// Gets whether a user is muted
        /// </summary>
        /// <param name="_user">The user</param>
        /// <returns>If they are muted</returns>
        public bool GetLocalMute(User _user)
        {
            return discordInstance?.GetVoiceManager().IsLocalMute(_user.Id) ?? false;
        }

        /// <summary>
        /// Sets whether a user is muted
        /// </summary>
        /// <param name="_user">The user</param>
        /// <param name="_isMuted">If they are muted or not</param>
        public void SetLocalMute(User _user, bool _isMuted)
        {
            discordInstance?.GetVoiceManager().SetLocalMute(_user.Id, _isMuted);
        }
        #endregion
    }
}
