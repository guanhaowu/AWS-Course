using System.Collections.Generic;
using System.Threading.Tasks;
using Album.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Album.Api.Models;
using Album.Api.Services;

namespace Album.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AlbumController : ControllerBase, IAlbumService
    {
        private readonly AlbumService _albumService;
        
        public AlbumController(DBContext context)
        {
            _albumService = new AlbumService(context);
        }

        /// <summary>
        /// Retrieves all albums.
        /// </summary>
        /// <response code="200">Returns all the albums.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Album>>> GetAlbums()
        {
            return await _albumService.GetAlbums();
        }
        
        /// <summary>
        /// Retrieve a specific album by id.
        /// </summary>
        /// <param name="id">The id of that specific album.</param>
        /// <response code="200">Returns the album with the same id.</response>
        /// <response code="404">No album has been found with this id.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.Album>> GetAlbumById(long id)
        {
            return await _albumService.GetAlbumById(id);
        }

        /// <summary>
        /// Update an album.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Album:
        ///     {
        ///         "id": 0,
        ///         "Name": "string",
        ///         "Artist": "string",
        ///         "ImageUrl": "strings"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">The id of the album you want to update.</param>
        /// <param name="album">The Album object with the new values.</param>
        /// <response code="204">No content.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Album does not exist.</response>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAlbum(long id, Models.Album album)
        {
            return await _albumService.UpdateAlbum(id, album);
        }

        /// <summary>
        /// Create a new album.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Album:
        ///     {
        ///         "Name": "string",
        ///         "Artist": "string",
        ///         "ImageUrl": "strings"
        ///     }
        /// 
        /// </remarks>
        /// <param name="album">The album object that you want to create</param>
        /// <returns>A newly created Album</returns>
        /// <response code="201">Created.</response>
        /// <response code="400">Bad request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.Album>> CreateAlbum(Models.Album album)
        {
           return await _albumService.CreateAlbum(album);
        }

        /// <summary>
        /// Delete an album by id.
        /// </summary>
        /// <param name="id">The id of the album you want to delete.</param>
        /// <response code="204">No content.</response>
        /// <response code="404">Not found.</response>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAlbumById(long id)
        {
            return await _albumService.DeleteAlbumById(id);
        }
    }
}
