using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Surfs_Up_Weather_Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StormGlassController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> ForeCast()
        {

            string key = "32ab9e20-4c47-11ec-b7e4-0242ac130002-32ab9e98-4c47-11ec-b7e4-0242ac130002";



            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync($"https://api.stormglass.io/v2/weather/point?lat=55.792&lng=9.613&start=2021-11-23&end=2021-11-23&params=airTemperature,waveHeight,wavePeriod,waterTemperature,gust,windSpeed&source=noaa");
            string rawData = await response.Content.ReadAsStringAsync();

            StormGlassRoot JsonStormGlassData = JsonSerializer.Deserialize<StormGlassRoot>(rawData);

            return Ok(JsonStormGlassData);


            //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.windy.com/api/point-forecast/v2");
            //requestMessage.Content = JsonContent.Create(new JsonModel
            //{
            //    lat = 55.792,
            //    lon = 9.613,
            //    model = "gfs",
            //    parameters = new string[1] { "temp" },
            //    levels = new string[1] { "surface" },
            //    key = "1Dwq3O3FSD3qrbEtSpRPhfRGKeNHSRfp"
            //});

            //var response = await client.SendAsync(requestMessage);


            //return Ok(response);


        }


    }
}