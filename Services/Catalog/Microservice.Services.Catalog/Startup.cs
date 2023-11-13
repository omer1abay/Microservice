using MassTransit;
using Microservice.Services.Catalog.Services;
using Microservice.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog
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


            //rabbitmq
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    //default port'tan ayaða kalkacak 5672 ayaða kalkacak
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest"); //bu username ve password rabbitmq tarafýndan default geliyor
                        host.Password("guest");
                    });
                });
            });
            services.AddMassTransitHostedService();
            //


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddAutoMapper(typeof(Startup)); //Startup'ýn baðlý olduðu tüm mapper'larý tarar, Profile'dan inherit alan nesneleri tarar.
            services.AddControllers(opt => {
                opt.Filters.Add(new AuthorizeFilter()); //tüm controller'lara ekledik
            });

            //appsettings okuma
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.AddSingleton<IDatabaseSettings>(sp=> {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value; //GetRequiredService ilgili servisi bulamazsa hata fýrlatýr. Zorunlu olduðu için kullanýrýz..
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice.Services.Catalog", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"]; //appsetting.json
                options.Audience = "resource_catalog";
                options.RequireHttpsMetadata = false; //https kullanmadýk.
            }
            ); //authorization için
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservice.Services.Catalog v1"));
            }

            app.UseRouting();
            app.UseAuthentication(); //authentication iþlemi
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
