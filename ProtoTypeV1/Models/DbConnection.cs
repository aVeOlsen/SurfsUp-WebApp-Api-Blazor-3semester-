using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public static class DbConnection
    {
        public static string Conectionstring { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=ProtoTypeV1;Trusted_Connection=True;MultipleActiveResultSets=true";

    }
}
