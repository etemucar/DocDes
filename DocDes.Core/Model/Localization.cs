
using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Localization : ModelBase
    {
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string EntityField { get; set; }
        public string LanguageCd { get; set; }
        public string Value { get; set; }
 
    }
}