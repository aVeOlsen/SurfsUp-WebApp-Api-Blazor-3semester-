using System.Collections.Generic; 
namespace Surfs_Up_Weather_Api
{ 

    public class GeoCodeRoot
    {
        public List<GeoCodeResult> results { get; set; }
        public string status { get; set; }
    }

}