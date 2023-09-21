using CitizenManagement.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class PeopleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetPeoples()
        {
            List<People> peoples = DB.get<People>("people").FindSync<People>(new BsonDocument()).ToList();

            return JsonConvert.SerializeObject(peoples);
        }

        [HttpPost]
        public async Task<ActionResult> Add(string values)
        {
            try
            {
                var people = new People();

                JsonConvert.PopulateObject(values, people);
                var groups = new List<Group>();

                await DB.get<People>("people").InsertOneAsync(people);

                return Json(people, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(string key, string values)
        {
            try
            {
                var people = DB.get<People>("people").AsQueryable().Where(p => p.Id == key).FirstOrDefault();
                JsonConvert.PopulateObject(values, people);

                var filter = Builders<People>.Filter.Eq(p => p.Id, key);
                var update = Builders<People>.Update.Set(p => p, people); ;

                var updatePeople = DB.get<People>("people").ReplaceOne(filter, people);
                return Json(updatePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public ActionResult Delete(string key)
        {
            try
            {
                var filter = Builders<People>.Filter.Eq(r => r.Id, key);
                var deletePeople = DB.get<People>("people").DeleteOne(filter);
                return Json(deletePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
