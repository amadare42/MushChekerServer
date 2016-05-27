using Mush.Common.Services.Fetching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mush.Common.Services.Notification.NotificationTargets.Slack
{
    public class SlackNotificationTarget : INotificationTarget
    {
        private readonly object lockRoot = new object();
        private readonly List<string> postUrls;

        public SlackNotificationTarget(IEnumerable<string> postUrls)
        {
            this.postUrls = new List<string>(postUrls);
        }

        public SlackNotificationTarget()
        {
            postUrls = new List<string>();
        }

        public void Notify(IEnumerable<HeroFetchResult> infos)
        {
            var infosList = infos.ToList();
            var commonNames = CommonNamesHelper.GetCommonCharacterNames(infosList);
            var messageJson = SlackJsonHelper.GenerateMessageJson(infosList, commonNames);

            foreach (var url in GetUrls())
            {
                Task.Run(() => PostMessage(url, messageJson));
            }
        }

        public void AddUrl(string url)
        {
            lock (lockRoot)
            {
                postUrls.Add(url);
            }
        }

        public void RemoveUrl(string url)
        {
            lock (lockRoot)
            {
                postUrls.Remove(url);
            }
        }

        public List<string> GetUrls()
        {
            lock (lockRoot)
            {
                return new List<string>(postUrls);
            }
        }

        private void PostMessage(string url, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            Uri uriResult;
            var urlValid = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                           && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!urlValid)
            {
                Debug.WriteLine($"Trying to post on invalid url: {url}");
                return;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Timeout = 7000;
            request.ContentLength = bytes.Length;
            try
            {
                using (var requeststream = request.GetRequestStream())
                {
                    requeststream.Write(bytes, 0, bytes.Length);
                    requeststream.Close();
                }
                var response = request.GetResponse();
                Debug.WriteLine($"Posted to {url}:\r\n{response}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during posting to {url}:\r\n{ex}");
            }
        }
    }
}