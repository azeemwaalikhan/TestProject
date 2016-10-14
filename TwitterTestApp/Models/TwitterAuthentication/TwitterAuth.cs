using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TwitterTestApp.Models.TwitterAuthentication
{
    public class TwitterAuth
    {
        public string OAuthConsumerKey = ConfigurationManager.AppSettings["OAuthConsumerKey"];
        public string OAuthConsumerSecret = ConfigurationManager.AppSettings["OAuthConsumerSecret"];
        public string AccessToken = ConfigurationManager.AppSettings["AccessToken"];
        public string RequestTokenUrl = ConfigurationManager.AppSettings["RequestTokenUrl"];
        public string NumberOfTwitts = ConfigurationManager.AppSettings["NumberOfTwitts"];
        public string RequestUserTimeLineUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json?count={0}&screen_name={1}&trim_user=1&exclude_replies=1";//ConfigurationManager.AppSettings["RequestUserTimeLineUrl"];
        
        public async Task<IEnumerable<dynamic>> GetTwitts(string userName, string accessToken = null)
        {
            if (accessToken == null)
            {
                //accessToken = await GetAccessToken();
                accessToken = AccessToken;
            }

            var requestUserTimeline = new HttpRequestMessage(HttpMethod.Get, string.Format(RequestUserTimeLineUrl, int.Parse(NumberOfTwitts), userName));
            requestUserTimeline.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            var responseUserTimeLine = await httpClient.SendAsync(requestUserTimeline).ConfigureAwait(false);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await responseUserTimeLine.Content.ReadAsStringAsync());
            var enumerableTwitts = (json as IEnumerable<dynamic>);

            return enumerableTwitts == null ? null : enumerableTwitts.Select(t => t);
        }

        public async Task<string> GetAccessToken()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, RequestTokenUrl);
            var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }
    }
}