using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.IO;
using YJIT.Data;
using TAFX_ELVISPRIME_HOME.Models;

using Newtonsoft.Json;

namespace TAFX_ELVISPRIME_HOME.Controllers
{
    public class AdminController : Controller
    {
        string _NoticeFilePath = "/data/notice/";
        string _EditorFilePath = "/data/editor/";

        private List<AdminNoticeModel> NoticeList = new List<AdminNoticeModel>();
        public class noticeParam
        {
            public string Option { get; set; }
            public string Type { get; set; }
            public string SearchText { get; set; }
            public int Page { get; set; }
        }

        public class noticeDel
        {
            public string Notice_ID { get; set; }
        }

        //데이터 조회
        public string Notice_CallAjax(noticeParam rtnVal)
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
                    DataTable dt = dbConn.SqlGet(con.Admin_Notice(strOpt, strType, strText, pageIndex));
                    if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt);
                }
                return rtnJson;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        //데이터 조회        
        public ActionResult Notice_DelAjax(string Notice_ID)
        {
            try
            {
                if (Notice_ID != null)
                {
                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                    NoticeFileDel(Notice_ID, 0);
                    dbConn.SqlSet(con.Admin_NoticeDel(Notice_ID));
                    dbConn.Commit();
                }
                var result = new { Success = "True", Message = "Complete" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                var result = new { Success = "False", Message = e.ToString() };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Notice()
        {
            return View();
        }

        public ActionResult NoticeWrite(string id)
        {
            try
            {
                ADO_Conn con = new ADO_Conn();
                DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                if (id == null) return View();

                DataTable dt = dbConn.SqlGet(con.Admin_NoticeView(id));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        NoticeList.Add(new AdminNoticeModel
                        {
                            FLAG = dr["FLAG"].ToString(),
                            NOTICE_ID = int.Parse(dr["NOTICE_ID"].ToString()),
                            TITLE = dr["TITLE"].ToString(),
                            TYPE = dr["TYPE"].ToString(),
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
                return View(NoticeList);
            }
            catch
            {
                return View(NoticeList);
            }
        }

        //데이터 삭제
        public void NoticeFileDel(string Notice_ID, int FileIndex)
        {
            try
            {
                if (!string.IsNullOrEmpty(Notice_ID))
                {
                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);

                    //먼저 해당 파일을 삭제하자
                    DataTable dt = dbConn.SqlGet(con.Admin_NoticeView(Notice_ID));
                    string strFile1 = "";
                    string strFile2 = "";
                    string strFile3 = "";
                    if (dt.Rows.Count > 0)
                    {
                        #region //File Delete

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strFile1 = dt.Rows[i]["FILE"].ToString();
                            strFile2 = dt.Rows[i]["FILE1"].ToString();
                            strFile3 = dt.Rows[i]["FILE2"].ToString();

                            string FullFilePath1 = Server.MapPath(_NoticeFilePath) + strFile1;
                            string FullFilePath2 = Server.MapPath(_NoticeFilePath) + strFile2;
                            string FullFilePath3 = Server.MapPath(_NoticeFilePath) + strFile3;

                            switch (FileIndex)
                            {
                                case 0:
                                    if (System.IO.File.Exists(FullFilePath1))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    if (System.IO.File.Exists(FullFilePath2))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    if (System.IO.File.Exists(FullFilePath3))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 1:
                                    if (System.IO.File.Exists(FullFilePath1))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 2:
                                    if (System.IO.File.Exists(FullFilePath2))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                                case 3:
                                    if (System.IO.File.Exists(FullFilePath3))    //파일이 존재한다면
                                    {
                                        System.IO.FileInfo file = new System.IO.FileInfo(FullFilePath1);
                                        file.Delete();
                                    }
                                    break;
                            }
                        }
                        #endregion

                        //int result = dbConn.SqlSet(con.Admin_NoticeDel(Notice_ID)); //데이터 삭제
                        //dbConn.Commit();

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        //입력-수정 모두 처리        
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult NoticeModify()
        {
            try
            {
                Hashtable htParam = new Hashtable();
                if (Request.Form.Count > 0)
                {
                    if (Request.Form.AllKeys.Contains("notice_id")) htParam.Add("NOTICE_ID", Request.Form["notice_id"]);
                    if (Request.Form.AllKeys.Contains("title")) htParam.Add("TITLE", Request.Form["title"]);
                    if (Request.Form.AllKeys.Contains("s_type")) htParam.Add("S_TYPE", Request.Form["s_type"]);
                    if (Request.Form.AllKeys.Contains("notice_yn")) htParam.Add("NOTICE_YN", Request.Form["notice_yn"]);
                    if (Request.Form.AllKeys.Contains("content")) htParam.Add("CONTENT", Request.Form["content"]);
                    if (Request.Form.AllKeys.Contains("use_yn")) htParam.Add("USE_YN", Request.Form["use_yn"]);
                    if (Request.Form.AllKeys.Contains("file_del")) htParam.Add("FILE_DEL", Request.Form["file_del"]);
                    if (Request.Form.AllKeys.Contains("file1_del")) htParam.Add("FILE1_DEL", Request.Form["file1_del"]);
                    if (Request.Form.AllKeys.Contains("file2_del")) htParam.Add("FILE2_DEL", Request.Form["file2_del"]);

                    htParam.Add("FILE", "");
                    htParam.Add("FILE_NAME", "");
                    htParam.Add("FILE1", "");
                    htParam.Add("FILE1_NAME", "");
                    htParam.Add("FILE2", "");
                    htParam.Add("FILE2_NAME", "");

                    if (htParam.ContainsKey("NOTICE_ID"))
                    {
                        if (!string.IsNullOrEmpty(htParam["NOTICE_ID"].ToString())) //notice id가 있다! => update
                        {
                            #region //파일 삭제 로직
                            if (htParam.ContainsKey("FILE_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 1);
                                    htParam["FILE"] = "";
                                    htParam["FILE_NAME"] = "";
                                }
                            }
                            else
                            {
                                htParam.Remove("FILE");
                            }

                            if (htParam.ContainsKey("FILE1_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE1_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 2);
                                    htParam["FILE1"] = "";
                                    htParam["FILE1_NAME"] = "";
                                }
                                else
                                {
                                    htParam.Remove("FILE1");
                                }
                            }

                            if (htParam.ContainsKey("FILE2_DEL")) //파일삭제가 체크 되어있고
                            {
                                if (htParam["FILE2_DEL"].ToString() == "y")  //삭제값이 y 이면
                                {
                                    NoticeFileDel(htParam["NOTICE_ID"].ToString(), 3);
                                    htParam["FILE2"] = "";
                                    htParam["FILE2_NAME"] = "";
                                }
                                else
                                {
                                    htParam.Remove("FILE2");
                                }
                            }
                            #endregion
                        }
                    }

                    //파일객체가 있다면
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        var filename = "";
                        var file1 = Request.Files[1];
                        var file1name = "";
                        var file2 = Request.Files[2];
                        var file2name = "";

                        if (file != null && file.ContentLength > 0)
                        {
                            filename = Path.GetFileName(file.FileName);
                            string file_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 1);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file_name);
                            file.SaveAs(path);

                            htParam["FILE"] = file_name;
                            htParam["FILE_NAME"] = filename;
                        }

                        if (file1 != null && file1.ContentLength > 0)
                        {
                            file1name = Path.GetFileName(file1.FileName);
                            string file1_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file1.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 2);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file1_name);
                            file1.SaveAs(path);

                            htParam["FILE1"] = file1_name;
                            htParam["FILE1_NAME"] = file1name;
                        }

                        if (file2 != null && file2.ContentLength > 0)
                        {
                            file2name = Path.GetFileName(file2.FileName);
                            string file2_name = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + GetRandomChar(20) + Path.GetExtension(file2.FileName);

                            NoticeFileDel(htParam["NOTICE_ID"].ToString(), 3);

                            var path = Path.Combine(Server.MapPath(_NoticeFilePath), file2_name);
                            file2.SaveAs(path);

                            htParam["FILE2"] = file2_name;
                            htParam["FILE2_NAME"] = file2name;
                        }
                    }

                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);

                    if (htParam.ContainsKey("NOTICE_ID"))
                    {
                        if (!string.IsNullOrEmpty(htParam["NOTICE_ID"].ToString())) //notice id가 있다! => update
                        {
                            //Update
                            dbConn.SqlSet(con.Admin_NoticeModify(htParam)); //데이터 수정
                            dbConn.Commit();
                        }
                        else
                        {
                            //Insert
                            dbConn.SqlSet(con.Admin_NoticeAdd(htParam)); //데이터 추가
                            dbConn.Commit();
                        }
                    }
                    else
                    {
                        //Insert
                        dbConn.SqlSet(con.Admin_NoticeAdd(htParam)); //데이터 추가
                        dbConn.Commit();
                    }
                }

                return RedirectToAction("Notice");
            }
            catch(Exception e)
            {
                return RedirectToAction("Notice");
            }
        }

        const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";

        [HttpPost]
        public ActionResult NoticeEditor()
        {
            //_EditorFilePath
            string ckEditor = System.Web.HttpContext.Current.Request["CKEditor"];
            string funcNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];
            string langCode = System.Web.HttpContext.Current.Request["langCode"];

            try
            {
                int total = System.Web.HttpContext.Current.Request.Files.Count;
                if (total == 0) return Content(string.Format(scriptTag, funcNum, "", "no File"), "text/html");

                HttpPostedFile theFile = System.Web.HttpContext.Current.Request.Files[0];
                string strFilename = theFile.FileName;
                string sFileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + GetRandomChar(20) + Path.GetExtension(theFile.FileName);//Path.GetFileName(strFilename);
                string name = Path.Combine(Server.MapPath(_EditorFilePath), sFileName);
                theFile.SaveAs(name);
                string url = _EditorFilePath + sFileName.Replace("'", "\'");

                return Content(
                    string.Format(scriptTag, funcNum, HttpUtility.JavaScriptStringEncode(url ?? ""), ""),
                    "text/html"
                    );
            }
            catch (Exception ex)
            {
                return Content(
                    string.Format(scriptTag, funcNum, "", ex.ToString()),
                    "text/html"
                    );
            }

        }

        private ContentResult BuildReturnScript(int FuncNum, string url, string errMsg)
        {
            return Content(
                    string.Format(scriptTag, FuncNum, HttpUtility.JavaScriptStringEncode(url ?? ""), HttpUtility.JavaScriptStringEncode(errMsg ?? "")),
                    "text/html"
                    );
        }



        //데이터 조회
        public string Member_CallAjax()
        {
            string rtnJson = "";
            try
            {
                ADO_Conn con = new ADO_Conn();
                DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                DataTable dt = dbConn.SqlGet(con.Admin_MemberSearch());
                //if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt, new JsonSerializerSettings() { NullValueHandling = NullValueHandling });
                if (dt.Rows.Count > 0) rtnJson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                return rtnJson;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public class data
        {
            public string msg { get; set; }
            public string color { get; set; }
            public string use_yn { get; set; }

        }

        public JsonResult id_check()
        {
            try
            {
                string strMsg = "사용가능한 아이디 입니다.";
                string strColor = "green";
                string strUse_yn = "y";
                ADO_Conn con = new ADO_Conn();
                DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                DataTable dt = dbConn.SqlGet(con.Admin_Member_Check(Request["m_id"].ToString()));
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        strMsg = "중복된 아이디 입니다.";
                        strColor = "red";
                        strUse_yn = "n";
                    }
                }

                data obj = new data
                {
                    msg = strMsg,
                    color = strColor,
                    use_yn = strUse_yn
                };
                return Json(obj);
            }
            catch (Exception ex)
            {
                data obj = new data
                {
                    msg = ex.ToString(),
                    color = "red",
                    use_yn = "n"
                };
                return Json(obj);
            }
        }


        public ActionResult Member()
        {
            return View();
        }

        private List<AdminModel> MemberList = new List<AdminModel>();

        public ActionResult MemberWrite(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                    if (id == null) return View();
                    DataTable dt = dbConn.SqlGet(con.Admin_MemberSelect(id));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] mList = dr["MOBILE"].ToString().Split(new char[] { '-' });
                            string mobile1 = "";
                            string mobile2 = "";
                            string mobile3 = "";

                            if (mList.Length > 0)
                            {
                                switch (mList.Length)
                                {
                                    case 1:
                                        mobile1 = mList[0];
                                        break;
                                    case 2:
                                        mobile1 = mList[0];
                                        mobile2 = mList[1];
                                        break;
                                    case 3:
                                        mobile1 = mList[0];
                                        mobile2 = mList[1];
                                        mobile3 = mList[2];
                                        break;
                                }
                            }

                            MemberList.Add(new AdminModel
                            {
                                MEMB_NO = int.Parse(dr["MEMB_NO"].ToString()),
                                M_ID = dr["M_ID"].ToString(),
                                PASSWORD = dr["PWD"].ToString(),
                                NAME = dr["M_NAME"].ToString(),
                                MOBILE = dr["MOBILE"].ToString(),
                                MOBILE1 = mobile1,
                                MOBILE2 = mobile2,
                                MOBILE3 = mobile3
                            });
                        }
                    }
                    return View(MemberList);
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View(MemberList);
            }
        }

        //입력-수정 모두 처리        
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult MemberModify()
        {
            try
            {
                Hashtable htParam = new Hashtable();
                if (Request.Form.Count > 0)
                {
                    if (Request.Form.AllKeys.Contains("memb_no")) htParam.Add("MEMB_NO", Request.Form["memb_no"]);
                    if (Request.Form.AllKeys.Contains("use_yn")) htParam.Add("USE_YN", Request.Form["use_yn"]);
                    if (Request.Form.AllKeys.Contains("m_id")) htParam.Add("M_ID", Request.Form["m_id"]);
                    if (Request.Form.AllKeys.Contains("password")) htParam.Add("PASSWORD", Request.Form["password"]);
                    if (Request.Form.AllKeys.Contains("name")) htParam.Add("NAME", Request.Form["name"]);
                    if (Request.Form.AllKeys.Contains("mobile1")) htParam.Add("MOBILE1", Request.Form["mobile1"]);
                    if (Request.Form.AllKeys.Contains("mobile2")) htParam.Add("MOBILE2", Request.Form["mobile2"]);
                    if (Request.Form.AllKeys.Contains("mobile3")) htParam.Add("MOBILE3", Request.Form["mobile3"]);
                    if (Request.Form.AllKeys.Contains("del_flag")) htParam.Add("DEL_FLAG", Request.Form["del_flag"]);

                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);

                    if (htParam.ContainsKey("MEMB_NO"))
                    {
                        if (!string.IsNullOrEmpty(htParam["MEMB_NO"].ToString())) //notice id가 있다! => update
                        {
                            //Update
                            dbConn.SqlSet(con.Admin_MemberUpdate(htParam)); //데이터 수정
                            dbConn.Commit();
                        }
                        else
                        {
                            //Insert
                            dbConn.SqlSet(con.Admin_MemberAdd(htParam)); //데이터 추가
                            dbConn.Commit();
                        }
                    }
                    else
                    {
                        //Insert
                        dbConn.SqlSet(con.Admin_MemberAdd(htParam)); //데이터 추가
                        dbConn.Commit();
                    }
                }

                return RedirectToAction("Member");
            }
            catch
            {
                return RedirectToAction("Member");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public class LoginCls
        {
            public string m_id { get; set; }
            public string pwd { get; set; }
        }

        public ActionResult adminLogin(LoginCls loginObj)
        {
            bool loginCheck = false;
            string strMessage = "";

            try
            {
                if (loginObj != null)
                {
                    ADO_Conn con = new ADO_Conn();
                    DBA dbConn = new DBA(con.ConnectionStr, DbConfiguration.Current.DatabaseType);
                    DataTable dt = dbConn.SqlGet(con.Admin_Login(loginObj.m_id, loginObj.pwd));
                    if (dt == null)
                    {
                        loginCheck = false;
                        strMessage = "로그인 정보가 없습니다.";
                        return Json(new { Success = loginCheck, Message = strMessage });
                    }

                    if (dt.Rows.Count > 0)
                    {
                        loginCheck = true;
                        strMessage = "로그인 성공";

                        #region // 로그인 성공시 정보를 Session에 저장하자
                        Session["admin_idx"] = loginObj.m_id;
                        #endregion
                    }
                    else
                    {
                        loginCheck = false;
                        strMessage = "없는 아이디거나 패스워드가 틀렸습니다. 다시 시도해 주세요";
                    }
                }
                else
                {
                    loginCheck = false;
                    strMessage = "로그인 정보가 없습니다.";
                }

                return Json(new { Success = loginCheck, Message = strMessage });

            }
            catch(Exception e)
            {
                loginCheck = false;
                strMessage = "로그인 실패했습니다. 관리자에게 문의 부탁드립니다.";
                return Json(new { Success = loginCheck, Message = strMessage });

            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        public static string GetRandomChar(int _totLen)
        {
            Random rand = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, _totLen).Select(x => input[rand.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
    }
}
