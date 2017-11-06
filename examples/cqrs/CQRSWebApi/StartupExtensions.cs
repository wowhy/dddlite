namespace CQRSWebApi
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using CQRSCore.CRUD.Handlers;
  using CQRSCore.EventSource.Commands;
  using CQRSCore.EventSource.Events;
  using CQRSCore.EventSource.Handlers;
  using CQRSCore.EventSource.Snapshots;
  using CQRSInfrastructure.EntityFramework;
  using CQRSInfrastructure.Store;
  using DDDLite.CQRS.Commands;
  using DDDLite.CQRS.Events;
  using DDDLite.CQRS.Messaging;
  using DDDLite.CQRS.Messaging.InMemory;
  using DDDLite.CQRS.Messaging.Redis;
  using DDDLite.CQRS.Repositories;
  using DDDLite.CQRS.Snapshots;
  using DDDLite.Repositories;
  using DDDLite.Repositories.EntityFramework;
  using DDDLite.WebApi;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Caching.Distributed;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;
  using StackExchange.Redis;
  using Crud = CQRSCore.CRUD.Domain;
  using EventSource = CQRSCore.EventSource.Domain;

  public static class StartupExtensions
  {
    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        scope.ServiceProvider.GetRequiredService<InventoryDbContext>().Database.EnsureCreated();
      }

      return app;
    }

    public static IApplicationBuilder RegisterEventHandlers(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        var bus = (RedisEventBus)scope.ServiceProvider.GetRequiredService<IEventPublisher>();
        var provider = scope.ServiceProvider;

        bus.RegisterHandler(GetEventHandler<InventoryItemCreated, InventoryEventHandlers>(provider));
        bus.RegisterHandler(GetEventHandler<InventoryItemRenamed, InventoryEventHandlers>(provider));
        bus.RegisterHandler(GetEventHandler<ItemsCheckedInToInventory, InventoryEventHandlers>(provider));
        bus.RegisterHandler(GetEventHandler<ItemsRemovedFromInventory, InventoryEventHandlers>(provider));
        bus.RegisterHandler(GetEventHandler<InventoryItemDeactivated, InventoryEventHandlers>(provider));
      }
      return app;
    }

    public static IApplicationBuilder RegisterCommandHandlers(this IApplicationBuilder app)
    {
      using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        var bus = (InMemoryCommandBus)scope.ServiceProvider.GetRequiredService<ICommandSender>();
        var provider = scope.ServiceProvider;

        app.RegisterCommandHandlers<InventoryCommandHandlers>(provider, bus);

        // bus.RegisterHandler(GetCommandHandler<CreateInventoryItem, InventoryCommandHandlers>(provider));
        // bus.RegisterHandler(GetCommandHandler<RenameInventoryItem, InventoryCommandHandlers>(provider));
        // bus.RegisterHandler(GetCommandHandler<CheckInItemsToInventory, InventoryCommandHandlers>(provider));
        // bus.RegisterHandler(GetCommandHandler<RemoveItemsFromInventory, InventoryCommandHandlers>(provider));
        // bus.RegisterHandler(GetCommandHandler<DeactivateInventoryItem, InventoryCommandHandlers>(provider));
      }

      return app;
    }

    public static IServiceCollection AddCrudContext(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("Default");

      services.AddDbContext<InventoryDbContext>(options => options.UseNpgsql(connectionString))
              .AddScoped<IUnitOfWork, InventoryDbContext>();
      services.AddScoped<IRepository<Crud.InventoryItem, Guid>, EFRepository<Crud.InventoryItem, Guid>>();

      return services;
    }

    public static IServiceCollection AddEventSourceContext(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("Default");
      var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));

      services.AddDistributedRedisCache(options =>
      {
        options.Configuration = configuration.GetConnectionString("Redis");
        options.InstanceName = "master";
      });
      services.AddSingleton<ICommandSender>(p => new InMemoryCommandBus());
      services.AddSingleton<IEventPublisher>(p => new RedisEventBus(redis));

      services.AddSingleton<IEventStore>(p => new InventoryEventStore(connectionString));
      services.AddSingleton<ISnapshotStore>(p => new InventorySnapshotStore(connectionString));

      services.AddScoped<SnapshotRepository<EventSource.InventoryItem, InventoryItemSnapshot>>();
      services.AddScoped<IDomainRepository<EventSource.InventoryItem>, CacheRepositoryDecorator<EventSource.InventoryItem>>(p => new CacheRepositoryDecorator<EventSource.InventoryItem>(p.GetService<IDistributedCache>(), p.GetService<SnapshotRepository<EventSource.InventoryItem, InventoryItemSnapshot>>()));

      services.AddTransient<InventoryCommandHandlers>();
      services.AddTransient<InventoryEventHandlers>();

      return services;
    }

    private static Func<T, Task> GetEventHandler<T, THandler>(IServiceProvider provider)
      where T : class, IEvent
      where THandler : class, IEventHandler<T>
    {
      return async (message) =>
      {
        await provider.GetRequiredService<THandler>().HandleAsync(message);
      };
    }

    private static Func<T, Task> GetCommandHandler<T, THandler>(IServiceProvider provider)
      where T : class, ICommand
      where THandler : class, ICommandHandler<T>
    {
      return async (message) =>
      {
        await provider.GetRequiredService<THandler>().HandleAsync(message);
      };
    }
  }
}