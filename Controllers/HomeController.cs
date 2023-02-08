using i247Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace i247Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<User> users = await FetchFiveRandomUsers();
            return View(users);
        }

        

        public async Task<string> FetchUserJson()
        {
            var httpClient = new HttpClient();
            string resultJson = "";
            using (var response = await httpClient.GetAsync("https://randomuser.me/api/"))
            {
                resultJson = await response.Content.ReadAsStringAsync();
            }
            return resultJson;
        }

        public async Task<List<User>> FetchFiveRandomUsers()
        {
            List<User> users = new List<User>();
            for(int i=0;i<5;i++)
            {
                string resultJson = await FetchUserJson();
                users.Add(FetchUserDataFromJson(resultJson));
            }
            return users;
        }

        public User FetchUserDataFromJson(string resultJson)
        {
            User user = new User(); 
            JObject obj= JObject.Parse(resultJson);
            JArray jArray = (JArray)obj["results"];
            foreach(JObject obj1 in jArray)
            {
                
                user.Title = obj1["name"]["title"].ToString();
                user.FirstName = obj1["name"]["first"].ToString();
                user.Surname= obj1["name"]["last"].ToString();
                user.Age = (int)obj1["dob"]["age"];
                user.Country = obj1["location"]["country"].ToString();
                user.Latitude = (decimal)obj1["location"]["coordinates"]["latitude"];
                user.Longitude = (decimal)obj1["location"]["coordinates"]["longitude"];
                
            }
            return user;
        }

        [HttpGet]
        public IActionResult Test()
        {
            List<User> users = new List<User>();

            return Json(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}