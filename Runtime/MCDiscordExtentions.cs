using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.DiscordManager
{
    public static class MCDiscordExtentions 
    {
        public static long TimeToUnixSeconds(this DateTime _dateTime)
        {
            return new DateTimeOffset(_dateTime).ToUnixTimeSeconds();
        }
    }
}
