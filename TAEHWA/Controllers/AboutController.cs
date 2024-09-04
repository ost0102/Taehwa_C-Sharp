using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MENU1 = "About";
            return View();
        }
    }
}