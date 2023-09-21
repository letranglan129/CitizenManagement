using CitizenManagement.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CitizenManagement.Controllers
{
    public class DieController : Controller
    {
        public ActionResult Save(string Id, string values)
        {

            var filter = Builders<People>.Filter.Eq(r => r.Id, Id);
            
            var people = DB.get<People>("people").Find(filter).First();
            if (people != null) {

                var die = people.Die != null ? people.Die : new Die();

                JsonConvert.PopulateObject(values, die);


                var update = Builders<People>.Update.Set(r => r.Die, die);

                var result = DB.get<People>("people").UpdateOne(filter, update);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
             else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
        }
    }
}