using OLMapAPI.Infrastructure.BasicInfo;
using OLMapAPI.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OLMapAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Layers")]
    public class LayersController : ApiController
    {

        /// <summary>
        /// 查詢所有圖層資料
        /// </summary>
        /// <returns></returns>
        [Route("getLayerResource")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<LayerResourceList>))]
        public async Task<HttpResponseMessage> getLayerResource()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, await BasicInfoFunc.getLayerResourceList());

            }
            catch (Exception SqlException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }

        }

    }
}
