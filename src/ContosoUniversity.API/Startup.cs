using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.API.Data;
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Azure;
using Azure.Core.Extensions;

// using Azure.Identity;
// using Microsoft.EntityFrameworkCore;
// using SimpleTodo.Api;

// var builder = WebApplication.CreateBuilder(args);
// var credential = new DefaultAzureCredential();
// builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"]), credential);

// builder.Services.AddScoped<ListsRepository>();
// builder.Services.AddDbContext<TodoDb>(options =>
// {
//     var connectionString = builder.Configuration[builder.Configuration["AZURE_SQL_CONNECTION_STRING_KEY"]];
//     options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
// });

// builder.Services.AddControllers();
// builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);


namespace ContosoUniversity.API
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
            // Add framework services.
            ConfigureDatabase(services);

            services.AddControllers();

            // Register the Swagger services
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Contoso University API";
                    document.Info.Description = "A ASP.NET Core web API for Contoso University";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Contact Name",
                        Email = "contactmail@domain.com",
                        Url = "https://twitter.com/azure"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });

            // Configure the Connection String/Instrumentation key in appsettings.json
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:ConnectionString"]);
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            if (Configuration["AZURE_SQL_CONNECTION_STRING_KEY"] != null)
            {
                services.AddDbContext<ContosoUniversityAPIContext>(options => options.UseSqlServer(Configuration["AZURE_SQL_CONNECTION_STRING_KEY"]));
            }
            else
            {
                services.AddDbContext<ContosoUniversityAPIContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ContosoUniversityAPIContext")));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}