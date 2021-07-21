namespace Album.Api.Models
{
    public class Album 
    {
        public long Id { get; set; }
        public string Name {get; set; }
        public string Artist {get; set; }
        public string ImageUrl {get; set; }
    }
}