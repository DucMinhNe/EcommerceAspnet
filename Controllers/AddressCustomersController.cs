using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressCustomersController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public AddressCustomersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/AddressCustomers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressCustomer>>> GetAddressCustomers()
        {
          if (_context.AddressCustomers == null)
          {
              return NotFound();
          }
            return await _context.AddressCustomers.ToListAsync();
        }

        // GET: api/AddressCustomers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressCustomer>> GetAddressCustomer(int id)
        {
          if (_context.AddressCustomers == null)
          {
              return NotFound();
          }
            var addressCustomer = await _context.AddressCustomers.FindAsync(id);

            if (addressCustomer == null)
            {
                return NotFound();
            }

            return addressCustomer;
        }

        // PUT: api/AddressCustomers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddressCustomer(int id, AddressCustomer addressCustomer)
        {
            if (id != addressCustomer.Id)
            {
                return BadRequest();
            }

            _context.Entry(addressCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressCustomerExists(id))
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

        // POST: api/AddressCustomers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AddressCustomer>> PostAddressCustomer(AddressCustomer addressCustomer)
        {
          if (_context.AddressCustomers == null)
          {
              return Problem("Entity set 'EcommerceContext.AddressCustomers'  is null.");
          }
            _context.AddressCustomers.Add(addressCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddressCustomer", new { id = addressCustomer.Id }, addressCustomer);
        }

        // DELETE: api/AddressCustomers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddressCustomer(int id)
        {
            if (_context.AddressCustomers == null)
            {
                return NotFound();
            }
            var addressCustomer = await _context.AddressCustomers.FindAsync(id);
            if (addressCustomer == null)
            {
                return NotFound();
            }

            _context.AddressCustomers.Remove(addressCustomer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressCustomerExists(int id)
        {
            return (_context.AddressCustomers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
