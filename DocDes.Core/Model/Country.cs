using DocDes.Core.Base;

namespace DocDes.Core.Model
{
    public class Country : ModelBase 
    {
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        
        public virtual ICollection<City> Citys { get; set; } = null!;
   }
}