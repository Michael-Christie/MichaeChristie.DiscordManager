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
        public string avatarUrl;
        public string content;

        public EmbedData[] embeds;
    }

    public struct EmbedData
    {
        public string title;
        public string description;
        public string url;
        public string color;

        public DateTime timestamp;

        public Author author;

        public Thumbnail thumbnail;

        public Field[] fields;

        public Image image;

        public Footer footer;
    }

    public struct Author
    {
        public string username;
        public string url;
        public string iconUrl;
    }

    public struct Thumbnail
    {
        public string url;

        public int width;
        public int height;
    }

    public struct Footer
    {
        public string text;
        public string iconUrl;
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
            catch (Exception _error)
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
            JSONObject _webhookData = new JSONObject();

            if (!string.IsNullOrEmpty(_webhook.content))
            {
                _webhookData.Add("content", _webhook.content);
            }

            //if embeds... run the EmbedToJson
            if (_webhook.embeds != null)
            {
                JSONArray _embedArray = new JSONArray();

                for (int i = 0; i < Mathf.Min(_webhook.embeds.Length, 4); i++)
                {
                    _embedArray.Add(EmbedToJson(_webhook.embeds[i]));
                }

                _webhookData.Add("embeds", _embedArray);
            }

            if (!string.IsNullOrEmpty(_webhook.username))
            {
                _webhookData.Add("username", _webhook.username);
            }

            if (!string.IsNullOrEmpty(_webhook.avatarUrl))
            {
                _webhookData.Add("avatar_url", _webhook.avatarUrl);
            }

            Debug.Log(_webhookData.ToString());

            return _webhookData;
        }

        private JSONObject EmbedToJson(EmbedData _embed)
        {
            JSONObject _embedData = new JSONObject();

            if (!string.IsNullOrEmpty(_embed.title))
            {
                JSONString _embedTitle = new JSONString(_embed.title);
                _embedData.Add("title", _embedTitle);
            }

            if (!string.IsNullOrEmpty(_embed.description))
            {
                JSONString _embedDecription = new JSONString(_embed.description);
                _embedData.Add("description", _embedDecription);
            }

            if (!string.IsNullOrEmpty(_embed.color))
            {
                JSONString _embedColor = new JSONString(_embed.color);
                _embedData.Add("color", _embedColor);
            }

            if (!string.IsNullOrEmpty(_embed.url))
            {
                JSONString _embedURL = new JSONString(_embed.url);
                _embedData.Add("url", _embedURL);
            }

            if (!_embed.timestamp.Equals(default))
            {
                JSONString _embedTimestamp = new JSONString(_embed.timestamp.ToString("o"));
                _embedData.Add("timestamp", _embedTimestamp);
            }

            ///Author
            if (!_embed.author.Equals(default(Author)))
            {
                JSONObject _embedAuthor = new JSONObject();

                if (!string.IsNullOrEmpty(_embed.author.username))
                {
                    JSONString _authorName = new JSONString(_embed.author.username);
                    _embedAuthor.Add("name", _authorName);
                }

                if (!string.IsNullOrEmpty(_embed.author.url))
                {
                    JSONString _authURL = new JSONString(_embed.author.url);
                    _embedAuthor.Add("url", _authURL);
                }

                if (!string.IsNullOrEmpty(_embed.author.iconUrl))
                {
                    JSONString _authIconURL = new JSONString(_embed.author.iconUrl);
                    _embedAuthor.Add("icon_url", _authIconURL);
                }

                _embedData.Add("author", _embedAuthor);
            }

            ///Thumbnails
            if (!_embed.thumbnail.Equals(default(Thumbnail)))
            {
                JSONObject _embedThumbnail = new JSONObject();

                if (!string.IsNullOrEmpty(_embed.thumbnail.url))
                {
                    JSONString _thumbURL = new JSONString(_embed.thumbnail.url);
                    _embedThumbnail.Add("url", _thumbURL);
                }

                if (_embed.thumbnail.height > 0)
                {
                    JSONNumber _thumbHeight = new JSONNumber(_embed.thumbnail.height);
                    _embedThumbnail.Add("height", _thumbHeight);
                }

                if (_embed.thumbnail.width > 0)
                {
                    JSONNumber _thumbWidth = new JSONNumber(_embed.thumbnail.width);
                    _embedThumbnail.Add("width", _thumbWidth);
                }

                _embedData.Add("thumbnail", _embedThumbnail);
            }

            ///Footer
            if (!_embed.footer.Equals(default(Footer)))
            {
                JSONObject _embedFooter = new JSONObject();

                if (!string.IsNullOrEmpty(_embed.footer.text))
                {
                    JSONString _footerText = new JSONString(_embed.footer.text);
                    _embedFooter.Add("text", _footerText);
                }

                if (!string.IsNullOrEmpty(_embed.footer.iconUrl))
                {
                    JSONString _footerIconUrl = new JSONString(_embed.footer.iconUrl);
                    _embedFooter.Add("icon_url", _footerIconUrl);
                }

                _embedData.Add("footer", _embedFooter);
            }

            ///Fields
            if (_embed.fields != null)
            {
                JSONArray _embedFields = new JSONArray();

                JSONObject _embedField;

                for (int i = 0; i < Mathf.Min(25, _embed.fields.Length); i++)
                {
                    _embedField = new JSONObject();

                    if (!string.IsNullOrEmpty(_embed.fields[i].name))
                    {
                        JSONString _fieldName = new JSONString(_embed.fields[i].name);
                        _embedField.Add("name", _fieldName);
                    }

                    if (!string.IsNullOrEmpty(_embed.fields[i].value))
                    {
                        JSONString _fieldValue = new JSONString(_embed.fields[i].value);
                        _embedField.Add("value", _fieldValue);
                    }

                    JSONBool _fieldInline = new JSONBool(_embed.fields[i].inline);
                    _embedField.Add("inline", _fieldInline);

                    _embedFields.Add(_embedField);
                }

                _embedData.Add("fields", _embedFields);
            }

            ///Images
            if (!_embed.image.Equals(default(Image)))
            {
                JSONObject _embedImage = new JSONObject();

                if (!string.IsNullOrEmpty(_embed.image.url))
                {
                    JSONString _imageURL = new JSONString(_embed.image.url);
                    _embedImage.Add("url", _imageURL);
                }

                if (_embed.image.width > 0)
                {
                    JSONNumber _imageWidth = new JSONNumber(_embed.image.width);
                    _embedImage.Add("width", _imageWidth);
                }

                if (_embed.image.height > 0)
                {
                    JSONNumber _imageHeight = new JSONNumber(_embed.image.height);
                    _embedImage.Add("height", _imageHeight);
                }

                _embedData.Add("image", _embedImage);
            }


            return _embedData;
        }
    }
}
