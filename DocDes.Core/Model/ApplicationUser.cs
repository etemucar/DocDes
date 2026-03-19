using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class ApplicationUser : ModelBase
    {
        public int OrganizationId { get; set; }
        public int PartyRoleId { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }

        public virtual PartyRole PartyRole { get; set; }
        public virtual Organization Organization { get; set; }

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
