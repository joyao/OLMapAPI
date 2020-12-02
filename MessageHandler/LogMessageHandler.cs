using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Generic;
using NLog;
using OLMapAPI.Infrastructure.auth;

namespace OLMapAPI.MessageHandler
{
    ////自訂一個 Nlog extension
    //public static class NLogExtension
    //{
    //    public static void LogExt(this NLog.Logger logger, NLog.LogLevel level, String message, String userName, string columnName)
    //    {
    //        NLog.LogEventInfo theEvent = new NLog.LogEventInfo(level, logger.Name, message);
    //        theEvent.Properties[columnName] = userName;
    //        logger.Log(theEvent);
    //    }
    //}

    public class actionlog : ILog
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void Save(string logContent)
        {
            // 原本 Nlog 的寫法
            logger.Info(logContent);

            //// 擷取自訂訊息
            //List<string> singleAttribute = logContent.Split(',').ToList<string>();

            //string name = singleAttribute[0].Trim(new Char[] { '{', '}' }).Split(new char[] { ':' }, 2)[1];
            //string ip = singleAttribute[1].Trim(new Char[] { '{', '}' }).Split(new char[] { ':' }, 2)[1];

            //// 紀錄自訂訊息
            //logger.LogExt(LogLevel.Info, logContent, name, "UserName"); //userid



        }
    }


    public class actionlogISerializer : ISerializer
    {
        public string Serialize<T>(RequestLogInfo info)
        {
            //return info.IpAddress + "|" + info.RequestTime + "|" + info.Signature + "|" + info.HttpMethod + "|" + info.UrlAccessed + "|" + info.BodyContent;
            return "{userid:" + info.Signature + "},{ip:" + info.IpAddress + "},{method:" + info.HttpMethod + "},{time:" + info.RequestTime.ToString("yyyy/MM/dd HH:mm:ss") + "},{request:" + info.UrlAccessed + "}";

        }
        public string Serialize<T>(ResponseLogInfo info)
        {
            return info.ReturnCode + "|" + info.ReturnMessage + "|" + info.ResponseTime + "|" + info.BodyContent;
        }
    }

    public interface ILog
    {
        void Save(string logContent);
    }

    public interface ISerializer
    {
        string Serialize<T>(RequestLogInfo info);
        string Serialize<T>(ResponseLogInfo info);
    }


    public class LogMessageHandler : MessageProcessingHandler
    {
        private ILog _log;
        private ISerializer _serializer;

        public LogMessageHandler(ILog log, ISerializer serializer)
        {
            this._log = log;
            this._serializer = serializer;
        }

        /// <summary>
        /// 將 request 相關訊息記錄 log
        /// </summary>
        /// <param name="request">The HTTP request message to process.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Net.Http.HttpRequestMessage" />.The HTTP request message that was processed.
        /// </returns>
        private const string _header = "Authorization";
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            //由token換成userid

            string userid = "null";
            var token = Enumerable.Empty<string>();
            bool isHeaderExist = request.Headers.TryGetValues(_header, out token);
            if (isHeaderExist)
            {
                if (token.Count() > 0)
                {
                    authFunc auth = new authFunc();
                    userid = auth.validatesToken(token.First());
                }
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var info = new RequestLogInfo
            {
                HttpMethod = request.Method.Method,
                UrlAccessed = request.RequestUri.AbsoluteUri,
                IpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : "0.0.0.0",
                RequestTime = DateTime.Now,
                Token = this.GetToken(request),
                Signature = userid,
                Timestamp = this.GetTimestamp(request),
                BodyContent = request.Content == null ? string.Empty : request.Content.ReadAsStringAsync().Result
            };

            var logContent = this._serializer.Serialize<RequestLogInfo>(info);
            this._log.Save(logContent);

            return request;
        }

        /// <summary>
        /// 將 response 相關訊息記錄 log
        /// </summary>
        /// <param name="response">The HTTP response message to process.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Net.Http.HttpResponseMessage" />.The HTTP response message that was processed.
        /// </returns>
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, System.Threading.CancellationToken cancellationToken)
        {
            //if (response == null)
            //{
            //    throw new ArgumentNullException("response");
            //}

            //var info = new ResponseLogInfo
            //{
            //    StatusCode = ((int)response.StatusCode).ToString(),
            //    ResponseTime = DateTime.Now,
            //    ReturnCode = this.GetReturnCode(response),
            //    ReturnMessage = this.GetReturnMessage(response),
            //    Signature = this.GetSignature(response),
            //    BodyContent = response.Content == null ? string.Empty : response.Content.ReadAsStringAsync().Result
            //};

            //var logContent = this._serializer.Serialize<ResponseLogInfo>(info);
            //this._log.Save(logContent);

            return response;
        }

        private string GetReturnCode(HttpResponseMessage response)
        {
            return HeaderHelper.GetHeaderValue(response.Headers, "api-returnCode").Item2;
        }

        private string GetReturnMessage(HttpResponseMessage response)
        {
            return HeaderHelper.GetHeaderValue(response.Headers, "api-returnMessage").Item2;
        }

        private string GetSignature(HttpResponseMessage response)
        {
            return HeaderHelper.GetHeaderValue(response.Headers, "api-signature").Item2;
        }

        private string GetSignature(HttpRequestMessage request)
        {
            return HeaderHelper.GetHeaderValue(request.Headers, "api-signature").Item2;
        }

        private string GetTimestamp(HttpRequestMessage request)
        {
            return HeaderHelper.GetHeaderValue(request.Headers, "api-timestamp").Item2;
        }

        private string GetToken(HttpRequestMessage request)
        {
            return HeaderHelper.GetHeaderValue(request.Headers, "api-token").Item2;
        }
    }

    public class RequestLogInfo
    {
        public string BodyContent { get; set; }

        public string HttpMethod { get; set; }

        public string IpAddress { get; set; }

        public DateTime RequestTime { get; set; }

        public string Signature { get; set; }

        public string Timestamp { get; set; }

        public string Token { get; set; }

        public string UrlAccessed { get; set; }
    }

    public class ResponseLogInfo
    {
        public string BodyContent { get; set; }

        public DateTime ResponseTime { get; set; }

        public string ReturnCode { get; set; }

        public string ReturnMessage { get; set; }

        public string Signature { get; set; }

        public string StatusCode { get; set; }
    }

    internal class HeaderHelper
    {
        internal static Tuple<bool, string> GetHeaderValue(HttpResponseHeaders httpResponseHeaders, string headerName)
        {
            var result = string.Empty;
            var specialHeaders = Enumerable.Empty<string>();
            var isExistHeader = httpResponseHeaders.TryGetValues(headerName, out specialHeaders);

            if (isExistHeader)
            {
                result = specialHeaders.LastOrDefault();
            }

            return Tuple.Create(isExistHeader, result);
        }

        internal static Tuple<bool, string> GetHeaderValue(HttpRequestHeaders httpRequestHeaders, string headerName)
        {
            var result = string.Empty;
            var specialHeaders = Enumerable.Empty<string>();
            var isExistHeader = httpRequestHeaders.TryGetValues(headerName, out specialHeaders);

            if (isExistHeader)
            {
                result = specialHeaders.LastOrDefault();
            }

            return Tuple.Create(isExistHeader, result);
        }
    }
}
