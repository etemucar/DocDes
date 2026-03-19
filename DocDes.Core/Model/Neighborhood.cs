using DocDes.Core.Base;

namespace DocDes.Core.Model
{
    public class Neighborhood : ModelBase
    {


        public int TownId { get; set; }
        public string Name { get; set; } = null!;

        public virtual Town Town { get; set; } = null!;
        public virtual ICollection<Address>Addresses { get; set; } = null!;

    }
}