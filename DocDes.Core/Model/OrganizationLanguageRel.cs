using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class OrganizationLanguageRel : ModelBase<int>
{
    public int OrganizationId { get; set; }
    public string LanguageCd { get; set; } = null!;

    public virtual Organization Organization { get; set; } = null!;
}