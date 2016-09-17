namespace DDDLite.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Entity : IEntity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public void NewIdentity()
        {
            this.Id = SequentialGuid.Create(SequentialGuidType.SequentialAsString);
        }
    }
}
