using System.Collections.Generic;
using System.Linq;
using Album.Api.Models;
using Album.Api.Services;
using Album.Api.Tests.Lib;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Album.Api.Tests
{
    public class AlbumTests
    {
        //Input-value test
        
        /// <summary>
        /// Assert if the number of records returned by GetAlbum() function are the same as the seeder rows.
        /// </summary>
        [Fact]
        public async void Assert_GetAlbums_SameNumberOfRecordsWithSeeder()
        {
            int objCounter;
            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                objCounter = context.Albums.Count();
            }

            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                AlbumService albumService = new AlbumService(context);
                var albums = (await albumService.GetAlbums()).Value;

                Assert.Equal(objCounter, albums.Count());
            }
        }
        
        /// <summary>
        /// Assert given Id matches with the return object id. 
        /// </summary>
        /// <param name="id"></param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Assert_GetAlbumById_MatchingRecord_ID(long id)
        {
            await using DBContext context = DatabaseContext.CreateInMemoryDb();
            AlbumService albumService = new AlbumService(context);
            var existingId = await albumService.GetAlbumById(id);
            
            Assert.Equal(id, existingId.Value.Id);
        }
        
        /// <summary>
        /// Assert non-existing id to return NotFoundResult.
        /// </summary>
        /// <param name="id"></param>
        [Theory]
        [InlineData(2000)]
        [InlineData(9999)]
        [InlineData(-1)]
        [InlineData(0)]
        public async void Assert_GetAlbumByNonExistingId_Failing(long id)
        {
            await using DBContext context = DatabaseContext.CreateInMemoryDb();
            AlbumService albumService = new AlbumService(context);
            var notExistingId = await albumService.GetAlbumById(id);

            Assert.IsType<NotFoundResult>(notExistingId.Result);
        }

        /// <summary>
        /// Assert value update for each property on existing id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="album"></param>

        [Theory]
        [MemberData(nameof(AlbumItemData1))]
        public async void Assert_UpdateAlbum_ExistingId(long id, Models.Album album)
        {
            Models.Album initialRecord = new Models.Album()
            {
                Name = "Name1",
                Artist = "Artist1",
                ImageUrl = "URL1"
            };

            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                context.Add(initialRecord);
                await context.SaveChangesAsync();
            }
            
            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                Models.Album newValues = new Models.Album()
                {
                    Id = 1,
                    Name = album.Name,
                    Artist = album.Artist,
                    ImageUrl = album.ImageUrl
                };
                
                AlbumService albumService = new AlbumService(context);
                await albumService.UpdateAlbum(id, newValues);
                var editedDbAlbumRecord = await context.Albums.FindAsync(id);
                
                Assert.Equal(id, editedDbAlbumRecord.Id);
                Assert.Equal(album.Name, editedDbAlbumRecord.Name);
                Assert.Equal(album.Artist, editedDbAlbumRecord.Artist);
                Assert.Equal(album.ImageUrl, editedDbAlbumRecord.ImageUrl);
            }
        }
        
        /// <summary>
        /// Assert Non existing ID on UpdateAlbum - Returns BadRequest
        /// </summary>
        /// <param name="id"></param>
        /// <param name="album"></param>
        [Theory]
        [MemberData(nameof(AlbumItemData2))]
        public async void Assert_UpdateAlbum_NonExistingId(long id, Models.Album album)
        {
            Models.Album initialRecord = new Models.Album()
            {
                Name = "Name1",
                Artist = "Artist1",
                ImageUrl = "URL1"
            };

            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                context.Add(initialRecord);
                await context.SaveChangesAsync();
            }
            
            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                Models.Album newValues = new Models.Album()
                {
                    Id = 1,
                    Name = album.Name,
                    Artist = album.Artist,
                    ImageUrl = album.ImageUrl
                };
                
                AlbumService albumService = new AlbumService(context);
                var response = albumService.UpdateAlbum(id,newValues);
                
                Assert.IsType<BadRequestResult>(response.Result);
            }
            
        }


        /// <summary>
        /// Assert CreateAlbum Creation and actual data in DB. 5 records in seeded. With the test data, it will be id 6.
        /// </summary>
        /// <param name="album"></param>
        [Theory]
        [MemberData(nameof(AlbumItemData3))]
        public async void Assert_CreateAlbum_Creation(Models.Album album)
        {
            await using var context = DatabaseContext.CreateInMemoryDb();
            
            AlbumService albumService = new AlbumService(context);

            await albumService.CreateAlbum(album);
            var currentDbAlbumRecord = await context.Albums.FindAsync(album.Id);
            
            Assert.Equal(album.Name, currentDbAlbumRecord.Name);
            Assert.Equal(album.Artist, currentDbAlbumRecord.Artist);
            Assert.Equal(album.ImageUrl, currentDbAlbumRecord.ImageUrl);
        }

        /// <summary>
        /// Delete Test to delete a single known id record. Seeder containers 5 record.
        /// </summary>
        [Theory]
        [InlineData(1)]
        public async void Assert_DeleteAlbum_ByExistingId(long id)
        {
            int objCounterOld;
            await using (var context = DatabaseContext.CreateInMemoryDb())
            {
                objCounterOld = context.Albums.Count();
            }

            await using (var context = DatabaseContext.CreateInMemoryDb())
            {
                AlbumService albumService = new AlbumService(context);
                await albumService.DeleteAlbumById(id);
                int objCounterNew = context.Albums.Count();
                Assert.NotEqual(objCounterOld, objCounterNew);
            }
            
        }
        
        /// <summary>
        /// Delete Test to delete a fresh added record. 5 records from seed by default.
        /// </summary>
        [Fact]
        public async void Assert_DeleteAlbum_AddedId()
        {
            Models.Album initialRecord = new Models.Album()
            {
                Name = "Name1",
                Artist = "Artist1",
                ImageUrl = "URL1"
            };
            
            int objCounterOld;
            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                objCounterOld = context.Albums.Count();
                context.Add(initialRecord);
                await context.SaveChangesAsync();
            }

            await using (DBContext context = DatabaseContext.CreateInMemoryDb())
            {
                AlbumService albumService = new AlbumService(context);
                await albumService.DeleteAlbumById(initialRecord.Id);
                var objCounterNew = context.Albums.Count();
                Assert.Equal(objCounterOld, objCounterNew);
            }
        }
        

        /// <summary>
        /// Data to test Assert_UpdateAlbum_ExistingId
        /// </summary>
        public static IEnumerable<object[]> AlbumItemData1 =>
            new List<object[]>
            {
                new object[]
                {
                    1,
                    new Models.Album
                    {
                        Name = "Name1", Artist = "Artist1", ImageUrl = "URL 1"
                    }
                },
                new object[]
                {
                    1,
                    new Models.Album
                    {
                        Name = "Name2", Artist = "Artist1", ImageUrl = "URL 1"
                    }
                },
                new object[]
                {
                    1,
                    new Models.Album
                    {
                        Name = "Name1", Artist = "Artist2", ImageUrl = "URL 1"
                    }
                },
                new object[]
                {
                    1,
                    new Models.Album
                    {
                        Name = "Name1", Artist = "Artist1", ImageUrl = "URL 2"
                    }
                }
            };
        
        /// <summary>
        /// Non existing Id data to test Assert_UpdateAlbum_NonExistingId
        /// </summary>
        public static IEnumerable<object[]> AlbumItemData2 =>
            new List<object[]>
            {
                new object[]
                {
                    9999,
                    new Models.Album
                    {
                        Name = "Name1", Artist = "Artist1", ImageUrl = "URL 1"
                    }
                }
            };
        
        /// <summary>
        /// New data to Assert AlbumCreate
        /// </summary>
        public static IEnumerable<object[]> AlbumItemData3 =>
            new List<object[]>
            {
                new object[]
                {
                    new Models.Album
                    {
                        Name = "Name1", Artist = "Artist1", ImageUrl = "URL 1"
                    }
                }
            };
    }
}