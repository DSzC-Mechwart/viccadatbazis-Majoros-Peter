using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private ViccDbContext _context;

        public HomeController(ViccDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vicc>>> GetViccek()
        {
            return await _context.Viccek.Where(G => G.Aktiv).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Vicc>> GetViccById(int id)
        {
            Vicc? vicc = await _context.Viccek.FirstOrDefaultAsync(G => G.Id == id);

            return vicc is null ? NotFound() : vicc;
        }

        [HttpPost]
        public async Task<ActionResult<Vicc>> PostVicc([FromBody] Vicc ujVicc)
        {
            _context.Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetViccById), new { id = ujVicc.Id }, ujVicc);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutVicc(int id, Vicc modositottVicc)
        {
            if (id != modositottVicc.Id)
                return BadRequest();

            _context.Entry(modositottVicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            Vicc? torlendoVicc = await _context.Viccek.FirstOrDefaultAsync(G => G.Id == id);
            
            if(torlendoVicc is null)
                return NotFound();

            if (torlendoVicc.Aktiv)
            {
                torlendoVicc.Aktiv = false;
                _context.Entry(torlendoVicc).State = EntityState.Modified;
            }
            else
                _context.Viccek.Remove(torlendoVicc);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
