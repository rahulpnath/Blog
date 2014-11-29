namespace RepositoryPattern
{
    using System.Collections.Generic;

    public class Blog : IIdentifiable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Article> Articles { get; set; }

        public string Url { get; set; }
    }
}