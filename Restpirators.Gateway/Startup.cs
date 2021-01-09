using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Hosting;
using MMLib.Ocelot.Provider.AppConfiguration;
using Microsoft.OpenApi.Models;
using System.Linq;
using Prometheus;
using Elastic.Apm.AspNetCore;

namespace Restpirators.Gateway
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddOcelot()
                .AddAppConfiguration();
            services.AddSwaggerForOcelot(Configuration);

            services.AddControllers();
<<<<<<< HEAD

            services.AddCors(options =>
            {
                options.AddPolicy(name: "corsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

=======
>>>>>>> 0bda2999863ede1115324386ef40cd4c9b516c97
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.ResolveConflictingActions(x => x.First());
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("corsPolicy");
            app.UseElasticApm(Configuration);
            app.UseRouting();
            app.UseSwagger();
            app.UseDeveloperExceptionPage();
            app.UseMetricServer();
            app.UseRequestMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
            app.UseStaticFiles();
            //app.UseOcelot().Wait();
            app.UseSwaggerForOcelotUI(x => x.PathToSwaggerGenerator = "/swagger/docs").UseOcelot().Wait();
        }
    }
}
