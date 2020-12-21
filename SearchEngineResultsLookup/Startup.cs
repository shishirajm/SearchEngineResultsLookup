using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SearchEngineResultsLookup.Parsers;
using SearchEngineResultsLookup.Providers;
using SearchEngineResultsLookup.Services;

namespace SearchEngineResultsLookup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());

            services.AddControllersWithViews();
            services.AddHttpClient();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "yarn-start");
                }
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Current thing could have been done with Microsoft DI
            // I feel Keyed services is neater with Autofac hence the choice

            builder.RegisterType<GoogleSearchEngineProvider>().Keyed<ISearchEngineProvider>(SearchProviders.Google).SingleInstance();
            builder.RegisterType<BingSearchEngineProvider>().Keyed<ISearchEngineProvider>(SearchProviders.Bing).SingleInstance();

            builder.RegisterType<GoogleParserConfiguration>().Keyed<IParserConfiguration>(SearchProviders.Google).SingleInstance();
            builder.RegisterType<BingParserConfiguration>().Keyed<IParserConfiguration>(SearchProviders.Bing).SingleInstance();

            builder.RegisterType<SearchEngineService>().As<ISearchEngineService>().SingleInstance();
            builder.RegisterType<Parser>().As<IParser>().SingleInstance();
        }
    }
}
