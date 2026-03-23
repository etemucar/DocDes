using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class PartyRoleType : ModelBase<int>
    {
    public int? OrganizationId { get; set; }  // null = sistem rolü, dolu = kuruma özel
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;


    public virtual Organization? Organization { get; set; }      
    public virtual ICollection<PartyRole> PartyRoles { get; set; } = null!;

}