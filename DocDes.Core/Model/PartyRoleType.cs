using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class PartyRoleType : ModelBase
    {
        public string PartyRoleTypeCd { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<PartyRole> PartyRoles { get; set; } = null!;

    }
}