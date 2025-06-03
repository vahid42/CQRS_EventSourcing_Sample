using AccountFlow.Api.V2.CQRS.Queries;

namespace AccountFlow.Api.V2.CQRS
{
    public class GetBalanceQuery : IQuery
    {
        public Guid AccountId { get; set; }
    }
}
