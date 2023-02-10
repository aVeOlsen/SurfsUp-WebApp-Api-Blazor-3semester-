using System.Collections.Generic; 
namespace Surfs_Up_Weather_Api{ 

    public class StormGlassRoot
    {
        public List<StormGlassHour> hours { get; set; }
        public StormGlassMeta meta { get; set; }
    }

}