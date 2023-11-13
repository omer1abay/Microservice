using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.FakePayment
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
            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            //eðer payload'da sadece okuma yetkisi bekleseydik bir policy daha oluþturacaktýk.
            //var abc = new AuthorizationPolicyBuilder().RequireClaim("scope","discount_read"); //bu policy'i controller'a verecektik

            //sub, nameidentifier olarak gelmesi
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //artýk sub olarak gelecek map'lenmeden

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"]; //appsetting.json
                options.Audience = "resource_fake_payment";
                options.RequireHttpsMetadata = false; //https kullanmadýk.
            }
            ); //authorization için

            services.AddControllers(opt => {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice.Services.FakePayment", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservice.Services.FakePayment v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
