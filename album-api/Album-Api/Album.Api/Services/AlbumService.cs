using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Album.Api.Interfaces;
using Album.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Services
{
    public class AlbumService : ControllerBase, IAlbumService
    {
        private readonly DBContext _context;

        public AlbumService(DBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Models.Album>>> GetAlbums()
        {
            return await _context.Albums.ToListAsync();
        }

        public async Task<ActionResult<Models.Album>> GetAlbumById(long id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null) return NotFound();
            return album;
        }

        public async Task<IActionResult> UpdateAlbum(long id, Models.Album album)
        {
            if (id != album.Id) return BadRequest();

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        public async Task<ActionResult<Models.Album>> CreateAlbum(Models.Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            // return await _context.Albums.FindAsync(album.Id);
            return CreatedAtAction("GetAlbumById", new { id = album.Id }, album);
        }

        public async Task<IActionResult> DeleteAlbumById(long id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null) return NotFound();
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        private bool AlbumExists(long id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}