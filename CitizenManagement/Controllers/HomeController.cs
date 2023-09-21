using CitizenManagement.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using MongoDB.Bson.Serialization.Conventions;
using System.Web.Mvc;
using System.Net;
using System;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}