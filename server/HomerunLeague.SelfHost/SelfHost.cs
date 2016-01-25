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
                JsConfig<TeamUpdateOptions>.IncludeTypeInfo = true;

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

                    db.Save(new Setting {Name = "RegistrationOpen", Value = "true"});
                    db.Save(new Setting {Name = "BaseballYear", Value = "2015"});
                }

                var baseballYear = Container.Resolve<AdminServices>().Get(new GetSettings()).BaseballYear;

                // Add some test players via service
                container.Resolve<PlayerServices>()
                    .Post(new CreatePlayers
                    {
                        Players =
                            new List<Player>
                            {
                                new Player {MlbId = 516770, Active = true},
                                new Player {MlbId = 458085, Active = true},
                                new Player {MlbId = 605218, Active = true},
                                new Player {MlbId = 471083, Active = true},
                                new Player {MlbId = 467008, Active = true},
                                new Player {MlbId = 120074, Active = true},
                                new Player {MlbId = 547180, Active = true},
                                new Player {MlbId = 545361, Active = true}
                            }
                    });

                // Add some test divisions via service
                Container.Resolve<DivisionServices>()
			        .Post(new CreateDivisions
			        {
			            Divisions = new List<CreateDivision>
			            {
			                new CreateDivision
			                {
			                    Name = "Division 1",
			                    Description = "Description for division 1",
			                    PlayerRequirement = 2,
			                    Order = 1,
			                    Active = true,
                                PlayerIds = new List<int> {1, 2, 3}
			                },
                            new CreateDivision
                            {
                                Name = "Division 2",
                                Description = "Description for division 2",
                                PlayerRequirement = 2,
                                Order = 2,
                                Active = true,
                                PlayerIds = new List<int> {4, 5}
                            },
                            new CreateDivision
                            {
                                Name = "Division 3",
                                Description = "Description for division 3",
                                PlayerRequirement = 3,
                                Order = 3,
                                Active = true,
                                PlayerIds = new List<int> {6, 7, 8}
                            }
                        }
			        });

                // Add a team
			    Container.Resolve<TeamServices>()
			        .Post(new CreateTeam
			        {
			            Email = "Bob@yahoo.com",
                        Name = "Bob's team team",
                        PlayerIds =  new List<int> { 1, 2, 4, 5, 6, 7, 8 }
			        });

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
