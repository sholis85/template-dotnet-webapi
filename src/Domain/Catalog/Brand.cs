﻿using de.WebApi.Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.WebApi.Domain.Catalog;
public class Brand : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Brand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public Brand Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }
}
