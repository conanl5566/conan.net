#region using
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using conan.Saas.Framework;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text;
using Microsoft.Extensions.Logging;
using dotNET.Core;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using conan.Saas.Framework.Middlewares;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Mvc.Controllers;
using dotNET.EFCoreRepository;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MySql.Core;
using dotNET.Application;
#endregion
namespace conan.Saas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IContainer ApplicationContainer { get; private set; }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddMemoryCache();
            services.AddOptions();
            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));
            services.AddLogging();
            services.AddCloudscribePagination();
            services.AddResponseCompression();
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddMiniProfiler().AddEntityFramework();
            services.AddMvc(cfg =>
            {
                cfg.Filters.Add(typeof(ExceptionAttribute));//异常捕获
            });
            services.AddMvc(cfg =>
            {
                cfg.Filters.Add(typeof(MvcMenuFilter));
            });

            services.AddHangfire(x =>
            {
                var connectionString = Configuration["Data:Redis:ConnectionString"];
                x.UseRedisStorage(connectionString,new Hangfire.Redis.RedisStorageOptions() { Db=10});
               
            });


            #region mysql
            ////services.AddHangfire(x => x.UseStorage(new MySqlStorage(Configuration["Hangfire:ConStr"]
            ////    ,
            ////       new MySqlStorageOptions
            ////       {
            ////           QueuePollInterval = TimeSpan.FromSeconds(600)            //- 作业队列轮询间隔。默认值为15秒。
            ////       }
            ////    ))); 
            #endregion
            services.AddDbContext<EFCoreDBContext>(options => options.UseMySql(Configuration["Data:MyCat:ConnectionString"]));
            return new AutofacServiceProvider(AutofacExt.InitAutofac(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ICompositeViewEngine engine)
        {
            app.UseAuthentication();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件
            app.UseDefaultImage(defaultImagePath: Configuration.GetSection("defaultImagePath").Value);
            app.UseResponseCompression();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseMiniProfiler();
            app.UseStaticFiles();
            //页面的执行时间
            //app.UseExecuteTime();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "cloudscribeWebPagination",
                template: "pager/{page?}"
                , defaults: new { controller = "Paging", action = "Index" }
            );
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "" }//队列名称，只能为小写

            };

            app.UseHangfireServer(jobOptions);
        }
    }
}