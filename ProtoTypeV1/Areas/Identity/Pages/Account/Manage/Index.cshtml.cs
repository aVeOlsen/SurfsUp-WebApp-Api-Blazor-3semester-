using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProtoTypeV1.Data;
using ProtoTypeV1.Models;

namespace ProtoTypeV1.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext _context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._context = _context;
        }
        public string Username { get; set; }
        public List<Rental> Rentals { get; set; }
        public List<Product> Products { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Spot> Spots { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        private async Task LoadAsync(User user)
        {
            user = await _userManager.GetUserAsync(User);
            
            Username = user.FirstName;
            
        }
       
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            //Her søger vi for at det kun er det brugeren har oprettet, som brugeren kan se
            Rentals = await _context.Rental.ToListAsync();
            Rentals.RemoveAll(c => c.RentedOutByID != user.Id);
            Products = await _context.Product.ToListAsync();
            Products.RemoveAll(c => c.UserID != user.Id);
            Reviews = await _context.Reviews.ToListAsync();
            Reviews.RemoveAll(c => c.ByUserID != user.Id);
            Spots = await _context.Spot.ToListAsync();
            Spots.RemoveAll(c => c.UserID != user.Id);
            return Page();
        }
    }
}
