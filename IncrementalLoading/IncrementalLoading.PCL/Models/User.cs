namespace IncrementalLoading.PCL.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string fullname { get; set; }
        public string userpic_url { get; set; }
        public int upgrade_status { get; set; }
        public int followers_count { get; set; }
        public int affection { get; set; }
    }
}