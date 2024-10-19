using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCacheWebApi.Models;

namespace NCacheWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICache _cache;

        public CacheController(ICache cache)
        {
            _cache = cache;
        }

        [HttpGet("get/{key}")]
        public IActionResult GetValue(string key)
        {
            //var readThruOptions = new ReadThruOptions(); // Set your options if needed

            var value = _cache.Get<string>(key) ; 
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        [HttpPost("set")]
        public IActionResult SetValue([FromBody] CacheItemDto item)
        {
            _cache.Insert(item.Key, item.Value);
            return Ok();
        }
    }
}
