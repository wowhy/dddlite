namespace DDDLite.Domain
{
    public static class EntityExtension
    {
        public static void NewIdentity(this IEntity entity)
        {
            entity.Id = SequentialGuid.Create();
        }
    }
}