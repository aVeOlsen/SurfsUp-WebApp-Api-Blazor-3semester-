using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Surfs_Up_Weather_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoReverseController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> GeoCodeReverse()
        {
            var response = await client.GetAsync("https://maps.googleapis.com/maps/api/geocode/json?address=kolding,?language=da,+CA&key=AIzaSyByUufDE9K8yiQDMu1YqyzL4RYpe_-MCNs");
            string rawData = await response.Content.ReadAsStringAsync();

            GeoCodeRoot JsonGeoReverseData = JsonSerializer.Deserialize<GeoCodeRoot>(rawData);

            return Ok(JsonGeoReverseData);
        }
    }

}
