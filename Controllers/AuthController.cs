using OLMapAPI.Infrastructure.auth;
using OLMapAPI.Models.auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OLMapAPI.Controllers
{
    /// <summary>
    /// VS驗證
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        /// <summary>
        /// 由帳號密碼返回token
        /// </summary>
        /// <remarks>使用已註冊的帳號密碼取得token</remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Not found</response>
        //介接api用的帳號，僅提供token返回，不提供token取得資訊，該token僅可使用一般API
        [Route("validatesApiUser")]
        [HttpPost, AllowAnonymous]
        public tokenObj validatesApiUser(loginInfo login)
        {
            authFunc auth = new authFunc();
            tokenObj token = auth.validatesApiUser(login);
            return token;
        }
    }
}
