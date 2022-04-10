using System;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEngine.Networking;
using System.Collections;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        #region UserData
        public delegate void GetUser(bool _sucess, ref User user);
        public delegate void GetUsersTexture(bool _sucess, Texture2D _avatarTexture);

        public bool DoesUserHaveHouse
        {
            get
            {
                return DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse1)
                    || DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse2)
                    || DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse3);
            }
        }

        public bool IsUserPartner
        {
            get
            {
                return DoesUserHaveHouseFlag(UserFlag.Partner);
            }
        }

        public bool IsUserHypeSquadEvents
        {
            get
            {
                return DoesUserHaveHouseFlag(UserFlag.HypeSquadEvents);
            }
        }

        //
        /// <summary>
        /// This is setup on start and is a callback
        /// </summary>
        private void OnUserUpdate()
        {
            userData = discordInstance.GetUserManager().GetCurrentUser();

            GetUsersAvatar(
                delegate (bool _success, Texture2D _texture)
                {
                    if (_success)
                    {
                        usersAvatar = _texture;
                    }
                });

            onUserUpdate?.Invoke();
        }

        /// <summary>
        /// Returns the Username of the discord user.
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            if(!IsInitialized)
            {
                return "UserName";
            }

            if (userData.Bot)
            {
                return "Bot";
            }

            return userData.Username;
        }

        /// <summary>
        /// Currently this is broken on discords end 
        /// </summary>
        /// <returns></returns>
        public void GetUsersAvatar(GetUsersTexture _onComplete)
        {
            if (usersAvatar == null)
            {
                GetAnotherUsersAvatar(userData, _onComplete);
            }
            else
            {
                _onComplete?.Invoke(true, usersAvatar);
            }
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
        /// Gets the Users premium type
        /// </summary>
        /// <returns>Returns none, tier 1 (classic nitro) or tier 2 (nitro)</returns>
        public PremiumType GetUsersPremiumType()
        {
            if (!IsInitialized)
                return PremiumType.None;

            return discordInstance.GetUserManager().GetCurrentUserPremiumType();
        }

        /// <summary>
        /// Get the current users house flag. Should Check if they have house with DoesUserHaveHouse before calling this.
        /// </summary>
        /// <returns>Defaults return of House 1</returns>
        public UserFlag GetHouseFlag()
        {
            if (DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse1))
            {
                return UserFlag.HypeSquadHouse1;
            }
            else if (DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse2))
            {
                return UserFlag.HypeSquadHouse2;
            }
            else if (DoesUserHaveHouseFlag(UserFlag.HypeSquadHouse3))
            {
                return UserFlag.HypeSquadHouse3;
            }

            return 0;
        }

        /// <summary>
        /// Check if the user has a flag. Exposed if you want to do this yourself.
        /// </summary>
        /// <param name="_flag">The user flag to check</param>
        /// <returns>return true if they have that flag</returns>
        public bool DoesUserHaveHouseFlag(UserFlag _flag)
        {
            if (!IsInitialized)
                return false;

            return discordInstance.GetUserManager().CurrentUserHasFlag(_flag);
        }

        /// <summary>
        /// Get the user of a given ID
        /// </summary>
        /// <param name="_userID">The users ID you want to get.</param>
        /// <param name="_onComplete">The callback for when the user is found.</param>
        public void GetAnotherUser(long _userID, GetUser _onComplete)
        {
            discordInstance?.GetUserManager().GetUser(_userID,
                delegate (Result _result, ref User _user)
                {
                    _onComplete?.Invoke(_result == Result.Ok, ref _user);
                });
        }

        public void GetAnotherUsersAvatar(User _user, GetUsersTexture _onComplete)
        {
            StartCoroutine(IGetUsersAvatar(_user, _onComplete));
        }

        private IEnumerator IGetUsersAvatar(User _user, GetUsersTexture _onComplete)
        {
            string _url = $"https://cdn.discordapp.com/avatars/{_user.Id}/{_user.Avatar}.png?size=256";

            using (UnityWebRequest _request = UnityWebRequest.Get(_url))
            {

                yield return _request.SendWebRequest();

                if (!string.IsNullOrEmpty(_request.error))
                {
                    Debug.Log(_request.error);

                    _onComplete?.Invoke(false, null);
                    yield break;
                }

                Texture2D _texture = new Texture2D(256, 256, TextureFormat.RGBA32, false, true);
                _texture.LoadImage(_request.downloadHandler.data);
                _texture.Apply();

                _onComplete?.Invoke(true, _texture);
            }
        }
        #endregion
    }
}
