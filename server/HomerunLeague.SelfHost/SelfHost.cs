using System;
using System.IO;
using System.Reflection;
using Funq;
using HomerunLeague.GameEngine;
using HomerunLeague.GameEngine.Bios;
using HomerunLeague.GameEngine.Stats;
using HomerunLeague.ServiceInterface;
using HomerunLeague.ServiceInterface.Authentication;
using HomerunLeague.ServiceInterface.Validation;
using HomerunLeague.ServiceModel.Types;
using Mono.Unix;
using Mono.Unix.Native;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using ServiceStack.Validation;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;

namespace HomerunLeague.SelfHost
{
    class SelHost
    {
        //Define the Web Services AppHost
        public class AppHost : AppSelfHostBase
        {
            public AppHost()
                : base("Homerun League Self-Hosted Server", typeof(PlayerServices).Assembly)
            {
            }

            public override void Configure(Container container)
            {
                var settings = new AppSettings();

                Plugins.Add(new CorsFeature());
                Plugins.Add(new ValidationFeature());
                Plugins.Add(new PostmanFeature());

                // SQLite
                // Build path for portability between win/linux
                var dbPath =
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        .CombineWith("data")
                        .CombineWith("leaguedata.sqlite");

                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(dbPath, SqliteDialect.Provider));

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

                JsConfig.EmitCamelCaseNames = true;

                OrmLiteConfig.InsertFilter = (dbCmd, row) =>
                {
                    var auditRow = row as IAudit;
                    if (auditRow != null)
                        auditRow.Created = auditRow.Modified = DateTime.UtcNow;
                };

                // Update filter for IAudit
                OrmLiteConfig.UpdateFilter = (dbCmd, row) =>
                {
                    var auditRow = row as IAudit;
                    if (auditRow != null)
                        auditRow.Modified = DateTime.UtcNow;
                };

                // Setup tables
                using (var db = container.Resolve<IDbConnectionFactory>().Open())
                {
                    db.CreateTableIfNotExists(
                        typeof(Team), typeof(Player), typeof(Division), typeof(Teamate),
                        typeof(DivisionalPlayer), typeof(LeagueEvent), typeof(GameLog), typeof(PlayerTotals),
                        typeof(TeamTotals), typeof(Setting)
                    );
                }

                // Log any exception coming out of the services.
                ServiceExceptionHandlers.Add((httpReq, request, exception) =>
                {
                    LogManager.GetLogger(request.GetType()).Error($"{request.ToJson()}|{exception}");

                    return null; // continues default error handling
                });

                var game = container.Resolve<LeagueEngine>();

                game.Start();
            }
        }

        //Run it!
        static void Main(string[] args)
        {
//	        var port = args.Length > 0 ? args[0] : "9001";
//	        var listeningOn = $"http://*:{port}/api/";
            var listeningOn = $"http://*:9001/api/";

            LogManager.LogFactory = new NLogFactory();

            new AppHost()
                .Init()
                .Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}",
                DateTime.Now, listeningOn);

            if (IsRunningOnMono())
            {
                var terminationSignals = GetUnixTerminationSignals();
                UnixSignal.WaitAny(terminationSignals);
            }
            else
            {
                Console.ReadLine();
            }
        }

        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        private static UnixSignal[] GetUnixTerminationSignals()
        {
            return new[]
            {
                new UnixSignal(Signum.SIGINT),
                new UnixSignal(Signum.SIGTERM),
                new UnixSignal(Signum.SIGQUIT),
                new UnixSignal(Signum.SIGHUP)
            };
        }
    }
}