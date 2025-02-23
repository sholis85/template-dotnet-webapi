namespace de.WebApi.Application.Common.ApiClient;

public interface IApiClient : IDisposable
{
    Task<TDto> Adapt<T, TDto>(T source)
        where T : class
        where TDto : class, IDto;
}
