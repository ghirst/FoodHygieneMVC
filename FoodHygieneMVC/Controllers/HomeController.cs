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
        //ToDo move to appSettings
        private readonly string Baseurl = "https://api.ratings.food.gov.uk";

        public async Task<ActionResult> Index()
        {
            var AuthInfo = new List<Authority>();
            var EstablishmentInfo = new List<Establishment>();
            var AuthoritiesInfo = new List<Ratings>();

            Uri uri = new Uri(Baseurl);
            HttpClient httpClient = new HttpClient
            {
                //Passing service base url
                BaseAddress = uri
            };
            using var client = httpClient;
            ClientDefinitions(client);

            //Sending request to find web api REST service resource GETRegions using HttpClient
            using (HttpResponseMessage Res = await client.GetAsync("Authorities/basic"))
            {

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    getAuthorities(out AuthInfo, Res, out List<string> distinctLocalAuthorityIDCodeList);
                    ResetStarValues(out double StarExcept, out double Star5, out double Star4, out double Star3, out double Star2, out double Star1, out double StarAll);

                    //ToDo
                    //Make async, no need to wait for each one to finish
                    foreach (var authID in distinctLocalAuthorityIDCodeList)
                    {
                        ResetStarValues(out StarExcept, out Star5, out Star4, out Star3, out Star2, out Star1, out StarAll);
                        HttpResponseMessage ResEstablishment = await client.GetAsync("Establishments?localAuthorityId=" + authID + "");

                        if (ResEstablishment.IsSuccessStatusCode)
                        {
                            var establishmentResponse = ResEstablishment.Content.ReadAsStringAsync().Result;

                            //Something wrong here?
                            var dataEstablishmentArray = (RootEstablishments)Newtonsoft.Json.JsonConvert.DeserializeObject(establishmentResponse, typeof(RootEstablishments));
                            countRatings(ref EstablishmentInfo, ref StarExcept, ref Star5, ref Star4, ref Star3, ref Star2, ref Star1, ref StarAll, dataEstablishmentArray);
                        }
                        percentageConvert(ref StarExcept, ref Star5, ref Star4, ref Star3, ref Star2, ref Star1, StarAll);
                        //TODO Send to AuthoritiesRatings
                        //Ratings(StarExcept, Star5, Star4, Star3, Star2, Star1, LocalAuthorityIdCode);
                    }
                }
            }
            //TODO Amend this to send out the AuthoritiesRatings
            return View(EstablishmentInfo);
        }

        private static void getAuthorities(out List<Authority> AuthInfo, HttpResponseMessage Res, out List<string> distinctLocalAuthorityIDCodeList)
        {
            var AuthResponse = Res.Content.ReadAsStringAsync().Result;
            var dataArray = (Root)Newtonsoft.Json.JsonConvert.DeserializeObject(AuthResponse, typeof(Root));
            AuthInfo = dataArray.Authorities;
            distinctLocalAuthorityIDCodeList = AuthInfo.Select(o => o.LocalAuthorityIdCode).Distinct().ToList();
        }

        private static void countRatings(ref List<Establishment> EstablishmentInfo, ref double StarExcept, ref double Star5, ref double Star4, ref double Star3, ref double Star2, ref double Star1, ref double StarAll, RootEstablishments dataEstablishmentArray)
        {
            if (dataEstablishmentArray.establishments != null)
            {
                EstablishmentInfo = dataEstablishmentArray.establishments;

                var establishmentList = EstablishmentInfo.ToList();

                StarAll += establishmentList.Count();
                Star5 = establishmentList.Where(x => x.RatingValue == "5").Count();
                Star4 = establishmentList.Where(x => x.RatingValue == "4").Count();
                Star3 = establishmentList.Where(x => x.RatingValue == "3").Count();
                Star2 = establishmentList.Where(x => x.RatingValue == "2").Count();
                Star1 = establishmentList.Where(x => x.RatingValue == "1").Count();
                StarExcept += establishmentList.Where(x => x.RatingValue != "5" || x.RatingValue != "4" || x.RatingValue != "3" || x.RatingValue != "2" || x.RatingValue != "1").Count();
            }
        }

        private static void percentageConvert(ref double StarExcept, ref double Star5, ref double Star4, ref double Star3, ref double Star2, ref double Star1, double StarAll)
        {
            if (StarAll != 0)
            {
                //Convert to percentage and save
                Star5 = Math.Round(((Star5 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star4 = Math.Round(((Star4 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star3 = Math.Round(((Star3 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star2 = Math.Round(((Star2 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                Star1 = Math.Round(((Star1 / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
                StarExcept = Math.Round(((StarExcept / StarAll) * 100), 0, MidpointRounding.AwayFromZero);
            }
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