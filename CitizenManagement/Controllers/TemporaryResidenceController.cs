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
    public class TemporaryResidenceController : Controller
    {
        public string GetTemporaryResidences(string Id)
        {
            var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
            var people = DB.get<People>("people").Find(filter).FirstOrDefault();

            return JsonConvert.SerializeObject(people.TemporaryResidence != null ? people.TemporaryResidence : new List<TemporaryResidence>());
        }

        [HttpPost]
        public ActionResult Add(string Id, string values)
        {
            try
            {
                var job = new TemporaryResidence();
                JsonConvert.PopulateObject(values, job);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);

                var update = Builders<People>.Update.Push<TemporaryResidence>(e => e.TemporaryResidence, job);

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
                var keyObject = new TemporaryResidence();
                JsonConvert.PopulateObject(key, keyObject);
                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var people = DB.get<People>("people").Find(filter).FirstOrDefault();
                var job = people.TemporaryResidence.Find(x => x.DateCreate.Equals(keyObject.DateCreate) && x.HouseholdId.Equals(x.HouseholdId) && x.RelationshipHeadHouse == keyObject.RelationshipHeadHouse);

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
                var keyObject = new TemporaryResidence();
                JsonConvert.PopulateObject(key, keyObject);

                var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
                var update = Builders<People>.Update.PullFilter(p => p.TemporaryResidence, Builders<TemporaryResidence>.Filter.Where(x => x.HouseholdId == keyObject.HouseholdId && x.RelationshipHeadHouse == keyObject.RelationshipHeadHouse && x.DateCreate.Equals(keyObject.DateCreate)));

                var updatePeople = DB.get<People>("people").UpdateOne(filter, update);
                return Json(keyObject.DateCreate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}