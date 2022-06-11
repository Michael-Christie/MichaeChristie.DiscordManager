# Discord Package

Last Updated: June 11, 2022 12:27 PM

To install ``https://github.com/Michael-Christie/unity.michaelchristie.discordmanager.git#1.0.5`` in the unity package manager.

# Prerequisite

Once you’ve downloading this package into your unity project, you will need to set up a Scriptable Object that holds all the data needed for the Discord Manager. Under Discord Manager > Create Asset Menu. This will need to create a Settings Scriptable Object  in Resources > DiscordManager for you to edit.

The most important thing you need to do first is enter in the appID to this Discord Settings Object (from now on I’ll refer to this object as Discord Settings). To create or get your Discord app ID, go to:

[Discord Developer Portal - API Docs for Bots and Developers](https://discord.com/developers/applications)

and create a new application.

# Starting Up A Discord Manager Instance

Once your ready, you need to attach the DiscordManager script to a GameObject in the scene.  In Discord Settings, you can set whether you want the Discord Manager to run from start, or whether you wish to manually call the set up of this object. There is also a drop down list, Discord Load Flag, which defines how discord should load with this application.

| Discord Load Flag | Meaning |
| --- | --- |
| Not Required | If discord isn’t running, then this isn’t a problem. The discord manager will return out. |
| Default | If discord isn’t running, the application will restart and load discord up first to ensure discord is running with the application |

<aside>
💡 Note: If the load flag is set to default in editor, and you don’t have discord open, the editor will close it self down, load open discord, and then do nothing.

</aside>

If you intend to manually call the create for the setting up the Discord Manager, you can do that by calling :

```csharp
DiscordManager.Instance.Create(OnCreateCallback);

private void OnCreateCallback(bool _wasCreated)
{
	//some code
}
```

As early on as possible in your application, you should also assign a function callback to onUserUpdate. This function will be called every time there is an update to the discord user.

```csharp
//Is called when the user is updated from discord.
DiscordManager.Instance.onUserUpdate += void Function()
```

# Activities

Activities help share with others what game or what you are currently doing in a game with discord. There is a lot of customizable options you can do with activities.

## Functions

To set up an activity there are multiple ways you could do this.

```csharp
//Sets an activity with just text
DiscordManager.Instance.SetActivity("Doing A Job", "Cleaned 4/5");

//Sets an activity with time played
DiscordManager.Instance.SetActivity("Doing A Job", "Cleaned 4/5", DateTime.Now.TimeToUnixSeconds());

//Sets an activity with time remaining
DiscordManager.Instance.SetActivity("Doing A Job", "Cleaned 4/5", DateTime.Now.TimeToUnixSeconds(), DateTime.SomeEndTime.TimeToUnixSeconds());

//Create a custom activity and set it to that.
Activity _newActivity = new Activity();
DiscordManager.Instance.SetActivity(_newActivity);
```

<aside>
💡 Note: The name of the game isn’t set up via code, but is actually set up via your application you make on the discord developers portal.

</aside>

<aside>
💡 Note: TimeToUnixSeconds() is a extension function that converts a DateTime to a long value, which is what discord requires for setting up activities.

</aside>

# Users

There is a bunch of infomation you can get about the current discord user that is logged in that you can use to make your game feel more personal.

## Properties

| Name | Type | Description |
| --- | --- | --- |
| IsUserPartner | Bool (get;) | Returns if the user is a discord partner |
| IsUserHypeSquadEvents | Bool (get;) | Returns if the user is part of the hype squad event |
| DoesUserHaveHouse | Bool (get;) | Returns if the user has selected a house role |

## Functions

To get the players Discord Username :

```csharp
DiscordManager.Instance.GetUserName();
```

To get the users profile picture:

```csharp
DiscordManager.Instance.GetUsersAvatar(OnAvatarReturn);

Function OnAvatarReturn(bool _successful, Texture2D _texture){}
```

To get if a user is a premium member:

```csharp
//Will return None, Tier1 (Classic Nitro) or Tier2 (Nitro)
DiscordManager.Instance.GetUserPremierType();
```

To get which house the user is in:

```csharp
//Returns UserFlag Enum
DiscordManager.Instance.GetHouseFlag();

//You can also individually check for each house flag with
DiscordManager.Instance.DoesHaveHouseFlag(UserFlag.HypeSquadHouse1);
```

On some occasion you might want to get data about another user. To do this you must know the user ID first.

To get another users raw data, call:

```csharp
DiscordManager.Instance.GetAnotherUser(330818353142824962, OnUserGot)

Function OnUserGot(bool _successful, ref User _collectedUser){}
```

Or to just get the profile picture of another user:

```csharp
DiscordManager.Instance.GetAnotherUsersAvatar(330818353142824962, GetUsersTexture _onComplete);

Function OnAvatarReturn(bool _successful, Texture2D _texture){}
```

# Overlay

## Functions

To invite a user to a discord server:

```csharp
DiscordManager.Instance.RequestInviteToDiscordServer(OnRequestComplete);

Function OnRequestComplete(bool _wasCompleted){}
```

<aside>
💡 Note: The discord server code should be placed in the discord settings object for this to run successfully. If the discord manager isn’t connected to a discord application, it will open up a webpage with the link.

</aside>

If you are using discord for communication, you can open up the voice settings with:

```csharp
DiscordManager.Instance.OpenVoiceSettings();
```

To get the current state of the overlay, or to lock it:

```csharp
DiscordManager.Instance.GetOverlayLockedState();

DiscordManager.Instance.SetOverlayLockedState(bool);
```

# Web Hooks

## Functions

To send a web hook into a discord server:

```csharp
//if using the webhook url from settings object
DiscordManager.Instance.SendWebhook(new WebhookData(), OnComplete);
DiscordManager.Instance.SendWebhook(new EmbedData(), OnComplete);
DiscordManager.Instance.SendWebhook("Hello World", OnComplete);

//if you want to use a other webhook url.
DiscordManager.Instance.SendWebhook(new WebhookData(), OnComplete, "www.otherWebhookURL.com");
DiscordManager.Instance.SendWebhook(new EmbedData(), OnComplete, "www.otherWebhookURL.com");
DiscordManager.Instance.SendWebhook("Hello World", OnComplete, "www.otherWebhookURL.com");

Function OnComplete(bool _successful);
```

## Structs

### WebhookData

| Name | Type | Description |
| --- | --- | --- |
| username? | string | The username who is sending the message |
| avatarULR? | string | The url of the avatar from the user sending the message |
| content? | string | The main text body of the web hook |
| embeds? | EmbedData[] | An array of embeded data. (Maxed of 4) |

### EmbedData

| Name | Type | Description |
| --- | --- | --- |
| title | string | The title of the embed |
| description? | string | The main body of the embed |
| url? | string | A link to an external source |
| color? | string | Color for the webhook, should be a hex value (47359) not (#47359) |
| timestamp? | DateTime | The time this message was created |
| author? | Author | The Author of the embed |
| thumbnail? | Thumbnail | The thumbnail to show in the embed |
| fields? | Field[] | The fields of the embed. (Maxed at 25) |
| image? | Image | The image in the embed |
| footer? | Footer | The footer of the embed |

### Author

| Name | Type | Description |
| --- | --- | --- |
| username | string | A display name of who is sending the message |
| url? | string | A external link if the author is pressed |
| iconUrl? | string | A url to an image to display of the user |

### Thumbnail

| Name | Type | Description |
| --- | --- | --- |
| url | string | A url to an image to display as the thumbnail |
| width? | int | The width you want of the image |
| height? | int | The height you want of the image |

### Field

| Name | Type | Description |
| --- | --- | --- |
| name | string | The header of the field |
| value | string | The main body of the field |
| bool? | bool | If its inline |

### Image

| Name | Type | Description |
| --- | --- | --- |
| url | string | A url to an image to display as the thumbnail |
| width? | int | The width you want of the image |
| height? | int | The height you want of the image |

### Footer

| Name | Type | Description |
| --- | --- | --- |
| text | string | The text to have in the footer |
| iconUrl? | string | The url of an image to include in the footer |
