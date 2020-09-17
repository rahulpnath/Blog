using System;

namespace CosmosRest.Domain.Aggregates
{
    public class Customer : AggregateRoot
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public int Age { get; private set; }

        public Customer(Guid id, string name, int age, string address) : base(id.ToString())
        {
            Name = name;
            Age = age;
            Address = address;
        }
    }
}