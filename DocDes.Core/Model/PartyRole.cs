using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class PartyRole : ModelBase
    {
        public int PartyId { get; set; }
        public int? IndividualId { get; set; }
        public int? OrganizationId { get; set; }
        public int PartyRoleTypeId { get; set; } //1 : Site Admin 2 : Store Admin 3 : ApplicationUser 4 : Customer 5 : BillAccount

        public virtual Party Party { get; set; }
        public virtual PartyRoleType PartyRoleType { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual PartyRoleAccount PartyRoleAccount { get; set; }

    }
}