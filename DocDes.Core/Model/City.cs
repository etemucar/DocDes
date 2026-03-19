using DocDes.Core.Base;

namespace DocDes.Core.Model
{
    public class City : ModelBase
    {
        public int CountryId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string? Coordinates { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}