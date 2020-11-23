using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GRC.Infrastructure;
using MediatR;
using GRC.Core.Interfaces;
using GRC.Infrastructure.Data;
using Serilog;
using GRC.Web.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace GRC.Web
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
            // ----------------- Config SQL Server Options ------------------------------
            services.AddDbContext<GRCContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //------------------ Add Dependency Inversion -------------------------------
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IStandardInterface, StandardRepository>();
            services.AddScoped<IDomainInterface, DomainRepository>();
            services.AddScoped<IControlInterface, ControlRepository>();
            services.AddScoped<IQuestionInterface, QuestionRepository>();


            //------------------ Manage Services ----------------------------------------
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeFilter()));

            //------------------ Manage Authentication ----------------------------------
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.Authority = Configuration["OpenId:Server"];

                    options.ClientId = Configuration["OpenId:ClientId"];
                    options.ClientSecret = Configuration["OpenId:ClientSecret"];
                    options.CallbackPath = "/signin-oidc";

                    options.Scope.Add("grc");
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.MapAll();
                    options.ResponseType = "code";
                    options.ResponseMode = "form_post";

                    options.UsePkce = true;

                    options.TokenValidationParameters.RoleClaimType = "Role";
                    options.TokenValidationParameters.NameClaimType = "FullName";
                    
                });

            //------------------ Manage Authorization ----------------------------------
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", p => p.RequireRole("Administrator"));
            });

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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<UserNameEnricher>();
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
