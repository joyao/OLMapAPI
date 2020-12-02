using OLMapAPI.Infrastructure.apiUser;
using OLMapAPI.Models.apiUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OLMapAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/apiUser")]
    public class apiUserController : ApiController
    {
        #region API User操作，不對外開放


        ///// <summary>
        ///// 取得apiUser列表
        ///// </summary>
        ///// <returns></returns>
        //[Route("getApiUserList")]
        //[HttpPost]
        //public List<apiUserObj> getApiUserList()
        //{
        //    apiUserFunc apiUserF = new apiUserFunc();
        //    return apiUserF.getApiUserList();
        //}

        ///// <summary>
        ///// 新增apiUser
        ///// </summary>
        ///// <param name="apiUser"></param>
        ///// <returns></returns>
        //[Route("insertApiUser")]
        //[HttpPost]
        //public string insertApiUser(insertApiUserObj apiUser)
        //{
        //    apiUserFunc apiUserF = new apiUserFunc();
        //    return apiUserF.insertApiUser(apiUser);
        //}

        ///// <summary>
        ///// 移除apiUser
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //[Route("removeApiUser")]
        //[HttpPost]
        //public string removeApiUser(removeApiUserObj obj)
        //{
        //    apiUserFunc apiUserF = new apiUserFunc();
        //    return apiUserF.removeApiUser(obj);
        //}

        ///// <summary>
        ///// 更新apiUser狀態(鎖1→不鎖0、不鎖0→鎖1)
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //[Route("lockApiUser")]
        //[HttpPost]
        //public string lockApiUser(lockApiUserObj obj)
        //{
        //    apiUserFunc apiUserF = new apiUserFunc();
        //    return apiUserF.lockApiUser(obj);
        //}

        #endregion

    }
}
