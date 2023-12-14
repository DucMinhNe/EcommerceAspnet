using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;
using Microsoft.AspNetCore.Hosting;

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
        public async Task<ActionResult<IEnumerable<AddressCustomer>>> GetAddressCustomers(int? customerId,bool? isDeleted = null)
        {
            if (_context.AddressCustomers == null)
            {
                return NotFound();
            }

            IQueryable<AddressCustomer> addressCustomersQuery = _context.AddressCustomers;

            if (customerId.HasValue)
            {
                addressCustomersQuery = addressCustomersQuery.Where(c => c.CustomerId == customerId.Value);
            }
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                addressCustomersQuery = addressCustomersQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var addressCustomers = await addressCustomersQuery.ToListAsync();
            return addressCustomers;
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
        public async Task<IActionResult> PutAddressCustomer(int id, [FromForm] AddressCustomer addressCustomer)
        {
            addressCustomer.Id = id;
            // Lấy thông tin khách hàng cũ từ database
            var existingAddressCustomer = await _context.AddressCustomers.FindAsync(id);

            if (existingAddressCustomer == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khách hàng
            //_context.Entry(existingAddressCustomer).CurrentValues.SetValues(addressCustomer);
            foreach (var property in typeof(AddressCustomer).GetProperties())
            {
                var newValue = property.GetValue(addressCustomer);
                if (newValue != null)
                {
                    property.SetValue(existingAddressCustomer, newValue);
                }
            }
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

            return Ok(addressCustomer);
        }

        // POST: api/AddressCustomers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AddressCustomer>> PostAddressCustomer([FromForm] AddressCustomer addressCustomer)
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

            //_context.AddressCustomers.Remove(addressCustomer);
            addressCustomer.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/AddressCustomers/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreAddressCustomer(int id)
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

            // Restore the addressCustomer by setting IsDeleted to false
            addressCustomer.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(addressCustomer);
        }

        private bool AddressCustomerExists(int id)
        {
            return (_context.AddressCustomers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
