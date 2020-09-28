namespace FoodHygieneMVC.Models
{
    public class AuthoritiesRatings
    { 
        public class Ratings
        {
            public int LocalAuthorityId { get; set; }
            public string Name { get; set; }

            public double Star5 { get; set; }
            public double Star4 { get; set; }
            public double Star3 { get; set; }

            public double Star2 { get; set; }
            public double Star1 { get; set; }
            public double StarExcept { get; set; }
        }
    }
}
