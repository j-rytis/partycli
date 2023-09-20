using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using partycli.Domain;
using partycli.Enums;

namespace partycli
{
    class Program
    {
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                services.GetRequiredService<App>().Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IArgumentProcessor, ArgumentProcessor>();
                    services.AddSingleton<INordvpnService, NordvpnService>();
                    services.AddSingleton<IListPrinter, ListPrinter>();
                    services.AddSingleton<ILogging, Logging>();
                    services.AddSingleton<IValueStorage, ValueStorage>();
                    services.AddSingleton<App>();
                });
        }
    }
}




// config does not do anything. Dead code?





//Q:






//    IHost host = CreateHostBuilder(args).Build();
//    var scope = host.Services.CreateScope();

//    var services = scope.ServiceProvider;

//    try
//    {
//        services.GetRequiredService<App>().Run(args);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//        Console.Read();
//    }
//}

//public static IHostBuilder CreateHostBuilder(string[] args)
//{
//    return Host.CreateDefaultBuilder(args)
//        .ConfigureServices((_, services) =>
//        {
//            services.AddSingleton<App>();
//            services.AddSingleton<IArgumentProcessor, ArgumentProcessor>();
//            services.AddSingleton<INordvpnService, NordvpnService>();
//            services.AddSingleton<IListPrinter, ListPrinter>();
//            services.AddSingleton<IValueStorage, ValueStorage>();
//        });
//}