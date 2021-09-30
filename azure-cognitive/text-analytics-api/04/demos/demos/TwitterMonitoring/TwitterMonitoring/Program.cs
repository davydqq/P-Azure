using System.Reactive.Linq;
using TwitterHelper;

namespace TwitterMonitoring
{
	class Program
	{
		static void Main(string[] args)
		{
			var twitterObserver = new TwitterObserver();
			var twitterConfig = new TwitterConfig(Constants.TWITTER_CONSUMER_KEY, Constants.TWITTER_CONSUMER_SECRET, Constants.TWITTER_ACCESS_TOKEN, Constants.TWITTER_TOKEN_SECRET);
			Services.StreamStatuses(twitterConfig, "Microsoft").ToObservable().Subscribe(twitterObserver);
			
		}
	}
}
