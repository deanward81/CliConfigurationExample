using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CliConfigurationExample
{
    class Program
    {
        static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(
                    (ctx, services) =>
                    {
                        var connectionString = ctx.Configuration.GetConnectionString("Default");
                        // example one: connection string from the HostBuilderContext
                        services.AddHostedService(
                            s => new MyService1(connectionString)
                        );

                        // example two: IConfiguration from service provider
                        services.AddHostedService(
                            s =>
                            {
                                var configuration = s.GetRequiredService<IConfiguration>();
                                var connectionString = configuration.GetConnectionString("Default");
                                return new MyService2(connectionString);
                            }
                        );
                    }
                )
                .Build()
                .RunAsync();
        }
    }
    
    class MyService1 : IHostedService
    {
        private readonly string _connectionString;

        public MyService1(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello from Service1: " + _connectionString);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    class MyService2 : IHostedService
    {
        private readonly string _connectionString;

        public MyService2(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello from Service2: " + _connectionString);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
