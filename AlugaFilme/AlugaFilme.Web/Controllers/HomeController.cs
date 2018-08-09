using AlugaFilme.Web.Helpers;
using AlugaFilme.Web.Models;
using AlugaFilme.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlugaFilme.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Sobre()
        {

            return View();
        }

    }
}