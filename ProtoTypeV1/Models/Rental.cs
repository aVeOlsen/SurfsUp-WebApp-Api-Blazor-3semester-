using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class Rental
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RentalID { get; set; }
        public double Depositum { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ByDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        public User RentedOutBy { get; set; }
        public string RentedOutByID { get; set; }
        public List<ProductEquipment> Equipments { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Product> Product { get; set; } = new List<Product>();
        public int ProductID { get; set; }
    }
}
