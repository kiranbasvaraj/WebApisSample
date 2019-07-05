using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using WebApisSample.Attributes;
using WebApisSample.Models;
using WebApisSample.Services;

namespace WebApisSample.Controllers
{
    public class SpeechController : ApiController
    {
        DataAccess objda = new DataAccess();
        string ConnectionString = "mydatabase";

        [HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public HttpResponseMessage GetSpeakers(string UserId)
        {

            JsonMediaTypeFormatter formatter = null;
            try
            {
                formatter = JsonFormator.GetFormator();
                // string sql = "select MP17Key,POID,POAmount,MP16Vendor,VendorName from MP17PurchaseOrder P inner join MP16Vendor V on V.MP16Key=P.MP16Vendor where P.MP17Status=0 and V.Status='A' and V.MP16Status=0 and POAmount>" + id.ToString() + "";
                string sql = "exec getSpeakers" + " " + UserId;
                DataSet ds = new DataSet();
                ds = objda.GetDatasetForCCS(ConnectionString, sql);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    List<SpeakersModel> SpeakerList = new List<SpeakersModel>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SpeakersModel spk = new SpeakersModel();
                        spk.UserId = dr["UserId"].ToString();
                        spk.UserName = dr["UserName"].ToString();
                        spk.About = dr["About"].ToString();
                        spk.SpeechDateTime = dr["SpeechDateTime"].ToString();
                        spk.SpeechId = dr["SpeechId"].ToString();
                        spk.SpeakerId = dr["SpeakerId"].ToString();
                        SpeakerList.Add(spk);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, SpeakerList, formatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Content", formatter);
                }
            }
            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, exc.Message.ToString(), formatter);
            }

        }


        [HttpPost]
        [BasicAuthentication(RequireSsl = false)]
        public HttpResponseMessage CreateUser(string userName, string mobileNumber, string password)
        {
            JsonMediaTypeFormatter formatter = null;
            try
            {
                formatter = JsonFormator.GetFormator();
                // string sql = "select MP17Key,POID,POAmount,MP16Vendor,VendorName from MP17PurchaseOrder P inner join MP16Vendor V on V.MP16Key=P.MP16Vendor where P.MP17Status=0 and V.Status='A' and V.MP16Status=0 and POAmount>" + id.ToString() + "";
                string sqlformat = "declare @output int;    EXECUTE user_creationSp '" + userName + "','" + mobileNumber + "','+" + password + "',	@output  output 	select @output as res";
                string sql = string.Format(sqlformat, userName, mobileNumber, password);

                DataSet ds = new DataSet();
                ds = objda.GetDatasetForCCS(ConnectionString, sql);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    List<SpeakersModel> SpeakerList = new List<SpeakersModel>();
                    string res = string.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        res = dr["res"].ToString();

                    }
                    if (res == "1")
                        return Request.CreateResponse(HttpStatusCode.OK, "user created", formatter);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, "user already exist", formatter);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Content", formatter);

                }
            }
            catch (Exception exc)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, exc.Message.ToString(), formatter);


            }
        }



        
    }
}
