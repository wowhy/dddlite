namespace DDDLite.CommandStack.DomainServices
{
    using Repository;

    public interface IDomanService
    {
        IDomainRepositoryContext Context { get; }
    }
}
