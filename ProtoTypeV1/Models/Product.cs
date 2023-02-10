using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        public string Brand { get; set; }
        public string TypeBoard { get; set; }
        public string TypeDescription { get; set; }
        public string Difficulity { get; set; }
        public string Size { get; set; }
        public int Volume { get; set; }
        public string Condition { get; set; }
        public byte[] ProductImage { get; set; }
        [NotMapped]
        public string ImageDataUrl { get; set; }
        public List<Rental> Rentals { get; set; }
        public User ByUser { get; set; }
        public string UserID { get; set; }
    }
}
