using Microsoft.AspNetCore.Mvc;
using System.Web;
using TestAPI.Services;
using Hazelcast;
using Hazelcast.DistributedObjects;
using Newtonsoft.Json;

namespace TestAPI.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IHazelcastService _service;
        public TestController(IHazelcastService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("test/get-attempts/{pan}")]
        public async Task<JsonResult> GetInvalidAttempts(string pan)
        {
            var rec = await _service.GetRecordAsync(pan).ConfigureAwait(false);
            return new JsonResult(rec);
        }

        [HttpGet]
        [Route("test/add-attempt/{pan}")]
        public async Task<JsonResult> AddInvalidAttempt(string pan)
        {
            await _service.PutRecordAsync(pan).ConfigureAwait(false);
            return new JsonResult("success");
        }
    }
}
