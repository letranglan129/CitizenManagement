using CitizenManagement.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public string GetAccounts()
        {
            List<Account> accounts = DB.get<Account>("account").Aggregate().Lookup<Account, Account>("people", "Info", "_id", "InfoData").ToList();

            return JsonConvert.SerializeObject(accounts);
        }

        [HttpPost]
        public async Task<ActionResult> Add(string values)
        {
            try
            {
                var account = new Account();
                JsonConvert.PopulateObject(values, account);

                var people = DB.get<People>("people").AsQueryable().Where(p => p.Id == account.Info).FirstOrDefault();

                string formattedDate = people.Birthday.ToString("ddMMyyyy");
                account.Password = BCrypt.Net.BCrypt.HashPassword(formattedDate);

                await DB.get<Account>("account").InsertOneAsync(account);

                return Json(account, JsonRequestBehavior.AllowGet);
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
                var access = new Account();
                JsonConvert.PopulateObject(values, access);

                var filter = Builders<Account>.Filter.Eq(p => p.Id, key);
                var update = Builders<Account>.Update.Set(p => p.Permissions, access.Permissions);

                var updateAccount = DB.get<Account>("account").UpdateOne(filter, update);
                return Json(updateAccount, JsonRequestBehavior.AllowGet);
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
                var filter = Builders<Account>.Filter.Eq(r => r.Id, key);
                var deleteAccess = DB.get<Account>("account").DeleteOne(filter);
                return Json(deleteAccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission(string Id, string values)
        {
            try
            {
                var access = new Access();
                JsonConvert.PopulateObject(values, access);
                var filter = Builders<Account>.Filter.Eq(r => r.Id, Id);

                var update = Builders<Account>.Update.AddToSet<Access>(e => e.Permissions, access);

                var updateAccount = DB.get<Account>("account").UpdateOne(filter, update);
                return Json(updateAccount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public ActionResult DeletePermission(string Id, string key)
        {
            try
            {
                var keyObject = new Access();
                JsonConvert.PopulateObject(key, keyObject);

                var filter = Builders<Account>.Filter.Eq(r => r.Id, Id);
                var update = Builders<Account>.Update.PullFilter(x => x.Permissions, Builders<Access>.Filter.Where(j => j.Name == keyObject.Name && j.Permission == keyObject.Permission));



                var updateAccount = DB.get<Account>("account").UpdateOne(filter, update);
                return Json(updateAccount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string currentPass, string newPass, string renewPass)
        {
            Account person = Session["info"] as Account;
            var check = DB.get<Account>("account").AsQueryable().Where(x => x.UserName == person.UserName).FirstOrDefault();
            
            bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(currentPass, check.Password);

            if (isPasswordMatch)
            {
                if (newPass == renewPass)
                {
                    check.Password = BCrypt.Net.BCrypt.HashPassword(newPass);
                    var update = Builders<Account>.Update.Set(x => x.Password, check.Password);
                    var filter = Builders<Account>.Filter.Eq(x => x.UserName, check.UserName);
                    DB.get<Account>("account").UpdateOne(filter, update);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }
    }
}