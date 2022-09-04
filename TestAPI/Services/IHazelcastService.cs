namespace TestAPI.Services
{
    public interface IHazelcastService<TKey, TValue>
    {
        Task<TValue> GetRecordAsync(TKey key);
        Task SetRecordAsync(TKey key, TValue value);
        Task SetRecordAsync(TKey key, TValue value, TimeSpan timeToLive);
        Task PutRecordAsync(TKey key, TValue value);
        Task PutRecordAsync(TKey key, TValue value, TimeSpan timeToLive);
        Task DeleteRecordAsync(TKey key);
    }
}
