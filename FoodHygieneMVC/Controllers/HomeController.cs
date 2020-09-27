using FoodHygieneMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                 
               

                foreach (var authID in distinctLocalAuthorityIDCodeList)
                {
                    AuthInfo = await GetEstablistmentStars(AuthInfo, client, Res, AuthResponse, authID);

                }

                //Get the API for each Authority based on the ID
                // Get each star count
                // Get a percentage of each (nearest % rounded)
                //Add to AuthInfo value(s)

            }
            //returning the employee list to view
            return View(AuthInfo);
        }

        private static async Task<List<Authority>> GetEstablistmentStars(List<Authority> AuthInfo, HttpClient client, HttpResponseMessage Res, string AuthResponse, string authID)
        {

            var Star5 = 0;
            var Star4 = 0;
            var Star3 = 0;
            var Star2 = 0;
            var Star1 = 0;
            var StarEx = 0;

            //API here to get each establishment 
            HttpResponseMessage ResEstablishment = await client.GetAsync("Establiment/{" + authID + "}");

            //Checking the response is successful or not which is sent using HttpClient
            if (ResEstablishment.IsSuccessStatusCode)

                //Storing the response details recieved from web api
                var AuthResponse = Res.Content.ReadAsStringAsync().Result;

            var dataArray = (Root)Newtonsoft.Json.JsonConvert.DeserializeObject(AuthResponse, typeof(Root));

            AuthInfo = dataArray.Authorities;
            return AuthInfo;
        }
    }
}