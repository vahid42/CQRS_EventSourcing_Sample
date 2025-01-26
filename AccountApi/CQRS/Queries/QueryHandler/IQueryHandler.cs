namespace AccountApi.CQRS.Queries.QueryHandler
{
    public interface IQueryHandler<in TRequest, TResponse> where TRequest : IQuery
    {
        Task<TResponse> HandlerAsync(TRequest request);
    }
}
