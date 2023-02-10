using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SurfsUpClassLibrary
{
    public class Review
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }
        

        [Required]
        [Range(0, 5, ErrorMessage = "Rating skal være mellem (0-5).")]
        public double Rating { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; } = DateTime.Now;
        public ApplicationUser ByUser { get; set; }
        public string ByUserID { get; set; }
        public Spot Spot { get; set; }
        public int SpotID { get; set; }


    }
}
