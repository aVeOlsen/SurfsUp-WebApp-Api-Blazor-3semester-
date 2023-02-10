using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ProtoTypeV1.Models;
using ProtoTypeV1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProtoTypeV1.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IGenericRepo<Address> _aRepo;
        public EmailModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _aRepo = new AddressRepoDB(context);
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Ny email")]
            public string NewEmail { get; set; }
            
            [Phone]
            [Display(Name = "Telefon nummer")]
            public string NewPhoneNumber { get; set; }

            [PersonalData]
            [Display(Name = "Fornavn")]
            public string NewFirstName { get; set; }

            [PersonalData]
            [Display(Name = "Efternavn")]
            public string NewLastName { get; set; }


            [Display(Name = "Vejnavn")]
            public string NewStreet { get; set; }

            [Display(Name = "Husnummer")]
            public string NewHouseNumber { get; set; }

            [Display(Name = "Postnummer")]
            public int NewPostalCode { get; set; }

            [Display(Name = "By")]
            public string NewCity { get; set; }
            [Display(Name = "Region")]
            public string NewRegion { get; set; }

            [Display(Name = "Land")]
            public string NewCountry { get; set; }



        }

        private async Task LoadAsync(User user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;
            Input = new InputModel
            {
                NewEmail = Email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);


        }

        //Henter user og adresse data til user når brugeroplysninger tilgåes
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (user.AddressID == 0)
            {
                return NotFound($"Unable to load user with address ID '{user.AddressID}'.");
            }

            user.Address = _aRepo.GetByID(user.AddressID);

            await LoadAsync(user);
            Input = new InputModel
            {
                NewEmail = user.Email,
                NewPhoneNumber = user.PhoneNumber,
                NewFirstName = user.FirstName,
                NewLastName = user.LastName,
                NewStreet = user.Address.StreetName,
                NewHouseNumber = user.Address.HouseNumber,
                NewPostalCode = user.Address.PostalCode,
                NewRegion = user.Address.Region,
                NewCity = user.Address.City,
                NewCountry = user.Address.Country,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostChangePhoneAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.NewPhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.NewPhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profilen blev opdateret";
            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Input.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verificerings email sendt. Tjek inbox.";
            return RedirectToPage();
        }


        //Addresser bliver stadig tilføjet med nyt ID
        //Opdatere brugerens oplysninger
        public async Task<IActionResult> OnPostChangeUserDataAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            user.FirstName = Input.NewFirstName;
            user.LastName = Input.NewLastName;

            user.Address = new Address
            {
                StreetName = Input.NewStreet,
                HouseNumber = Input.NewHouseNumber,
                City = Input.NewCity,
                PostalCode = Input.NewPostalCode,
                Region = Input.NewRegion,
                Country = Input.NewCountry,
            };
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Profilen blev opdateret";
                return RedirectToPage(result);
            }
            else
            {
                StatusMessage = "Unexpected error";
                return RedirectToPage();
            }
        }
    }
}
