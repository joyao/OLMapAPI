using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLMapAPI.Models.auth
{
    public class loginInfo
    {
        public string userid;
        public string password;
        public loginInfo(string _userid, string _password)
        {
            userid = _userid;
            password = _password;
        }
        public loginInfo() { }
    }

    /// <summary>
    /// 權限設定input
    /// </summary> 
    public class tokenObj
    {
        public string status;
        public string token;
        public tokenObj(string _status, string _token)
        {
            status = _status;
            token = _token;
        }
        public tokenObj() { }
    }

}