﻿using System;
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
    public class SpacesController : ControllerBase
    {
        private readonly spacemeetContext _context;

        public SpacesController(spacemeetContext context)
        {
            _context = context;
        }

        // GET: api/Spaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Space>>> GetSpace()
        {
          if (_context.Space == null)
          {
              return NotFound();
          }
            return await _context.Space.ToListAsync();
        }

        // GET: api/Spaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Space>> GetSpace(int id)
        {
          if (_context.Space == null)
          {
              return NotFound();
          }
            var space = await _context.Space.FindAsync(id);

            if (space == null)
            {
                return NotFound();
            }

            return space;
        }

        // PUT: api/Spaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpace(int id, Space space)
        {
            if (id != space.Id)
            {
                return BadRequest();
            }

            _context.Entry(space).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpaceExists(id))
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

        // POST: api/Spaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Space>> PostSpace(Space space)
        {
          if (_context.Space == null)
          {
              return Problem("Entity set 'spacemeetContext.Space'  is null.");
          }
            _context.Space.Add(space);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpace", new { id = space.Id }, space);
        }

        // DELETE: api/Spaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpace(int id)
        {
            if (_context.Space == null)
            {
                return NotFound();
            }
            var space = await _context.Space.FindAsync(id);
            if (space == null)
            {
                return NotFound();
            }

            _context.Space.Remove(space);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpaceExists(int id)
        {
            return (_context.Space?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
