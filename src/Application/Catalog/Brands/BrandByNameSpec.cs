﻿using de.WebApi.Domain.Catalog;

namespace de.WebApi.Application.Catalog.Brands;

public class BrandByNameSpec : Specification<Brand>, ISingleResultSpecification<Brand>
{
    public BrandByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}

