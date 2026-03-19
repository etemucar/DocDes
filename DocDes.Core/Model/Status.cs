
using DocDes.Core.Base;

namespace DocDes.Core.Model {
    public class Status : ModelBase
    {
        public string StatusType { get; set; }
        public string StatusCode { get; set; } 
        public string? Name { get; set; } 
        public string? Description { get; set; }
    }
}