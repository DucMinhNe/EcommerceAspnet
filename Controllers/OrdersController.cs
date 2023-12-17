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
    public class OrdersController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public OrdersController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int? employeeId, int? customerId, bool? isDeleted = null)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            IQueryable<Order> ordersQuery = _context.Orders;
            if (employeeId.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                ordersQuery = ordersQuery.Where(c => c.EmployeeId == employeeId.Value);
            }
            if (customerId.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                ordersQuery = ordersQuery.Where(c => c.CustomerId == customerId.Value);
            }
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                ordersQuery = ordersQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var orders = await ordersQuery.ToListAsync();
            return orders;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromForm] Order order)
        {
            order.Id = id;
            // Lấy thông tin khách hàng cũ từ database
            var existingOrder = await _context.Orders.FindAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin khách hàng
            //_context.Entry(existingOrder).CurrentValues.SetValues(Order);
            foreach (var property in typeof(Order).GetProperties())
            {
                var newValue = property.GetValue(order);
                if (newValue != null)
                {
                    property.SetValue(existingOrder, newValue);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromForm] Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'EcommerceContext.Orders'  is null.");
          }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            //_context.Orders.Remove(order);
            order.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT: api/Orders/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            // Restore the order by setting IsDeleted to false
            order.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(order);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
