using FoodHygieneMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FoodHygieneMVC.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url  
        readonly string Baseurl = "https://api.ratings.food.gov.uk";
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

                //Deserializing the response recieved from web api and storing into the list   
                //TODO Deserialize correctly!
                AuthInfo = JsonConvert.DeserializeObject<List<Authority>>(AuthResponse);

                 
                  
            }
            //returning the employee list to view  
            return View(AuthInfo);
        }
    }
}