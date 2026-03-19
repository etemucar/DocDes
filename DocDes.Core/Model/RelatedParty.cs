using DocDes.Core.Base;

namespace DocDes.Core.Model {

    public class RelatedParty : ModelBase
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }

        public virtual Party Party { get; set; }

    }
}
