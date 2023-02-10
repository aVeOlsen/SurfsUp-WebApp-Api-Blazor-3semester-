using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surfs_Up_Weather_Api.Models
{
    public class ForecastResponseData
    {

        public double lat { get; set; }
        public double lng { get; set; }
        public string long_name { get; set; }
        public string Short_country { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double waterTempture { get; set; }
        public double waveHeight { get; set; }
        public double wavePeriode { get; set; }
        
    }
}
