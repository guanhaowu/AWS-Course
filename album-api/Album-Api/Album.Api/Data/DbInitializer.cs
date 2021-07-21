using System.Linq;
using Album.Api.Models;

namespace Album.Api.Data
{
    public class DbInitializer
    {
        public static void Initialize(DBContext context)
        {
            context.Database.EnsureCreated();
            
            // Look for any Albums
            if (context.Albums.Any()) return; // DB has been seeded already.

            var albums = new Models.Album[]
            {
                new() {Name = "Name1", Artist = "Artist1", ImageUrl = "URL1"},
                new() {Name = "Name2", Artist = "Artist2", ImageUrl = "URL2"},
                new() {Name = "Name3", Artist = "Artist3", ImageUrl = "URL3"},
                new() {Name = "Name4", Artist = "Artist4", ImageUrl = "URL4"},
                new() {Name = "Name5", Artist = "Artist5", ImageUrl = "URL5"}
            };

            foreach (Models.Album album in albums)
            {
                context.Albums.Add(album); // Add each record to DB Album table.
            }
            context.SaveChanges(); // Confirm changes
        }
    }
}