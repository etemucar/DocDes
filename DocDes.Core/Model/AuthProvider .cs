using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class AuthProvider : ModelBase<int>
{
    public int? OrganizationId { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Config { get; set; }

    public virtual Organization? Organization { get; set; }
}