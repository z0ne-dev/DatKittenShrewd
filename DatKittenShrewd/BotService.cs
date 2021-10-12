// <copyright file="BotService.cs" company="z0ne">
// Copyright (c) z0ne. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ThatKittenShrewd
{
    /// <summary>
    ///     The bot service.
    /// </summary>
    public class BotService : BackgroundService
    {
        private readonly IHostApplicationLifetime lifetime;

        private readonly ILogger<BotService> logger;

        private readonly IServiceProvider serviceProvider;

        private readonly DiscordShardedClient shardedClient;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BotService" /> class.
        /// </summary>
        /// <param name="lifetime">The <see cref="IHostApplicationLifetime" />.</param>
        /// <param name="logger">The <see cref="ILogger{BotService}" />.</param>
        /// <param name="shardedClient">The <see cref="DiscordShardedClient" />.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" />.</param>
        public BotService(
            IHostApplicationLifetime lifetime,
            ILogger<BotService> logger,
            DiscordShardedClient shardedClient,
            IServiceProvider serviceProvider)
        {
            this.lifetime = lifetime;
            this.logger = logger;
            this.shardedClient = shardedClient;
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // immediatly switch to async context.
            await Task.Yield();

            try
            {
                await RunAsync(stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Unhandled exception occured");
                lifetime.StopApplication();
                throw;
            }
        }

        private async Task RunAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting...");

            // ReSharper disable once UnusedVariable
            var commands = await shardedClient.UseApplicationCommandsAsync(
                                                  new ApplicationCommandsConfiguration
                                                      {
                                                          Services = serviceProvider,
                                                      })
                                              .ConfigureAwait(continueOnCapturedContext: false);

            await shardedClient.StartAsync().ConfigureAwait(continueOnCapturedContext: false);

            stoppingToken.Register(() => { shardedClient.StopAsync(); });

            await Task.Delay(millisecondsDelay: 0, stoppingToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
