using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class City : ModelBase<int>
{
    public int CountryId { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public string? Coordinates { get; set; }
    
    public virtual Country Country { get; set; } = null!;
    public virtual ICollection<District> Districts { get; set; } = null!;
    public virtual ICollection<Address> Addresses { get; set; } = null!;
}
