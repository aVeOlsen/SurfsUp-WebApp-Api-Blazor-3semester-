using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Models
{
    public class User:IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public enum Genders
        {
            Mand = 0, 
            Kvinde = 1, 
            Andet = 2,
        }

        [PersonalData]
        public DateTime DayOfBirth { get; set; }
        public Address Address { get; set; }
        public int AddressID { get; set; }
        public List<Spot> Spots { get; set; }


    }
}
