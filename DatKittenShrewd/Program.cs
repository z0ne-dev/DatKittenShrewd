// <copyright file="Program.cs" company="z0ne">
// Copyright (c) z0ne. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DisCatSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ThatKittenShrewd
{
    /// <summary>
    /// The program entry point.
    /// </summary>
    internal static class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(
                (context, services) =>
                    {
                        // configuration
                        var applicationSettings = context.Configuration.Get<ApplicationSettings>();
                        var validationContext = new ValidationContext(applicationSettings);
                        Validator.ValidateObject(applicationSettings, validationContext);
                        _ = services.AddSingleton(applicationSettings).AddSingleton(provider => new DiscordShardedClient(new DiscordConfiguration
                            {
                                LoggerFactory = provider.GetRequiredService<ILoggerFactory>(),
                                Token = applicationSettings.DiscordToken,
                            })).AddHostedService<BotService>();
                    }).UseConsoleLifetime();
        }

#pragma warning disable ASA001 // Entry is always called Main
        private static Task Main(string[] args)
#pragma warning restore ASA001
        {
            return CreateHostBuilder(args).Build().RunAsync();
        }
    }
}
