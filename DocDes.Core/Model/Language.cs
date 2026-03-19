using DocDes.Core.Base;

namespace DocDes.Core.Model
{
    public class Language : ModelBase 
    {
        public string LanguageCd { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrganizationLanguageRel> OrganizationLanguageRels { get; set; }
   }
}