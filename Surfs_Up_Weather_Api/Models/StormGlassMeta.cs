using System.Collections.Generic; 
namespace Surfs_Up_Weather_Api{ 

    public class StormGlassMeta
    {
        public int cost { get; set; }
        public int dailyQuota { get; set; }
        public string end { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public List<string> @params { get; set; }
        public int requestCount { get; set; }
        public List<string> source { get; set; }
        public string start { get; set; }
    }

}