using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JsonCMS
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
            //config the db connection string 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            var connection = Configuration.GetConnectionString("CoreEFdb");
            services.AddDbContext<dbContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<IConfiguration>(Configuration);

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                                  name: "default",
                                  template: "{controller=Home}/{action=Index}/{id?}/{id2?}");

                routes.MapRoute(
                    name: "pages",
                    template: "{id}",
                        defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "pagesWiParam2",
                    template: "{id}/{id2}",
                        defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "pagesWiParam3",
                    template: "{id}/{id2}/{id3}",
                        defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
