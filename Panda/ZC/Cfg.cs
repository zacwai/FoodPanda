using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.ZC
{
    public class Cfg
    {
        public static string GetCfg(string App_Cfg)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

            return configuration.GetValue<string>("AppData:" + App_Cfg);
        }

    }
}
