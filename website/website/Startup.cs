using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using website.Models;
using website.Models.Interfaces;

namespace website
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
            services.AddDbContext<UMDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("UMDB")));
            services.AddTransient<IUMDBRepository, UMDBRepository>();
            //Identity
            services.AddIdentity<User, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<UMDBContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Profile/Login");
            //Identity

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Identity
            app.UseAuthentication();
            //Identity
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Search}/{action=Query}/{id?}");
            });
        }
    }
}
