using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Default
        public ActionResult SetLang(string strLang)
        {
            // 언어를 소문자로 세션에 저장
            if (!string.IsNullOrEmpty(strLang))
            {
                Session["Language"] = strLang; // "ko" 또는 "en"으로 저장
            }

            // 이전 페이지로 리다이렉트
            return RedirectToAction("Index", "Home");
        }
    }
}