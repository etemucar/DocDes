using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class District : ModelBase
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public string Coordinates { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Town> Towns { get; set; }
        public virtual ICollection<Address>Addresses { get; set; }
    }
}
