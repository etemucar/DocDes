using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class ApplicationUser : ModelBase
    {
        public int OrganizationId { get; set; }
        public int PartyRoleId { get; set; }
        public string EMail { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual PartyRole PartyRole { get; set; } = null!;
        public virtual Organization Organization { get; set; } = null!;

        public void CopyFrom(ApplicationUser entity)
        {
            OrganizationId = entity.OrganizationId;
            PartyRoleId = entity.PartyRoleId;
            EMail = entity.EMail;
            Password = entity.Password;
            StatusId = entity.StatusId;
        }
        
    }
}
