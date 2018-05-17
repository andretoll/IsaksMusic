using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IsaksMusic.Data;
using IsaksMusic.Services;
using Microsoft.AspNetCore.Routing;

namespace IsaksMusic
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IsaksMusicConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            /* Lower case URLs */
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            /* Authorization policies */
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            });

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin", "RequireAdminRole");
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            services.AddMvc().AddRazorOptions(options =>
            {
                options.PageViewLocationFormats.Add("/Pages/Partials/{0}.cshtml");
            });

            /* Configure Identity options */
            services.Configure<IdentityOptions>(options =>
            {
                /* Password */
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                /* User */  
                options.User.RequireUniqueEmail = true;

                /* Lockout */
                options.Lockout.MaxFailedAccessAttempts = 5;
            });            

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseStaticFiles();
            app.UseStatusCodePages();

            /* Perform middleware for static files and end processing */
            app.MapWhen(
                context => context.Request.Path.HasValue && context.Request.Path.Value.Contains('.'),
                appbuilder =>
                {
                    app.UseStaticFiles();
                }
                );

            /* Perform middleware for custom 404 page */
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Error404";
                    await next();
                }
            });

            app.UseAuthentication();

            app.UseMvc();

            //CreateUserRoles(services).Wait();
        }

        /* Create new user roles */
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


            IdentityResult roleResult;
            /* Add Admin role */ 
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");

            if (!roleCheck)
            {
                /* Seed role to database */ 
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            /* Assign role to provided user */  
            ApplicationUser user = await UserManager.FindByEmailAsync("isaktoll@live.se");
            var User = new ApplicationUser();
            await UserManager.AddToRoleAsync(user, "Admin");

        }
    }
}
