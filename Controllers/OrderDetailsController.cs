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
    public class OrderDetailsController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public OrderDetailsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        //{
        //  if (_context.OrderDetails == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.OrderDetails.ToListAsync();
        //}
        // GET: api/OrderDetails

        // GET: api/OrderDetails
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails(bool? isDeleted = null)
        //{
        //    if (_context.OrderDetails == null)
        //    {
        //        return NotFound();
        //    }
        //    IQueryable<OrderDetail> orderDetailsQuery = _context.OrderDetails;

        //    if (isDeleted.HasValue)
        //    {
        //        // Filter by IsDeleted if the parameter is provided
        //        orderDetailsQuery = orderDetailsQuery.Where(c => c.IsDeleted == isDeleted.Value);
        //    }
        //    var orderDetails = await orderDetailsQuery.ToListAsync();
        //    return orderDetails;
        //}
        [HttpGet]
        //[HttpGet("GetOrderDetailsByOrderId")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetailsByOrderId(int? orderId, bool? isDeleted = null)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }

            IQueryable<OrderDetail> orderDetailsQuery = _context.OrderDetails;

            // Filter by orderId
            if (orderId.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                orderDetailsQuery = orderDetailsQuery.Where(c => c.OrderId == orderId);
            }
            if (isDeleted.HasValue)
            {
                orderDetailsQuery = orderDetailsQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }

            var orderDetails = await orderDetailsQuery.ToListAsync();
            return orderDetails;
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
          if (_context.OrderDetails == null)
          {
              return NotFound();
          }
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // PUT: api/OrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // POST: api/OrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail([FromForm] OrderDetail orderDetail)
        {
          if (_context.OrderDetails == null)
          {
              return Problem("Entity set 'EcommerceContext.OrderDetails'  is null.");
          }
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return (_context.OrderDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
