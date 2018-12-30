using ChatApp.Hubs;
using ChatApp.Hubs.FriendRequestHub;
using ChatApp.Models.Context;
using ChatApp.Models.Entities;
using ChatApp.Models.Identity;
using ChatApp.Services;
using ChatApp.Services.AuthService;
using ChatApp.Services.FriendRequestService;
using ChatApp.Services.FriendService;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IViewModelService, MainVM>();
            services.AddTransient<IFriendRequestService, FriendRequestService>();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ChatAppDb"));
            });
            services.AddDbContext<ChatContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ChatAppDb")
            ));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth";
                options.SlidingExpiration = true;
            });
            services.AddSignalR();

            services.AddMvc();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<FriendRequestHub>("/friendRequestHub");
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}