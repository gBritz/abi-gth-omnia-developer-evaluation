using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Seeding;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Representa um semeador de dados.
/// </summary>
/// <param name="instances">Contexto de dados</param>
public class WebSeeder(IDataContext instances)
  : ISeedable
{
    private readonly IDataContext _instances = instances ?? throw new ArgumentNullException(nameof(instances));

    public WebSeeder On<T>(Action<T> visitor)
      where T : class
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));

        var entity = _instances.Last<T>();
        visitor(entity);

        return this;
    }

    public WebSeeder NewCart(Guid? id = null)
    {
        var card = new Cart
        {
            Id = id ?? Guid.NewGuid(),
            SaleNumber = 123456,
            BoughtBy = new User(),
            CreatedBy = new User()!,
        };

        _instances.Add(card);

        return this;
    }
}
