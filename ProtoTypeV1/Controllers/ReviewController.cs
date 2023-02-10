using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProtoTypeV1.Data;
using ProtoTypeV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Controllers
{
    public class ReviewController : Controller
    {

        private IGenericRepo<Review> _repo;
        private readonly UserManager<User> _manager;
        public ReviewController(ApplicationDbContext _context, UserManager<User> manager)
        {
            _repo = new ReviewRepoDB(_context);
            _manager = manager;
        }

        // GET: ReviewController
        public ActionResult Index()
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // GET: ReviewController/Details/5
        public ActionResult Details(int id)
        {
            if(id<= 0)
            {
                return NotFound();
            }
            var review = _repo.GetByID(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // GET: ReviewController/Create
        [Authorize]
        [HttpGet]
        [Route("Review/Create")]
        public async Task<IActionResult> Create()
        {
            var user = await _manager.GetUserAsync(User);
            return View();
        }

        // manager sørger her for at hente vores user, og dermed UserID
        // Der kunne også være brugt:    review.ByUser = await _manager.GetUserAsync(User);     Og dette havde gjort det samme
        // POST: ReviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID, Rating, Description, Date, ByUserID, RentalID, SpotID")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ByUser = await _manager.FindByEmailAsync(User.Identity.Name);
                try
                {
                    _repo.Add(review);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return BadRequest();
        }

        // GET: ReviewController/Edit/5
        [HttpGet]
        [Authorize]
        [Route("Review/Edit")]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var review = _repo.GetByID(id);
            if (review == null)
            {
                return NotFound();
            }
            

            return View(review);
        }

        // POST: ReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ReviewID, Rating, Description, Date")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ByUser = await _manager.GetUserAsync(User);
                try
                {
                    _repo.UpdateItem(review);
                    return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
                }
                catch
                {
                    return View();
                }
            }
            return View(review);
        }

        // GET: ReviewController/Delete/5

        [Authorize]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var review = _repo.GetByID(id);

            if (review == null)
            {
                return NotFound();
            }
            DeleteConfirmed(review.ReviewID);
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // POST: ReviewController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed(int id)
        {
            var review = _repo.GetByID(id);
            _repo.Remove(review);

        }
    }
}
