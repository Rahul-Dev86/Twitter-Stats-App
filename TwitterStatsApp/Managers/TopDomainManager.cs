using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Exceptions;
using Tweetinvi.Models.V2;
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

        public string getTopDomain(TweetContextAnnotationV2[] annotations, int count, ConcurrentDictionary<string, int> dictDomain)
        {
            string msg = string.Empty;
            try
            {
                if (annotations != null)
                {
                    foreach (var domain in annotations)
                    {
                        if (dictDomain.ContainsKey(domain.Domain.Name))
                            dictDomain[domain.Domain.Name] = dictDomain[domain.Domain.Name] + 1;
                        else
                            dictDomain.TryAdd(domain.Domain.Name, 1);
                    }

                    if (dictDomain.Count > 0)
                    {
                        KeyValuePair<string, int> topDomain = dictDomain.Aggregate((l, r) => l.Value > r.Value ? l : r);
                        msg = topDomain.Key + "! found in " + topDomain.Value + " tweets out of " + count+ "                                                ";
                    }
                    else
                        msg = "No domain(s) found in " + count + " tweets                                                ";
                }

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
            return msg;
        }
    }
}
