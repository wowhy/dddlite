namespace DDDLite.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Entity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public void NewIdentity()
        {
            this.Id = SequentialGuid.Create(SequentialGuidType.SequentialAsString);
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
