using System;
using System.Collections;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel.Types;

namespace HomerunLeague.SelfHost
{
	class SelHost
	{
		//Define the Web Services AppHost
		public class AppHost : AppSelfHostBase {
			public AppHost() 
                : base("HttpListener Self-Host", typeof(PlayerServices).Assembly) {}

			public override void Configure(Funq.Container container) 
			{ 
                Plugins.Add(new CorsFeature());

                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory("../../../Database/leaguedata.sqlite",SqliteDialect.Provider));

                using (var db = container.Resolve<IDbConnectionFactory>().Open())
                {
                    db.CreateTableIfNotExists<Player>();
                }
			}

		}

		//Run it!
		static void Main(string[] args)
		{
			var listeningOn = args.Length == 0 ? "http://*:9001/api/" : args[0];
			new AppHost()
				.Init()

				.Start(listeningOn);

			Console.WriteLine("AppHost Created at {0}, listening on {1}", 
				DateTime.Now, listeningOn);

			Console.ReadKey();
		}
	}
}
