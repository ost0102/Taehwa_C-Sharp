using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MENU3 = "Service";
            return View();
        }
    }
}