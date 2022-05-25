using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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



            await Getstream(userClient);



            await Gettweet(userClient);
        }



        static async Task Getstream(TwitterClient userClient)
        {
            Console.WriteLine("Enter Key words for the stream(you can enter multiple words with comma seperating them, for example: Greece,trucks,space) :");
            string KeyWord = Console.ReadLine();
            var stream = userClient.Streams.CreateFilteredStream();
            string[] keywords = KeyWord.Split(',');
            foreach (var x in keywords)
            {
                stream.AddTrack(x);
            }
            Console.Write("Keywords added: ");
            foreach (var x in keywords)
            {
                Console.Write(x + " ");
            }

            Console.WriteLine("\nEnter the ammount of tweets you would like to retrieve: ");
            int a = Int32.Parse(Console.ReadLine());

            List<string> tweets = new List<string>();
            int[] counter = new int[keywords.Length];

            int i = 0;
            stream.MatchingTweetReceived += (sender, eventReceived) =>
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                string tweet = eventReceived.Tweet.ToString();
                Console.WriteLine("Tweet: " + tweet);
                tweets.Add(tweet);

                foreach (var t in keywords)
                {
                    if (Regex.Match(tweet, @"\b" + t + @"\b").Index > 0)
                    {
                        counter[Array.IndexOf(keywords, t)] += 1;
                    }
                }

                Console.Write("Tweets counter: ");
                for (int j = 0; j < keywords.Length; j++)
                {
                    Console.Write(keywords[j] + "-" + counter[j] + " ");
                }
                Console.WriteLine();
                if (i == a)
                {
                    stream.Stop();
                    Console.WriteLine("Complete!");
                }


                ++i;
            };

            await stream.StartMatchingAnyConditionAsync();

        }



        static async Task Gettweet(TwitterClient userClient)
        {
            Console.WriteLine("Enter a keyword to search for the tweet: ");
            string KeyWord = Console.ReadLine();
            var tweets = await userClient.Search.SearchTweetsAsync(KeyWord);
            foreach (var x in tweets)
            {
                Console.WriteLine("Tweet: " + x);
            }
        }
    }
}