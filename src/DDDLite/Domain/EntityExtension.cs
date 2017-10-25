namespace DDDLite.Domain
{
  using System;

  public static class EntityExtension
  {
    public static void NewIdentity(this IEntity<Guid> entity)
    {
      entity.Id = SequentialGuid.Create();
    }

    public static void NewIdentity(this IEntity<string> entity)
    {
      entity.Id = SequentialGuid.Create().ToString();
    }
  }
}