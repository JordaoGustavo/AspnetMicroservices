namespace Ordering.Domain.Common
{
    public abstract class Entity
    {
        public int Id { get; protected set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LasModifiedDate { get; set; }
    }
}
