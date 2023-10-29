#pragma warning disable CS8618
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Data.Entities;

public class CatalogType : IEntityWithId
{
    public int Id { get; set; }

    public string Type { get; set; }
}