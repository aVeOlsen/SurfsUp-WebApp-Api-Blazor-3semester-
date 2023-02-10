using System.Collections.Generic; 
namespace Surfs_Up_Weather_Api
{ 

    public class GeoCodeResult
    {
        public List<GeoCodeAddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public GeoCodeGeometry geometry { get; set; }
        public bool partial_match { get; set; }
        public string place_id { get; set; }
        public GeoCodePlusCode plus_code { get; set; }
        public List<string> types { get; set; }
    }

}