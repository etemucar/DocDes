using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class Address : ModelBase<int>
{
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public int? TownId { get; set; } 
    public int? NeighborhoodId { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? StreetLine1 { get; set; }
    public string? StreetLine2 { get; set; }

    public virtual Country Country { get; set; } = null!;
    public virtual City City { get; set; } = null!;
    public virtual District District { get; set; } = null!;
    public virtual Neighborhood? Neighborhood { get; set; }

}