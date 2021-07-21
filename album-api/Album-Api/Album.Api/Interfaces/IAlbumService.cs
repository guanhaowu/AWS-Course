using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Interfaces
{
    public interface IAlbumService
    {
        Task<ActionResult<IEnumerable<Models.Album>>> GetAlbums();
        Task<ActionResult<Models.Album>> GetAlbumById(long id);
        Task<IActionResult> UpdateAlbum(long id, Models.Album album);
        Task<ActionResult<Models.Album>> CreateAlbum(Models.Album album);
        Task<IActionResult> DeleteAlbumById(long id);
    }
}