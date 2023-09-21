using CitizenManagement.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class ReligionController : Controller
    {
        public string getList()
        {
            return JsonConvert.SerializeObject(ReligionData.list);
        }
    }
}