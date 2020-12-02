using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using System.Web.Http.Cors;
using OLMapAPI.MessageHandler;
using OLMapAPI.filter;

namespace OLMapAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務
            // 將 Web API 設定成僅使用 bearer 權杖驗證。
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new AuthMessageHandler());
            config.MessageHandlers.Add(new LogMessageHandler(new actionlog(), new actionlogISerializer()));

            /*允許所有的方法跨域*/
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));


            //註冊整個專案的Filter
            string lockYN = ConfigurationManager.ConnectionStrings["lockYN"].ConnectionString;
            if (lockYN == "Y")
            {
                config.Filters.Add(new apiAuthorizationFilter());
            }
        }
    }
}
