using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class ContactMedium : ModelBase
    {
        public int PartyId { get; set; }
        public string MediumType { get; set; } 
        public int IsPreferred { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }


        public virtual Party Party { get; set; }

    }
}
