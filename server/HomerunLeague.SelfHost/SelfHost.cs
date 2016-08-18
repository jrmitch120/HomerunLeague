using System;
using Funq;

using HomerunLeague.GameEngine;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceInterface.Authentication;
using HomerunLeague.ServiceInterface.Validation;
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
			    Plugins.Add(new PostmanFeature());

                // SqlServer
                //container.Register<IDbConnectionFactory>(
                //    new OrmLiteConnectionFactory(
                //        @"Data Source=.\SQLEXPRESS;Initial Catalog=HomerunLeague;Integrated Security=True",
                //        SqlServer2012Dialect.Provider));


                // SQLite
                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(@"../../Database/leaguedata.sqlite",
                    SqliteDialect.Provider));

                container.Register<IKeys>(new ApiKeys(AppSettings.GetList("apiKeys"))); // API Keys

                container.RegisterAutoWiredAs<MlbBioProvider, IBioData>(); // Bios from MLB
                container.RegisterAutoWiredAs<MlbStatProvider, IStatData>(); // Stats from MLB
                container.RegisterAutoWiredAs<FunqServiceFactory, IServiceFactory>(); // Service Factory
			    container.RegisterAutoWired<Services>();
                container.RegisterAutoWired<LeagueEngine>();
                
                // Validators
                container.RegisterValidators(typeof(TeamValidator).Assembly);

                // Include type info for update options.
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
                    db.CreateTableIfNotExists(typeof(Team), typeof(Player), typeof(Division), typeof(Teamate), typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals), typeof(TeamTotals), typeof(Setting));

                    //db.DropTables(typeof(TeamTotals), typeof(Teamate), typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals), typeof(Team), typeof(Player), typeof(Division), typeof(Setting));
                    //db.CreateTables(true,typeof(Team), typeof(Player), typeof(Division), typeof(Teamate), typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals), typeof(TeamTotals), typeof(Setting));

                    //db.Save(new Setting { Name = "RegistrationOpen", Value = "true" });
                    //db.Save(new Setting { Name = "BaseballYear", Value = "2016" });
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
