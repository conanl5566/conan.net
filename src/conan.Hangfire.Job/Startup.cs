using System;
using System.Data;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using dotNET.Application;
using dotNET.Core;
using dotNET.EFCoreRepository;
using Hangfire;
using Hangfire.Autofac;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql.Core;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
namespace conan.Hangfire.Job
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddOptions();
            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));
            services.AddLogging();
            services.AddResponseCompression();
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddHangfire(x =>
            {
                var connectionString = Configuration["Data:Redis:ConnectionString"];
                x.UseRedisStorage(connectionString, new RedisStorageOptions() { Db = int.Parse(Configuration["Data:Redis:Db"]) });
            });

            #region mysql
            //services.AddHangfire(x => x.UseStorage(new MySqlStorage(
            // Configuration["Hangfire:ConStr"],
            //    new MySqlStorageOptions
            //    {
            //        TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事务隔离级别。默认是读取已提交。
            //        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
            //        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
            //        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
            //        PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
            //        DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
            //        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
            //        TablePrefix = "Hangfire"                                  //- 数据库中表的前缀。默认为none
            //    }
            //    ))); 
            #endregion

            services.AddDbContext<EFCoreDBContext>(options => options.UseMySql(Configuration["Data:MyCat:ConnectionString"]));
            var container = AutofacExt.InitAutofac(services);
            // GlobalConfiguration.Configuration.UseAutofacActivator(container);
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var filter = new BasicAuthAuthorizationFilter(
               new BasicAuthAuthorizationFilterOptions
               {
                   SslRedirect = false,          // 是否将所有非SSL请求重定向到SSL URL
                   RequireSsl = false,           // 需要SSL连接才能访问HangFire Dahsboard。强烈建议在使用基本身份验证时使用SSL
                   LoginCaseSensitive = false,   //登录检查是否区分大小写
                   Users = new[]
                   {
                        new BasicAuthAuthorizationUser
                        {
                            Login = Configuration["Hangfire:Login"] ,
                            PasswordClear= Configuration["Hangfire:PasswordClear"]
                        }
                   }
               });
            app.UseHangfireDashboard("", new DashboardOptions
            {
                Authorization = new[]
                {
                   filter
                },
            });
            //配置要处理的队列列表 ，如果多个服务器同时连接到数据库，会认为是分布式的一份子。可以通过这个配置根据服务器性能的按比例分配请求，不会导致服务器的压力。不配置则平分请求
            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = new[] { "critical", "test", "default" },//队列名称，只能为小写
                WorkerCount = Environment.ProcessorCount * int.Parse(Configuration["Hangfire:ProcessorCount"]), //并发任务数  --超出并发数。将等待之前任务的完成  (推荐并发线程是cpu 的 5倍)
                ServerName = "hangfire",//服务器名称
            };
            app.UseHangfireServer(jobOptions); //启动hangfire服务
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件
        }
    }
}
