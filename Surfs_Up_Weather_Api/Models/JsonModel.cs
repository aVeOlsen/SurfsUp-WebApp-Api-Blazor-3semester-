namespace Surfs_Up_Weather_Api.Controllers
{
    internal class JsonModel
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string model { get; set; }
        public string[] parameters { get; set; }
        public string[] levels { get; set; }
        public string key { get; set; }
    }
}