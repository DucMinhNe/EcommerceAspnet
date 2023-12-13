using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public ProvidersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProviders(bool? isDeleted = null)
        {
          if (_context.Providers == null)
          {
              return NotFound();
          }
            IQueryable<Provider> providersQuery = _context.Providers;
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                providersQuery = providersQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var providers = await providersQuery.ToListAsync();
            return providers;
        }

        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(int id)
        {
          if (_context.Providers == null)
          {
              return NotFound();
          }
            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }

        // PUT: api/Providers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvider(int id, Provider provider)
        {
            if (id != provider.Id)
            {
                return BadRequest();
            }

            _context.Entry(provider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderExists(id))
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

        // POST: api/Providers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Provider>> PostProvider(Provider provider)
        {
          if (_context.Providers == null)
          {
              return Problem("Entity set 'EcommerceContext.Providers'  is null.");
          }
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProvider", new { id = provider.Id }, provider);
        }

        // DELETE: api/Providers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            if (_context.Providers == null)
            {
                return NotFound();
            }
            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            provider.IsDeleted = true;
            //_context.Providers.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Providers/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreProvider(int id)
        {
            if (_context.Providers == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FindAsync(id);

            if (provider == null)
            {
                return NotFound();
            }

            // Restore the provider by setting IsDeleted to false
            provider.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(provider);
        }

        private bool ProviderExists(int id)
        {
            return (_context.Providers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
