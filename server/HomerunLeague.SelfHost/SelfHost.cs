using System;
using System.Configuration;
using Funq;
using HomerunLeague.GameEngine;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace HomerunLeague.SelfHost
{
	class SelHost
	{
		//Define the Web Services AppHost
		public class AppHost : AppSelfHostBase {
			public AppHost() 
                : base("HttpListener Self-Host", typeof(PlayerServices).Assembly) {}

			public override void Configure(Container container)
			{
                Plugins.Add(new CorsFeature());

                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(@"../../../Database/leaguedata.sqlite", SqliteDialect.Provider));
                container.RegisterAutoWiredAs<MlbBioProvider, IBioData>();
                container.RegisterAutoWiredAs<MlbStatProvider, IStatData>();
                container.Register(new ApiKeys(ConfigurationManager.AppSettings["apiKeys"].Split(',')));

                JsConfig<BioUpdateOptions>.IncludeTypeInfo = true;
                JsConfig<StatUpdateOptions>.IncludeTypeInfo = true;
                
                OrmLiteConfig.InsertFilter = (dbCmd, row) =>
                {
                    var auditRow = row as IAudit;
                    if (auditRow != null)
                        auditRow.Created = auditRow.Modified = DateTime.UtcNow;
                };

                OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
                {
                    var auditRow = row as IAudit;
                    if (auditRow != null)
                        auditRow.Modified = DateTime.UtcNow;
                };

                using (var db = container.Resolve<IDbConnectionFactory>().Open())
                {
                    db.DropAndCreateTables(typeof (Division), typeof (Player), typeof (Team), typeof (Teamate),
                        typeof (DivisionalPlayer), typeof (LeagueEvent), typeof (GameLog), typeof(SeasonTotals));

                    var divId1 = 
                    db.Insert(new Division{Name = "Ron Santo", PlayerRequirment = 2, Description = "First division test.", Year = DateTime.Now.Year, Order = 1, Active = true}, selectIdentity:true);                    

                    var divId2 =
                    db.Insert(new Division{Name = "Billy Williams", PlayerRequirment = 2, Description = "Second division test.", Year = DateTime.Now.Year, Order = 2, Active = true}, selectIdentity:true);

                    db.Insert(new Division{ Name = "Ron Cey", PlayerRequirment = 3, Description = "Third division test.", Year = DateTime.Now.Year , Order = 3, Active = true});
                    db.Insert(new Division{ Name = "Inactive Division", PlayerRequirment = 3, Description = "Inactive division test.", Year = DateTime.Now.Year , Order = 4, Active = false});

                    var pid1 = db.Insert(new Player{FirstName = "Jeff", LastName = "Mitchell", MlbId = 516770, Active = true}, selectIdentity:true);
                    var pid2 = db.Insert(new Player{FirstName = "Bone", LastName = "Jones", MlbId = 458085, Active = false}, selectIdentity:true);
                    var pid3 = db.Insert(new Player{FirstName = "Joe", LastName = "Test", MlbId = 605218}, selectIdentity:true);
                    var pid4 = db.Insert(new Player{FirstName = "Bull", LastName = "Pucky", MlbId = 471083}, selectIdentity:true);
                    var pid5 = db.Insert(new Player{FirstName = "Fifth", LastName = "Man", MlbId = 467008}, selectIdentity:true);
                    var pid6 = db.Insert(new Player { FirstName = "Sixth", LastName = "Man", MlbId = 120074 }, selectIdentity: true);
                    var pid7 = db.Insert(new Player { FirstName = "Sixth", LastName = "Man", MlbId = 547180 }, selectIdentity: true);
                    
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid1});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid2});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid3});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid7});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId2, PlayerId = (int) pid4});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId2, PlayerId = (int) pid5});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId2, PlayerId = (int) pid6});

                    db.Insert(new LeagueEvent
                    {
                        Created = DateTime.Now,
                        Action = LeagueAction.BioUpdate,
                        Options = new BioUpdateOptions { IncludeInactive = true }
                    });

                    db.Insert(new LeagueEvent
                    {
                        Created = DateTime.Now,
                        Action = LeagueAction.StatUpdate
                    });

                    var tid1 = db.Insert(new Team{ Name = "Test test 1", Year = DateTime.Now.Year });
                    tid1 = Convert.ToInt32(tid1);
                    db.Insert(new Teamate{ PlayerId = (int)pid1, TeamId = (int)tid1 });
                    db.Insert(new Teamate{ PlayerId = (int)pid2, TeamId = (int)tid1 });
                }

                var game = new LeagueEngine(container.Resolve<IBioData>(), container.Resolve<IStatData>(),
                    new Services(Container.Resolve<AdminServices>(), Container.Resolve<PlayerServices>(), Container.Resolve<StatService>()));

                game.Start();
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
