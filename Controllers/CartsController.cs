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
    public class CartsController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public CartsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts(bool? isDeleted = null)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            IQueryable<Cart> cartsQuery = _context.Carts;

            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                cartsQuery = cartsQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var carts = await cartsQuery.ToListAsync();
            return carts;
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
          if (_context.Carts == null)
          {
              return NotFound();
          }
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // PUT: api/Carts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, [FromForm] Cart cart)
        {
            cart.Id = id;
            // Lấy thông tin khách hàng cũ từ database
            var existingCart = await _context.Carts.FindAsync(id);

            if (existingCart == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khách hàng
            //_context.Entry(existingCart).CurrentValues.SetValues(cart);
            foreach (var property in typeof(Cart).GetProperties())
            {
                var newValue = property.GetValue(cart);
                if (newValue != null)
                {
                    property.SetValue(existingCart, newValue);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(cart);
        }

        // POST: api/Carts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart([FromForm] Cart cart)
        {
          if (_context.Carts == null)
          {
              return Problem("Entity set 'EcommerceContext.Carts'  is null.");
          }
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            //_context.Carts.Remove(cart);
            cart.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PUT: api/Carts/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreCart(int id)
        {
            if (_context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            // Restore the cart by setting IsDeleted to false
            cart.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(cart);
        }
        private bool CartExists(int id)
        {
            return (_context.Carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
