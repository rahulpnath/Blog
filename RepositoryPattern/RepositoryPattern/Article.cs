namespace RepositoryPattern
{
    public class Article : IIdentifiable
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }
    }
}