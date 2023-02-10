using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProtoTypeV1.Data;
using ProtoTypeV1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Controllers
{
    public class SpotsController : Controller
    {

        private SpotRepoDB _repo;
        private readonly UserManager<User> _manager;
        public SpotsController(ApplicationDbContext _context, UserManager<User> manager)
        {
            _repo = new SpotRepoDB(_context);
            _manager = manager;

        }

        // GET: SpotsController
        public IActionResult Index()
        {
            var spots = _repo.GetAll();
            foreach (var item in spots)
            {
                string base64Data = Convert.ToBase64String(item.SpotImage);
                string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
                item.ImageDataUrl = imageDataUrl;
            }

            return View(spots);
        }

        // GET: SpotsController/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var spot = _repo.GetByID(id);
            if (spot == null)
            {
                return NotFound();
            }
            string base64Data = Convert.ToBase64String(spot.SpotImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
            spot.ImageDataUrl = imageDataUrl;

            return View(spot);
        }

        // GET: SpotsController/Create
        [Authorize]
        [HttpGet]
        [Route("Spots/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SpotsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpotID,Address,SpotName,City,SpotDescription,SpotImage,UserID")] Spot spot)
        {
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                spot.SpotImage = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            if (ModelState.IsValid)
            {
                spot.User = await _manager.GetUserAsync(User);
                try
                {
                    _repo.Add(spot);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return BadRequest();
        }

        // GET: SpotsController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var spot = _repo.GetByID(id);
            if (spot == null)
            {
                return NotFound();
            }

            return View(spot);
        }

        // POST: SpotsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SpotID,Address,SpotName,City,SpotDescription,SpotImage")] Spot spot)
        {
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                spot.SpotImage = ms.ToArray();
                ms.Close();
                ms.Dispose();

            }
            if (spot.SpotImage == null)
            {
                spot.SpotImage = _repo.GetImageByID(spot.SpotID);
            }
            if (ModelState.IsValid)
            {
                spot.User = await _manager.GetUserAsync(User);
                try
                {
                    _repo.UpdateItem(spot);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View(spot);
        }

        // GET: SpotsController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var spot = _repo.GetByID(id);

            if (spot == null)
            {
                return NotFound();
            }
            DeleteConfirmed(spot.SpotID);
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // POST: SpotsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed(int id)
        {
            var spot = _repo.GetByID(id);
            _repo.Remove(spot);
        }
    }
}
