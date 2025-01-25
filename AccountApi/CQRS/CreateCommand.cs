namespace AccountApi.CQRS
{
    public class CreateCommand
    {
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
