using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class PartyRole : ModelBase
    {
        public int PartyId { get; set; }
        public int? IndividualId { get; set; }
        public int? OrganizationId { get; set; }
        public int PartyRoleTypeId { get; set; } //1 : Site Admin 2 : Store Admin 3 : ApplicationUser 4 : Customer 5 : BillAccount

        public virtual Party Party { get; set; } = null!;
        public virtual PartyRoleType PartyRoleType { get; set; } = null!;
        public virtual Individual Individual { get; set; } = null!;
        public virtual Organization Organization { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual PartyRoleAccount PartyRoleAccount { get; set; } = null!;

    }
}