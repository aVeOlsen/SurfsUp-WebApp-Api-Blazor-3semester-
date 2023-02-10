using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorPrototype.Server.Data;
using BlazorPrototype.Server.Models;
using System.IO;

namespace BlazorPrototype.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Spots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spot>>> GetSpot()
        {
            var spots = await _context.Spot.ToListAsync();
            for (int i = 0; i < spots.Count; i++)
            {
                string base64Data = Convert.ToBase64String(spots[i].SpotImage);
                string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
                spots[i].ImageDataUrl = imageDataUrl;
            }

            return spots;
        }

        // GET: api/Spots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spot>> GetSpot(int id)
        {
            var spot = await _context.Spot.FindAsync(id);

            if (spot == null)
            {
                return NotFound();
            }
            string base64Data = Convert.ToBase64String(spot.SpotImage);
            string imageDataUrl = string.Format("data:image/jpg;base64,{0}", base64Data);
            spot.ImageDataUrl = imageDataUrl;
            return spot;
        }

        // PUT: api/Spots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpot(int id, Spot spot)
        {
            if (id != spot.SpotID)
            {
                return BadRequest();
            }

            _context.Entry(spot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Spots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Spot>> PostSpot(Spot spot)
        {

            _context.Spot.Add(spot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpot", new { id = spot.SpotID }, spot);
        }

        // DELETE: api/Spots/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpot(int id)
        {
            var spot = await _context.Spot.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            _context.Spot.Remove(spot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpotExists(int id)
        {
            return _context.Spot.Any(e => e.SpotID == id);
        }

    }
}
