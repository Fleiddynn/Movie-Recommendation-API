namespace WebApplication1
{
    public class MovieInfo
    {
        public string title { get; set; }
        public string description { get; set; }
        public List<string> cast { get; set; }
        public string director { get; set; }
        public List<string> categories { get; set; }
        public int imdb { get; set; }
        public int length { get; set; }
        public DateOnly releaseDate { get; set; }


    }
}
