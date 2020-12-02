using OLMapAPI.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OLMapAPI.Infrastructure.auth;
using OLMapAPI.Infrastructure.BasicInfo;

namespace OLMapAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Basic")]
    public class BasicController : ApiController
    {
        /// <summary>
        /// 使用者圖形操作功能
        /// </summary>
        /// <returns></returns>
        [Route("UserDrawFeatures_SQL")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<UserDrawOutput>))]
        public async Task<HttpResponseMessage> UserDrawFeatures_SQL(UserDrawInput UserDraw)
        {
            bool isValidJson = authFunc.IsValidJson(UserDraw);

            string token = authFunc.parseTokenFromHeader(this.Request);
            string userid = authFunc.getUserNameByToken_true(token);

            if (!isValidJson)
            {
                string message = "非法JSON格式";
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);
                return response;
            }

            if (string.IsNullOrEmpty(UserDraw.SQLtype))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad Request");
            }
            else
            {
                UserDraw = authFunc.translateNullIntoEmpty(UserDraw);

                try
                {
                    DateTime myDate = DateTime.Now;
                    Random rnd = new Random();
                    string drawsaveid = UserDraw.drawsaveid == "" ? "G-" + myDate.ToString("yyyyMMddHHmmss") + "-" + rnd.Next(0, 10).ToString() : UserDraw.drawsaveid;
                    if (UserDraw.features.Count != 0)
                    {
                        for (int i = 0; i < UserDraw.features.Count - 1; i++)
                        {
                            await BasicInfoFunc.UserDrawFeaturesSQL(UserDraw.features[i], userid, UserDraw.SQLtype, UserDraw.title, UserDraw.info, drawsaveid);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.UserDrawFeaturesSQL(UserDraw.features[UserDraw.features.Count - 1], userid, UserDraw.SQLtype, UserDraw.title, UserDraw.info, drawsaveid));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.UserDrawFeaturesSQL(null, userid, UserDraw.SQLtype, UserDraw.title, UserDraw.info, drawsaveid));
                    }


                }
                catch (Exception SqlException)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                }

            }



        }

        /// <summary>
        /// 獲得使用者儲存繪圖列表
        /// </summary>
        /// <returns></returns>
        [Route("getUserDrawCase")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<UserDrawCaseOutput>))]
        public async Task<HttpResponseMessage> getUserDrawCase()
        {
            string token = authFunc.parseTokenFromHeader(this.Request);
            string userid = authFunc.getUserNameByToken_true(token);


            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.getUserDrawCaseList(userid));

            }
            catch (Exception SqlException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }


        }

        /// <summary>
        /// 獲取GPS測站列表
        /// </summary>
        /// <returns></returns>
        [Route("getGPSStationList")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<GPSStationList>))]
        public async Task<HttpResponseMessage> getGPSStationList(Keyword keyword)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.getGPSList(keyword));

            }
            catch (Exception SqlException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        /// <summary>
        /// 得到GPS測站歷史資料列表
        /// </summary>
        /// <returns></returns>
        [Route("getGPSDataList")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<GPSDataList>))]
        public async Task<HttpResponseMessage> getGPSDataList(GPSInputData parms)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.getGPSbyID(parms));

            }
            catch (Exception SqlException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

    }
}
