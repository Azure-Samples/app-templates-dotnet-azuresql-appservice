using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.API.Data;
using Microsoft.Extensions.Hosting;

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
            if (Configuration["DBHOST"] != null)
            {
                //https://hk.saowen.com/a/c28ce380a9ef33bdacd04a1a6c1f8ca396b7caa5c5bd7ee1445bce0d609b64d5
                var host = Configuration["DBHOST"];
                var db = Configuration["DBNAME"];
                var port = Configuration["DBPORT"];
                var username = Configuration["DBUSERNAME"];
                var password = Configuration["DBPASSWORD"];

                string connStr = String.Format("Data Source={0},{1};Integrated Security=False; User ID={2};Password={3};Database={4}; Connect Timeout=30; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite; MultiSubnetFailover=False", host, port, username, password, db );
                services.AddDbContext<ContosoUniversityAPIContext>(options => options.UseSqlServer(connStr));
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