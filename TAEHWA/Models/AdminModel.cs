using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAFX_ELVISPRIME_HOME.Models
{
    public class AdminModel
    {
        public int RNUM { get; set; }
        public int MEMB_NO { get; set; }
        public string M_ID { get; set; }
        public int LEVEL { get; set; }
        public int AUTH_LEVEL { get; set; }
        public string STATUS { get; set; }
        public string PASSWORD { get; set; }
        public string NAME { get; set; }
        public string MOBILE { get; set; }
        public string MOBILE1 { get; set; }
        public string MOBILE2 { get; set; }
        public string MOBILE3 { get; set; }
        public string REGDT { get; set; }
        public string LAST_LOGIN { get; set; }
        public string LAST_LOGIN_IP { get; set; }
        public int CNT_LOGIN { get; set; }
        public string DELDATE { get; set; }
        public string DROP_COMMENT { get; set; }
        public string DEL_FLAG { get; set; }
    }
}