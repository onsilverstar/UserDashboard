using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserDashboard.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore.SqlServer;

namespace UserDashboard
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
            services.AddSession();
            services.AddAuthentication();
            services.AddDbContext<MyContext>(options=>options.UseMySql(Configuration["DBInfo:ConnectionString"]));
            services.AddControllersWithViews();
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MyContext>()
                .AddDefaultTokenProviders();
        }
    private async Task InitializeRoles(IServiceProvider serviceProvider)
    {
    // Array of Roles to create
    string[] RolesToCreate = new string[] {"Level1", "Level2", "Level3"};
    RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach( string role in RolesToCreate )
    {
        // If a Role doesn't already exist, create it
        if( !await roleManager.RoleExistsAsync(role) )
        {
            await roleManager.CreateAsync( new IdentityRole(role) );
        }
    }
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
            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
