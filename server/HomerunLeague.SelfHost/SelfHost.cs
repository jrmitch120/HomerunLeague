using System;
using System.Collections;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel;
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
                container.Register<Paging>(new Paging(RequestContext.Instance.ToAbsoluteUri));

                using (var db = container.Resolve<IDbConnectionFactory>().Open())
                {
                    //db.CreateTableIfNotExists<Player>();
                    db.DropAndCreateTables(new []{typeof(Division), typeof(Player)});

                    db.Insert(new Division{Name = "Ron Santo", PlayerRequirment = 2, Description = "First division test."}, selectIdentity:true);
                    db.Insert(new Division{Name = "Billy Williams", PlayerRequirment = 2, Description = "Second division test."}, selectIdentity:true);
                    db.Insert(new Division{Name = "Ron Cey", PlayerRequirment = 3, Description = "Third division test."}, selectIdentity:true);

                    db.Insert(new Player{FirstName = "Jeff", LastName = "Mitchell", DivisionId = 1});
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
