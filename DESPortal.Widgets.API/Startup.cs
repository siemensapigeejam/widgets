using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DESPortal.Widgets.API.Models.SwaggerConfig;
using DESPortal.Widgets.Infrastructure.Configuration;
using DESPortal.Widgets.Infrastructure.DataAccess.DESPortal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace DESPortal.Widgets.API
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
            services.AddDbContext<DESPortalDBContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddWidgetDiService();
            services.AddDashboardDiService();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Widget API", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Widget API V1");
            });

            app.UseCors(
               options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new string[] { "*" });
                context.Response.Headers.Add("Access-Control-Allow-Methods", new string[] { "GET, POST, PUT, PATCH, DELETE, OPTIONS" });
                context.Response.Headers.Add("Access-Control-Allow-Headers", new string[] { "*" });
                await next();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
