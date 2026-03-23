using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class Language : ModelBase<int>
{
    public string LanguageCd { get; set; } = null!;
    public string Name { get; set; } = null!;

    public virtual ICollection<OrganizationLanguageRel> OrganizationLanguageRels { get; set; } = null!;
}