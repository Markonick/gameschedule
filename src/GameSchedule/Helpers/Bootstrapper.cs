using GameSchedule.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Nancy;
using Nancy.TinyIoc;
using RestSharp;
using RestSharp.Authenticators;

namespace GameSchedule.Helpers
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public IConfigurationRoot Configuration;
        public static TinyIoCContainer Container;

        public Bootstrapper(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(new RootPathProvider().GetRootPath())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            var apiSettings = Configuration.GetSection("ApiSettings");
            var apiBaseUrl = apiSettings.GetValue<string>("MYSPORTSFEEDS_BASEURL");
            var apiUsername = Configuration.GetValue<string>("MYSPORTSFEEDS_USERNAME");
            var apiPassword = Configuration.GetValue<string>("MYSPORTSFEEDS_PASSWORD");
            var seasonName = apiSettings.GetValue<string>("SEASON_NAME");
            var gameScheduleUrl = apiSettings.GetValue<string>("GAMESCHEDULE_URL");
            var format = apiSettings.GetValue<string>("FORMAT");

            var databaseConfiguration = Configuration.GetSection("DatabaseConfiguration");
            var connectionString = databaseConfiguration.GetValue<string>("CONNECTION_STRING");
            var databaseName = databaseConfiguration.GetValue<string>("DATABASE_NAME");
            var collectionName = databaseConfiguration.GetValue<string>("COLLECTION_NAME");

            base.ConfigureApplicationContainer(container);
            var logger = new LoggerFactory();
            var mongoClient = new MongoClient(connectionString);
            var restClient = new RestClient(apiBaseUrl){Authenticator = new HttpBasicAuthenticator(apiUsername, apiPassword) };

            container.Register<IWebApiConsumer, WebApiConsumer>(new WebApiConsumer(restClient, gameScheduleUrl, format, seasonName));
            var games = container.Resolve<IWebApiConsumer>().GetResponse();
            
            container.Register<IGameScheduleRepository, GameScheduleRepository>(new GameScheduleRepository(mongoClient, databaseName, logger, collectionName));
            var repo = container.Resolve<IGameScheduleRepository>();
            repo.StoreGameSchedule(games);
            container.Register<IConfiguration>(Configuration);

            Container = container;
        }
    }
}
