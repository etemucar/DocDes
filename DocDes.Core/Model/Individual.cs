
using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Individual : ModelBase
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<PartyRole> PartyRoles { get; set; }
 
    }
}