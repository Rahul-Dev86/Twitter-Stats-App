using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class TopDomainManager : ITopDomainManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TopDomainManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<KeyValuePair<string, int>> getTopDomainAsync(TwitterClient appClient, int streamLimit)
        {
            KeyValuePair<string, int> topDomain = new KeyValuePair<string, int>();            
            try
            {
                var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                int i = 0;
                Dictionary<string, int> dictDomain = new Dictionary<string, int>();
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    i++;
                    if (args.Tweet.ContextAnnotations != null)
                    {
                        foreach (var domain in args.Tweet.ContextAnnotations)
                        {
                            if (dictDomain.ContainsKey(domain.Domain.Name))
                                dictDomain[domain.Domain.Name] = dictDomain[domain.Domain.Name] + 1;
                            else
                                dictDomain.Add(domain.Domain.Name, 1);
                        }
                    }
                    if (i == streamLimit)
                    {
                        sampleStreamV2.StopStream();
                        if (dictDomain.Count > 0)
                        {
                            topDomain = dictDomain.Aggregate((l, r) => l.Value > r.Value ? l : r);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("'" + topDomain.Key + "' is the top Domain!" + " It's found in " + topDomain.Value + " tweets out of " + i);
                            Console.ResetColor();
                        }
                        else
                            Console.WriteLine("No domain(s) found in " + i + " tweets");
                    }
                };
                await sampleStreamV2.StartAsync();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet url proportion");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet url proportion");
                Console.ResetColor();
            }
            return topDomain;
        }
    }
}
