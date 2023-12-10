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
    public class ProductsController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(EcommerceContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(bool? isDeleted = null)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            IQueryable<Product> productsQuery = _context.Products;

            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                productsQuery = productsQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var products = await productsQuery.ToListAsync();
            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] IFormFile? productImageFile, [FromForm] Product product)
        {
            product.Id = id;
            // Lấy thông tin khách hàng cũ từ database
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có hình ảnh cũ không
            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                // Xóa hình ảnh cũ
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ProductImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Kiểm tra xem có dữ liệu mới để cập nhật hình ảnh không
            if (productImageFile != null && productImageFile.Length > 0)
            {
                // Lưu trữ hình ảnh mới
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(productImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "product_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImageFile.CopyToAsync(stream);
                }

                // Cập nhật thông tin của khách hàng
                product.ProductImage = "product_images/" + fileName;
            }

            // Cập nhật thông tin khách hàng
            //_context.Entry(existingProduct).CurrentValues.SetValues(product);
            foreach (var property in typeof(Product).GetProperties())
            {
                var newValue = property.GetValue(product);
                if (newValue != null)
                {
                    property.SetValue(existingProduct, newValue);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] IFormFile? productImageFile, [FromForm] Product product)
        {

            if (productImageFile != null && productImageFile.Length > 0)
            {
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(productImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "product_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImageFile.CopyToAsync(stream);
                }

                product.ProductImage = "product_images/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            //_context.Products.Remove(product);
            product.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Products/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Restore the product by setting IsDeleted to false
            product.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
