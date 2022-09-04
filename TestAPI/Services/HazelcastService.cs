using Hazelcast;
using Hazelcast.DistributedObjects;

namespace TestAPI.Services
{
    public class HazelcastService<TKey, TValue> : IHazelcastService<TKey, TValue>
    {
        private readonly IHazelcastClient _client;
        private readonly string _map;
        public HazelcastService(HazelcastOptions options, string map)
        {
            _client = HazelcastClientFactory.GetNewStartingClient(options).Client;
            _map = map; 
        }
        public async Task<TValue> GetRecordAsync(TKey key)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            var rec = await worker.GetRecordAsync(key).ConfigureAwait(false);
            return rec;
        }
        public async Task SetRecordAsync(TKey key, TValue value)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.SetRecordAsync(key, value).ConfigureAwait(false);
        }
        public async Task SetRecordAsync(TKey key, TValue value, TimeSpan timeToLive)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.SetRecordAsync(key, value, timeToLive).ConfigureAwait(false);
        }
        public async Task PutRecordAsync(TKey key, TValue value)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.PutRecordAsync(key, value).ConfigureAwait(false);
        }
        public async Task PutRecordAsync(TKey key, TValue value, TimeSpan timeToLive)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.PutRecordAsync(key, value, timeToLive).ConfigureAwait(false);
        }
        public async Task DeleteRecordAsync(TKey key)
        {
            var worker = new HazelcastWorker<TKey, TValue>(_client, _map);
            await worker.DeleteRecordAsync(key).ConfigureAwait(false);
        }
    }
}
