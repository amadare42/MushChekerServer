using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mush.Common.Intergrations.Slack
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
            this.postUrls = new List<string>();
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

        public void SendUserInfos(IEnumerable<HeroCheckResult> infos)
        {
            var messageJson = SlackJsonHelper.GenerateMessageJson(infos);
            foreach (string url in GetUrls())
            {
                Task.Run(() => PostMessage(url, messageJson));
            }
        }

        private void PostMessage(string url, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            Uri uriResult;
            bool urlValid = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!urlValid)
            {
                Debug.WriteLine($"Trying to post on invalid url: {url}");
                return;
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            request.UserAgent = ".NET Framework Example Client";
            request.Method = "POST";
            request.Timeout = 7000;
            request.ContentLength = bytes.Length;
            try
            {
                using (Stream requeststream = request.GetRequestStream())
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