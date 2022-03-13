using System;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
        #region UserData
        public delegate void GetUser(bool _sucess, ref User user);

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
        /// Gets the Users premium type
        /// </summary>
        /// <returns>Returns none, tier 1 (classic nitro) or tier 2 (nitro)</returns>
        public PremiumType GetUsersPremiumType()
        {
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

            return UserFlag.HypeSquadHouse1;
        }

        /// <summary>
        /// Check if the user has a flag. Exposed if you want to do this yourself.
        /// </summary>
        /// <param name="_flag">The user flag to check</param>
        /// <returns>return true if they have that flag</returns>
        public bool DoesUserHaveHouseFlag(UserFlag _flag)
        {
            return discordInstance.GetUserManager().CurrentUserHasFlag(_flag);
        }

        /// <summary>
        /// Get the user of a given ID
        /// </summary>
        /// <param name="_userID">The users ID you want to get.</param>
        /// <param name="_onComplete">The callback for when the user is found.</param>
        public void GetOtherUser(long _userID, GetUser _onComplete)
        {
            discordInstance.GetUserManager().GetUser(_userID,
                delegate (Result _result, ref User _user)
                {
                    _onComplete?.Invoke(_result == Result.Ok, ref _user);
                });
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
                        if (DiscordManagerData.Settings.useDebugLogging)
                        {
                            Debug.LogError(_result);
                        }
                    }
                });
        }
        #endregion
    }
}
