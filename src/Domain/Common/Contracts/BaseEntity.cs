using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;

namespace de.WebApi.Domain.Common.Contracts;

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; } = default!;

}

public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}