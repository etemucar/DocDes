
using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Individual : ModelBase
    {
        public int PartyId { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual Party Party { get; set; } = null!;
        public virtual ICollection<PartyRole> PartyRoles { get; set; } = null!;
 
    }
}