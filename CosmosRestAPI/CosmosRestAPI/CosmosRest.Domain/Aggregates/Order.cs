using System;

namespace CosmosRest.Domain.Aggregates
{
    public class Order: AggregateRoot
    {
        public string OrderName { get; private set; }
        public string Address { get; private set; }

        public Order(Guid id, string orderName, string address) : base(id.ToString())
        {
            OrderName = orderName;
            Address = address;
        }
    }
}