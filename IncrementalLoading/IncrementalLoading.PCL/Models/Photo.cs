namespace IncrementalLoading.PCL.Models
{
    using System.Collections.Generic;

    public class Photo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int times_viewed { get; set; }
        public double rating { get; set; }
        public string created_at { get; set; }
        public int category { get; set; }
        public bool privacy { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int votes_count { get; set; }
        public int favorites_count { get; set; }
        public int comments_count { get; set; }
        public bool nsfw { get; set; }
        public int license_type { get; set; }
        public string image_url { get; set; }
        public List<Image> images { get; set; }
        public User user { get; set; }
    }
}