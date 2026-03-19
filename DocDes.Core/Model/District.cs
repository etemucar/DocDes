using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class District : ModelBase
    {
        public string Name { get; set; } = null!;
        public int CityId { get; set; }
        public string Coordinates { get; set; } = null!;

        public virtual City City { get; set; } = null!;
        public virtual ICollection<Town> Towns { get; set; } = null!;
        public virtual ICollection<Address>Addresses { get; set; } = null!;
    }
}
