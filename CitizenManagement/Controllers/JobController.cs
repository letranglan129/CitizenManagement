using CitizenManagement.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class JobController : Controller
    {
        public string GetJobs(string Id)
        {
            var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
            var people = DB.get<People>("people").Find(filter).FirstOrDefault();

            return JsonConvert.SerializeObject(people.Jobs != null ? people.Jobs : new List<Job>());
        }

        [HttpPost]
        public ActionResult Add(string Id, string values)
        {
            try
            {
                var job = new Job();
                JsonConvert.PopulateObject(values, job);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
               
                var update = Builders<People>.Update.Push<Job>(e => e.Jobs, job);

                var updatePeople = DB.get<People>("people").UpdateOne(filter, update);
                return Json(updatePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public ActionResult Update(string Id, string key, string values)
        {
            try
            {
                var keyObject = new Job();
                JsonConvert.PopulateObject(key, keyObject);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var people = DB.get<People>("people").Find(filter).FirstOrDefault();
                var job = people.Jobs.Find(x => x.Name == keyObject.Name && x.DateStart.Equals(keyObject.DateStart) && x.DateEnd.Equals(keyObject.DateEnd));

                JsonConvert.PopulateObject(values, job);

                var updatePeople = DB.get<People>("people").ReplaceOne(filter, people);
                return Json(updatePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(string Id, string key)
        {
            try
            {
                var keyObject = new Job();
                JsonConvert.PopulateObject(key, keyObject);

                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var update = Builders<People>.Update.PullFilter(x => x.Jobs, Builders<Job>.Filter.Where(j => j.Name == keyObject.Name && j.DateStart.Equals(keyObject.DateStart) && j.DateEnd.Equals(keyObject.DateEnd)));

               

                var updatePeople = DB.get<People>("people").UpdateOne(filter, update);
                return Json(updatePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}