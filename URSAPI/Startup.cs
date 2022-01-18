using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using URSAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace URSAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string ConnectionString
        {
            get;
            private set;
        }
        public static string timezone
        {
            get;
            private set;
        }
        public IConfigurationRoot Configurationroot
        {
            get;
            set;
        }

        public IConfiguration Configuration { get; }
        public static Int32 Orgid { get; set; }
        public static Int32 Userid { get; set; }
        public static string URL { get; set; }

        public class TestContext : DbContext
        {
            public TestContext(DbContextOptions<TestContext> options) : base(options) { }
        }
        // This method gets called by the runtime. Use this method to add services to the container.

       
        public IConfigurationRoot Configuration1 { get; set; }

        public Startup(IHostingEnvironment _environment)
        {
            Configuration1 = new ConfigurationBuilder()
                            .SetBasePath(_environment.ContentRootPath)
                            .AddJsonFile("appsettings.json")
                            .Build();
        }
   


public void ConfigureServices(IServiceCollection services)
        {
            Orgid = Configuration.GetValue<Int32>("Orgid");
            Userid = Configuration.GetValue<Int32>("Userid");
            URL = Configuration.GetValue<string>("URL");
            timezone = Configuration.GetValue<string>("TimeZone");
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //string conStr = this.Configuration.GetConnectionString("SQLDatabase");
            //services.AddDbContext<dbURSContext>(options => options.UseSqlServer(conStr));

            // var connectionString = Configuration.GetValue<string>("Myconnection");
            ConnectionString = Configuration.GetValue<string>("Myconnection");
            // var connectionString = Configuration1.GetConnectionString("Myconnection");

            //services.AddDbContext<dbURSContext>(
            //    options => options.UseSqlServer(connectionString)
            //);


            // services.AddControllers();
            //services.Configure<DbConnection>(Configuration.GetSection("Myconnection"));


            //services.AddDbContext<dbURSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Myconnection")));


        //    services.AddDbContext<dbURSContext>(options =>
        //options.UseSqlServer(Configuration.GetConnectionString(connectionString)));
            //For Authendication purpose
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero //the default for this setting is 5 minutes
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddMvc();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //This middleware is used reports app runtime errors in development environment
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                ////This middleware is catches exceptions thrown in production environment
                ///app.UseExceptionHandler("/Error");  
                app.UseHsts();
            }

           app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //   //This middleware is used to redirects HTTP requests to HTTPS. 
            app.UseHttpsRedirection();
         
            //For Authendication purpose
            app.UseAuthentication();

            //add this for CORS before app.UseMvc()
            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            app.UseMvc();
            //This middleware is used to returns static files and short-circuits further request processing. 
            app.UseStaticFiles();

            app.UseCookiePolicy();

            //This middleware is used to route requests.   
           // app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //public ContentResult OnGet()
        //{
        //    var myKeyValue = Configuration["URL"];
            
        //    return Content($"MyKey value:" {myKeyValue} ;


        //}
    }
}
