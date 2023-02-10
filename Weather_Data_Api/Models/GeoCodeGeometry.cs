namespace Weather_Data_Api{ 

    public class GeoCodeGeometry
    {
        public GeoCodeLocation location { get; set; }
        public string location_type { get; set; }
        public GeoCodeViewport viewport { get; set; }
    }

}