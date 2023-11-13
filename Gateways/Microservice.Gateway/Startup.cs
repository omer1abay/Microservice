using Microservice.Gateway.DelegateHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Gateway
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<TokenExchangeDelegateHandler>();
            services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options => {
                options.Authority = configuration["IdentityServerURL"]; //appsetting.json
                options.Audience = "resource_gateway";
                options.RequireHttpsMetadata = false; //https kullanmadýk.
            });

            services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>(); //ekledik.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //middleware
            await app.UseOcelot(); //async fonk.
        }
    }
}
