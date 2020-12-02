using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using OLMapAPI.Models;
using OLMapAPI.Infrastructure.auth;

namespace OLMapAPI.MessageHandler
{
    public class AuthMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Header名稱預設為「APIKey」
        /// </summary>
        private const string _header = "Authorization";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //// 檢查request裡是否有「Authorization」這個Header
            var token = Enumerable.Empty<string>();
            bool isHeaderExist = request.Headers.TryGetValues(_header, out token);
            if (isHeaderExist)
            {
                if (token.Count() > 0)
                {
                    authFunc auth = new authFunc();
                    string userid = auth.validatesToken(token.First());
                    this.SetPrincipal(userid);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 設定IPrincipal
        /// </summary>
        private void SetPrincipal(string userid)
        {
            //// 設定使用者識別 => 就是使用者名稱啦
            //// GenericIdentity.IsAuthenticated 預設為true
            GenericIdentity identity = new GenericIdentity(userid);

            account acc = new account();
            String[] mMyStringArray = acc.ListUserRoles(userid).ToArray();//{ "admin" };
            //// 將使用者的識別與其所屬群組設定到GenericPrincipal類別上
            GenericPrincipal principal = new GenericPrincipal(identity, mMyStringArray);

            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}