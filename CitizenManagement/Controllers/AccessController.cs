using CitizenManagement.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class AccessController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetAccess()
        {
            List<Access> peoples = DB.get<Access>("access").FindSync<Access>(new BsonDocument()).ToList();

            return JsonConvert.SerializeObject(peoples);
        }

        [HttpPost]
        public async Task<ActionResult> Add(string values)
        {
            try
            {
                var access = new Access();

                JsonConvert.PopulateObject(values, access);

                await DB.get<Access>("access").InsertOneAsync(access);

                return Json(access, JsonRequestBehavior.AllowGet);
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
                var access = DB.get<Access>("access").AsQueryable().Where(p => p.Id == key).FirstOrDefault();
                JsonConvert.PopulateObject(values, access);

                var filter = Builders<Access>.Filter.Eq(p => p.Id, key);
                var update = Builders<Access>.Update.Set(p => p, access);

                var updateAccess = DB.get<Access>("access").ReplaceOne(filter, access);
                return Json(updateAccess, JsonRequestBehavior.AllowGet);
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
                var filter = Builders<Access>.Filter.Eq(r => r.Id, key);
                var deleteAccess = DB.get<Access>("access").DeleteOne(filter);
                return Json(deleteAccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}