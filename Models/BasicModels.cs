using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OLMapAPI.Models
{
    public class BasicModels
    {
    }
    public class Keyword
    {
        public string keyword { get; set; } //關鍵字
    }
    public class LayerResourceList
    {
        public string ID { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string LayerID { get; set; }
        public string LayerOrder { get; set; }
        public string LayerQueryable { get; set; }
        public string LayerTitle { get; set; }
        public string LayerType { get; set; }
        public string DataType { get; set; }
        public string DataURL { get; set; }
        public string LayerVisibleCode { get; set; }
        public string OpenOpacity { get; set; }
    }

    public class UserDrawInput
    {
        public string SQLtype { get; set; }
        public string title { get; set; }
        public string info { get; set; }
        public string drawsaveid { get; set; }
        public List<DrawGraphicList> features { get; set; }
    }

    public class DrawGraphicList
    {
        public string geom { get; set; }
        public string style { get; set; }
    }

    public class UserDrawOutput : DrawGraphicList
    {
        public string userid { get; set; }
        public string drawsaveid { get; set; }
        public string title { get; set; }
        public string info { get; set; }
    }

    public class UserDrawCaseOutput
    {
        public string drawsaveid { get; set; }
        public string DDate { get; set; }
        public string title { get; set; }
        public string info { get; set; }
    }

    // input資料-測站輸入
    public class GPSInputData
    {
        public string sdate { get; set; } //開始時間
        public string edate { get; set; } //結束時間
        public string gpsid { get; set; } //結束時間
    }

    /// <summary>
    /// GPS測站資料內容
    /// </summary> 
    public class GPSDataList
    {
        public string year { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string hgt { get; set; }
        public string dN { get; set; }
        public string dE { get; set; }
        public string dU { get; set; }
    }

    /// <summary>
    /// GPS測站列表
    /// </summary> 
    public class GPSStationList
    {
        public string gpsid { get; set; }
        public string sdate { get; set; }
        public string edate { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string distance { get; set; }
    }
}