namespace Surfs_Up_Weather_Api{ 

    public class OpenWeatherMapRoot
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public OpenWeatherMapCurrent current { get; set; }
    }

}