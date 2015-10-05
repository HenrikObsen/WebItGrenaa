using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepoWebIT;

namespace WebItGrenaa.Controllers
{
    public class HomeController : Controller
    {
        ImageFac ImF = new ImageFac();
        // GET: Home
        public ActionResult Index()
        {
            return View("Index", ImF.GetAll());
        }
    }
}