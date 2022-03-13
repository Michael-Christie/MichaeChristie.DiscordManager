using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

namespace MC.DiscordManager
{
    public partial class MCDiscordManager : MonoBehaviour
    {
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
