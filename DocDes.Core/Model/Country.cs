using DocDes.Core.Base;

namespace DocDes.Core.Model;

public class Country : ModelBase<int>
{
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}