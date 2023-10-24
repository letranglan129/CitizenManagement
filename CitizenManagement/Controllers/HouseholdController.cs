using CitizenManagement.Models;
using CloudinaryDotNet.Actions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class HouseholdController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetHouseholds()
        {
            var result = DB.get<Household>("household").Aggregate().Lookup<Household, Household>("people", "Members.MemberId", "_id", "MembersLookup").Lookup<Household, Household>("people", "Owner", "_id", "OwnerInfo").ToList().OrderByDescending(x => x.DateCreate).Select(x => new { x.CreatedAt, x.DateCreate, x.HouseholdId, x.Id, x.Members, x.MembersLookup, x.Owner, x.OwnerInfo, x.Place, CountMember = x.Members != null ? x.Members.Count : 0}).ToList();
            return JsonConvert.SerializeObject(result);
        }

        public string GetRelatives(string Id)
        {
            var filter = Builders<Household>.Filter.Or(
                Builders<Household>.Filter.Eq("Members.MemberId", ObjectId.Parse(Id)),
                Builders<Household>.Filter.Eq("Owner", ObjectId.Parse(Id))
            );
            var result = DB.get<Household>("household").Aggregate().Match(filter).Lookup<Household, Household>("people", "Members.MemberId", "_id", "MembersLookup").Lookup<Household, Household>("people", "Owner", "_id", "OwnerInfo").First();
            if (result != null)
            {
                result.MembersLookup.Add(result.OwnerInfo.First());
            }
            return JsonConvert.SerializeObject(result.MembersLookup);
        }

        public string GetMembers(string Id)
        {
            var result = DB.get<Household>("household").Aggregate().Lookup<Household, Household>("people", "Members.MemberId", "_id", "MembersLookup").ToList().Find(h => h.Id == Id);
            return JsonConvert.SerializeObject(result.MembersLookup != null ? result.MembersLookup : new List<People>());
        }

        [HttpPost]
        public async Task<ActionResult> Add(string values)
        {
            try
            {
                var household = new Household();

                JsonConvert.PopulateObject(values, household);

                await DB.get<Household>("household").InsertOneAsync(household);

                return Json(household, JsonRequestBehavior.AllowGet);
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
                var household = DB.get<Household>("household").AsQueryable().Where(p => p.Id == key).FirstOrDefault();
                JsonConvert.PopulateObject(values, household);

                var filter = Builders<Household>.Filter.Eq(p => p.Id, key);
                var update = Builders<Household>.Update.Set(p => p, household);

                var updateHousehold = DB.get<Household>("household").ReplaceOne(filter, household);
                return Json(updateHousehold, JsonRequestBehavior.AllowGet);
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
                var filter = Builders<Household>.Filter.Eq(r => r.Id, key);
                var deleteHousehold = DB.get<Household>("household").DeleteOne(filter);
                return Json(deleteHousehold, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddMember(string Id, string values)
        {
            try
            {
                var householdMember = new HouseholdMember();
                JsonConvert.PopulateObject(values, householdMember);
                var filter = Builders<Household>.Filter.Eq(r => r.Id, Id);

                var household = DB.get<Household>("household").Find(filter).First();
                if (household.Members == null)
                {
                    var list = new List<HouseholdMember>();
                    list.Add(householdMember);
                    var update = Builders<Household>.Update.Set<List<HouseholdMember>>(e => e.Members, list);
                    var updateHouseholdMember = DB.get<Household>("household").UpdateOne(filter, update);
                    return Json(updateHouseholdMember, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var update = Builders<Household>.Update.Push<HouseholdMember>(e => e.Members, householdMember);
                    var updateHouseholdMember = DB.get<Household>("household").UpdateOne(filter, update);
                    return Json(updateHouseholdMember, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        public ActionResult UpdateMember(string Id, string key, string values)
        {
            try
            {
                var filter = Builders<Household>.Filter.Eq(r => r.Id, Id);
                var household = DB.get<Household>("household").Find(filter).FirstOrDefault();
                var member = household.Members.Find(x => x.MemberId == key);

                JsonConvert.PopulateObject(values, member);

                var updateHousehold = DB.get<Household>("household").ReplaceOne(filter, household);
                return Json(updateHousehold, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public ActionResult DeleteMember(string Id, string key)
        {
            try
            {
                var filter = Builders<Household>.Filter.Eq(r => r.Id, Id);
                var update = Builders<Household>.Update.PullFilter(x => x.Members, Builders<HouseholdMember>.Filter.Where(j => j.MemberId == key));

                var updatePeople = DB.get<Household>("household").UpdateOne(filter, update);
                return Json(updatePeople, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}