using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurfsUpClassLibrary
{
    public class Spot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpotID { get; set; }
        public string SpotName { get; set; }
        public string City { get; set; }
        public string SpotDescription { get; set; }
        public int Views { get; set; }
        public Byte[] SpotImage { get; set; }
        [NotMapped]
        public string ImageDataUrl { get; set; }
        public string Address { get; set; }
        public ApplicationUser User { get; set; }
        public string UserID { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
