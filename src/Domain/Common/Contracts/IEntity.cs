﻿namespace de.WebApi.Domain.Common.Contracts;

public interface IEntity
{
}

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}