namespace AccountApi.CQRS.Comands.CommandHandler
{
    public interface ICommandHandler<in TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        Task<TResponse> HandlerAsync(TRequest request);
    }
}
