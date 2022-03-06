using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.DiscordManager
{
    public class DiscordSettings : ScriptableObject
    {
        [Tooltip("The app ID from Discord Dev page")] public long discordAppId;

        [Tooltip("Steams App ID")] public uint steamAppID;

        //These image keys are found by uploading an image to discord art asset section in the developers section
        public string largeImageKey;
        public string largeImageText;
        public string smallImageKey;
        public string smallImageText;
        public string serverInviteCode;

        public bool hasSteamID;
    }
}
