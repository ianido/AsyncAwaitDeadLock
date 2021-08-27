using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Website.DLock.Models;

namespace Website.DLock.Controllers
{
    public class MyController : ApiController
    {
        [HttpGet]
        public string GetDeadLock()
        {
            var baseUrl = $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}";
            var jsonTask = GetJsonAsync(new Uri($"{baseUrl}/api/my/items"));
            return jsonTask.Result.ToString();
        }
        [HttpGet]
        public string GetDeadLock1()
        {
            var baseUrl = $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}";
            var json = GetJsonAsync(new Uri($"{baseUrl}/api/my/items")).GetAwaiter().GetResult();
            return json.ToString();
        }

        [HttpGet]
        public string GetDeadLock2()
        {
            var baseUrl = $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}";
            var task = GetJsonAsync(new Uri($"{baseUrl}/api/my/items"));
            task.Wait();
            return task.Result.ToString();
        }

        [HttpGet]
        public string GetSafe()
        {
            var baseUrl = $"{Request.RequestUri.Scheme}://{Request.RequestUri.Authority}";
            var json= TaskUtils.RunSync(() => GetJsonAsync(new Uri($"{baseUrl}/api/my/items")));
            return json.ToString();
        }

        [HttpGet]
        public IHttpActionResult Items()
        {
            return Ok(new ItemResult() { Id = 1 });
        }

        public static async Task<JObject> GetJsonAsync(Uri uri)
        {
            // (real-world code shouldn't use HttpClient in a using block; this is just example code)
            using (var client = new HttpClient())
            {
                var jsonString = await client.GetStringAsync(uri);
                return JObject.Parse(jsonString);
            }
        }
    }
}
