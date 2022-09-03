namespace TestAPI.Services
{
    public interface IHazelcastService
    {
        Task<int> GetRecordAsync(string key);
        Task PutRecordAsync(string key);
    }
}
