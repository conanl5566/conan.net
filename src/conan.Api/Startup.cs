using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using System.Text;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
//http://localhost:51880/swagger/ui/index.html#/Values
//https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?tabs=visual-studio
namespace conan.Api
{
    /// <summary>
    /// 
    /// </summary>
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
            services.AddMvc()
           .AddJsonOptions(options =>
       {

           options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
           options.SerializerSettings.Converters.Add(
                 new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                 {
                     DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                 }
               );

           //小写
           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();



           options.SerializerSettings.ContractResolver = new DefaultContractResolver();
           //   //  options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
       });
            //   services.AddMvc().AddXmlSerializerFormatters();
            //  services.AddMvc().AddXmlDataContractSerializerFormatters();
            services.AddLogging();

            services.AddCors(options =>
         options.AddPolicy("AllowSameDomain", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
     ));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSameDomain"));
            });

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "conan.Api",
                    Description = "RESTful API for My Web Application",
                    TermsOfService = "None"
                });
                options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    "conan.Api.xml")); // 注意：此处替换成所生成的XML documentation的文件名。
                options.OperationFilter<HttpHeaderOperation>();
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//这是为了防止中文乱码
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseCors("AllowSameDomain");
            app.UseMvc();

        }
    }
}
