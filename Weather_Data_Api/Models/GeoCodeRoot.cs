using System.Collections.Generic; 
namespace Weather_Data_Api{ 

    public class GeoCodeRoot
    {
        public List<GeoCodeResult> results { get; set; }
        public string status { get; set; }
    }

}