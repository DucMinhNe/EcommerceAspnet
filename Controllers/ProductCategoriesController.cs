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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public ProductCategoriesController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories(bool? isDeleted = null)
        {
          if (_context.ProductCategories == null)
          {
              return NotFound();
          }

            IQueryable<ProductCategory> productCategoriesQuery = _context.ProductCategories;
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                productCategoriesQuery = productCategoriesQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var productCategories = await productCategoriesQuery.ToListAsync();
            return productCategories;
        }

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
          if (_context.ProductCategories == null)
          {
              return NotFound();
          }
            var productCategory = await _context.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
          if (_context.ProductCategories == null)
          {
              return Problem("Entity set 'EcommerceContext.ProductCategories'  is null.");
          }
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.Id }, productCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            if (_context.ProductCategories == null)
            {
                return NotFound();
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            productCategory.IsDeleted = true;
            //_context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/ProductCategories/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreProductCategory(int id)
        {
            if (_context.ProductCategories == null)
            {
                return NotFound();
            }

            var productCategory = await _context.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            // Restore the productCategory by setting IsDeleted to false
            productCategory.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(productCategory);
        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
