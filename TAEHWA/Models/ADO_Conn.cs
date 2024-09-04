using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Text;

using System.Configuration;
using YJIT.Data;

namespace TAFX_ELVISPRIME_HOME.Models
{
    public class ADO_Conn
    {
        public string ConnectionStr = ConfigurationManager.ConnectionStrings["TAFX"].ConnectionString;
        private string memberKey = ConfigurationManager.AppSettings["memberKey"].ToString();

        public void ThrowMsg(bool ErrorOccur, string Msg)
        {
            ErrorOccur = true;
            throw new Exception(Msg);
        }

        public string Search_Notice(string Opt, string Type, string SearchText, int pageIndex)
        {
            string sSql = "";
            #region //이전 소스                        
            //sSql += "  SELECT * FROM ";
            //sSql += "  ( ";
            //sSql += "      SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM ";
            //sSql += "              , A.* ";
            //sSql += "      FROM NOTICE A ";
            //sSql += "      WHERE A.USE_YN = 'y' ";
            //sSql += "         AND A.NOTICE_YN = 'y' ";
            //sSql += "      AND A.TITLE LIKE '%" + SearchText + "%' ";
            //sSql += "      ORDER BY A.REGDT DESC "; 
            //sSql += "  ) ";
            //sSql += "  UNION ALL ";
            //sSql += "  SELECT * FROM ";
            //sSql += "  ( ";
            //sSql += "      SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM ";
            //sSql += "              , A.* ";
            //sSql += "      FROM NOTICE A ";
            //sSql += "      WHERE A.USE_YN = 'y' ";
            //sSql += "         AND A.NOTICE_YN = 'n' ";
            //sSql += "      AND A.TITLE LIKE '%" + SearchText + "%'";
            //sSql += "      ORDER BY A.REGDT DESC ";
            //sSql += " ) ";
            #endregion

            if (pageIndex == 0) pageIndex = 1;

            sSql += " SELECT * ";
            sSql += "  FROM ( ";
            sSql += "              SELECT ROWNUM AS RNUM ";
            sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
            sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
            sSql += "                     , X.* ";
            sSql += "             FROM ( ";
            sSql += "                         SELECT * FROM             ";
            sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                             FROM NOTICE A  ";
            sSql += "                            WHERE A.USE_YN = 'y'  ";
            sSql += "                               AND A.NOTICE_YN = 'y' ";
            if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                           ORDER BY A.REGDT DESC )  ";
            sSql += "                           UNION ALL ";
            sSql += "                         SELECT * FROM     ";
            sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                              FROM NOTICE A  ";
            sSql += "                             WHERE A.USE_YN = 'y'  ";
            sSql += "                                AND A.NOTICE_YN = 'n'  ";
            if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                             ORDER BY A.REGDT DESC )   ";
            sSql += "                      ) X  ";
            sSql += "           ) XX ";
            sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

            return sSql;
        }

        public string Search_NoticeView(string noticeID)
        {
            string sSql = "";
            sSql += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sSql += " WHERE V.NOTICE_ID = " + noticeID + " ";
            sSql += " UNION ALL ";
            sSql += " SELECT 'PREV' AS FLAG, P.* FROM NOTICE P  ";
            sSql += " WHERE P.NOTICE_ID  = (SELECT MAX(NOTICE_ID) FROM NOTICE WHERE NOTICE_ID < " + noticeID + ") ";
            sSql += " UNION ALL ";
            sSql += " SELECT 'NEXT' AS FLAG, N.* FROM NOTICE N  ";
            sSql += " WHERE N.NOTICE_ID  = (SELECT MIN(NOTICE_ID) FROM NOTICE WHERE NOTICE_ID > " + noticeID + ") ";
            return sSql;
        }

        public string Update_ViewCnt(string noticeID)
        {
            string sSql = "";
            sSql += " UPDATE NOTICE SET CNT = CNT + 1 WHERE NOTICE_ID = '" + noticeID + "'";
            return sSql;
        }

        public string Admin_Notice(string Opt, string Type, string SearchText, int pageIndex)
        {
            string sSql = "";
            if (pageIndex == 0) pageIndex = 1;

            sSql += " SELECT * ";
            sSql += "  FROM ( ";
            sSql += "              SELECT ROWNUM AS RNUM ";
            sSql += "                     , FLOOR((ROWNUM-1) /10 + 1) AS PAGE ";
            sSql += "                     , COUNT(*) OVER () AS TOTCNT ";
            sSql += "                     , X.* ";
            sSql += "             FROM ( ";
            sSql += "                         SELECT * FROM             ";
            sSql += "                         ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                             FROM NOTICE A  ";
            sSql += "                           WHERE A.NOTICE_YN = 'y' ";
            if (Opt == "ALL")
            {
                sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            }
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                           ORDER BY A.REGDT DESC )  ";
            sSql += "                           UNION ALL ";
            sSql += "                         SELECT * FROM     ";
            sSql += "                           ( SELECT ROW_NUMBER() OVER(ORDER BY A.REGDT) NUM, A.*  ";
            sSql += "                              FROM NOTICE A  ";
            sSql += "                                WHERE A.NOTICE_YN = 'n'  ";
            if (Opt == "ALL")
            {
                sSql += "                               AND ( A.TITLE LIKE '%" + SearchText + "%' OR A.CONTENT LIKE '%" + SearchText + "%') ";
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchText)) sSql += "                               AND A." + Opt + " LIKE '%" + SearchText + "%' ";
            }
            if (!string.IsNullOrEmpty(Type)) sSql += "                               AND A.TYPE = '" + Type + "' ";
            sSql += "                             ORDER BY A.REGDT DESC )   ";
            sSql += "                      ) X  ";
            sSql += "           ) XX ";
            sSql += "  WHERE XX.PAGE = " + pageIndex + " ";

            return sSql;
        }

        public string Admin_NoticeView(string noticeID)
        {
            string sSql = "";
            sSql += " SELECT 'VIEW' AS FLAG, V.* FROM NOTICE V ";
            sSql += " WHERE V.NOTICE_ID = " + noticeID + " ";
            return sSql;
        }

        public string Admin_NoticeDel(string noticeID)
        {
            string sSql = "";
            sSql += " DELETE FROM NOTICE ";
            sSql += " WHERE NOTICE_ID = " + noticeID + " ";
            return sSql;
        }

        public string Admin_NoticeAdd(Hashtable ht)
        {
            string sSql = "";
            sSql += " INSERT INTO NOTICE VALUES ( ";
            sSql += "  (SELECT NVL(MAX(NOTICE_ID), 0) + 1 FROM NOTICE) ";   // NOTICE_ID
            sSql += " , '" + ht["TITLE"].ToString() + "'";                            //TITLE
            sSql += " , 0";                                                             //CNT
            sSql += " , '관리자'";                                                     //WRITER
            sSql += " , '" + ht["USE_YN"].ToString() + "'";                       //USE_YN
            sSql += " , '" + ht["NOTICE_YN"].ToString() + "'";                  //NOTICE_YN
            sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD')";               //REGDT
            sSql += " , '' ";                                                           //EDITDT
            sSql += " , '" + ht["FILE"].ToString() + "'";                          //FILE
            sSql += " , '" + ht["FILE_NAME"].ToString() + "'";                 //FILE_NAME
            sSql += " , '" + ht["FILE1"].ToString() + "'";                          //FILE1
            sSql += " , '" + ht["FILE1_NAME"].ToString() + "'";                 //FILE1_NAME
            sSql += " , '" + ht["FILE2"].ToString() + "'";                          //FILE2
            sSql += " , '" + ht["FILE2_NAME"].ToString() + "'";                 //FILE2_NAME
            sSql += " , '" + ht["CONTENT"].ToString() + "'";                 //CONTENT
            sSql += " , '" + ht["S_TYPE"].ToString() + "'";                 //TYPE
            sSql += " ) ";
            return sSql;
        }

        public string Admin_NoticeModify(Hashtable ht)
        {
            string sSql = "";
            sSql += " UPDATE NOTICE SET " + "\r\n";
            sSql += "     TITLE = '" + ht["TITLE"].ToString() + "' " + "\r\n";
            sSql += "    , USE_YN = '" + ht["USE_YN"].ToString() + "' " + "\r\n";
            sSql += "    , NOTICE_YN = '" + ht["NOTICE_YN"].ToString() + "' " + "\r\n";
            sSql += "    , EDITDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') " + "\r\n";
            if (ht.ContainsKey("FILE"))
            {
                sSql += "    , \"FILE\" = '" + ht["FILE"].ToString() + "'" + "\r\n";                          //FILE
                sSql += "    , FILE_NAME = '" + ht["FILE_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE1"))
            {
                sSql += "    , FILE1 = '" + ht["FILE1"].ToString() + "'";                          //FILE
                sSql += "    , FILE1_NAME = '" + ht["FILE1_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            if (ht.ContainsKey("FILE2"))
            {
                sSql += "    , FILE2 = '" + ht["FILE2"].ToString() + "'" + "\r\n";                          //FILE
                sSql += "    , FILE2_NAME = '" + ht["FILE2_NAME"].ToString() + "'" + "\r\n";                          //FILE
            }
            sSql += "    , CONTENT = '" + ht["CONTENT"].ToString() + "'" + "\r\n";                 //FILE2_NAME
            sSql += "    , TYPE = '" + ht["S_TYPE"].ToString() + "'" + "\r\n";                 //TYPE
            sSql += " WHERE NOTICE_ID = " + ht["NOTICE_ID"].ToString() + " " + "\r\n";
            return sSql;
        }

        public string Admin_MemberSearch()
        {
            string sSql = "";
            sSql += " SELECT * FROM ( ";
            sSql += " SELECT ROWNUM AS RNUM ";
            sSql += "         , MEMB_NO ";
            sSql += "         , M_ID ";
            sSql += "         , \"LEVEL\" AS LVL ";
            sSql += "         , AUTH_LEVEL ";
            sSql += "         , STATUS ";
            sSql += "         ,  CryptString.decrypt(PASSWORD, '" + memberKey + "') AS PWD ";
            sSql += "         , \"NAME\" AS M_NAME ";
            sSql += "         , MOBILE ";
            sSql += "         , SUBSTR(NVL(REGDT, ''), 0, 10) AS REGDT ";
            sSql += "         , LAST_LOGIN ";
            sSql += "  FROM MEMBER ";
            sSql += " WHERE NVL(DEL_FLAG, 'n') = 'n' ";
            sSql += "  )  ORDER BY RNUM DESC ";
            return sSql;
        }

        public string Admin_MemberSelect(string id)
        {
            string sSql = "";
            sSql += " SELECT MEMB_NO ";
            sSql += "         , M_ID ";
            sSql += "         , \"LEVEL\" AS LVL ";
            sSql += "         , AUTH_LEVEL ";
            sSql += "         , STATUS ";
            sSql += "         ,  CryptString.decrypt(PASSWORD, '" + memberKey + "') AS PWD ";
            sSql += "         , \"NAME\" AS M_NAME ";
            sSql += "         , MOBILE ";
            sSql += "         , SUBSTR(NVL(REGDT, ''), 0, 10) AS REGDT ";
            sSql += "         , LAST_LOGIN ";
            sSql += "  FROM MEMBER ";
            sSql += " WHERE MEMB_NO = " + id + " ";
            return sSql;
        }

        public string Admin_MemberUpdate(Hashtable ht)
        {
            string sSql = "";
            sSql += " UPDATE MEMBER ";
            sSql += " SET ";
            if (ht.ContainsKey("DEL_FLAG"))
            {
                if (ht["DEL_FLAG"].ToString() == "y")
                {
                    sSql += "       DEL_FLAG = 'y' ";
                    sSql += "     , DELDATE = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
                }
                else
                {

                    sSql += "         \"NAME\" = '" + ht["NAME"].ToString() + "' ";
                    if (!string.IsNullOrEmpty(ht["PASSWORD"].ToString())) sSql += "         , PASSWORD = CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";
                    sSql += "         , MOBILE = '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
                    sSql += "         , REGDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
                }
            }
            else
            {
                sSql += "         \"NAME\" = '" + ht["NAME"].ToString() + "' ";
                if (!string.IsNullOrEmpty(ht["PASSWORD"].ToString())) sSql += "         , PASSWORD = CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";
                sSql += "         , MOBILE = '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
                sSql += "         , REGDT = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
            }

            sSql += " WHERE MEMB_NO = '" + ht["MEMB_NO"].ToString() + "' ";
            return sSql;
        }

        public string Admin_MemberAdd(Hashtable ht)
        {
            string sSql = "";
            sSql += " INSERT INTO MEMBER VALUES ( ";
            sSql += " (SELECT NVL(MAX(MEMB_NO), 0) + 1 FROM MEMBER) ";
            sSql += " , '" + ht["M_ID"].ToString() + "' ";
            sSql += " , 50 ";   //level
            sSql += " , '' ";   // auth_level
            sSql += " , '' ";   // status
            sSql += " , CryptString.encrypt('" + ht["PASSWORD"].ToString() + "', '" + memberKey + "') ";
            sSql += " , '" + ht["NAME"].ToString() + "' ";
            sSql += " , '" + ht["MOBILE1"].ToString() + "-" + ht["MOBILE2"].ToString() + "-" + ht["MOBILE3"].ToString() + "' ";
            sSql += " , TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
            sSql += " , '' ";   // last_login
            sSql += " , '' ";   // last_loginIP
            sSql += " , 0 ";   // cnt_login
            sSql += " , '' ";   // deldate
            sSql += " , '' ";   // drop_comment
            sSql += " , '' ";   // del_flag
            sSql += " ) ";
            return sSql;
        }

        public string Admin_Login(string id, string pwd)
        {
            string sSql = "";
            sSql += " SELECT MEMB_NO FROM MEMBER ";
            sSql += " WHERE M_ID = '" + id + "' ";
            sSql += " AND CryptString.decrypt(PASSWORD, '" + memberKey + "') = '" + pwd + "' ";
            return sSql;
        }

        public string Admin_Login_Update(string id, string userIP)
        {
            string sSql = "";
            sSql += " UPDATE MEMBER ";
            sSql += " SET LAST_LOGIN = TO_CHAR(SYSDATE, 'YYYY-MM-DD hh24:mi:ss') ";
            sSql += " AND LAST_LOGIN_IP = '" + userIP + "' ";
            sSql += " WHERE MEMB_NO = '" + id + "' ";
            return sSql;
        }

        public string Admin_Member_Check(string id)
        {
            string sSql = "";
            sSql += " SELECT * FROM MEMBER ";
            sSql += " WHERE M_ID = '" + id + "' ";
            return sSql;
        }
    }
}