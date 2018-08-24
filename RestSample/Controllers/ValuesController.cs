using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using RestSample.Models;
using RestSample.Services;

namespace RestSample.Controllers
{

    public class ValuesController : ApiController
    {
        // GET api/values
        private sampleRepository _sampleRepository;

        public ValuesController()
        {
            this._sampleRepository = new sampleRepository();
        }

        string JSonFile = HttpContext.Current.Server.MapPath("~/sample.json");
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            var jsonfile = File.ReadAllText(JSonFile);
            var jsonObject = JObject.Parse(jsonfile);
            return Request.CreateResponse(HttpStatusCode.OK, jsonObject);
        }

        [HttpPost]
        public HttpResponseMessage Add(SampleModel sample)
        {
            try
            {
                var json = File.ReadAllText(JSonFile);
                var jsonObj = JObject.Parse(json);
                var sampleArrary = jsonObj.GetValue("sample") as JArray;
                int ID = 0;

                if (sampleArrary.Count > 0)
                    ID = sampleArrary.Max(obj => obj["Id"].Value<int>()) + 1;
                var newTransMember = "{ 'Id': " + ID + ", 'ApplicationId': " + sample.ApplicationId + ",'Type':'" + sample.Type + "','Summary':'" + sample.Summary + "','Amount':" + sample.Amount + ",'PostingDate':'" + sample.PostingDate + "','IsCleared':'" + sample.IsCleared + "','ClearedDate':'" + sample.ClearedDate + "' }";

                var newTrans = JObject.Parse(newTransMember);
                sampleArrary.Add(newTrans);

                jsonObj["sample"] = sampleArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                       Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(JSonFile, newJsonResult);
                return Request.CreateResponse(HttpStatusCode.OK, "Created Successfully");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }

        }

        [HttpPut]
        public HttpResponseMessage Update(SampleModel sample)
        {
            try
            {
                var json = File.ReadAllText(JSonFile);
                var jObject = JObject.Parse(json);
                JArray sampleArrary = (JArray)jObject["sample"];

                if (sample.Id > 0)
                {
                    foreach (var tran in sampleArrary.Where(a => a["Id"].Value<int>() == sample.Id))
                    {
                        tran["ApplicationId"] = sample.ApplicationId;
                        tran["PostingDate"] = string.IsNullOrEmpty(sample.PostingDate) ? "": sample.PostingDate ;
                        tran["Type"] = string.IsNullOrEmpty(sample.Type) ?  "": sample.Type ;
                        tran["Summary"] = string.IsNullOrEmpty(sample.Summary) ?  "":sample.Summary ;
                        tran["ClearedDate"] = string.IsNullOrEmpty(sample.ClearedDate) ? "": sample.ClearedDate;
                        tran["Amount"] = sample.Amount;
                        tran["IsCleared"] = sample.IsCleared;
                    }

                    jObject["sample"] = sampleArrary;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(JSonFile, output);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpDelete]

        public void Delete(SampleModel sample)
        {
            var jsonobject = File.ReadAllText(JSonFile);
            var jObject = JObject.Parse(jsonobject);
            JArray resArrary = (JArray)jObject["sample"];

            if (sample.Id > 0)
            {
                var tranToDeleted = resArrary.FirstOrDefault(obj => obj["Id"].Value<int>() == sample.Id);
                resArrary.Remove(tranToDeleted);
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(JSonFile, output);
            }
        }
    }
}

