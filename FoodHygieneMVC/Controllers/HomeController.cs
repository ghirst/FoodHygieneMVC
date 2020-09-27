using FoodHygieneMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FoodHygieneMVC.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url
        private readonly string Baseurl = "https://api.ratings.food.gov.uk";

        public async Task<ActionResult> Index()
        {
            List<Authority> AuthInfo = new List<Authority>();

            using var client = new HttpClient
            {
                //Passing service base url
                BaseAddress = new Uri(Baseurl)
            };

            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-version", "2");

            //Sending request to find web api REST service resource GETRegions using HttpClient
            HttpResponseMessage Res = await client.GetAsync("Authorities/basic");  

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var AuthResponse = Res.Content.ReadAsStringAsync().Result;

                var dataArray = (Root)Newtonsoft.Json.JsonConvert.DeserializeObject(AuthResponse, typeof(Root));

                AuthInfo = dataArray.Authorities;

                var distinctLocalAuthorityIDCodeList = AuthInfo.Select(o => o.LocalAuthorityIdCode).Distinct().ToList();

                var StarExcept = 0;
                var Star5 = 0;
                var Star4 = 0;
                var Star3 = 0;
                var Star2 = 0;
                var Star1 = 0;

                foreach (var authID in distinctLocalAuthorityIDCodeList)
                {
                    //API here to get each establishment 
                    HttpResponseMessage ResEstablishment = await client.GetAsync("Establishments?localAuthorityId=760");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (ResEstablishment.IsSuccessStatusCode)
                    {

                        var starResponse = ResEstablishment.Content.ReadAsStringAsync().Result;

                        var starArray = (Root)Newtonsoft.Json.JsonConvert.DeserializeObject(starResponse, typeof(Root));

                        AuthInfo = starArray.Authorities;

                        //StarExcept = AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey != 5 || x.ratingKey != 4 || x.ratingKey != 3 || x.ratingKey != 2 || x.ratingKey != 1).Count();
                        //Star5 += AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey = 5).Count();
                        //Star4 += AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey = 4).Count();
                        //Star3 += AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey = 3).Count();
                        //Star2 += AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey = 2).Count();
                        //Star1 += AuthInfo.Select(o => o.LocalAuthorityIdCode).Where(x => x.ratingKey = 1).Count(); 
                    }
                }

                //Get the API for each Authority based on the ID
                // Get each star count
                // Get a percentage of each (nearest % rounded)
                //Add to AuthInfo value(s)

            }
            //returning the employee list to view
            return View(AuthInfo);
        }

       
    }
}