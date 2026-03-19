using DocDes.Core.Base;

namespace DocDes.Core.Model {

    public class RelatedParty : ModelBase
    {
        public int PartyId { get; set; }
        public string Name { get; set; } = null!;
        public string EntityType { get; set; } = null!;
        public int EntityId { get; set; }
        public string Role { get; set; } = null!;
        public string Type { get; set; } = null!;

        public virtual Party Party { get; set; } = null!;

    }
}
