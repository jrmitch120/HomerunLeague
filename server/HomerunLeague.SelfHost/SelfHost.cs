using System;
using System.Collections.Generic;
using Funq;

using HomerunLeague.GameEngine;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceInterface.Authentication;
using HomerunLeague.ServiceInterface.Validation;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;

using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Validation;

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
                Plugins.Add(new ValidationFeature());

                // SqlServer
                //container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(@"Data Source=.\SQLEXPRESS;Initial Catalog=HomerunLeague;Integrated Security=True", SqlServer2012Dialect.Provider));

                // SQLite
                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(@"../../../Database/leaguedata.sqlite", SqliteDialect.Provider));

                container.RegisterAutoWiredAs<MlbBioProvider, IBioData>(); // Bios from MLB
                container.RegisterAutoWiredAs<MlbStatProvider, IStatData>(); // Stats from MLB
                container.RegisterAutoWiredAs<ApiKeys, IKeys>(); // API Keys
                container.RegisterAutoWiredAs<FunqServiceFactory, IServiceFactory>(); // Service Factory
			    container.RegisterAutoWired<Services>();
                container.RegisterAutoWired<LeagueEngine>();
                
                // Validators
                container.RegisterValidators(typeof(TeamValidator).Assembly);

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
                    db.DropTables(typeof(TeamTotals), typeof(Teamate), typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals), typeof(Team), typeof(Player), typeof(Division), typeof(Setting));
                    db.CreateTables(true,typeof(Team), typeof(Player), typeof(Division), typeof(Teamate), typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals), typeof(TeamTotals), typeof(Setting));

                    var baseballYear = Container.Resolve<AdminServices>().Get(new GetSettings()).BaseballYear;

                    var divId1 = 
                    db.Insert(new Division{Name = "Ron Santo", PlayerRequirment = 3, Description = "First division test.", Year = baseballYear, Order = 1, Active = true}, selectIdentity:true);                    

                    var divId2 =
                    db.Insert(new Division{Name = "Billy Williams", PlayerRequirment = 2, Description = "Second division test.", Year = baseballYear, Order = 2, Active = true}, selectIdentity:true);

                    var divId3 = 
                    db.Insert(new Division{ Name = "Ron Cey", PlayerRequirment = 1, Description = "Third division test.", Year = baseballYear, Order = 3, Active = true}, selectIdentity: true);

                    db.Insert(new Division{ Name = "Inactive Division", PlayerRequirment = 3, Description = "Inactive division test.", Year = baseballYear, Order = 4, Active = false});

                    var pid1 = db.Insert(new Player{FirstName = "Jeff", LastName = "Mitchell", MlbId = 516770, Active = true}, selectIdentity:true);
                    var pid2 = db.Insert(new Player{FirstName = "Bone", LastName = "Jones", MlbId = 458085, Active = true}, selectIdentity:true);
                    var pid3 = db.Insert(new Player{FirstName = "Joe", LastName = "Test", MlbId = 605218, Active = true }, selectIdentity:true);
                    var pid4 = db.Insert(new Player{FirstName = "Bull", LastName = "Pucky", MlbId = 471083, Active = true }, selectIdentity:true);
                    var pid5 = db.Insert(new Player{FirstName = "Fifth", LastName = "Man", MlbId = 467008, Active = true }, selectIdentity:true);
                    var pid6 = db.Insert(new Player {FirstName = "Sixth", LastName = "Man", MlbId = 120074, Active = true }, selectIdentity: true);
                    var pid7 = db.Insert(new Player {FirstName = "Sixth", LastName = "Man", MlbId = 547180, Active = true }, selectIdentity: true);
                    
                    container.Resolve<PlayerServices>()
                        .Post(new CreatePlayers { Players = new List<Player> { new Player { MlbId = 545361, Active = true } } });

                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid1});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid2});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId1, PlayerId = (int) pid3});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId2, PlayerId = (int) pid4});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId2, PlayerId = (int) pid5});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId3, PlayerId = (int) pid6});
                    db.Insert(new DivisionalPlayer {DivisionId = (int) divId3, PlayerId = (int) pid7});

                    //Container.Resolve<TeamServices>()
                    //    .Post(new CreateTeam
                    //    {
                    //        Email = "bob@yahoo.com",
                    //        Name = "Bob Rocks",
                    //        Year = baseballYear,
                    //        PlayerIds =
                    //            new List<int> { (int)pid1, (int)pid2, (int)pid3, (int)pid4, (int)pid5, (int)pid6 }
                    //    });
                }

			    var game = container.Resolve<LeagueEngine>();

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
