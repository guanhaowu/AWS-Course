using Album.Api.Data;
using Album.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Tests.Lib
{
    public static class DatabaseContext
    {
        public static DBContext CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<DBContext>().UseInMemoryDatabase("albumdatabase")
                .Options;

            using var context = new DBContext(options);
            DbInitializer.Initialize(context);
            return new DBContext(options);
        }
    }
}