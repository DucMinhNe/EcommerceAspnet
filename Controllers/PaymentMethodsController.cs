using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;
using Microsoft.DotNet.Scaffolding.Shared;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public PaymentMethodsController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethods(bool? isDeleted = null)
        {
            if (_context.PaymentMethods == null)
            {
                return NotFound();
            }
            IQueryable<PaymentMethod> paymentMethodsQuery = _context.PaymentMethods;
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                paymentMethodsQuery = paymentMethodsQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var paymentMethods = await paymentMethodsQuery.ToListAsync();
            return paymentMethods;
        }

        // GET: api/PaymentMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(int id)
        {
          if (_context.PaymentMethods == null)
          {
              return NotFound();
          }
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return paymentMethod;
        }

        // PUT: api/PaymentMethods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentMethod(int id, PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(id))
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

        // POST: api/PaymentMethods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentMethod>> PostPaymentMethod(PaymentMethod paymentMethod)
        {
          if (_context.PaymentMethods == null)
          {
              return Problem("Entity set 'EcommerceContext.PaymentMethods'  is null.");
          }
            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentMethod", new { id = paymentMethod.Id }, paymentMethod);
        }

        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            if (_context.PaymentMethods == null)
            {
                return NotFound();
            }
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            paymentMethod.IsDeleted = true;
            //_context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/PaymentMethods/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestorePaymentMethod(int id)
        {
            if (_context.PaymentMethods == null)
            {
                return NotFound();
            }

            var paymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            // Restore the paymentMethod by setting IsDeleted to false
            paymentMethod.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(paymentMethod);
        }

        private bool PaymentMethodExists(int id)
        {
            return (_context.PaymentMethods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
