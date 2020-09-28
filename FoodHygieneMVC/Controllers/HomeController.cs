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
            List<AuthoritiesRatings> AuthoritiesInfo = new List<AuthoritiesRatings>();

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

                ResetStarValues(out double StarExcept, out double Star5, out double Star4, out double Star3, out double Star2, out double Star1, out double StarAll);

                foreach (var authID in distinctLocalAuthorityIDCodeList)
                {
                    ResetStarValues(out StarExcept, out Star5, out Star4, out Star3, out Star2, out Star1, out StarAll); 
                    HttpResponseMessage ResEstablishment = await client.GetAsync("Establishments?localAuthorityId=" + authID + "");
                     
                    if (ResEstablishment.IsSuccessStatusCode)
                    { 
                        var establishmentResponse = ResEstablishment.Content.ReadAsStringAsync().Result; 
                         
                        //Something wrong here?
                        var dataEstablishmentArray = (RootEstablishments)Newtonsoft.Json.JsonConvert.DeserializeObject(establishmentResponse, typeof(RootEstablishments));
                         
                        if (dataEstablishmentArray.establishments != null)
                        {

                            EstablishmentInfo = dataEstablishmentArray.establishments;

                            var establishmentList = EstablishmentInfo.ToList();

                            StarAll += establishmentList.Count();
                            Star5 += establishmentList.Where(x => x.RatingValue == "5").Count();
                            Star4 += establishmentList.Where(x => x.RatingValue == "4").Count();
                            Star3 += establishmentList.Where(x => x.RatingValue == "3").Count();
                            Star2 += establishmentList.Where(x => x.RatingValue == "2").Count();
                            Star1 += establishmentList.Where(x => x.RatingValue == "1").Count();
                            StarExcept += establishmentList.Where(x => x.RatingValue != "5" || x.RatingValue != "4" || x.RatingValue != "3" || x.RatingValue != "2" || x.RatingValue != "1").Count();
                        } 
                    }
                }

                //Convert to percentage and save  
                Star5 = Math.Round(((Star5 / StarAll) * 100), 0,MidpointRounding.AwayFromZero);
                Star4 = Math.Round(((Star4 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star3 = Math.Round(((Star3 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star2 = Math.Round(((Star2 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star1 = Math.Round(((Star1 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                StarExcept = Math.Round(((StarExcept / StarAll) * 100), 0, MidpointRounding.AwayFromZero); 
                 
                //TODO Send to AuthoritiesRatings
               // RootEstablishment.Add(StarExcept, Star5, Star4, Star3, Star2, Star1, LocalAuthorityIdCode);  
            }
            //returning needs to be amended to AuthoritiesRatings instead
            return View(EstablishmentInfo); 
        }

     
        private static void ClientDefinitions(HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-version", "2");
        }

        private static void ResetStarValues(out double StarExcept, out double Star5, out double Star4, out double Star3, out double Star2, out double Star1, out double StarAll)
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