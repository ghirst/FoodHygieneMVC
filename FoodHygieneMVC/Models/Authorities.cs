using System;
using System.Collections.Generic;

namespace FoodHygieneMVC.Models
{
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class RootEstablishment
    {
        public List<Establishment> Establishments { get; set; }

        public string RatingValue {get; set; }
    }

    public class Establishment
    {
        public int LocalAuthorityId { get; set; } 
        public string Name { get; set; } 

        public int Star5 { get; set; }
        public int Star4 { get; set; }
        public int Star3 { get; set; }

        public int Star2 { get; set; }
        public int Star1 { get; set; }
        public int StarExcept { get; set; }
    }
    public class Authority
    {
        public int LocalAuthorityId { get; set; }
        public string LocalAuthorityIdCode { get; set; }
        public string Name { get; set; }
        public int EstablishmentCount { get; set; }
        public int SchemeType { get; set; }
        public List<Link> Links { get; set; } 
         
    }

    public class Meta
    {
        public string DataSource { get; set; }
        public DateTime ExtractDate { get; set; }
        public int ItemCount { get; set; }
        public string Returncode { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class Link2
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class Root
    {
        public List<Authority> Authorities { get; set; }
        public Meta Meta { get; set; }
        public List<Link2> Links { get; set; }
    }

    
}