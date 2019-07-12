using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace dotNET.Core
{

    public class Zconfig
    {
        #region 读取配置信息
        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static string Getconfig(string name)
        {
            IConfiguration config = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
            var appconfig = new ServiceCollection()
            .AddOptions()
            .Configure<SiteConfig>(config.GetSection("SiteConfig"))
            .BuildServiceProvider()
            .GetService<IOptions<SiteConfig>>()
            .Value;

            return appconfig.Configlist.FirstOrDefault(o => o.Key == name).Values;
        }
        #endregion
    }


    #region 读取配置相关类
    public class SiteConfig
    {
        public List<SiteConfiglist> Configlist { get; set; }
    }

    public class SiteConfiglist
    {
        public string Key { get; set; }
        public string Values { get; set; }
    }


    public class Data
    {
        public connstr MyCat { get; set; }

        public connstr Redis { get; set; }

        public long WorkerId { get; set; }
    }




    public class connstr
    {
        public string ConnectionString { get; set; }

    }
    #endregion
}
