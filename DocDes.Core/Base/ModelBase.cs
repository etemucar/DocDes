namespace DocDes.Core.Base {
    public class ModelBase
    {
        public virtual int Id { get; set; }
        public virtual int StatusId { get; set; } = 1;
        public virtual int IsDeleted { get; set; } = 0;
        public virtual DateTime? CreateDate { get; set; }
        public virtual int? CreatedBy { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual int? UpdatedBy { get; set; }
    }
}

