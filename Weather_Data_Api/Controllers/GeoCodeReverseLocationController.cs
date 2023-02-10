using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Weather_Data_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoCodeReverseLocationController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();

        private readonly ILogger<GeoCodeReverseLocationController> _logger;

        public GeoCodeReverseLocationController(ILogger<GeoCodeReverseLocationController> logger)
        {
            _logger = logger;
        }

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
