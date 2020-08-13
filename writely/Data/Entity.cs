using System;
namespace writely.Data
{
    public class Entity
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public Entity()
        {
            CreatedAt = LastModified = DateTimeOffset.UtcNow;
        }
    }
}
