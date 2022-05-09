using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Tweetornyc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var appCredentials = new ConsumerOnlyCredentials("yjqoED31on4ZQL6VXtLI7BpX6", "fPRgFnxVqLV6znax3IvjjIVdfAodB5guGyM73AWd3KTxP4ZJ3Y")
            {
                BearerToken = "AAAAAAAAAAAAAAAAAAAAALfAagEAAAAArq15M%2FS9GBjFELBHHRxAXbdfJEE%3DK7kABEuy5wrIeYhRYgVw9JBbOSosOPnGbofehszSjuW9aKyqPk"
            };

            var appClient = new TwitterClient(appCredentials);

            var userCredentials = new TwitterCredentials("yjqoED31on4ZQL6VXtLI7BpX6", "fPRgFnxVqLV6znax3IvjjIVdfAodB5guGyM73AWd3KTxP4ZJ3Y", "1507042722775023617-w2CcbIZSLwcOHUNtupk1YX0NhpDUcQ", "FKXrSCIka2Cs7jsqRHSJLoLFLAUHwgOJTJvMdpbFQhp4I");
            var userClient = new TwitterClient(userCredentials);

            /*            var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                        sampleStreamV2.TweetReceived += (sender, args) =>
                        {
                            System.Console.WriteLine(args.Tweet.Text);
                        };

                        await sampleStreamV2.StartAsync();*/

            var stream = userClient.Streams.CreateFilteredStream();
            stream.AddTrack("UK");

            List<string> tweets = new List<string>();

            int i = 0;
            stream.MatchingTweetReceived += (sender, eventReceived) =>
            {
                //Console.WriteLine(eventReceived.Tweet);
                tweets.Add(eventReceived.Tweet.ToString());
                if(i == 20)
                {
                    stream.Stop();
                    Console.WriteLine("Complete!");
                }

                ++i;
            };

            await stream.StartMatchingAnyConditionAsync();


            foreach(var t in tweets)
            {
                Console.WriteLine("Tweet: "+t);
            }

            Console.WriteLine("Total tweets: " + tweets.Count);
            /*var tweets = await userClient.Search.SearchTweetsAsync("2022");
            List<string> output = new List<string>();
            foreach (var x in tweets)
            {
                Console.WriteLine(x);
            }*/


            /* parto */
        }
    }
}
