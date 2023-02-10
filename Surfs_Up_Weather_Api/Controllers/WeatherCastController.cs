using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Surfs_Up_Weather_Api.Controllers;
using Surfs_Up_Weather_Api.Models;
using System.Net.Http.Headers;
using SurfsUpClassLibrary;
using System.Diagnostics;



namespace Surfs_Up_Weather_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherCastController : ControllerBase
    {
        static readonly HttpClient geoClient = new HttpClient();
        static readonly HttpClient openWeatherMapClient = new HttpClient();
        static readonly HttpClient stromGlassClient = new HttpClient();

        GeoCodeRoot JsonGeoReverseData;
        OpenWeatherMapRoot JsonOpenWeatherMapData;
        StormGlassRoot JsonStormGlassData;
        ForecastResponseData responseData;

        private readonly ILogger<WeatherCastController> _logger;

        public WeatherCastController(ILogger<WeatherCastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> OpenWeatherMap(string address)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };


            // her henter vi locations data ned fra google med api call.
            // sender by navn og google retuner med locations data i from af lat og lng 
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string GeoUrl = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + ",?language=da,+CA&key=AIzaSyByUufDE9K8yiQDMu1YqyzL4RYpe_-MCNs";
            Debug.WriteLine(GeoUrl);
            var Georesponse = await geoClient.GetAsync(GeoUrl);
            string GeoRawData = await Georesponse.Content.ReadAsStringAsync();
            JsonGeoReverseData = JsonSerializer.Deserialize<GeoCodeRoot>(GeoRawData, options);
            string lat = Convert.ToString(JsonGeoReverseData.results[0].geometry.location.lat);
            string lng = Convert.ToString(JsonGeoReverseData.results[0].geometry.location.lng);


            // efter at retuneret locations data'en fra google GeoReverseLocation
            // geo dataen sender vi til openweathermap API, som retuner data om verjet
            string openWeatherMapUrl = "https://api.openweathermap.org/data/2.5/onecall?lat=" + lat + "&lon=" + lng + "&units=metric&exclude=minutely,hourly,daily&appid=6bffa8dec9dc4800d583b41bddcdf65a";
            Debug.WriteLine(openWeatherMapUrl);
            var openWeatherMapresponse = await openWeatherMapClient.GetAsync(openWeatherMapUrl);
            string openWeatherMapRawData = await openWeatherMapresponse.Content.ReadAsStringAsync();
            JsonOpenWeatherMapData = JsonSerializer.Deserialize<OpenWeatherMapRoot>(openWeatherMapRawData, options);

            // efter at retuneret locations data'en fra google GeoReverseLocation
            // her efter sender vi geo dataen 

            DateTime today = DateTime.Now;
            ReplaceChar replacechar = new ReplaceChar();

            string key = "32ab9e20-4c47-11ec-b7e4-0242ac130002-32ab9e98-4c47-11ec-b7e4-0242ac130002";
            string stormGlassUrl = "https://api.stormglass.io/v2/weather/point?lat=" + replacechar.replace(',', '.', lat) + "&lng=" + replacechar.replace(',', '.', lng) + "&start=" + today.ToString("yyyy/MM/dd") + "&end=" + today.ToString("yyyy/MM/dd") + "&params=airTemperature,waveHeight,wavePeriod,waterTemperature,gust,windSpeed&source=noaa";
            
            Debug.WriteLine(stormGlassUrl);

            stromGlassClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(key);
            stromGlassClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            stromGlassClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            stromGlassClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            stromGlassClient.DefaultRequestHeaders.Accept.Clear();
            stromGlassClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var stormGlassResponse = await stromGlassClient.GetAsync(stormGlassUrl);
            string stormGlasRawData = await stormGlassResponse.Content.ReadAsStringAsync();

            JsonStormGlassData =  JsonSerializer.Deserialize<StormGlassRoot>(stormGlasRawData, options);


            //her samler vi dataen fra de 3 api kald, og gemmer dem i et obj og retuner dem.

            responseData = new ForecastResponseData
            {
                lat = JsonGeoReverseData.results[0].geometry.location.lat,
                lng = JsonGeoReverseData.results[0].geometry.location.lng,
                long_name = JsonGeoReverseData.results[0].address_components[2].long_name,
                Short_country = JsonGeoReverseData.results[0].address_components[3].short_name,
                temp = JsonOpenWeatherMapData.current.temp,
                feels_like = JsonOpenWeatherMapData.current.feels_like,
                clouds = JsonOpenWeatherMapData.current.clouds,
                visibility = JsonOpenWeatherMapData.current.visibility,
                wind_speed = JsonOpenWeatherMapData.current.wind_speed,
                wind_deg = JsonOpenWeatherMapData.current.wind_deg,
                waterTempture = JsonStormGlassData.hours[0].waterTemperature.noaa,
                waveHeight = JsonStormGlassData.hours[0].waveHeight.noaa,
                wavePeriode = JsonStormGlassData.hours[0].wavePeriod.noaa


            };
            Debug.WriteLine(_logger);
            return Ok(responseData);
        }


    }
}
