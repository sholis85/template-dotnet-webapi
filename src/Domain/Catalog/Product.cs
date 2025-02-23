using de.WebApi.Domain.Common.Contracts;

namespace de.WebApi.Domain.Catalog;

public class Product : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Rate { get; private set; }
    public Guid BrandId { get; private set; }
    public virtual Brand Brand { get; private set; } = default!;

    public Product(string name, string? description, decimal rate, Guid brandId)
    {
        Name = name;
        Description = description;
        Rate = rate;
        BrandId = brandId;
    }

    public Product Update(string? name, string? description, decimal? rate, Guid? brandId)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        if (rate.HasValue && Rate != rate) Rate = rate.Value;
        if (brandId.HasValue && brandId.Value != Guid.Empty && !BrandId.Equals(brandId.Value)) BrandId = brandId.Value;
        return this;
    }
}