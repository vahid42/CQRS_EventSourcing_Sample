namespace AccountFlow.Api.V2.CQRS.Queries.QueryHandler
{
    public interface IQueryHandler<in TRequest, TResponse> where TRequest : IQuery
    {
        Task<TResponse> HandlerAsync(TRequest request);
    }
}
