using Restpirators.Dispatcher.Handlers;
using Restpirators.Dispatcher.Models.ConfigurationModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Restpirators.Dispatcher.Services;
using Microsoft.EntityFrameworkCore;
using Restpirators.Repository.Repositories.Abstract;
using Restpirators.DataAccess.Entities;
using Restpirators.Repository.Repositories;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Restpirators.Dispatcher
{
    public class Startup
    {
        readonly string corsPolicy = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Dispatcher API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });

            services.AddDbContext<Repository.EmergencyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            // configure DI for application services
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRepository<Emergency>, Repository<Emergency>>();
            services.AddScoped<IRepository<Team>, Repository<Team>>();

            services.AddScoped<IEmergencyService, EmergencyService>();
            services.AddScoped<ITeamService, TeamService>();

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection(RabbitMqConfiguration.ConfigurationKey));
            services.AddHostedService<EmergencyReportHandler>(factory =>
            {
                var scope = factory.CreateScope();
                var options = scope.ServiceProvider.GetService<IOptions<RabbitMqConfiguration>>();
                var emergencyService = scope.ServiceProvider.GetRequiredService<IEmergencyService>();
                return new EmergencyReportHandler(options, emergencyService);
            });
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
            app.UseCors(corsPolicy);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(corsPolicy);
            });

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
        }
    }
}
