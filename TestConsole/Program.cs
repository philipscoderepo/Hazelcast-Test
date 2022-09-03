using System;
using System.Threading.Tasks;
using Hazelcast.Exceptions;
using Microsoft.Extensions.Logging;
using Hazelcast;

namespace TestConsole
{
    // ReSharper disable once UnusedMember.Global
    public class Program
    {
        //
        // this is a complete example of a simple console application where
        // every component is configured and created explicitly.
        //
        // configuration (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration)
        //
        //   environment
        //     optional Hazelcast.Build(...) 'environmentName' argument: Development, Staging, Production (default), ...
        //     falls back to DOTNET_ENVIRONMENT + ASPNETCORE_ENVIRONMENT environment variables
        //     determines <env>, default is Production
        //
        //   configuration file
        //     appsettings.json
        //     appsettings.<env>.json
        //     hazelcast.json
        //     hazelcast.<env>.json
        //
        //     {
        //       "hazelcast": {
        //         "networking": {
        //           "addresses": [ "server:port" ]
        //         }
        //       }
        //     }
        //
        //   environment variables
        //     hazelcast__networking__addresses__0=server:port (standard .NET)
        //     hazelcast.networking.addresses.0=server:port (hazelcast-specific)
        //
        //   command line
        //     hazelcast:networking:addresses:0=server:port (standard .NET)
        //     hazelcast.networking.addresses.0=server:port (hazelcast-specific)
        //
        // the simplest way to run this example is to build the code:
        //  ./hz.ps1 build
        //
        // then to execute the example:
        //  ./hz.ps1 run-example Client.SimpleExample --- --hazelcast.networking.addresses.0=server.port
        //
        // it is possible to run more than once with --hazelcast.example.runCount=2
        // the pause between the runs can be configured to 10s with --hazelcast.example.pauseDuration=00:00:10
        // you may want to enable re-connection with --hazelcast.networking.reconnectMode=reconnectAsync
        // it is possible to change the Hazelcast logging level from Information to anything else,
        // with --Logging:LogLevel:Hazelcast=Debug (note that for this non-Hazelcast property, the dot-separator
        // is not supported and a true .NET supported separator must be used - here, a colon)

        public static async Task Main(string[] args)
        {
            // build options
            var exampleOptions = new ExampleOptions();

            var options = new HazelcastOptionsBuilder()
                .With(o => o.ClusterName = "dev")
                .With(o => o.ClientName = "net-ex")
                //.With(o => o.Networking.Addresses.Add("localhost:5701"))
                .Build();

            // create a client
            //await using var client = await HazelcastClientFactory.StartNewClientAsync(options).ConfigureAwait(false); // disposed when method exits
            var client = HazelcastClientFactory.GetNewStartingClient(options).Client;

            // create a worker
            var worker = new Worker(client);

            // end

            // run
            for (var i = 0; i < exampleOptions.RunCount; i++)
            {
                // pause?
                if (i > 0 && exampleOptions.PauseDuration > TimeSpan.Zero)
                {
                
                    await Task.Delay(exampleOptions.PauseDuration).ConfigureAwait(false);
                }

                
                try
                {
                    await worker.GetExistingAsync().ConfigureAwait(false);
                    await worker.RunAsync().ConfigureAwait(false);
                }
                catch (ClientOfflineException)
                {
                    
                }
                catch (Exception e)
                {
                    
                }
            }

            // end
            Console.WriteLine("done");
            Console.ReadLine();
        }

        public class ExampleOptions
        {
            public TimeSpan PauseDuration { get; set; } = TimeSpan.Zero;

            public int RunCount { get; set; } = 1;

        }

        public class Worker
        {
            private readonly IHazelcastClient _client;

            public Worker(IHazelcastClient client)
            {
                _client = client;
            }

            public async Task RunAsync()
            {
                
                await using var map = await _client.GetMapAsync<string, int>("test_map").ConfigureAwait(false);

                // NOTE that regardless of ConfigureAwait(false) the map operations below may seem to
                // hand if the cluster is currently managing the loss of a member (i.e. if a member pod
                // was just deleted) because the *member* does not respond because it's presumably
                // dealing with the situation - nothing we can do nor need to fix at .NET level

                
                await map.SetAsync("key", 42).ConfigureAwait(false);

                
                var value = await map.GetAsync("key").ConfigureAwait(false);
                Console.WriteLine(value);
                if (value != 42) throw new Exception("Error!");
                

                // destroy the map
                
                //await _client.DestroyAsync(map).ConfigureAwait(false);
                

            }

            public async Task GetExistingAsync()
            {
                await using var map = await _client.GetMapAsync<string, string>("invalid").ConfigureAwait(false);

                var value = await map.GetAsync("1").ConfigureAwait(false);
                Console.WriteLine(value);
            }
        }
    }
}