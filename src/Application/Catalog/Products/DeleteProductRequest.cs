﻿using de.WebApi.Domain.Catalog;

namespace de.WebApi.Application.Catalog.Products;

public class DeleteProductRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteProductRequest(Guid id) => Id = id;
}

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, Guid>
{
    private readonly IRepository<Product> _repository;
    private readonly IStringLocalizer _t;

    public DeleteProductRequestHandler(IRepository<Product> repository, IStringLocalizer<DeleteProductRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<Guid> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = product ?? throw new NotFoundException(_t["Product {0} Not Found."]);
        await _repository.DeleteAsync(product, cancellationToken);

        return request.Id;
    }
}
