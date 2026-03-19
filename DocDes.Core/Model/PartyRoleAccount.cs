using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class PartyRoleAccount : ModelBase
    {
        public int PartyRoleId { get; set; }
        public string CurrencyCode { get; set; }

        public virtual PartyRole PartyRole { get; set; }

    }
}