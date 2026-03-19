
using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Party : ModelBase
    {
        public int ParentId { get; set; }

        public virtual Individual? Individual { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<PartyRole>? PartyRoles { get; set; }
        public virtual ICollection<ContactMedium>? ContactMedium { get; set; }
        public virtual ICollection<RelatedParty>? RelatedParties { get; set; }
        
    }
}