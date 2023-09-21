using CitizenManagement.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class TemporaryAbsentController : Controller
    {
        public string GetTemporaryAbsents(string Id)
        {
            var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
            var people = DB.get<People>("people").Find(filter).FirstOrDefault();

            return JsonConvert.SerializeObject(people.TemporaryAbsent != null ? people.TemporaryAbsent : new List<TemporaryAbsent>());
        }

        [HttpPost]
        public ActionResult Add(string Id, string values)
        {
            try
            {
                var job = new TemporaryAbsent();
                JsonConvert.PopulateObject(values, job);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);

                var update = Builders<People>.Update.Push<TemporaryAbsent>(e => e.TemporaryAbsent, job);

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
                var keyObject = new TemporaryAbsent();
                JsonConvert.PopulateObject(key, keyObject);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var people = DB.get<People>("people").Find(filter).FirstOrDefault();
                var job = people.TemporaryAbsent.Find(x => x.Destination == keyObject.Destination && x.Reason.Equals(keyObject.Reason) && x.DateCreate.Equals(keyObject.DateCreate));

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
                var keyObject = new TemporaryAbsent();
                JsonConvert.PopulateObject(key, keyObject);

                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var update = Builders<People>.Update.PullFilter(p => p.TemporaryAbsent, Builders<TemporaryAbsent>.Filter.Where(x => x.Destination == keyObject.Destination && x.Reason.Equals(keyObject.Reason) && x.DateCreate.Equals(keyObject.DateCreate)));



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