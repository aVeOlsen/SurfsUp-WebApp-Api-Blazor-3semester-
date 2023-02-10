using System; 
namespace Surfs_Up_Weather_Api{ 

    public class StormGlassHour
    {
        public StormGlassAirTemperature airTemperature { get; set; }
        public StormGlassGust gust { get; set; }
        public DateTime time { get; set; }
        public StormGlassWaterTemperature waterTemperature { get; set; }
        public StormGlassWaveHeight waveHeight { get; set; }
        public StormGlassWavePeriod wavePeriod { get; set; }
        public StormGlassWindSpeed windSpeed { get; set; }
    }

}