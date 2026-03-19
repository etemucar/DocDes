using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Address : ModelBase
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? StreetLine1 { get; set; }
        public string? StreetLine2 { get; set; }

    }
}
