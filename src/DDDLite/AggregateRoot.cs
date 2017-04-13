namespace DDDLite
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Commands;
    using AutoMapper;
    using Validation;

    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RowVersion { get; set; }

        public Guid? CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void HandleCommand(ICommand command)
        {
            try
            {
                var commandType = command.GetTypeName();
                if (commandType.StartsWith("DDDLite.Commands.CreateCommand`1"))
                {
                    this.Create(command as IAggregateRootCommand);
                    return;
                }

                if (commandType.StartsWith("DDDLite.Commands.UpdateCommand`1"))
                {
                    this.Update(command as IAggregateRootCommand);
                    return;
                }

                if (commandType.StartsWith("DDDLite.Commands.DeleteCommand`1"))
                {
                    this.Delete(command as IAggregateRootCommand);
                    return;
                }

                var method = GetType().GetMethod("Handle", new[] { command.GetType() });
                if (method == null)
                {
                    throw new ArgumentNullException("method", $"没有找到\"{this.GetType().FullName}.Handle({command.GetType().FullName} command)\"方法");
                }

                method.Invoke(this, new object[] { command });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                else
                    throw;
            }
        }

        public virtual void Create(IAggregateRootCommand command)
        {
            if (command == null || command.AggregateRoot == null)
            {
                throw new CoreValidateException("命令参数不能为空！");
            }

            var validator = new EntityValidator();
            validator.DoValidate(command);

            AutoMapper.Mapper.Map(command.AggregateRoot, this, command.AggregateRoot.GetType(), this.GetType());

            this.Id = command.AggregateRootId;
            this.CreatedById = command.OperatorId;
            this.CreatedOn = DateTime.Now;
            this.RowVersion = 1;
        }

        public virtual void Update(IAggregateRootCommand command)
        {
            var validator = new EntityValidator();
            validator.DoValidate(command);

            command.AggregateRoot.Id = this.Id;

            AutoMapper.Mapper.Map(command.AggregateRoot, this, command.AggregateRoot.GetType(), this.GetType());

            this.ModifiedById = command.OperatorId;
            this.ModifiedOn = DateTime.Now;
            this.RowVersion = command.RowVersion;
        }

        public virtual void Delete(IAggregateRootCommand command)
        {
            this.Id = command.AggregateRootId;

            if (this is ILogicalDelete)
            {
                ((ILogicalDelete)this).Deleted = true;
                this.ModifiedById = command.OperatorId;
                this.ModifiedOn = DateTime.Now;
                this.RowVersion = command.RowVersion;
            }            
        }
    }
}
