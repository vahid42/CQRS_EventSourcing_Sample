namespace AccountFlow.Api.V2.CQRS.Comands
{
    public class CreateCommand : ICommand<Guid>
    {
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal Bonus { get; set; }

    }
}
