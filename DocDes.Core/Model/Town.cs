using DocDes.Core.Base;

namespace DocDes.Core.Model
{
    public class Town : ModelBase
    {


        public int DistrictId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public virtual District District { get; set; }
        public virtual ICollection<Neighborhood> Neighborhoods { get; set; }
        public virtual ICollection<Address>Addresses { get; set; }

    }
}