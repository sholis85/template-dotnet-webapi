﻿using de.WebApi.Domain.Catalog;

namespace de.WebApi.Application.Catalog.Products;

public class ProductsByBrandSpec : Specification<Product>
{
    public ProductsByBrandSpec(Guid brandId) =>
        Query.Where(p => p.BrandId == brandId);
}