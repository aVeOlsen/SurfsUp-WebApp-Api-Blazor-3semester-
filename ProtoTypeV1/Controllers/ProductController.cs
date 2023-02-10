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
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProtoTypeV1.Controllers
{

    public class ProductController : Controller
    {

        private ProductRepoDB _repo;
        private readonly UserManager<User> _manager;

        public ProductController(ApplicationDbContext _context, UserManager<User> manager)
        {
            _repo = new ProductRepoDB(_context);
            _manager = manager;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        //Får her fat i id, og tilføjer det så til en tom liste.
        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var product = _repo.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }
            string base64Data = Convert.ToBase64String(product.ProductImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
            product.ImageDataUrl = imageDataUrl;
            return View(product);
        }

        // GET: ProductController/Create
        [Authorize]
        [HttpGet]
        [Route("Product/Create")]
        public IActionResult Create()
        {
            return View();
        }


        // manager sørger her for at hente vores user, og dermed UserID
        // Der kunne også være brugt:    review.ByUser = await _manager.GetUserAsync(User);     Og dette havde gjort det samme
        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID, Brand, TypeBoard, TypeDescription, Difficulity, Size, Volume, Condition, ProductImage, UserID")]
        Product product)
        {
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                product.ProductImage = ms.ToArray();
                ms.Close();
                ms.Dispose();

            }
            if (ModelState.IsValid)
            {
                product.ByUser = await _manager.FindByEmailAsync(User.Identity.Name);
                try
                {
                    _repo.Add(product);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return BadRequest();
        }

        // GET: ProductController/Edit/5
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var product = _repo.GetByID(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //Henter her først bruger, og og efter id af bruger. 
        //virkede på samme måde med poduct.byuser= await _manager.getuserAsync(User).
        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductID, Brand, TypeBoard, TypeDescription, Difficulity, Size, Volume, Condition, ProductImage, UserID")] Product product)
        {
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                product.ProductImage = ms.ToArray();
                ms.Close();
                ms.Dispose();

            }
            if (product.ProductImage == null)
            {
                product.ProductImage = _repo.GetImageByID(product.ProductID);
            }
            if (ModelState.IsValid)
            {
                product.ByUser = await _manager.GetUserAsync(User);
                //product.ByUser = await _manager.FindByEmailAsync(User.Identity.Name);
                //product.UserID = await _manager.GetUserIdAsync(product.ByUser);
                try
                {
                    _repo.UpdateItem(product);
                    return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
                }
                catch
                {
                    return View();
                }
            }
            return View(product);
        }

        // GET: ProductController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var product = _repo.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }
            DeleteConfirmed(product.ProductID);
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed(int id)
        {
            var product = _repo.GetByID(id);
            _repo.Remove(product);
        }
    }
}
