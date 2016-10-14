using System;
using System.Collections.Generic;
using System.Globalization;
using TwitterTestApp.Models.TwitterAuthentication;

namespace TwitterTestApp.Models.TwitterFactory
{
    public class TwitterViewModelFactory
    {
        public TwitterCollectionModel CreateViewModel(string username)
        {
            var twitter = new TwitterAuth();
            var userTwitts = twitter.GetTwitts(username).Result;
            var twitts = new List<TwitterViewModel>();
            const string format = "ddd MMM dd HH:mm:ss zzz yyyy";
            
            if (userTwitts != null)
            {
                foreach (var item in userTwitts)
                {
                    var dateString = (string)(item["created_at"].ToString());
                    DateTime date;
                    DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                    twitts.Add(new TwitterViewModel
                    {
                        Twitt = (string)(item["text"].ToString()),
                        CreatedDate = date
                    });
                }
            }

            return new TwitterCollectionModel
            {
                UserName = username,
                Twitts = twitts
            };
        }
    }
}