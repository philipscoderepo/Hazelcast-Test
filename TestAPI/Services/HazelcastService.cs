using Hazelcast;
using Hazelcast.DistributedObjects;

namespace TestAPI.Services
{
    public class HazelcastService : IHazelcastService
    {
        private readonly IHazelcastClient _client;
        public HazelcastService()
        {
            var options = new HazelcastOptions();
            options.ClusterName = "dev";
            options.ClientName = "net-client";
            options.Networking.Addresses.Add("localhost:5701");

            _client = HazelcastClientFactory.GetNewStartingClient(options).Client;
        }

        public async Task<int> GetRecordAsync(string key)
        {
            var worker = new HazelcastWorker(_client);
            var rec = await worker.GetRecordAsync(key).ConfigureAwait(false);
            return rec;
        }

        public async Task PutRecordAsync(string key)
        {
            var worker = new HazelcastWorker(_client);
            await worker.PutRecordAsync(key).ConfigureAwait(false);
        }
    }
}
