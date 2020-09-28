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
            List<Establishment> EstablishmentInfo = new List<Establishment>();

            using var client = new HttpClient
            {
                //Passing service base url
                BaseAddress = new Uri(Baseurl)
            };
            ClientDefinitions(client);

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
                
                int StarExcept, Star5, Star4, Star3, Star2, Star1, StarAll; 
                SetStars(out StarExcept, out Star5, out Star4, out Star3, out Star2, out Star1, out StarAll);

                foreach (var authID in distinctLocalAuthorityIDCodeList)
                {
                    ResetStarValues(out StarExcept, out Star5, out Star4, out Star3, out Star2, out Star1, out StarAll);
                    //API here to get each establishment 
                    HttpResponseMessage ResEstablishment = await client.GetAsync("Establishments?localAuthorityId=" + authID + "");

                    //Checking the response is successful or not which is sent using HttpClient
                    if (ResEstablishment.IsSuccessStatusCode)
                    {

                        var starResponse = ResEstablishment.Content.ReadAsStringAsync().Result;

                        var dataEstablishmentArray = (RootEstablishment)Newtonsoft.Json.JsonConvert.DeserializeObject(AuthResponse, typeof(RootEstablishment));

                        if(dataEstablishmentArray.Establishments != null)
                        { 
                        EstablishmentInfo = dataEstablishmentArray.Establishments;

                        var establishmentList = EstablishmentInfo.ToList();
 
                        StarAll = establishmentList.Count();
                        StarExcept = StarAll - (Star5 + Star4 + Star3 + Star2 + Star1);
                            //Star5 += establishmentList.Where(x => x.RatingValue == '5').Count();
                            //Star4 += establishmentList.Where(x => x.RatingValue == '4').Count();
                            //Star3 += establishmentList.Where(x => x.RatingValue == '3').Count();
                            //Star2 += establishmentList.Where(x => x.RatingValue == '2').Count();
                            //Star1 += establishmentList.Where(x => x.RatingValue == '1').Count();
                        }
                    }
                }

                //Convert to percentage and save

                //RootEstablishment.Add(StarExcept, Star5, Star4, Star3, Star2, Star1, LocalAuthorityIdCode);


            }
            //returning the employee list to view
            return View(AuthInfo);

           // return View(AuthInfo);
        }

        private static void SetStars(out int StarExcept, out int Star5, out int Star4, out int Star3, out int Star2, out int Star1, out int StarAll)
        {
            StarExcept = 0;
            Star5 = 0;
            Star4 = 0;
            Star3 = 0;
            Star2 = 0;
            Star1 = 0;
            StarAll = 0;
        }

        private static void ClientDefinitions(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-version", "2");
        }

        private static void ResetStarValues(out int StarExcept, out int Star5, out int Star4, out int Star3, out int Star2, out int Star1, out int StarAll)
        {
            StarExcept = 0;
            Star5 = 0;
            Star4 = 0;
            Star3 = 0;
            Star2 = 0;
            Star1 = 0;
            StarAll = 0;
        }

    }
}