﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Mush.Common.HeroChecking
{
    public class WebRequestHeroFetcher : IHeroFetcher
    {
        private static readonly Regex regex = new Regex("<h2>(.*)</h2>");

        private HttpWebRequest CreateRequest(string mush_sid, string saved_tid_sess)
        {
            var request = WebRequest.CreateHttp("http://mush.twinoid.com/chooseHero");

            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("mush_sid", mush_sid, "/", "mush.twinoid.com"));
            cookieContainer.Add(new Cookie("saved_tid_sess", saved_tid_sess, "/", "mush.twinoid.com"));

            request.CookieContainer = cookieContainer;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            return request;
        }

        public IEnumerable<string> GetCharacters(string mush_sid, string saved_tid_sess)
        {
            var request = CreateRequest(mush_sid, saved_tid_sess);
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(responseStream);
            var responseString = sr.ReadToEnd();

            return regex.Matches(responseString).Cast<Match>().Select(match => match.Groups[1].Value).ToList();
        }
    }
}