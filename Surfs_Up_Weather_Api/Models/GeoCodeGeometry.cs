namespace Surfs_Up_Weather_Api
{ 

    public class GeoCodeGeometry
    {
        public GeoCodeLocation location { get; set; }
        public string location_type { get; set; }
        public GeoCodeViewport viewport { get; set; }
    }

}