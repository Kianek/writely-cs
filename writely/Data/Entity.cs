using System;
namespace writely.Data
{
    public class Entity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }

        public Entity()
        {
            CreatedAt = LastModified = DateTime.UtcNow;
        }
    }
}
