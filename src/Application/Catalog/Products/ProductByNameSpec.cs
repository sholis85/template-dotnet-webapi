using de.WebApi.Domain.Catalog;

namespace de.WebApi.Application.Catalog.Products;

public class ProductByNameSpec : Specification<Product>, ISingleResultSpecification<Product>
{
    public ProductByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}