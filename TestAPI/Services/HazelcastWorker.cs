using Hazelcast;

namespace TestAPI.Services
{
    public class HazelcastWorker
    {
        private readonly IHazelcastClient _client;
        public HazelcastWorker(IHazelcastClient client)
        {
            _client = client;
        }

        public async Task<int> GetRecordAsync(string pan)
        {
            var map = await _client.GetMapAsync<string, int>("invalid").ConfigureAwait(false);
            var rec = await map.GetAsync(pan).ConfigureAwait(false);
            return rec;
        }

        public async Task PutRecordAsync(string pan) 
        {
            var map = await _client.GetMapAsync<string, int>("invalid").ConfigureAwait(false);
            int? rec = await map.GetAsync(pan).ConfigureAwait(false);
            if(rec != null)
            {
                rec++;
                await map.PutAsync(pan, (int)rec).ConfigureAwait(false);
            }
            else
            {
                await map.SetAsync(pan, 1).ConfigureAwait(false);
            }
        }
    }
}
