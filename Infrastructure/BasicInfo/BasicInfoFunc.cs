using System;
using System.Collections.Generic;
using OLMapAPI.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using OLMapAPI.Models.auth;
using System.Threading.Tasks;
using System.IO;

namespace OLMapAPI.Infrastructure.BasicInfo
{
    public class BasicInfoFunc
    {

        public static async Task<List<LayerResourceList>> getLayerResourceList()
        {
            SqlDataReader dr = null;
            SqlConnection myConnection = new SqlConnection();
            string Constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            myConnection.ConnectionString = Constr;

            SqlCommand sqlCmd = new SqlCommand();

            string sqlStr;
            sqlStr = "SELECT [ID] ,[GroupID] ,[GroupName] ,[LayerID] ,[LayerOrder] ,[LayerQueryable] ,[LayerTitle] ,[LayerType],[DataType] ,[DataURL] ,[LayerVisibleCode] ,[OpenOpacity]  FROM [OLDemo].[dbo].[LayerResource]  order by [GroupID], [LayerOrder], [LayerType]";

            sqlCmd.CommandText = sqlStr;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = myConnection;
            List<LayerResourceList> arrList = new List<LayerResourceList>();

            try
            {
                myConnection.Open();
                dr = sqlCmd.ExecuteReader();

                while (dr.Read())
                {
                    arrList.Add(new LayerResourceList()
                    {
                        ID = dr["ID"].ToString(),
                        GroupID = dr["GroupID"].ToString(),
                        GroupName = dr["GroupName"].ToString(),
                        LayerID = dr["LayerID"].ToString(),
                        LayerOrder = dr["LayerOrder"].ToString(),
                        LayerQueryable = dr["LayerQueryable"].ToString(),
                        LayerTitle = dr["LayerTitle"].ToString(),
                        LayerType = dr["LayerType"].ToString(),
                        DataType = dr["DataType"].ToString(),
                        DataURL = dr["DataURL"].ToString(),
                        LayerVisibleCode = dr["LayerVisibleCode"].ToString(),
                        OpenOpacity = dr["OpenOpacity"].ToString()
                    });
                }
                myConnection.Close();
                myConnection.Dispose();
                return arrList;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static async Task<List<UserDrawOutput>> UserDrawFeaturesSQL(DrawGraphicList UserDraw, string userid, string SQLtype, string title, string info, string drawsaveid)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            string Constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            myConnection.ConnectionString = Constr;

            SqlCommand sqlCmd = new SqlCommand();

            string sqlStr;
            sqlStr = "exec [dbo].[procUserDrawSQL] @SQLtype=@p1,@userid=@p2,@drawsaveid=@p3,@title=@p4,@info=@p5,@geomstr=@p6,@stylestr=@p7 ";
            sqlCmd.Parameters.AddWithValue("@p1", SQLtype.Trim());
            sqlCmd.Parameters.AddWithValue("@p2", userid.Trim());
            sqlCmd.Parameters.AddWithValue("@p3", drawsaveid.Trim());
            sqlCmd.Parameters.AddWithValue("@p4", title.Trim());
            sqlCmd.Parameters.AddWithValue("@p5", info == null ? "" : info.Trim());
            sqlCmd.Parameters.AddWithValue("@p6", UserDraw == null? "" : (UserDraw.geom == null ? "" : UserDraw.geom.Trim()));
            sqlCmd.Parameters.AddWithValue("@p7", UserDraw == null ? "" : (UserDraw.style == null ? "" : UserDraw.style.Trim()));


            sqlCmd.CommandText = sqlStr;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = myConnection;
            List<UserDrawOutput> data = new List<UserDrawOutput> { };

            try
            {
                myConnection.Open();
                reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(new UserDrawOutput()
                    {
                        userid = reader["userid"].ToString(),
                        drawsaveid = reader["drawsaveid"].ToString(),
                        title = reader["title"].ToString(),
                        info = reader["info"].ToString(),
                        geom = reader["geomstr"].ToString(),
                        style = reader["stylestr"].ToString()
                    });
                }
                myConnection.Close();
                myConnection.Dispose();
                return data;

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public static async Task<List<UserDrawCaseOutput>> getUserDrawCaseList(string userid)
        {
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            string Constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            myConnection.ConnectionString = Constr;

            SqlCommand sqlCmd = new SqlCommand();

            string sqlStr;
            sqlStr = "SELECT * FROM (SELECT [drawsaveid],[DDate],[title],[info],ROW_NUMBER() OVER (PARTITION BY [drawsaveid] ORDER BY [DDate]) AS RowNumber FROM [OLDemo].[dbo].[UserDrawSave] where [userid]=@userid ) AS a WHERE a.RowNumber = 1";
            sqlCmd.Parameters.AddWithValue("@userid", userid.Trim());

            sqlCmd.CommandText = sqlStr;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = myConnection;
            List<UserDrawCaseOutput> data = new List<UserDrawCaseOutput> { };

            try
            {
                myConnection.Open();
                reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    data.Add(new UserDrawCaseOutput()
                    {
                        drawsaveid = reader["drawsaveid"].ToString(),
                        DDate = reader["DDate"].ToString(),
                        title = reader["title"].ToString(),
                        info = reader["info"].ToString()
                    });
                }
                myConnection.Close();
                myConnection.Dispose();
                return data;

            }
            catch (Exception ex)
            {

                throw;
            }


        }

        /// <summary>
        /// 獲取GPS測站列表
        /// </summary>
        public static async Task<List<GPSStationList>> getGPSList(Keyword keyword)
        {
            List<GPSStationList> GPSStationList = new List<GPSStationList>();
            string SqlStr;
            if (keyword.keyword == "*")
            {
                SqlStr = @"SELECT Station, sdate ,edate, lon, lat FROM [OLDemo].[dbo].[GPS_STATION] order by Station";
            }
            else
            {
                SqlStr = @"SELECT Station, sdate ,edate, lon, lat FROM [OLDemo].[dbo].[GPS_STATION] where County='" + keyword.keyword + "' order by Station";
            }

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand(SqlStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                GPSStationList GPSStation = new GPSStationList();
                GPSStation.gpsid = dr["Station"].ToString();
                GPSStation.sdate = dr["sdate"].ToString();
                GPSStation.edate = dr["edate"].ToString();
                GPSStation.lon = dr["lon"].ToString();
                GPSStation.lat = dr["lat"].ToString();

                GPSStationList.Add(GPSStation);
            }
            dr.Close(); dr.Dispose(); conn.Close(); conn.Dispose();
            return GPSStationList;
        }

        /// <summary>
        /// 獲取GPS測站歷史資料列表
        /// </summary>
        public static async Task<List<GPSDataList>> getGPSbyID(GPSInputData parms)
        {
            String line;
            List<GPSDataList> GPSData = new List<GPSDataList>();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                string gpsPath = ConfigurationManager.ConnectionStrings["gpsPath"].ConnectionString;
                StreamReader sr = new StreamReader(gpsPath + parms.gpsid + ".COR");

                //Read the first line of text
                line = sr.ReadLine();
                Int32 count = 0;
                float starthgt = 0;
                float startn = 0;
                float starte = 0;
                //Continue to read until you reach end of file
                while (line != null)
                {

                    line = sr.ReadLine();
                    line = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(line, " ");
                    char[] delimiterChars = { ' ', '\t' };
                    string[] words = line.Split(delimiterChars);

                    if (Int32.Parse(words[0]) >= Int32.Parse(parms.sdate) && Int32.Parse(words[0]) <= Int32.Parse(parms.edate))
                    {
                        GPSDataList GPS = new GPSDataList();
                        if (count == 0)
                        {
                            starthgt = float.Parse(words[3]);
                            startn = float.Parse(words[4]);
                            starte = float.Parse(words[5]);
                        }
                        float calhgt = (float.Parse(words[3]) - starthgt) * 1000;
                        float caln = (float.Parse(words[4]) - startn);
                        float cale = (float.Parse(words[5]) - starte);
                        GPS.year = words[0];
                        GPS.lat = words[1];
                        GPS.lon = words[2];
                        GPS.hgt = calhgt.ToString();
                        GPS.dN = caln.ToString();
                        GPS.dE = cale.ToString();
                        GPS.dU = words[6];
                        GPSData.Add(GPS);
                        count++;
                    }

                }

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception r)
            {
                Console.WriteLine("Exception: " + r.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            //return GPSData;
            //string jsonData = JsonConvert.SerializeObject(GPSData);
            //System.Console.WriteLine(jsonData);

            return GPSData;
        }


    };
}