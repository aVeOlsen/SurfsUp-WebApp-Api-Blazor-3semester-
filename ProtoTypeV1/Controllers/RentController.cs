using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ProtoTypeV1.Data;
using ProtoTypeV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ProtoTypeV1.Controllers
{
    public class RentController : Controller
    {
        private readonly RentalRepoDB _repo;
        private readonly ProductRepoDB _pRepo;
        private readonly UserManager<User> _manager;
        private readonly IEmailSender _emailSender;
        public RentController(ApplicationDbContext _context, UserManager<User> manager, IEmailSender emailSender)
        {
            _repo = new RentalRepoDB(_context);
            _pRepo = new ProductRepoDB(_context);
            _manager = manager;
            _emailSender = emailSender;
        }


        // GET: RentController
        public ActionResult Index()
        {
            var rentals = _repo.GetAll();
            foreach (var item in rentals)
            {
                item.Product.Add(_pRepo.GetByID(item.ProductID));
                string base64Data = Convert.ToBase64String(item.Product[0].ProductImage);
                string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
                item.Product[0].ImageDataUrl = imageDataUrl;
            }


            return View(rentals);
        }

        //Får her fat i id, og tilføjer det så til en tom liste.
        // GET: RentController/Details/5
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var rental = _repo.GetByID(id);
            rental.Product.Add(_pRepo.GetByID(rental.ProductID));
            if (rental.Product == null)
            {
                return NotFound();
            }
            string base64Data = Convert.ToBase64String(rental.Product[0].ProductImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
            rental.Product[0].ImageDataUrl=imageDataUrl;


            return View(rental);

        }




        //Get: Rental/Create
        [Authorize]
        [HttpGet]
        [Route("Rent/Create")]
        public async Task<IActionResult> Create()
        {
            var user = await _manager.FindByEmailAsync(User.Identity.Name);
            Rental r = new Rental();
            r.Product = _pRepo.GetAll();
            r.Product.RemoveAll(c => c.UserID != user.Id);
            return View(r);
        }

        //Henter her først vores bruger, når vi er sikre på model.isvalid, og dette gør, at når brugeren tilføjes
        //så kommer id med
        // POST: RentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalID,ProductID,Depositum, ByDate, ToDate, Description, RentedOutByID")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.RentedOutBy = await _manager.FindByEmailAsync(User.Identity.Name);
                try
                {
                    _repo.Add(rental);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return BadRequest();
        }

        // GET: RentController/Edit/5
        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var rental = _repo.GetByID(id);
            if (rental == null)
            {
                return NotFound();
            }
            rental.Product.Add(_pRepo.GetByID(rental.ProductID));
            return View(rental);
        }

        //Henter her det produkt vi beder om skal redigeres gennem produktrepo
        // POST: RentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("RentalID,ProductID,Depositum,ByDate,ToDate,Description,RentedOutByID")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.ProductID = _repo.GetProductByID(rental.RentalID);
                try
                {
                    _repo.UpdateItem(rental);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View(rental);
        }

        // GET: RentController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var rental = _repo.GetByID(id);

            if (rental == null)
            {
                return NotFound();
            }
            DeleteConfirmed(rental.RentalID);
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // POST: RentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed(int id)
        {
            var rental = _repo.GetByID(id);
            _repo.Remove(rental);
        }

        //HttpGet:rentcontroller/opendetails/5
        [Authorize]
        public IActionResult OpenDetails(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var rental = _repo.GetByID(id);
            rental.Product.Add(_pRepo.GetByID(rental.ProductID));
            if (rental.Product == null)
            {
                return NotFound();
            }
            string base64Data = Convert.ToBase64String(rental.Product[0].ProductImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);

            rental.Product[0].ImageDataUrl = imageDataUrl;

            return View(rental);
        }
        //Post: Rent
        //Først hentes lejende bruger, derefter bruger vi product id til at få fat i udlejeren. 
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RentIt(Rental rental)
        {
            
            var rentingUser = await _manager.FindByEmailAsync(User.Identity.Name);
            var product = _pRepo.GetByID(rental.ProductID);
            var rentOutUser = await _manager.FindByIdAsync(product.UserID);
            string subject = "Leje af udstyr";
            string rentSent = "En forespørgsel om at leje, er blevet sendt til udlejeren: ";
            string rentOutSent = "En forespørgsel om at leje, er modtaget fra: ";

            //Her sørges der først for at de får mailen, derefter emne, besked, og til sidst modpartens email 
            await _emailSender.SendEmailAsync(rentingUser.ToString(), subject, rentSent + rentOutUser);
            //Og udlejeren får lejerens mail
            await _emailSender.SendEmailAsync(rentOutUser.ToString(), subject, rentOutSent + rentingUser);
            return RedirectToAction(nameof(Index));
        }

    }
}
