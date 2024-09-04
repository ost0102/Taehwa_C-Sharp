using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using System.Configuration;
using YJIT.Data;
using TAFX.ELVISPRIME.HOME.Models;

//using PagedList;
using Newtonsoft.Json;
using System.Text;
//using EASendMail;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net;
using AegisImplicitMail;
using TAFX_ELVISPRIME_HOME.Models;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class RecruitmentController : Controller
    {
        private List<NoticeModel> NoticeList = new List<NoticeModel>();
        public List<DataRow> dtList { get; set; }
        public class pageParam
        {
            public string Option { get; set; }
            public string Type { get; set; }
            public string SearchText { get; set; }
            public int Page { get; set; }
        }
        //public PagedList<DataRow> pList { get; set; }

        //
        // GET: /community/

        //데이터 조회
        public string CallAjax(pageParam rtnVal)
        {
            string rtnJson = "";
            try
            {
                if (rtnVal != null)
                {
                    string strOpt = rtnVal.Option;
                    string strType = rtnVal.Type;
                    string strText = rtnVal.SearchText;
                    int pageIndex = rtnVal.Page;

                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                    DataTable dt = dbConn.SqlGet(con.Search_Notice(strOpt, strType, strText, pageIndex));
                    if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt);
                }
                return rtnJson;
            }
            catch
            {
                return "";
            }
        }

        public ActionResult Index()
        {
            ViewBag.MENU4 = "Recruitment";
            return View();
        }
        public ActionResult view(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ADO_Conn con = new ADO_Conn();
                DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                dbConn.SqlSet(con.Update_ViewCnt(id));
                dbConn.Commit();
                DataTable dt = dbConn.SqlGet(con.Search_NoticeView(id));


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        NoticeList.Add(new NoticeModel
                        {
                            FLAG = dr["FLAG"].ToString(),
                            NOTICE_ID = int.Parse(dr["NOTICE_ID"].ToString()),
                            TITLE = dr["TITLE"].ToString(),
                            CNT = int.Parse(dr["CNT"].ToString()),
                            WRITER = dr["WRITER"].ToString(),
                            USE_YN = dr["USE_YN"].ToString(),
                            NOTICE_YN = dr["NOTICE_YN"].ToString(),
                            REGDT = dr["REGDT"].ToString(),
                            EDITDT = dr["EDITDT"].ToString(),
                            FILE = dr["FILE"].ToString(),
                            FILE_NAME = dr["FILE_NAME"].ToString(),
                            FILE1 = dr["FILE1"].ToString(),
                            FILE_NAME1 = dr["FILE1_NAME"].ToString(),
                            FILE2 = dr["FILE2"].ToString(),
                            FILE_NAME2 = dr["FILE2_NAME"].ToString(),
                            CONTENT = dr["CONTENT"].ToString()
                        });
                    }
                }
            }
            ViewBag.MENU5 = "View";
            return View(NoticeList);
        }

	}
}