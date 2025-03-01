﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EchoBot1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //permitir requisicoes externas
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3978", "http://localhost:57799")
                                            .AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });

                //Access-Control-Allow-Origin

                options.AddPolicy(name: "Access-Control-Allow-Origin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3978", "http://localhost:57799")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            // Create the storage we'll be using for User and Conversation state.
            // (Memory is great for testing purposes - examples of implementing storage with
            // Azure Blob Storage or Cosmos DB are below).
            var storage = new MemoryStorage();

            /* AZURE BLOB STORAGE - Uncomment the code in this section to use Azure blob storage */

            // var storage = new BlobsStorage("<blob-storage-connection-string>", "bot-state");

            /* END AZURE BLOB STORAGE */

            /* COSMOSDB STORAGE - Uncomment the code in this section to use CosmosDB storage */

            // var cosmosDbStorageOptions = new CosmosDbPartitionedStorageOptions()
            // {
            //     CosmosDbEndpoint = "<endpoint-for-your-cosmosdb-instance>",
            //     AuthKey = "<your-cosmosdb-auth-key>",
            //     DatabaseId = "<your-database-id>",
            //     ContainerId = "<cosmosdb-container-id>"
            // };
            // var storage = new CosmosDbPartitionedStorage(cosmosDbStorageOptions);

            /* END COSMOSDB STORAGE */

            services.AddHttpClient().AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bots.EchoBot>();
            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();
            // Create the User state.
            services.AddSingleton<UserState>();
            // Create the Conversation state.
            services.AddSingleton<ConversationState>();

            // Create the User state passing in the storage layer.
            var userState = new UserState(storage);
            services.AddSingleton(userState);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
