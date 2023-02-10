using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class Utility
    {
        public int UtilityID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Spot Spot { get; set; }

    }
}
