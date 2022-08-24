 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spacemeet.Data;
using spacemeet.Models;

namespace spacemeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualsController : ControllerBase
    {
        private readonly spacemeetContext _context;

        public IndividualsController(spacemeetContext context)
        {
            _context = context;
        }

        // GET: api/Individuals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Individual>>> GetIndividual()
        {
          if (_context.Individual == null)
          {
              return NotFound();
          }
            return await _context.Individual.ToListAsync();
        }

        // GET: api/Individuals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Individual>> GetIndividual(int id)
        {
          if (_context.Individual == null)
          {
              return NotFound();
          }
            var individual = await _context.Individual.FindAsync(id);

            if (individual == null)
            {
                return NotFound();
            }

            return individual;
        }

        // PUT: api/Individuals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndividual(int id, Individual individual)
        {
            if (id != individual.Id)
            {
                return BadRequest();
            }

            _context.Entry(individual).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndividualExists(id))
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

        // POST: api/Individuals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Individual>> PostIndividual(Individual individual)
        {
          if (_context.Individual == null)
          {
              return Problem("Entity set 'spacemeetContext.Individual'  is null.");
          }
            _context.Individual.Add(individual);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndividual", new { id = individual.Id }, individual);
        }

        // DELETE: api/Individuals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndividual(int id)
        {
            if (_context.Individual == null)
            {
                return NotFound();
            }
            var individual = await _context.Individual.FindAsync(id);
            if (individual == null)
            {
                return NotFound();
            }

            _context.Individual.Remove(individual);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IndividualExists(int id)
        {
            return (_context.Individual?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
