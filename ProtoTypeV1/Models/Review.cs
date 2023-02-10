using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; } = DateTime.Now;
        public User ByUser { get; set; }
        public string ByUserID { get; set; }
        //public Rental Rental { get; set; }
        //public int RentalID { get; set; }
        //public Spot Spot { get; set; }
        //public int SpotID { get; set; }


    }
}
