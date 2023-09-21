using System.Collections.Generic;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CitizenManagement.Models;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Linq;
using System.Web.Routing;

namespace CitizenManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class RefreshActionAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);
                var citizenToken = filterContext.HttpContext.Request.Cookies.Get("citizenToken");
                var Session = filterContext.HttpContext.Session;

                try
                {
                    var token = new Token();
                    var decodeTokenJson = JwtService.Decode(citizenToken.Value);
                    JsonConvert.PopulateObject(decodeTokenJson, token);

                    DateTime currentTime = DateTime.Now;
                    long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

                    var person = DB.get<Account>("account").AsQueryable().Where(x => x.UserName == token.UserName).First();
                    if (token.exp < timestamp)
                    {
                        if (person != null)
                        {
                            string newToken = JwtService.GenerateToken(new Dictionary<string, object>()
                                {
                                    { "Info", person.Info },
                                    { "UserName", person.UserName },
                                    { "exp", timestamp + 600 }
                                });

                            var x = new HttpCookie("citizenToken", newToken);
                            filterContext.HttpContext.Response.Cookies.Add(x);
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                            {
                                controller = "Login",
                                action = "Index"
                            }));
                        }
                    }

                    FormsAuthentication.SetAuthCookie(person.UserName, true);
                    Session["info"] = person;
                }
                catch (Exception)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Index"
                    }));
                }
            }
        }
    }
}
