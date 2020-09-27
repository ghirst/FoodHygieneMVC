using System;
using System.Collections.Generic;

namespace FoodHygieneMVC.Models
{
    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Authority
    {
        public int LocalAuthorityId { get; set; }
        public string LocalAuthorityIdCode { get; set; }
        public string Name { get; set; }
        public int EstablishmentCount { get; set; }
        public int SchemeType { get; set; }
        public List<Link> links { get; set; }
    }

    public class Meta
    {
        public string dataSource { get; set; }
        public DateTime extractDate { get; set; }
        public int itemCount { get; set; }
        public string returncode { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }

    public class Link2
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Root
    {
        public List<Authority> authorities { get; set; }
        public Meta meta { get; set; }
        public List<Link2> links { get; set; }
    }

}
