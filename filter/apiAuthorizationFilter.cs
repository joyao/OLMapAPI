using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;



namespace OLMapAPI.filter
{
    public class apiAuthorizationFilter : AuthorizationFilterAttribute
    {
        /// <summary>
        /// The authorization service
        /// </summary>
        private IAuthorizationService authorizationService = new CustomAuthorizationService();

        /// <summary>
        /// The authenticated username
        /// </summary>
        //private const string authenticatedUsername = "Kevin";

        /// <summary>
        /// 在處理序要求授權時呼叫。
        /// </summary>
        /// <param name="actionContext">動作內容，該內容封裝 <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" /> 的使用資訊。</param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool isAuthorizated = false;

            //// 從Thread取出IPrincipal
            IPrincipal principal = Thread.CurrentPrincipal;
            isAuthorizated = authorizationService.IsAuthorizated(principal, "");

            if (SkipAuthorization(actionContext))
            {
                return;
            }

            if (!isAuthorizated)
            {
                //// CreateResponse是System.Net.Http命名空間的擴充方法
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (!Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()))
                return Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>());
            else
                return true;
        }
    }

    /// <summary>
    /// admin Filter
    /// </summary>
    public class apiAdminFilter : AuthorizationFilterAttribute
    {
        private IAuthorizationService authorizationService = new CustomAuthorizationService();
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            bool isAuthorizated = false;
            IPrincipal principal = Thread.CurrentPrincipal;
            isAuthorizated = authorizationService.IsAuthorizated(principal, "admin");
            if (SkipAuthorization(actionContext)) { return; }
            if (!isAuthorizated) { actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized); }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (!Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()))
                return Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>());
            else
                return true;
        }
    }

    ///// <summary>
    ///// drinkPointM Filter
    ///// </summary>
    //public class apilv2DrinkPointMFilter : AuthorizationFilterAttribute
    //{
    //    private IAuthorizationService authorizationService = new CustomAuthorizationService();
    //    public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
    //    {
    //        bool isAuthorizated = false;
    //        IPrincipal principal = Thread.CurrentPrincipal;
    //        isAuthorizated = authorizationService.IsAuthorizated(principal, "lv2_drinkPointM");
    //        if (SkipAuthorization(actionContext)) { return; }
    //        if (!isAuthorizated) { actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized); }
    //    }
    //    private static bool SkipAuthorization(HttpActionContext actionContext)
    //    {
    //        if (!Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()))
    //            return Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>());
    //        else
    //            return true;
    //    }
    //}

    ///// <summary>
    ///// 可以設定權限的Filter
    ///// </summary>
    //public class roleSettingPowerFilter : AuthorizationFilterAttribute
    //{
    //    /// <summary>
    //    /// The authorization service
    //    /// </summary>
    //    private IAuthorizationService authorizationService = new CustomAuthorizationService();
    //    public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
    //    {
    //        bool isAuthorizated = false;

    //        //// 從Thread取出IPrincipal
    //        IPrincipal principal = Thread.CurrentPrincipal;
    //        bool lv1_cloudM_YN = authorizationService.IsAuthorizated(principal, "lv1_cloudM");
    //        bool lv2_Admin_YN = authorizationService.IsAuthorizated(principal, "lv2_Admin");
    //        bool lv2_smallAreaM_YN = authorizationService.IsAuthorizated(principal, "lv2_smallAreaM");
    //        bool lv2_inpM_YN = authorizationService.IsAuthorizated(principal, "lv2_inpM");

    //        if (lv1_cloudM_YN || lv2_Admin_YN || lv2_smallAreaM_YN || lv2_inpM_YN)
    //        {
    //            isAuthorizated = true;
    //        }

    //        if (SkipAuthorization(actionContext))
    //        {
    //            return;
    //        }

    //        if (!isAuthorizated)
    //        {
    //            //// CreateResponse是System.Net.Http命名空間的擴充方法
    //            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
    //        }
    //    }

    //    private static bool SkipAuthorization(HttpActionContext actionContext)
    //    {
    //        if (!Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()))
    //            return Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>());
    //        else
    //            return true;
    //    }
    //}

    /// <summary>
    /// IAuthorizationService
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// 檢查該使用者的名稱是否有權限
        /// </summary>
        bool IsAuthorizated(IPrincipal principal, string checkrole);
    }

    /// <summary>
    /// CustomAuthorizationService
    /// </summary>
    public class CustomAuthorizationService : IAuthorizationService
    {
        public bool IsAuthorizated(IPrincipal principal, string checkrole)
        {
            string userId = principal.Identity.Name;
            if (userId == "")
            {
                return false;
            }
            else if (userId != "" && checkrole == "")
            {
                return true;
            }
            else
            {
                return principal.IsInRole(checkrole);
            }
        }
    }

}