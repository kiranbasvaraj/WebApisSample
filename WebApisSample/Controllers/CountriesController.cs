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
    public class CountriesController : ApiController
    {
        DataAccess objda = new DataAccess();
        string ConnectionString = "mydatabase";

        [HttpGet]
        [BasicAuthentication(RequireSsl = false)]
        public HttpResponseMessage GetCountries()
        {
            JsonMediaTypeFormatter formatter = null;
            try
            {
                formatter= JsonFormator.GetFormator();
                // string sql = "select MP17Key,POID,POAmount,MP16Vendor,VendorName from MP17PurchaseOrder P inner join MP16Vendor V on V.MP16Key=P.MP16Vendor where P.MP17Status=0 and V.Status='A' and V.MP16Status=0 and POAmount>" + id.ToString() + "";
                string sql = "select * from [dbo].[country]";
                DataSet ds = new DataSet();
                ds = objda.GetDatasetForCCS(ConnectionString, sql);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    List<Countries> CountryList = new List<Countries>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Countries objDocketEmployeeDetails = new Countries();
                        objDocketEmployeeDetails.Id = Int32.Parse(dr["Id"].ToString());
                        objDocketEmployeeDetails.Name = dr["Name"].ToString();
                        objDocketEmployeeDetails.Name = dr["Country"].ToString();
                        CountryList.Add(objDocketEmployeeDetails);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, CountryList, formatter);
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

        public HttpResponseMessage GetCountry(string id)
        {
            JsonMediaTypeFormatter formatter = null;
            try
            {
                formatter = JsonFormator.GetFormator();
                // string sql = "select MP17Key,POID,POAmount,MP16Vendor,VendorName from MP17PurchaseOrder P inner join MP16Vendor V on V.MP16Key=P.MP16Vendor where P.MP17Status=0 and V.Status='A' and V.MP16Status=0 and POAmount>" + id.ToString() + "";
                string sql = "select * from [dbo].[country] where Id= "+id;
                DataSet ds = new DataSet();
                ds = objda.GetDatasetForCCS(ConnectionString, sql);

                if (ds.Tables[0].Rows.Count != 0)
                {
                    List<Countries> CountryList = new List<Countries>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Countries objDocketEmployeeDetails = new Countries();
                        objDocketEmployeeDetails.Id = Int32.Parse(dr["Id"].ToString());
                        objDocketEmployeeDetails.Name = dr["Name"].ToString();
                        objDocketEmployeeDetails.Name = dr["Country"].ToString();
                        CountryList.Add(objDocketEmployeeDetails);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, CountryList, formatter);
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
