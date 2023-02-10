using System.Collections.Generic; 
namespace Weather_Data_Api{ 

    public class GeoCodeAddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

}