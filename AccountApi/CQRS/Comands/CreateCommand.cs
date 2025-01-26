namespace AccountApi.CQRS.Comands
{
    public class CreateCommand : ICommand<Guid>
    {
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
