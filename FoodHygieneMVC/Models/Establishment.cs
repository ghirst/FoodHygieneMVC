using System;
using System.Collections.Generic;

namespace FoodHygieneMVC.Models
{
    public class Scores
    {
        public int? Hygiene { get; set; }
        public int? Structural { get; set; }
        public int? ConfidenceInManagement { get; set; }
    }

    public class Geocode
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

    public class EstablishmentMeta
    {
        public object dataSource { get; set; }
        public DateTime extractDate { get; set; }
        public int? itemCount { get; set; }
        public object returncode { get; set; }
        public int? totalCount { get; set; }
        public int? totalPages { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
    }

    public class EstablishmentLink
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Establishment
    {
        public int? FHRSID { get; set; }
        public int? ChangesByServerID { get; set; }
        public string LocalAuthorityBusinessID { get; set; }
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public int? BusinessTypeID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string RatingValue { get; set; }
        public string RatingKey { get; set; }
        public DateTime RatingDate { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityWebSite { get; set; }
        public string LocalAuthorityEmailAddress { get; set; }
        public Scores scores { get; set; }
        public string SchemeType { get; set; }
        public Geocode geocode { get; set; }
        public string RightToReply { get; set; }
        public object Distance { get; set; }
        public bool NewRatingPending { get; set; }
        public Meta meta { get; set; }
        public List<Link> links { get; set; }
    }

    public class Meta2
    {
        public string dataSource { get; set; }
        public DateTime extractDate { get; set; }
        public int? itemCount { get; set; }
        public string returncode { get; set; }
        public int? totalCount { get; set; }
        public int? totalPages { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
    }

    public class EstablishmentLink2
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class RootEstablishments
    {
        public List<Establishment> establishments { get; set; }
        public Meta2 meta { get; set; }
        public List<Link2> links { get; set; }
    }
}