using AccountApi.CQRS.Queries;

namespace AccountApi.CQRS
{
    public class GetBalanceQuery : IQuery
    {
        public Guid AccountId { get; set; }
    }
}
