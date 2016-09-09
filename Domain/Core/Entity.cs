namespace Domain.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Entity : IEntity<Guid>
    {
        [Key]
        [Column]
        public Guid Id { get; set; }

        [ConcurrencyCheck]
        public long RowVersion { get; set; }

        public void NewIdentity()
        {
            this.Id = SequentialGuid.Create(SequentialGuidType.SequentialAtEnd);
        }

        public static TEntity Create<TEntity>()
            where TEntity : Entity, new()
        {
            var entity = new TEntity();
            entity.NewIdentity();
            return entity;
        }
    }
}
