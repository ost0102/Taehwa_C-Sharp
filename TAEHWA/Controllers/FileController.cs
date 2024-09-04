using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TAFX.ELVISPRIME.HOME.Controllers
{
    public class FileController : Controller
    {
        string _NoticeFilePath = "/data/notice/";
        string _RecruitmentFilePath = "/data/Recruitment/";
        //
        // GET: /File/

        public ActionResult Download(string filename, string rFilename)
        {
            try
            {
                string FullFilePath = Server.MapPath(_NoticeFilePath) + rFilename;
                if (System.IO.File.Exists(FullFilePath))    //파일이 존재한다면
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(FullFilePath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    return Content("<script>alert('파일이 존재하지 않습니다'); history.back(-1);</script>");
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Content("<script>alert('파일이 존재하지 않습니다');  history.back(-1);</script>");
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
        public ActionResult RecruitmentDownload(string filename, string rFilename)
        {
            try
            {
                string FullFilePath = Server.MapPath(_RecruitmentFilePath) + rFilename;
                if (System.IO.File.Exists(FullFilePath))    //파일이 존재한다면
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(FullFilePath);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    return Content("<script>alert('파일이 존재하지 않습니다'); history.back(-1);</script>");
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                }
            }
            catch
            {
                return Content("<script>alert('파일이 존재하지 않습니다');  history.back(-1);</script>");
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

    }
}
