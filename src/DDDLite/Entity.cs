namespace DDDLite
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
    }
}
