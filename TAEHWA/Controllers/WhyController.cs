using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class WhyController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MENU2 = "Why";
            return View();
        }
    }
}