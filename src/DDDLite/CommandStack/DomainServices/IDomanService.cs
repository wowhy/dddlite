namespace DDDLite.CommandStack.DomainServices
{
    using Domain;

    public interface IDomanService
    {
        IDomainRepositoryContext Context { get; }
    }
}
