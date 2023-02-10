using System.Collections.Generic; 
namespace Surfs_Up_Weather_Api{ 

    public class GeoCodeAddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }
}