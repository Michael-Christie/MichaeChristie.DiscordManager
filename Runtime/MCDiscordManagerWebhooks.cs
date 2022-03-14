using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Net;
using System.IO;
using System.Text;

namespace MC.DiscordManager
{
    public struct WebhookData
    {
        public string username;
        public string avatar_url;
        public string content;

        public EmbedData[] embeds;
    }

    public struct EmbedData
    {
        public string title;
        public string description;
        public string url;
        public string color;

        public long timestamp;

        public Aurthor author;

        public Thumbnail thumbnail;

        public Field[] fields;

        public Image[] image;

        public Footer footer;
    }

    public struct Aurthor
    {
        public string username;
        public string url;
        public string icon_url;
        public string proxy_icon_url;
    }

    public struct Thumbnail
    {
        public string url;
        public string proxy_url;

        public int width;
        public int height;
    }

    public struct Footer
    {
        public string text;
        public string icon_url;
        public string proxy_icon_url;
    }

    public struct Field
    {
        public string name;
        public string value;

        public bool inline;
    }

    public struct Image
    {
        public string url;
        public string proxy_url;

        public int width;
        public int height;
    }

    public partial class MCDiscordManager : MonoBehaviour
    {
        public void SendWebhook(WebhookData _webhookData, Action<bool> _onComplete)
        {
            SendWebhook(_webhookData, _onComplete, DiscordManagerData.Settings.defaultWebhookURL);
        }

        public void SendWebhook(EmbedData _embedData, Action<bool> _onComplete)
        {
            SendWebhook(_embedData, _onComplete, DiscordManagerData.Settings.defaultWebhookURL);
        }

        public void SendWebhook(string _message, Action<bool> _onComplete)
        {
            SendWebhook(_message, _onComplete, DiscordManagerData.Settings.defaultWebhookURL);
        }

        public void SendWebhook(WebhookData _webhookData, Action<bool> _onComplete, string _url)
        {
            JSONObject _body = WebhookToJson(_webhookData);

            StartCoroutine(IWebhook(_url, _body.ToString(), _onComplete));
        }

        public void SendWebhook(EmbedData _embedData, Action<bool> _onComplete, string _url)
        {
            JSONObject _body = EmbedToJson(_embedData);

            StartCoroutine(IWebhook(_url, _body.ToString(), _onComplete));
        }

        public void SendWebhook(string _message, Action<bool> _onComplete, string _url)
        {
            JSONObject _body = new JSONObject();
            _body.Add("content", _message);

            StartCoroutine(IWebhook(_url, _body.ToString(), _onComplete));
        }

        public void SendWebhook2(string _message)
        {
            JSONObject _body = new JSONObject();

            JSONArray _embedJson = new JSONArray();

            JSONObject _embedData = new JSONObject();

            JSONString _embedTitle = new JSONString("ScreenShot");
            _embedData.Add("title", _embedTitle);

            JSONString _embedDecription = new JSONString("This was sent from Unity");
            _embedData.Add("description", _embedDecription);

            JSONString _embedColor = new JSONString("47359");
            _embedData.Add("color", _embedColor);

            JSONObject _embedAurthor = new JSONObject();
            JSONString _aurthorName = new JSONString("Michael Christie");
            _embedAurthor.Add("name", _aurthorName);

            JSONString _aurthURL = new JSONString("https://i.imgur.com/R66g1Pe.jpg");
            _embedAurthor.Add("icon_url", _aurthURL);

            _embedData.Add("author", _embedAurthor);

            JSONObject _images = new JSONObject();
            JSONString _imageURL = new JSONString("https://i.imgur.com/R66g1Pe.jpg");
            _images.Add("url", _imageURL);

            _embedData.Add("image", _images);

            _embedJson.Add(_embedData);

            _body.Add("username", "Michael");
            _body.Add("content", _message);
            _body.Add("embeds", _embedJson);

            StartCoroutine(IWebhook(DiscordManagerData.Settings.defaultWebhookURL, _body.ToString(), null));
        }

        private IEnumerator IWebhook(string _url, string _data, Action<bool> onComplete)
        {
            WebRequest _request = (HttpWebRequest)HttpWebRequest.Create(_url);
            _request.ContentType = "application/json";
            _request.Method = "post";

            using (var _stream = new StreamWriter(_request.GetRequestStream()))
            { 
                _stream.Write(_data);
            }

            try
            {
                HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            }
            catch(Exception _error)
            {
                if (DiscordManagerData.Settings.useDebugLogging)
                {
                    Debug.Log($"Webhook error: {_error}");
                }

                onComplete?.Invoke(false);
                yield break;
            }

            yield return null;

            onComplete?.Invoke(true);
        }

        private JSONObject WebhookToJson(WebhookData _webhook)
        {
            //if embeds... run the EmbedToJson

            return null;
        }

        private JSONObject EmbedToJson(EmbedData _embed)
        {
            return null;
        }
    }
}
