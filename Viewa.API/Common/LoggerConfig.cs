using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;

namespace Viewa.Common
{
    public static class LoggerConfig
    {
        public static void RegisterLogger()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            string appSettingsEnvFile = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

            if (File.Exists(appSettingsEnvFile))
                configurationBuilder = configurationBuilder.AddJsonFile(appSettingsEnvFile);

            IConfigurationRoot configuration = configurationBuilder.Build();

            LoggerConfiguration logConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration);

            Log.Logger = logConfig.CreateLogger();
#if DEBUG
            var file = File.Create("C:\\Temp\\SeriLog.txt");
            StreamWriter writer = new StreamWriter(file);
            SelfLog.Enable(writer);
#endif
        }
    }
}
