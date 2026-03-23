
using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class Status : ModelBase<int>
{
    public string StatusType { get; set; } = null!;
    public string StatusCode { get; set; } = null!;
    public string? Name { get; set; } 
    public string? Description { get; set; }
}