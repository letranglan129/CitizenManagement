using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class PlaceController : Controller
    {

        public async Task<dynamic> GetCountries()
        {
            string apiUrl = "https://api.nosomovo.xyz/country/getalllist";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);

                    JArray resultJArray = result as JArray;
                    var provinces = resultJArray.Select(y => new {
                        Name = (string)y[key: "name_vi"],
                        Id = (string)y[key: "id"],
                    }).OrderByDescending(m => Int32.Parse(m.Id)).ToList();
                    return Json(provinces, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return "Yêu cầu không thành công. Mã trạng thái: " + response.StatusCode;
                }
            }
        }

        public async Task<dynamic> GetProvinces()
        {
            string apiUrl = "https://vapi.vnappmob.com/api/province/";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);

                    JArray resultJArray = result.results as JArray;
                    var provinces = resultJArray.Select(y => new {
                        Name = (string)y[key: "province_name"],
                        Id = (string)y[key: "province_id"],
                    }).ToList();
                    return Json(provinces, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return "Yêu cầu không thành công. Mã trạng thái: " + response.StatusCode;
                }
            }
        }

        public async Task<dynamic> GetDistricts(string code)
        {
            string apiUrl = "https://vapi.vnappmob.com/api/province/district/" + code;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);

                    JArray resultJArray = result.results as JArray;
                    var districts = resultJArray.Select(y => new {
                        Name = (string)y[key: "district_name"],
                        Id = (string)y[key: "district_id"],
                    }).ToList();
                    return Json(districts, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return "Yêu cầu không thành công. Mã trạng thái: " + response.StatusCode;
                }
            }
        }

        public async Task<dynamic> GetWards(string code)
        {
            string apiUrl = "https://vapi.vnappmob.com/api/province/ward/" + code;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);

                    JArray resultJArray = result.results as JArray;
                    var wards = resultJArray.Select(y => new {
                        Name = (string)y[key: "ward_name"],
                        Id = (string)y[key: "ward_id"],
                    }).ToList();
                    return Json(wards, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return "Yêu cầu không thành công. Mã trạng thái: " + response.StatusCode;
                }
            }
        }
    }
}