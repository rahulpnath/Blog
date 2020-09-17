namespace CosmosRest.Domain.Aggregates
{
    public interface IAggregateRoot
    {
        string Id { get; }
    }
    public abstract class AggregateRoot : IAggregateRoot
    {
        public string Id { get; }

        public AggregateRoot()
        {
            Id = string.Empty;
        }

        public AggregateRoot(string id)
        {
            Id = id;
        }
    }
}