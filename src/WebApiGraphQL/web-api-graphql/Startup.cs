namespace WebApiGraphQL
{
    using System;
    using Extensions;
    using GraphQL.Types;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Middleware.GraphiQl;
    using Middleware.GraphQl;
    using Queries;
    using Repositories;
    using Schema;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IBookRepository, BookRepository>();

            // Implement later
            //services.AddDistributedRedisCache(option =>
            //{
            //    option.Configuration = "localhost";
            //    option.InstanceName = "master";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseGraphQl(new GraphQlOptions
            {
                GraphQlPath = "/graphql",
                Schema = new GraphQL.Types.Schema
                {
                    Query = new BooksQuery(app.ApplicationServices.GetService<IBookRepository>()),
                    Mutation = new BooksMutation(app.ApplicationServices.GetService<IBookRepository>())
                }
            });

            app.UseGraphiQL(new GraphiQlOptionsV2
            {
                GraphiQlPath = "/graphiql"
            });

            app.UseMvc();
        }
    }
}