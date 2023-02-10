using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            //return View();
            return PartialView();
        }
    }
}
