using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda
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
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "voucher",
                    pattern: "voucher/{id?}",
                    defaults: new { controller = "Voucher", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "promocode",
                    pattern: "promocode/{id?}",
                    defaults: new { controller = "Promo", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "register/{id?}",
                    defaults: new { controller = "register", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "r",
                    pattern: "r/{id?}",
                    defaults: new { controller = "register", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "qr",
                    pattern: "qr/{id?}",
                    defaults: new { controller = "home", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "admin/{id?}",
                    defaults: new { controller = "admin", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "super",
                    pattern: "super/{id?}",
                    defaults: new { controller = "super", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "{id?}",
                    defaults: new { controller = "home", action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
